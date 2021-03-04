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

namespace Epoxy.Supplemental

open System
open System.Diagnostics
open System.Runtime.InteropServices
open System.Threading.Tasks

open Epoxy.Advanced
open Epoxy.Internal

[<DebuggerStepThrough>]
[<AutoOpen>]
module public GlobalServiceAccessorExtension =
    type public GlobalServiceAccessor with
        member __.executeAsync(action: 'TService -> ValueTask<unit>, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
            InternalGlobalService.ExecuteAsync<'TService>(action >> valueTaskUnitAsValueTaskVoid |> asFunc1, ignoreNotPresent)
        member __.executeAsync(action: 'TService -> ValueTask<'TResult>) =
            InternalGlobalService.ExecuteAsync<'TService, 'TResult>(action |> asFunc1)

        member __.executeAsync(action: 'TService -> Task<unit>, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
            InternalGlobalService.ExecuteAsync<'TService>(action >> taskUnitAsValueTaskVoid |> asFunc1, ignoreNotPresent)
        member __.executeAsync(action: 'TService -> Task<'TResult>) =
            InternalGlobalService.ExecuteAsync<'TService, 'TResult>(action >> taskAsValueTask |> asFunc1)

        member __.executeAsync(action: 'TService -> ValueTask, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
            InternalGlobalService.ExecuteAsync<'TService>(action |> asFunc1, ignoreNotPresent)
        member __.executeAsync(action: 'TService -> Task, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
            InternalGlobalService.ExecuteAsync<'TService>(action >> taskVoidAsValueTaskVoid |> asFunc1, ignoreNotPresent)
