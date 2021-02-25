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
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Threading.Tasks

open Epoxy
open Epoxy.Internal

[<DebuggerStepThrough>]
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module public ViewModelExtension =
    let fromValueTaskUnit (propertyChanged:'TValue -> ValueTask<unit>) =
        new Func<'TValue, ValueTask>(fun value -> InternalHelpers.FromTask((propertyChanged value).AsTask() :> Task))
    let fromTaskVoid (propertyChanged:'TValue -> Task) =
        new Func<'TValue, ValueTask>(fun value -> InternalHelpers.FromTask(propertyChanged value))
    let fromTask (propertyChanged:'TValue -> Task<unit>) =
        new Func<'TValue, ValueTask>(fun value -> InternalHelpers.FromTask(propertyChanged value :> Task))
    let fromAsync (propertyChanged:'TValue -> Async<unit>) =
        new Func<'TValue, ValueTask>(fun value -> InternalHelpers.FromTask(Async.StartImmediateAsTask(propertyChanged value) :> Task))

    type public ViewModel with
        member viewModel.setValueAsync<'TValue> (newValue, propertyChanged: 'TValue -> ValueTask<unit>, [<Optional; CallerMemberName>] propertyName) =
            viewModel.InternalSetValueAsync<'TValue>(newValue, fromValueTaskUnit propertyChanged, propertyName)
        member viewModel.setValueAsync<'TValue> (newValue, propertyChanged: 'TValue -> Task, [<Optional; CallerMemberName>] propertyName) =
            viewModel.InternalSetValueAsync<'TValue>(newValue, fromTaskVoid propertyChanged, propertyName)
        member viewModel.setValueAsync<'TValue> (newValue, propertyChanged: 'TValue -> Task<unit>, [<Optional; CallerMemberName>] propertyName) =
            viewModel.InternalSetValueAsync<'TValue>(newValue, fromTask propertyChanged, propertyName)
        member viewModel.setValueAsync<'TValue> (newValue, propertyChanged: 'TValue -> Async<unit>, [<Optional; CallerMemberName>] propertyName) =
            viewModel.InternalSetValueAsync<'TValue>(newValue, fromAsync propertyChanged, propertyName)
