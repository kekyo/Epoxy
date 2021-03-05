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

namespace Epoxy.Synchronized

open System
open System.ComponentModel
open System.Diagnostics
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Threading.Tasks

open Epoxy
open Epoxy.Internal

[<DebuggerStepThrough>]
[<AutoOpen>]
module public SyncViewModelExtension =
    type public ViewModel with
        member viewModel.setValueSync (newValue, propertyChanged: 'TValue -> unit, [<Optional; CallerMemberName>] propertyName) =
            viewModel.InternalSetValueAsync<'TValue>(newValue, propertyChanged >> unitAsValueTaskUnit |> asFunc1, propertyName)

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member viewModel.setValueSync (newValue, propertyChanged: 'TValue -> ValueTask<unit>, [<Optional; CallerMemberName>] propertyName) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member viewModel.setValueSync (newValue, propertyChanged: 'TValue -> ValueTask, [<Optional; CallerMemberName>] propertyName) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member viewModel.setValueSync (newValue, propertyChanged: 'TValue -> Task<unit>, [<Optional; CallerMemberName>] propertyName) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member viewModel.setValueSync (newValue, propertyChanged: 'TValue -> Task, [<Optional; CallerMemberName>] propertyName) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member viewModel.setValueSync (newValue, propertyChanged: 'TValue -> Async<unit>, [<Optional; CallerMemberName>] propertyName) =
            raise(InvalidOperationException("Use setValueAsync instead."))
