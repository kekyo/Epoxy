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

open Epoxy.Internal
open Epoxy.Supplemental

open System
open System.ComponentModel
open System.Diagnostics
open System.Threading.Tasks

[<DebuggerStepThrough>]
[<AutoOpen>]
module private ValueConverterFactoryGenerator =
    let inline create0 convert = new DelegatedValueConverter<'TFrom, 'TTo>(convert) :> ValueConverter
    let inline createP0 convert = new DelegatedValueConverter<'TFrom, 'TParameter, 'TTo>(convert) :> ValueConverter
    let inline create1 convert convertBack = new DelegatedValueConverter<'TFrom, 'TTo>(convert, convertBack) :> ValueConverter
    let inline createP1 convert convertBack = new DelegatedValueConverter<'TFrom, 'TParameter, 'TTo>(convert, convertBack) :> ValueConverter

[<AbstractClass; Sealed>]
type public ValueConverterFactory =
    static member create (convert: 'TFrom -> 'TTo) =
        create0 (convert |> asFunc1)
    static member create (convert: 'TFrom -> 'TTo, convertBack) =
        create1 (convert |> asFunc1) (convertBack |> asFunc1)
    static member create (convert: 'TFrom -> 'TParameter -> 'TTo) =
        createP0 (convert |> asFunc2)
    static member create (convert: 'TFrom -> 'TParameter -> 'TTo, convertBack) =
        createP1 (convert |> asFunc2) (convertBack |> asFunc2)

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> ValueTask<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> ValueTask<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> ValueTask<'TTo>, convertBack: 'TTo -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> ValueTask<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> 'TTo, convertBack: 'TTo -> 'TParameter -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> ValueTask<'TTo>, convertBack: 'TTo -> 'TParameter -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> ValueTask<'TTo>, convertBack: 'TTo -> 'TParameter -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> Task<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> Task<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> Task<'TTo>, convertBack: 'TTo -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> Task<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> 'TTo, convertBack: 'TTo -> 'TParameter -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> Task<'TTo>, convertBack: 'TTo -> 'TParameter -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> Task<'TTo>, convertBack: 'TTo -> 'TParameter -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> Async<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> Async<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> Async<'TTo>, convertBack: 'TTo -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> Async<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> 'TTo, convertBack: 'TTo -> 'TParameter -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> Async<'TTo>, convertBack: 'TTo -> 'TParameter -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create (convert: 'TFrom -> 'TParameter -> Async<'TTo>, convertBack: 'TTo -> 'TParameter -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
