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

using Epoxy.Supplemental;

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Epoxy
{
    public static class ValueConverterFactory
    {
        public static ValueConverter Create<TTo, TFrom>(Func<TFrom, TTo> convert) =>
            new DelegatedValueConverter<TTo, TFrom>(convert);

        public static ValueConverter Create<TTo, TFrom>(Func<TFrom, TTo> convert, Func<TTo, TFrom> convertBack) =>
            new DelegatedValueConverter<TTo, TFrom>(convert, convertBack);

        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, TTo> convert) =>
            new DelegatedValueConverter<TTo, TFrom, TParameter>(convert);

        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, TTo> convert, Func<TTo, TParameter, TFrom> convertBack) =>
            new DelegatedValueConverter<TTo, TFrom, TParameter>(convert, convertBack);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Avoid asynchronous XAML conversion.", true)]
        public static ValueConverter Create<TTo, TFrom>(Func<TFrom, ValueTask<TTo>> convert) =>
            throw new InvalidOperationException("Avoid asynchronous XAML conversion.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Avoid asynchronous XAML conversion.", true)]
        public static ValueConverter Create<TTo, TFrom>(Func<TFrom, ValueTask<TTo>> convert, Func<TTo, TFrom> convertBack) =>
            throw new InvalidOperationException("Avoid asynchronous XAML conversion.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Avoid asynchronous XAML conversion.", true)]
        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, ValueTask<TTo>> convert) =>
            throw new InvalidOperationException("Avoid asynchronous XAML conversion.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Avoid asynchronous XAML conversion.", true)]
        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, ValueTask<TTo>> convert, Func<TTo, TParameter, TFrom> convertBack) =>
            throw new InvalidOperationException("Avoid asynchronous XAML conversion.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Avoid asynchronous XAML conversion.", true)]
        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, TTo> convert, Func<TTo, TParameter, ValueTask<TFrom>> convertBack) =>
            throw new InvalidOperationException("Avoid asynchronous XAML conversion.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Avoid asynchronous XAML conversion.", true)]
        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, ValueTask<TTo>> convert, Func<TTo, TParameter, ValueTask<TFrom>> convertBack) =>
            throw new InvalidOperationException("Avoid asynchronous XAML conversion.");
    }
}
