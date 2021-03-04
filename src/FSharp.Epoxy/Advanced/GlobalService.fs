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

namespace Epoxy.Advanced

open System.Diagnostics
open System.Runtime.InteropServices

open Epoxy.Advanced
open Epoxy.Internal

[<DebuggerStepThrough>]
[<AbstractClass>]
[<Sealed>]
type public GlobalService =
    static member GlobalServiceAccessor accessor =
        new GlobalServiceAccessor()

    static member register(instance: obj, [<Optional; DefaultParameterValue(RegisteringValidations.Strict)>] validation: RegisteringValidations) =
        InternalGlobalService.Register(instance, validation)

    static member unRegister(instance: obj) =
        InternalGlobalService.UnRegister(instance)

    static member executeAsync(action: 'TService -> Async<unit>, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
        InternalGlobalService.ExecuteAsync<'TService>(action >> asyncUnitAsValueTaskVoid |> asFunc1, ignoreNotPresent)
        |> valueTaskVoidAsAsyncResult
    static member executeAsync(action: 'TService -> Async<'TResult>) =
        InternalGlobalService.ExecuteAsync<'TService, 'TResult>(action >> asyncAsValueTask |> asFunc1)

[<DebuggerStepThrough>]
[<AutoOpen>]
module public GlobalServiceAccessorExtension =
    type GlobalServiceAccessor with
        member __.executeAsync(action: 'TService -> Async<unit>, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
            InternalGlobalService.ExecuteAsync<'TService>(action >> asyncUnitAsValueTaskVoid |> asFunc1, ignoreNotPresent)

        member __.executeAsync(action: 'TService -> Async<'TResult>) =
            InternalGlobalService.ExecuteAsync<'TService, 'TResult>(action >> asyncAsValueTask |> asFunc1)
