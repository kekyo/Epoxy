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

namespace Epoxy

open Epoxy.Infrastructure
open Epoxy.Supplemental

open System
open System.Diagnostics
open System.Threading.Tasks

[<AbstractClass; Sealed>]
type public CommandFactory =
    static member create executeAsync =
        new DelegatedCommand(new Func<ValueTask>(executeAsync))
    static member create (executeAsync, canExecute) =
        new DelegatedCommand(new Func<ValueTask>(executeAsync), new Func<bool>(canExecute))
    static member create<'TParameter> executeAsync =
        new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(executeAsync))
    static member create<'TParameter> (executeAsync, canExecute) =
        new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(executeAsync), new Func<'TParameter, bool>(canExecute))

[<DebuggerStepThrough>]
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module public CommandFactoryExtension =
    type public CommandFactoryInstance with
        member __.create executeAsync =
            new DelegatedCommand(new Func<ValueTask>(executeAsync))
        member __.create (executeAsync, canExecute) =
            new DelegatedCommand(new Func<ValueTask>(executeAsync), new Func<bool>(canExecute))
        member __.create<'TParameter> executeAsync =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(executeAsync))
        member __.create<'TParameter> (executeAsync, canExecute) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(executeAsync), new Func<'TParameter, bool>(canExecute))
