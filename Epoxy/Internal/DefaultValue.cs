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

#if WINDOWS_WPF
using System.Windows;
#endif

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if AVALONIA
using Avalonia;
#endif

namespace Epoxy.Internal
{
    internal static class DefaultValue<TValue>
    {
        private static readonly TValue defaultValue = default!;

        public static bool ValueEquals(object? lhs, TValue rhs)
        {
            if ((lhs == null) && (rhs == null))
            {
                return true;
            }
            else if ((lhs == null) && (rhs != null))
            {
                return false;
            }
            else if ((lhs != null) && (rhs == null))
            {
                return false;
            }
            else
            {
                return lhs!.Equals(rhs);
            }
        }

        public static bool IsDefault(object? value) =>
            ValueEquals(value, defaultValue);
    }

    internal static class DefaultValue
    {
        public static bool IsDefaultValue<TValue>(this TValue value) =>
            DefaultValue<TValue>.IsDefault(value);

        public static bool IsDefault<TValue>(object? value) =>
            DefaultValue<TValue>.IsDefault(value);

#if XAMARIN_FORMS
        public static readonly object? XamlProperty =
            null;
#elif AVALONIA
        public static readonly object? XamlProperty =
            AvaloniaProperty.UnsetValue;
#else
        public static readonly object? XamlProperty =
            DependencyProperty.UnsetValue;
#endif
    }
}
