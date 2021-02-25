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

open Epoxy.Supplemental

open System
open System.ComponentModel
open System.Threading.Tasks

[<AbstractClass; Sealed>]
type public ValueConverterFactory =
    static member create<'TTo, 'TFrom> convert =
        new DelegatedValueConverter<'TTo, 'TFrom>(new Func<'TFrom, 'TTo>(convert)) :> ValueConverter
    static member create<'TTo, 'TFrom> (convert, convertBack) =
        new DelegatedValueConverter<'TTo, 'TFrom>(new Func<'TFrom, 'TTo>(convert), new Func<'TTo, 'TFrom>(convertBack)) :> ValueConverter
    static member create<'TTo, 'TFrom, 'TParameter> convert =
        new DelegatedValueConverter<'TTo, 'TFrom, 'TParameter>(new Func<'TFrom, 'TParameter, 'TTo>(convert)) :> ValueConverter
    static member create<'TTo, 'TFrom, 'TParameter> (convert, convertBack) =
        new DelegatedValueConverter<'TTo, 'TFrom, 'TParameter>(new Func<'TFrom, 'TParameter, 'TTo>(convert), new Func<'TTo, 'TParameter, 'TFrom>(convertBack)) :> ValueConverter

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> ValueTask<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> ValueTask<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> ValueTask<'TTo>, convertBack: 'TTo -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> ValueTask<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> ValueTask<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> ValueTask<'TTo>, convertBack: 'TTo -> ValueTask<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> Task<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> Task<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> Task<'TTo>, convertBack: 'TTo -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> Task<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> Task<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> Task<'TTo>, convertBack: 'TTo -> Task<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> Async<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> Async<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom> (convert: 'TFrom -> Async<'TTo>, convertBack: 'TTo -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> Async<'TTo>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> 'TTo, convertBack: 'TTo -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> Async<'TTo>, convertBack: 'TTo -> 'TFrom) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    [<Obsolete("Avoid asynchronous XAML conversion.", true)>]
    static member create<'TTo, 'TFrom, 'TParameter> (convert: 'TFrom -> Async<'TTo>, convertBack: 'TTo -> Async<'TFrom>) : ValueConverter =
        raise(InvalidOperationException("Avoid asynchronous XAML conversion."))
