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

namespace Epoxy.Internal

open Epoxy.Internal

open System
open System.Diagnostics
open System.Reflection
open System.ComponentModel
open System.Threading.Tasks

[<EditorBrowsable(EditorBrowsableState.Never)>]
type PropertyChangedFSharpAsyncDelegate<'TValue> = delegate of 'TValue -> Async<unit>

[<EditorBrowsable(EditorBrowsableState.Never)>]
[<DebuggerStepThrough>]
[<AbstractClass>]
[<Sealed>]
type public InternalFSharpModelHelper =
    
    // Injector helper: create delegate
    // Dodged inline generation: https://github.com/jbevain/cecil/discussions/737
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    static member createPropertyChangedAsyncDelegate<'TValue>
        (target: obj, method: RuntimeMethodHandle) =
        Delegate.CreateDelegate(
            typeof<PropertyChangedFSharpAsyncDelegate<'TValue>>,
            target,
            MethodBase.GetMethodFromHandle(method) :?> MethodInfo) :?> PropertyChangedFSharpAsyncDelegate<'TValue>

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    static member setValueWithHookAsyncT
        (newValue, propertyChanged: PropertyChangedFSharpAsyncDelegate<'TValue>, propertyName, sender, properties: InternalPropertyBag ref) =
        InternalModelHelper.SetValueAsyncT<'TValue>(
            newValue,
            new Func<'TValue, ValueTask<Epoxy.Internal.Unit>>(
                fun v -> propertyChanged.Invoke(v) |> asyncUnitAsValueTaskUnit),
            propertyName,
            sender,
            properties)
