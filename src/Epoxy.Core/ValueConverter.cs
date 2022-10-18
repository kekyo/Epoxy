////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
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

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
using System.Windows.Data;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if MAUI
using Microsoft.Maui.Controls;
#endif

#if AVALONIA
using Avalonia.Data.Converters;
#endif

using Epoxy.Internal;

namespace Epoxy;

public abstract class ValueConverter :
    IValueConverter
{
    private protected ValueConverter()
    { }

    private protected abstract object? Convert(object? value, Type targetType, object? parameter);

    private protected abstract object? ConvertBack(object? value, Type targetType, object? parameter);

    object? IValueConverter.Convert(object? value, Type targetType, object? parameter,
#if WINDOWS_UWP || WINUI || UNO
        string? language
#else
        CultureInfo? culture
#endif
        ) =>
        this.Convert(value, targetType, parameter);

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter,
#if WINDOWS_UWP || WINUI || UNO
        string? language
#else
        CultureInfo? culture
#endif
        ) =>
        this.ConvertBack(value, targetType, parameter);
}
