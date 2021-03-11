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
using System.Collections.Generic;
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
        private readonly TypeDefinition ignoreInjectAttributeType;
        private readonly TypeDefinition iViewModelImplementerType;
        private readonly TypeDefinition internalPropertyBagType;
        private readonly TypeDefinition internalModelHelperType;

        private readonly TypeReference propertyChangingEventHandlerType;
        private readonly TypeReference propertyChangedEventHandlerType;

        private readonly MethodDefinition addPropertyChanging;
        private readonly MethodDefinition removePropertyChanging;
        private readonly MethodDefinition addPropertyChanged;
        private readonly MethodDefinition removePropertyChanged;

        private readonly MethodDefinition getValueTMethod;
        private readonly MethodDefinition setValueAsyncTMethod;

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
            this.ignoreInjectAttributeType = epoxyCoreAssembly.MainModule.GetType(
                "Epoxy.IgnoreInjectAttribute")!;
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

            this.getValueTMethod = this.internalModelHelperType.Methods.
                First(m => m.Name == "GetValueT");
            this.setValueAsyncTMethod = this.internalModelHelperType.Methods.
                First(m => m.Name == "SetValueAsyncT");

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

        private void InjectPropertyChangeEvents(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField)
        {
            MethodDefinition CreatePropertyChangeEventTransfer(
                string name,
                TypeReference handlerType,
                MethodDefinition targetMethod)
            {
                var method = new MethodDefinition(
                    name, MethodAttributes.Public | MethodAttributes.Virtual, this.typeSystem.Void);

                //public static void AddPropertyChanging(
                //    PropertyChangingEventHandler? handler,
                //    ref InternalPropertyBag? properties)

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
                "epoxy_add_PropertyChanging__",
                this.propertyChangingEventHandlerType,
                this.addPropertyChanging);
            propertyChangingAddMethod.Overrides.Add(
                module.ImportReference(this.itAddPropertyChanging));

            var propertyChangingRemoveMethod = CreatePropertyChangeEventTransfer(
                "epoxy_remove_PropertyChanging__",
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
                "epoxy_add_PropertyChanged__",
                this.propertyChangedEventHandlerType,
                this.addPropertyChanged);
            propertyChangedAddMethod.Overrides.Add(
                module.ImportReference(this.itAddPropertyChanged));

            var propertyChangedRemoveMethod = CreatePropertyChangeEventTransfer(
                "epoxy_remove_PropertyChanged__",
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

        private bool InjectGetterProperty(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField,
            PropertyDefinition pd, MethodDefinition getter, MethodDefinition setter,
            Dictionary<FieldDefinition, MethodDefinition> candidateFields)
        {
            var ilp = getter.Body.GetILProcessor();

            if (getter.IsAbstract)
            {
                getter.IsAbstract = false;
                getter.IsVirtual = true;
            }
            else
            {
                var backingStoreFields = ilp.Body.Instructions.
                    Where(inst =>
                        (inst.OpCode == OpCodes.Ldfld) &&
                        inst.Operand is FieldReference fr &&
                        fr.Resolve() is { } fd &&
                        !fd.IsStatic && !fd.IsInitOnly &&
                        (fd.FieldType.FullName == pd.PropertyType.FullName) &&
                        fd.DeclaringType.FullName == targetType.FullName).
                    Select(inst => (FieldReference)inst.Operand).
                    ToArray();
                if (backingStoreFields.Length != 1)
                {
                    return false;
                }

                var backingStoreField = module.ImportReference(backingStoreFields[0]).Resolve();
                candidateFields[backingStoreField] = setter;
            }

            var body = ilp.Body;
            body.Instructions.Clear();
            body.Variables.Clear();

            var propertyType = module.ImportReference(pd.PropertyType);

            var defaultValueVariable = new VariableDefinition(propertyType);
            body.Variables.Add(defaultValueVariable);

            //public static TValue GetValueT<TValue>(
            //    TValue defaultValue,
            //    string? propertyName,
            //    ref InternalPropertyBag? properties)

            ilp.Append(Instruction.Create(OpCodes.Ldloc, defaultValueVariable));
            ilp.Append(Instruction.Create(OpCodes.Ldstr, pd.Name));
            ilp.Append(Instruction.Create(OpCodes.Ldarg_0));
            ilp.Append(Instruction.Create(OpCodes.Ldflda, propertiesField));
            var getValueTMethod = new GenericInstanceMethod(
                module.ImportReference(this.getValueTMethod));
            getValueTMethod.GenericArguments.Add(propertyType);
            ilp.Append(Instruction.Create(OpCodes.Call, getValueTMethod));
            ilp.Append(Instruction.Create(OpCodes.Ret));

            return true;
        }

        private bool InjectSetterProperty(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField,
            PropertyDefinition pd, MethodDefinition setter, 
            Dictionary<FieldDefinition, MethodDefinition> candidateFields)
        {
            var ilp = setter.Body.GetILProcessor();

            if (setter.IsAbstract)
            {
                setter.IsAbstract = false;
                setter.IsVirtual = true;
            }
            else
            {
                var backingStoreFields = ilp.Body.Instructions.
                    Where(inst =>
                        (inst.OpCode == OpCodes.Stfld) &&
                        inst.Operand is FieldReference fr &&
                        fr.Resolve() is { } fd &&
                        !fd.IsStatic && !fd.IsInitOnly &&
                        (fd.FieldType.FullName == pd.PropertyType.FullName) &&
                        fd.DeclaringType.FullName == targetType.FullName).
                    Select(inst => (FieldReference)inst.Operand).
                    ToArray();
                if (backingStoreFields.Length != 1)
                {
                    return false;
                }

                var backingStoreField = module.ImportReference(backingStoreFields[0]).Resolve();
                candidateFields[backingStoreField] = setter;
            }

            var body = ilp.Body;
            body.Instructions.Clear();
            body.Variables.Clear();

            var propertyType = module.ImportReference(pd.PropertyType);

            //public static ValueTask<Unit> SetValueAsyncT<TValue>(
            //    TValue newValue,
            //    Func<TValue, ValueTask<Unit>>? propertyChanged,
            //    string? propertyName,
            //    object sender,
            //    ref InternalPropertyBag? properties)

            ilp.Append(Instruction.Create(OpCodes.Ldarg_1));
            ilp.Append(Instruction.Create(OpCodes.Ldnull));
            ilp.Append(Instruction.Create(OpCodes.Ldstr, pd.Name));
            ilp.Append(Instruction.Create(OpCodes.Ldarg_0));
            ilp.Append(Instruction.Create(OpCodes.Dup));
            ilp.Append(Instruction.Create(OpCodes.Ldflda, propertiesField));
            var setValueAsyncTMethod = new GenericInstanceMethod(
                module.ImportReference(this.setValueAsyncTMethod));
            setValueAsyncTMethod.GenericArguments.Add(propertyType);
            ilp.Append(Instruction.Create(OpCodes.Call, setValueAsyncTMethod));
            ilp.Append(Instruction.Create(OpCodes.Pop));
            ilp.Append(Instruction.Create(OpCodes.Ret));

            return true;
        }

        private void RemoveBackingFields(
            ModuleDefinition module, TypeDefinition targetType,
            Dictionary<FieldDefinition, MethodDefinition> fields)
        {
            foreach (var ctor in targetType.Methods.
                Where(m => m.IsConstructor))
            {
                var ilp = ctor.Body.GetILProcessor();
                var index = 0;
                var body = ilp.Body;
                while (index < body.Instructions.Count)
                {
                    var inst = body.Instructions[index];

                    if ((inst.OpCode == OpCodes.Stfld) &&
                        inst.Operand is FieldReference fr &&
                        fr.Resolve() is { } fd &&
                        !fd.IsStatic && !fd.IsInitOnly &&
                        fields.TryGetValue(fd, out var setter) &&
                        fd.DeclaringType.FullName == targetType.FullName)
                    {
                        inst = Instruction.Create(OpCodes.Call, setter);
                        body.Instructions[index] = inst;
                    }

                    index++;
                }
            }

            foreach (var candidateField in fields.Keys)
            {
                targetType.Fields.Remove(candidateField);
            }
        }

        private void InjectProperties(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField)
        {
            var candidateFields = new Dictionary<FieldDefinition, MethodDefinition>();

            foreach (var pd in targetType.Properties.
                Where(pd => !pd.CustomAttributes.
                    Any(ca => ca.AttributeType.FullName == this.ignoreInjectAttributeType.FullName)))
            {
                if (pd.GetMethod is { } getter && !getter.IsStatic &&
                    pd.SetMethod is { } setter && !setter.IsStatic)
                {
                    this.InjectGetterProperty(
                        module, targetType, propertiesField, pd, getter, setter, candidateFields);
                    this.InjectSetterProperty(
                        module, targetType, propertiesField, pd, setter, candidateFields);
                }
            }

            this.RemoveBackingFields(module, targetType, candidateFields);
        }

        private bool InjectIntoType(ModuleDefinition module, TypeDefinition targetType)
        {
            var propertiesField = new FieldDefinition(
                "epoxy_properties__",
                FieldAttributes.Private,
                module.ImportReference(this.internalPropertyBagType));
            targetType.Fields.Add(propertiesField);

            var ii = new InterfaceImplementation(
                module.ImportReference(this.iViewModelImplementerType));
            targetType.Interfaces.Add(ii);

            this.InjectPropertyChangeEvents(module, targetType, propertiesField);
            this.InjectProperties(module, targetType, propertiesField);

            return true;
        }

        public bool Inject(string targetAssemblyPath, string injectedAssemblyPath)
        {
            this.assemblyResolver.AddSearchDirectory(
                Path.GetDirectoryName(targetAssemblyPath));

            var targetAssemblyName = Path.GetFileNameWithoutExtension(
                targetAssemblyPath);

            using (var targetAssembly = AssemblyDefinition.ReadAssembly(
                targetAssemblyPath,
                new ReaderParameters(ReadingMode.Immediate)
                {
                    ReadSymbols = true,
                    ReadWrite = false,
                    InMemory = true,
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
                    var injected = false;

                    foreach (var targetType in targetTypes)
                    {
                        if (this.InjectIntoType(targetAssembly.MainModule, targetType))
                        {
                            injected = true;
                            this.message($"Epoxy.Build: Injected a view model: Assembly={targetAssemblyName}, Type={targetType.FullName}");
                        }
                    }

                    if (injected)
                    {
                        if (File.Exists(injectedAssemblyPath))
                        {
                            File.Delete(injectedAssemblyPath);
                        }

                        var injectedName = Path.GetFileNameWithoutExtension(injectedAssemblyPath);
                        targetAssembly.Name = new AssemblyNameDefinition(
                            injectedName, targetAssembly.Name.Version);

                        targetAssembly.Write(
                            injectedAssemblyPath,
                            new WriterParameters
                            {
                                WriteSymbols = true,
                                DeterministicMvid = true,
                            });
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
