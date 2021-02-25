﻿////////////////////////////////////////////////////////////////////////////
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

open System;
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Threading.Tasks

[<AbstractClass>]
type public ViewModel =
    inherit ViewModelBase

    member self.getValue<'TValue> ([<Optional; CallerMemberName>] propertyName) =
        self.InternalGetValue<'TValue>(Unchecked.defaultof<_>, propertyName)
    member self.getValue<'TValue> (defaultValue, [<Optional; CallerMemberName>] propertyName) =
        self.InternalGetValue<'TValue>(defaultValue, propertyName)

    member self.setValueAsync<'TValue>(newValue, propertyChanged: 'TValue -> ValueTask, [<Optional; CallerMemberName>] propertyName) =
        self.InternalSetValueAsync<'TValue>(newValue, new Func<'TValue, ValueTask>(propertyChanged), propertyName)
    member self.setValue<'TValue>(newValue, [<Optional; CallerMemberName>] propertyName) =
        self.InternalSetValue<'TValue>(newValue, propertyName)

    member self.onPropertyChanging([<Optional; CallerMemberName>] propertyName) =
        self.InternalOnPropertyChanging(propertyName)
    member self.onPropertyChanged([<Optional; CallerMemberName>] propertyName) =
        self.InternalOnPropertyChanged(propertyName)