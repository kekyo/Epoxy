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

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
#endif

#if WINDOWS_WPF
using System.Windows.Data;
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if AVALONIA
using Avalonia.Data.Converters;
#endif

using Epoxy.Internal;

namespace Epoxy.Infrastructure
{
    /// <summary>
    /// The ValueConverter base class using internal only.
    /// </summary>
    /// <typeparam name="TFrom">Value conversion from this type.</typeparam>
    /// <typeparam name="TTo">Value conversion to this type.</typeparam>
    /// <remarks>You have to use Epoxy.ViewModel class instead this class directly.</remarks>
    public abstract class ValueConverterBase<TFrom, TTo> : ValueConverter
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        private protected ValueConverterBase()
        { }

        private protected abstract bool InternalTryConvert(TFrom from, out TTo result);

        private protected abstract bool InternalTryConvertBack(TTo to, out TFrom result);

        private protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            if (parameter != null)
            {
                throw new ArgumentException(
                    $"ValueConverter.Convert: Invalid parameter given in {this.GetPrettyTypeName()}, Parameter={parameter.GetPrettyTypeName()}");
            }
            if (!targetType.IsAssignableFrom(typeof(TTo)))
            {
                throw new ArgumentException(
                    $"ValueConverter.Convert: Type mismatched in {this.GetPrettyTypeName()}: From={typeof(TFrom).FullName}, To={targetType.FullName}");
            }

            if (value is TFrom from)
            {
                if (this.InternalTryConvert(from, out var result))
                {
                    return result;
                }
            }

            return DefaultValue.XamlProperty;
        }

        private protected override object? ConvertBack(object? value, Type targetType, object? parameter)
        {
            if (parameter != null)
            {
                throw new ArgumentException(
                    $"ValueConverter.Convert: Invalid parameter given in {this.GetPrettyTypeName()}, Parameter={parameter.GetPrettyTypeName()}");
            }
            if (!typeof(TFrom).IsAssignableFrom(targetType))
            {
                throw new ArgumentException(
                    $"ValueConverter.ConvertBack: Type mismatched in {this.GetPrettyTypeName()}: To={targetType.FullName}, From={typeof(TFrom).FullName}");
            }

            if (value is TTo to)
            {
                if (this.InternalTryConvertBack(to, out var result))
                {
                    return result;
                }
            }

            return DefaultValue.XamlProperty;
        }
    }

    /// <summary>
    /// The ValueConverter base class using internal only.
    /// </summary>
    /// <typeparam name="TFrom">Value conversion from this type.</typeparam>
    /// <typeparam name="TParameter">Value conversion with this type parameter.</typeparam>
    /// <typeparam name="TTo">Value conversion to this type.</typeparam>
    /// <remarks>You have to use Epoxy.ViewModel class instead this class directly.</remarks>
    public abstract class ValueConverterBase<TFrom, TParameter, TTo> : ValueConverter
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        private protected ValueConverterBase()
        { }

        private protected abstract bool InternalTryConvert(TFrom from, TParameter parameter, out TTo result);

        private protected abstract bool InternalTryConvertBack(TTo to, TParameter parameter, out TFrom result);

        private protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            if (!DefaultValue.IsDefault<TParameter>(value))
            {
                throw new ArgumentException(
                    $"ValueConverter.Convert: Invalid parameter given in {this.GetPrettyTypeName()}, Parameter={parameter.GetPrettyTypeName()}");
            }
            if (!targetType.IsAssignableFrom(typeof(TTo)))
            {
                throw new ArgumentException(
                    $"ValueConverter.Convert: Type mismatched in {this.GetPrettyTypeName()}: From={typeof(TFrom).FullName}, To={targetType.FullName}");
            }

            if (value is TFrom from)
            {
                if (this.InternalTryConvert(from, (TParameter)parameter!, out var result))
                {
                    return result;
                }
            }

            return DefaultValue.XamlProperty;
        }

        private protected override object? ConvertBack(object? value, Type targetType, object? parameter)
        {
            if (!DefaultValue.IsDefault<TParameter>(value))
            {
                throw new ArgumentException(
                    $"ValueConverter.ConvertBack: Invalid parameter given in {this.GetPrettyTypeName()}, Parameter={parameter.GetPrettyTypeName()}");
            }
            if (!typeof(TFrom).IsAssignableFrom(targetType))
            {
                throw new ArgumentException(
                    $"ValueConverter.ConvertBack: Type mismatched in {this.GetPrettyTypeName()}: To={targetType.FullName}, From={typeof(TFrom).FullName}");
            }

            if (value is TTo to)
            {
                if (this.InternalTryConvertBack(to, (TParameter)parameter!, out var result))
                {
                    return result;
                }
            }

            return DefaultValue.XamlProperty;
        }
    }
}
