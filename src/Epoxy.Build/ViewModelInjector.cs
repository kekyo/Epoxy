////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) 2019-2021 Kouji Matsui (@kozy_kekyo, @kekyo2)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

#nullable enable

using System;
using System.IO;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Epoxy
{
    public sealed class ViewModelInjector
    {
        private readonly Action<string> message;
        private readonly DefaultAssemblyResolver assemblyResolver = new();

        private readonly TypeSystem typeSystem;

        private readonly TypeDefinition viewModelDetectionAttributeType;
        private readonly TypeDefinition viewModelAttributeType;
        private readonly TypeDefinition iViewModelImplementerType;
        private readonly TypeDefinition internalPropertyBagType;
        private readonly TypeDefinition internalModelHelperType;

        private readonly TypeReference propertyChangingEventHandlerType;
        private readonly TypeReference propertyChangedEventHandlerType;

        private readonly MethodDefinition addPropertyChanging;
        private readonly MethodDefinition removePropertyChanging;
        private readonly MethodDefinition addPropertyChanged;
        private readonly MethodDefinition removePropertyChanged;

        private readonly MethodDefinition onPropertyChangingMethod;

        private readonly MethodDefinition itAddPropertyChanging;
        private readonly MethodDefinition itRemovePropertyChanging;
        private readonly MethodDefinition itAddPropertyChanged;
        private readonly MethodDefinition itRemovePropertyChanged;

        public ViewModelInjector(string epoxyCorePath, Action<string> message)
        {
            this.message = message;
            this.assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(epoxyCorePath));

            var epoxyCoreAssembly = AssemblyDefinition.ReadAssembly(
                epoxyCorePath,
                new ReaderParameters
                {
                    AssemblyResolver = assemblyResolver,
                }
            );

            this.typeSystem = epoxyCoreAssembly.MainModule.TypeSystem;

            this.viewModelDetectionAttributeType = epoxyCoreAssembly.MainModule.GetType(
                "Epoxy.Supplemental.ViewModelDetectionAttribute")!;
            this.viewModelAttributeType = epoxyCoreAssembly.MainModule.GetType(
                "Epoxy.ViewModelAttribute")!;
            this.iViewModelImplementerType = epoxyCoreAssembly.MainModule.GetType(
                "Epoxy.Infrastructure.IViewModelImplementer")!;
            this.internalPropertyBagType = epoxyCoreAssembly.MainModule.GetType(
                "Epoxy.Internal.InternalPropertyBag")!;
            this.internalModelHelperType = epoxyCoreAssembly.MainModule.GetType(
                "Epoxy.Internal.InternalModelHelper")!;

            this.propertyChangingEventHandlerType = internalPropertyBagType.Fields.
                First(f => f.Name == "propertyChanging").FieldType;
            this.propertyChangedEventHandlerType = internalPropertyBagType.Fields.
                First(f => f.Name == "propertyChanged").FieldType;

            this.addPropertyChanging = this.internalModelHelperType.Methods.
                First(m => m.Name == "AddPropertyChanging");
            this.removePropertyChanging = this.internalModelHelperType.Methods.
                First(m => m.Name == "RemovePropertyChanging");
            this.addPropertyChanged = this.internalModelHelperType.Methods.
                First(m => m.Name == "AddPropertyChanged");
            this.removePropertyChanged = this.internalModelHelperType.Methods.
                First(m => m.Name == "RemovePropertyChanged");

            this.onPropertyChangingMethod = this.internalModelHelperType.Methods.
                First(m => m.Name == "OnPropertyChanging");

            var itPropertyChangingType = iViewModelImplementerType.Interfaces.
                First(ii => ii.InterfaceType.FullName == "System.ComponentModel.INotifyPropertyChanging").InterfaceType.Resolve();
            var itPropertyChangedType = iViewModelImplementerType.Interfaces.
                First(ii => ii.InterfaceType.FullName == "System.ComponentModel.INotifyPropertyChanged").InterfaceType.Resolve();

            this.itAddPropertyChanging = itPropertyChangingType.Events.
                First().AddMethod;
            this.itRemovePropertyChanging = itPropertyChangingType.Events.
                First().RemoveMethod;
            this.itAddPropertyChanged = itPropertyChangedType.Events.
                First().AddMethod;
            this.itRemovePropertyChanged = itPropertyChangedType.Events.
                First().RemoveMethod;
        }

        private void InjectIntoType(ModuleDefinition module, TypeDefinition targetType)
        {
            var propertiesField = new FieldDefinition(
                "epoxy_properties__",
                FieldAttributes.Private,
                module.ImportReference(this.internalPropertyBagType));
            targetType.Fields.Add(propertiesField);

            var ii = new InterfaceImplementation(
                module.ImportReference(this.iViewModelImplementerType));
            targetType.Interfaces.Add(ii);

            MethodDefinition CreatePropertyChangeEventTransfer(
                string name,
                TypeReference handlerType,
                MethodDefinition targetMethod)
            {
                var method = new MethodDefinition(
                    name, MethodAttributes.Public, this.typeSystem.Void);

                method.Parameters.Add(new ParameterDefinition(
                    module.ImportReference(handlerType)));
                var ilp = method.Body.GetILProcessor();
                ilp.Append(Instruction.Create(OpCodes.Ldarg_1));
                ilp.Append(Instruction.Create(OpCodes.Ldarg_0));
                ilp.Append(Instruction.Create(OpCodes.Ldflda, propertiesField));
                ilp.Append(Instruction.Create(OpCodes.Call, module.ImportReference(targetMethod)));
                ilp.Append(Instruction.Create(OpCodes.Ret));

                targetType.Methods.Add(method);

                return method;
            }

            var propertyChangingAddMethod = CreatePropertyChangeEventTransfer(
                "epoxy_PropertyChanging_add__",
                this.propertyChangingEventHandlerType,
                this.addPropertyChanging);
            propertyChangingAddMethod.Overrides.Add(
                module.ImportReference(this.itAddPropertyChanging));

            var propertyChangingRemoveMethod = CreatePropertyChangeEventTransfer(
                "epoxy_PropertyChanging_remove__",
                this.propertyChangingEventHandlerType,
                this.removePropertyChanging);
            propertyChangingRemoveMethod.Overrides.Add(
                module.ImportReference(this.itRemovePropertyChanging));

            var propertyChangingEvent = new EventDefinition(
                "PropertyChanging",
                EventAttributes.None,
                module.ImportReference(this.propertyChangingEventHandlerType));
            propertyChangingEvent.AddMethod = propertyChangingAddMethod;
            propertyChangingEvent.RemoveMethod = propertyChangingRemoveMethod;
            targetType.Events.Add(propertyChangingEvent);

            var propertyChangedAddMethod = CreatePropertyChangeEventTransfer(
                "epoxy_PropertyChanged_add__",
                this.propertyChangedEventHandlerType,
                this.addPropertyChanged);
            propertyChangedAddMethod.Overrides.Add(
                module.ImportReference(this.itAddPropertyChanged));

            var propertyChangedRemoveMethod = CreatePropertyChangeEventTransfer(
                "epoxy_PropertyChanged_remove__",
                this.propertyChangedEventHandlerType,
                this.removePropertyChanged);
            propertyChangedRemoveMethod.Overrides.Add(
                module.ImportReference(this.itRemovePropertyChanged));

            var propertyChangedEvent = new EventDefinition(
                "PropertyChanged",
                EventAttributes.None,
                module.ImportReference(this.propertyChangedEventHandlerType));
            propertyChangedEvent.AddMethod = propertyChangedAddMethod;
            propertyChangedEvent.RemoveMethod = propertyChangedRemoveMethod;
            targetType.Events.Add(propertyChangedEvent);
        }

        public void Inject(string targetAssemblyPath)
        {
            this.assemblyResolver.AddSearchDirectory(
                Path.GetDirectoryName(targetAssemblyPath));

            var targetAssemblyName = Path.GetFileNameWithoutExtension(
                targetAssemblyPath);

            var wrote = false;

            using (var targetAssembly = AssemblyDefinition.ReadAssembly(
                targetAssemblyPath,
                new ReaderParameters(ReadingMode.Immediate)
                {
                    AssemblyResolver = this.assemblyResolver,
                }))
            {
                var detections =
                    targetAssembly.HasCustomAttributes &&
                    targetAssembly.CustomAttributes.
                    FirstOrDefault(ca => ca.AttributeType.FullName == this.viewModelDetectionAttributeType.FullName)?.
                    ConstructorArguments[0].Value is int dv ? dv : 0;
                var attributeDetection = (detections == 0) || ((detections & 1) == 1);
                var suffixDetection = (detections & 2) == 2;

                var targetTypes = targetAssembly.MainModule.GetTypes().
                    Where(td => (td.IsClass || td.IsValueType) &&
                            ((suffixDetection && td.FullName.EndsWith("ViewModel")) ||
                             (attributeDetection && td.HasCustomAttributes &&
                             td.CustomAttributes.Any(ca => ca.AttributeType.FullName == this.viewModelAttributeType.FullName))) &&
                         !td.Interfaces.Any(ii => ii.InterfaceType.FullName == this.iViewModelImplementerType.FullName)).
                    ToArray();

                if (targetTypes.Length >= 1)
                {
                    foreach (var targetType in targetTypes)
                    {
                        this.InjectIntoType(targetAssembly.MainModule, targetType);
                        this.message($"Epoxy.Build: Injected a view model: Assembly={targetAssemblyName}, Type={targetType.FullName}");
                    }

                    targetAssembly.Write(targetAssemblyPath + ".tmp");

                    wrote = true;
                }
            }

            if (wrote)
            {
                if (File.Exists(targetAssemblyPath + ".orig"))
                {
                    File.Delete(targetAssemblyPath + ".orig");
                }

                File.Move(targetAssemblyPath, targetAssemblyPath + ".orig");
                File.Move(targetAssemblyPath + ".tmp", targetAssemblyPath);

                this.message($"Epoxy.Build: Replaced injected assembly: Assembly={targetAssemblyName}");
            }
        }
    }
}
