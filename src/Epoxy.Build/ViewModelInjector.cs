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
    public enum LogLevels
    {
        Trace,
        Information,
        Warning,
        Error
    }

    public sealed class ViewModelInjector
    {
        private readonly Action<LogLevels, string> message;
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
        private readonly MethodDefinition prettyPrint;

        private readonly MethodDefinition initializeFSharpValueTMethod;
        private readonly MethodDefinition getValueTMethod;
        private readonly MethodDefinition setValueAsyncTMethod;

        private readonly MethodDefinition itAddPropertyChanging;
        private readonly MethodDefinition itRemovePropertyChanging;
        private readonly MethodDefinition itAddPropertyChanged;
        private readonly MethodDefinition itRemovePropertyChanged;

        public ViewModelInjector(string[] referencesBasePath, Action<LogLevels, string> message)
        {
            this.message = message;

            foreach (var referenceBasePath in referencesBasePath)
            {
                this.assemblyResolver.AddSearchDirectory(referenceBasePath);
            }

            var epoxyCorePath = referencesBasePath.
                Select(basePath => Path.Combine(basePath, "Epoxy.Core.dll")).
                First(File.Exists);

            var epoxyCoreAssembly = AssemblyDefinition.ReadAssembly(
                epoxyCorePath,
                new ReaderParameters
                {
                    AssemblyResolver = assemblyResolver,
                }
            );

            message(
                LogLevels.Trace,
                $"Epoxy.Core.dll is loaded: Path={epoxyCorePath}");

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
            this.prettyPrint = this.internalModelHelperType.Methods.
                First(m => m.Name == "PrettyPrint");

            this.initializeFSharpValueTMethod = this.internalModelHelperType.Methods.
                First(m => m.Name == "InitializeFSharpValueT");
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

        private void InjectPrettyPrint(
            ModuleDefinition module, TypeDefinition targetType)
        {
            if (!targetType.Methods.
                Any(m => (m.Name == "ToString") && m.IsPublic && m.IsVirtual && (m.Parameters.Count == 0)))
            {
                message(
                    LogLevels.Trace,
                    $"InjectPrettyPrint: Injected pretty printer: Type={targetType.FullName}");

                var method = new MethodDefinition(
                    "ToString", MethodAttributes.Public | MethodAttributes.Virtual, this.typeSystem.String);

                // public static string PrettyPrint(object self, bool includeFields)

                var ilp = method.Body.GetILProcessor();
                ilp.Append(Instruction.Create(OpCodes.Ldarg_0));
                ilp.Append(Instruction.Create(OpCodes.Ldc_I4_0));
                ilp.Append(Instruction.Create(OpCodes.Call, module.ImportReference(this.prettyPrint)));
                ilp.Append(Instruction.Create(OpCodes.Ret));

                targetType.Methods.Add(method);
            }
            else
            {
                message(
                    LogLevels.Trace,
                    $"InjectPrettyPrint: Ignored injecting pretty printer: Type={targetType.FullName}");
            }
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

        private struct AccessorInformation
        {
            public readonly PropertyDefinition Property;
            public readonly MethodDefinition Getter;
            public readonly MethodDefinition Setter;

            public AccessorInformation(
                PropertyDefinition property, MethodDefinition getter, MethodDefinition setter)
            {
                this.Property = property;
                this.Getter = getter;
                this.Setter = setter;
            }
        }

        private void RemoveBackingFields(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField,
            Dictionary<FieldDefinition, AccessorInformation> accessors)
        {
            if (accessors.Count >= 1)
            {
                var isFSharp = targetType.CustomAttributes.
                    Any(ca => ca.AttributeType.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute");

                foreach (var method in targetType.Methods.
                    Where(m => !m.IsStatic && !m.IsAbstract))
                {
                    message(
                        LogLevels.Trace,
                        $"RemoveBackingFields: Checking field usage: Method={method.FullName}");

                    var ilp = method.Body.GetILProcessor();
                    var index = 0;
                    var body = ilp.Body;
                    while (index < body.Instructions.Count)
                    {
                        var inst = body.Instructions[index];

                        if (GetBackingStore(targetType, inst,
                            opcode => (opcode == OpCodes.Ldfld) || (opcode == OpCodes.Stfld)) is { } fd &&
                            accessors.TryGetValue(fd, out var accessor))
                        {
                            if (inst.OpCode == OpCodes.Ldfld)
                            {
                                message(
                                    LogLevels.Trace,
                                    $"RemoveBackingFields: Found and replaced by getter: Field={fd.FullName}, OpCode={inst.OpCode}, Index={index}");

                                body.Instructions[index] = Instruction.Create(OpCodes.Call, accessor.Getter);
                            }
                            // HACK: F#'s constructor contains backing-field from auto implemented property,
                            //   but it will initialize at last sequence of constructor body.
                            //   These hack replaces to specialized initialize method 'InitializeFSharpValueT<TValue>(...)'.
                            //   The method will ignore if non-default value already assigned.
                            else if (isFSharp && method.IsConstructor)
                            {
                                message(
                                    LogLevels.Trace,
                                    $"RemoveBackingFields: Found and replaced by F# initializer: Field={fd.FullName}, OpCode={inst.OpCode}, Index={index}");

                                var initializeValueTMethod = new GenericInstanceMethod(
                                    module.ImportReference(this.initializeFSharpValueTMethod));
                                initializeValueTMethod.GenericArguments.Add(
                                    module.ImportReference(fd.FieldType));

                                body.Instructions[index++] = Instruction.Create(OpCodes.Ldstr, accessor.Property.Name);
                                body.Instructions.Insert(index++, Instruction.Create(OpCodes.Ldarg_0));
                                body.Instructions.Insert(index++, Instruction.Create(OpCodes.Ldflda, propertiesField));
                                body.Instructions.Insert(index++, Instruction.Create(OpCodes.Call, initializeValueTMethod));
                                body.Instructions.Insert(index++, Instruction.Create(OpCodes.Pop));
                            }
                            else
                            {
                                message(
                                    LogLevels.Trace,
                                    $"RemoveBackingFields: Found and replaced by setter: Field={fd.FullName}, OpCode={inst.OpCode}, Index={index}");

                                body.Instructions[index] = Instruction.Create(OpCodes.Call, accessor.Setter);
                            }
                        }

                        index++;
                    }
                }

                foreach (var candidateField in accessors.Keys)
                {
                    targetType.Fields.Remove(candidateField);
                }
            }
            else
            {
                message(
                    LogLevels.Trace,
                    $"RemoveBackingFields: Target field isn't found.");
            }
        }

        private void InjectProperties(
            ModuleDefinition module, TypeDefinition targetType, FieldDefinition propertiesField)
        {
            var candidateAccessors = new Dictionary<FieldDefinition, AccessorInformation>();

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

                        message(
                            LogLevels.Trace,
                            $"InjectProperties: Injected property: Property={pd.FullName}");

                        var backingStoreField = module.ImportReference(getterBackingStoreCandidates[0]).Resolve();
                        candidateAccessors[backingStoreField] = new AccessorInformation(
                            pd, getter, setter);
                    }
                    else
                    {
                        message(
                            LogLevels.Trace,
                            $"InjectProperties: Ignored property [2]: Property={pd.FullName}");
                    }
                }
                else
                {
                    message(
                        LogLevels.Trace,
                        $"InjectProperties: Ignored property [1]: Property={pd.FullName}");
                }
            }

            this.RemoveBackingFields(module, targetType, propertiesField, candidateAccessors);
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
            this.InjectPrettyPrint(module, targetType);
            this.InjectProperties(module, targetType, propertiesField);

            return true;
        }

        public bool Inject(string targetAssemblyPath, string? injectedAssemblyPath = null)
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
                            this.message(
                                LogLevels.Trace,
                                $"Injected a view model: Assembly={targetAssemblyName}, Type={targetType.FullName}");
                        }
                        else
                        {
                            message(
                                LogLevels.Trace,
                                $"InjectProperties: Ignored a type: Assembly={targetAssemblyName}, Type={targetType.FullName}");
                        }
                    }

                    if (injected)
                    {
                        injectedAssemblyPath = injectedAssemblyPath ?? targetAssemblyPath;

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
