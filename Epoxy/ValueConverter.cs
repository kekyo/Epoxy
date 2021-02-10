////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
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

using System.Diagnostics;
using System.Reflection;

#if WINDOWS_UWP
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

#if WINDOWS_WPF
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
#endif

#if XAMARIN_FORMS
using System;
using System.Globalization;
using Xamarin.Forms;
#endif

namespace Epoxy
{
    public abstract class ValueConverter :
        IValueConverter
    {
        private protected ValueConverter()
        { }

        private protected abstract object? Convert(object? value, Type targetType, object? parameter);

        private protected abstract object? ConvertBack(object? value, Type targetType, object? parameter);

        object? IValueConverter.Convert(object? value, Type targetType, object? parameter,
#if WINDOWS_UWP
            string? language
#else
            CultureInfo? culture
#endif
            ) =>
            this.Convert(value, targetType, parameter);

        object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter,
#if WINDOWS_UWP
            string? language
#else
            CultureInfo? culture
#endif
            ) =>
            this.ConvertBack(value, targetType, parameter);

        public static ValueConverter Create<TTo, TFrom>(Func<TFrom, TTo> convert) =>
            new DelegatedValueConverter<TTo, TFrom>(convert);

        public static ValueConverter Create<TTo, TFrom>(Func<TFrom, TTo> convert, Func<TTo, TFrom> convertBack) =>
            new DelegatedValueConverter<TTo, TFrom>(convert, convertBack);

        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, TTo> convert) =>
            new DelegatedValueConverter<TTo, TFrom, TParameter>(convert);

        public static ValueConverter Create<TTo, TFrom, TParameter>(Func<TFrom, TParameter, TTo> convert, Func<TTo, TParameter, TFrom> convertBack) =>
            new DelegatedValueConverter<TTo, TFrom, TParameter>(convert, convertBack);
    }

    public abstract class ValueConverter<TTo, TFrom> : ValueConverter
    {
        protected ValueConverter()
        { }

        public abstract bool TryConvert(TFrom from, out TTo result);

        public virtual bool TryConvertBack(TTo to, out TFrom result) =>
            throw new NotImplementedException();

        private protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            Debug.Assert(
                parameter == null,
                $"ValueConverter.Convert: Invalid parameter given in {this.GetType().FullName}");
            Debug.Assert(
                targetType.IsAssignableFrom(typeof(TTo)),
                $"ValueConverter.Convert: Type mismatched in {this.GetType().FullName}: From={typeof(TFrom).FullName}, To={targetType.FullName}");

            if (value is TFrom from &&
                targetType.IsAssignableFrom(typeof(TTo)))
            {
                if (this.TryConvert(from, out var result))
                {
                    return result;
                }
            }

#if XAMARIN_FORMS
            return default(TTo)!;
#else
            return DependencyProperty.UnsetValue;
#endif
        }

        private protected override object? ConvertBack(object? value, Type targetType, object? parameter)
        {
            Debug.Assert(
                parameter == null,
                $"ValueConverter.Convert: Invalid parameter given in {this.GetType().FullName}");
            Debug.Assert(
                typeof(TFrom).IsAssignableFrom(targetType),
                $"ValueConverter.ConvertBack: Type mismatched in {this.GetType().FullName}: To={targetType.FullName}, From={typeof(TFrom).FullName}");

            if (value is TTo to &&
                typeof(TFrom).IsAssignableFrom(targetType))
            {
                if (this.TryConvertBack(to, out var result))
                {
                    return result;
                }
            }

#if XAMARIN_FORMS
            return default(TFrom)!;
#else
            return DependencyProperty.UnsetValue;
#endif
        }
    }

    public abstract class ValueConverter<TTo, TFrom, TParameter> : ValueConverter
    {
        protected ValueConverter()
        { }

        public abstract bool TryConvert(TFrom from, TParameter parameter, out TTo result);

        public virtual bool TryConvertBack(TTo to, TParameter parameter, out TFrom result) =>
            throw new NotImplementedException();

        private protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            Debug.Assert(
                parameter is TParameter,
                $"ValueConverter.Convert: Invalid parameter given in {this.GetType().FullName}");
            Debug.Assert(
                targetType.IsAssignableFrom(typeof(TTo)),
                $"ValueConverter.Convert: Type mismatched in {this.GetType().FullName}: From={typeof(TFrom).FullName}, To={targetType.FullName}");

            if (value is TFrom from &&
                targetType.IsAssignableFrom(typeof(TTo)))
            {
                if (this.TryConvert(from, (TParameter)parameter!, out var result))
                {
                    return result;
                }
            }

#if XAMARIN_FORMS
            return default(TTo)!;
#else
            return DependencyProperty.UnsetValue;
#endif
        }

        private protected override object? ConvertBack(object? value, Type targetType, object? parameter)
        {
            Debug.Assert(
                parameter is TParameter,
                $"ValueConverter.Convert: Invalid parameter given in {this.GetType().FullName}");
            Debug.Assert(
                typeof(TFrom).IsAssignableFrom(targetType),
                $"ValueConverter.ConvertBack: Type mismatched in {this.GetType().FullName}: To={targetType.FullName}, From={typeof(TFrom).FullName}");

            if (value is TTo to &&
                typeof(TFrom).IsAssignableFrom(targetType))
            {
                if (this.TryConvertBack(to, (TParameter)parameter!, out var result))
                {
                    return result;
                }
            }

#if XAMARIN_FORMS
            return default(TFrom)!;
#else
            return DependencyProperty.UnsetValue;
#endif
        }
    }
}
