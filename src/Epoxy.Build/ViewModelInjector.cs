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

        public ViewModelInjector(string[] referencesBasePath, Action<string> message)
        {
            this.message = message;

            foreach (var referenceBasePath in referencesBasePath)
            {
                this.assemblyResolver.AddSearchDirectory(referenceBasePath);
            }

            var epoxyCorePath = referencesBasePath.
                First(basePath => File.Exists(Path.Combine(basePath, "Epoxy.Core.dll")));

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

        private static FieldDefinition? GetBackingStore(
            TypeDefinition targetType, Instruction inst, Func<OpCode, bool> targetOpCode) =>
            (targetOpCode(inst.OpCode) &&
             inst.Operand is FieldReference fr &&
             fr.Resolve() is { } fd &&
             !fd.IsStatic && !fd.IsInitOnly &&
             (fd.DeclaringType.FullName == targetType.FullName)) ?
                fd : null;

        private FieldReference[] GetBackingStoreCandidates(
            TypeDefinition targetType, PropertyDefinition pd, MethodDefinition method, OpCode targetOpCode)
        {
            var ilp = method.Body.GetILProcessor();

            var backingStoreFields = ilp.Body.Instructions.
                Where(inst =>
                    GetBackingStore(targetType, inst, opcode => opcode == targetOpCode) is { } fd &&
                    (fd.FieldType.FullName == pd.PropertyType.FullName)).
                Select(inst => (FieldReference)inst.Operand).
                ToArray();

            return backingStoreFields;
        }

        private bool InjectGetterProperty(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField,
            PropertyDefinition pd, MethodDefinition getter)
        {
            var ilp = getter.Body.GetILProcessor();

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
            PropertyDefinition pd, MethodDefinition setter)
        {
            var ilp = setter.Body.GetILProcessor();

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
            TypeDefinition targetType,
            Dictionary<FieldDefinition, (MethodDefinition getter, MethodDefinition setter)> fields)
        {
            if (fields.Count >= 1)
            {
                foreach (var method in targetType.Methods.
                    Where(m => !m.IsStatic && !m.IsAbstract))
                {
                    var ilp = method.Body.GetILProcessor();
                    var index = 0;
                    var body = ilp.Body;
                    while (index < body.Instructions.Count)
                    {
                        var inst = body.Instructions[index];

                        if (GetBackingStore(targetType, inst,
                            opcode => (opcode == OpCodes.Ldfld) || (opcode == OpCodes.Stfld)) is { } fd &&
                            fields.TryGetValue(fd, out var methods))
                        {
                            if (inst.OpCode == OpCodes.Ldfld)
                            {
                                inst = Instruction.Create(OpCodes.Call, methods.getter);
                            }
                            else
                            {
                                inst = Instruction.Create(OpCodes.Call, methods.setter);
                            }
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
        }

        private void InjectProperties(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField)
        {
            var candidateFields = new Dictionary<FieldDefinition, (MethodDefinition getter, MethodDefinition setter)>();

            foreach (var pd in targetType.Properties.
                Where(pd => !pd.CustomAttributes.
                    Any(ca => ca.AttributeType.FullName == this.ignoreInjectAttributeType.FullName)))
            {
                if (pd.GetMethod is { } getter && !getter.IsStatic && !getter.IsAbstract &&
                    pd.SetMethod is { } setter && !setter.IsStatic && !setter.IsAbstract)
                {
                    var getterBackingStoreCandidates = this.GetBackingStoreCandidates(
                        targetType, pd, getter, OpCodes.Ldfld);
                    var setterBackingStoreCandidates = this.GetBackingStoreCandidates(
                        targetType, pd, setter, OpCodes.Stfld);

                    if ((getterBackingStoreCandidates.Length == 1) &&
                        getterBackingStoreCandidates.SequenceEqual(setterBackingStoreCandidates))
                    {
                        this.InjectGetterProperty(
                            module, targetType, propertiesField, pd, getter);
                        this.InjectSetterProperty(
                            module, targetType, propertiesField, pd, setter);

                        var backingStoreField = module.ImportReference(getterBackingStoreCandidates[0]).Resolve();
                        candidateFields[backingStoreField] = (getter, setter);
                    }
                }
            }

            this.RemoveBackingFields(targetType, candidateFields);
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
