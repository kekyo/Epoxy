////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
// Copyright (c) 2020 Kouji Matsui (@kozy_kekyo, @kekyo2)
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
using System.Diagnostics;
using System.Threading;

#if WINDOWS_UWP
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

#if WINDOWS_WPF
using System.Windows;
using System.Windows.Media;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

namespace Epoxy
{
    public struct ValueHolder
    {
        private readonly object? value;

        internal ValueHolder(object? value) =>
            this.value = value;

        private TValue GetValue<TValue>()
        {
            Debug.Assert(
                SynchronizationContext.Current != null,
                "Cannot use OnPropertyChanged() on worker thread context.");

            return (value != null) ? (TValue)value : default;
        }

        public static implicit operator bool(ValueHolder vh) =>
            vh.GetValue<bool>();
        public static implicit operator byte(ValueHolder vh) =>
            vh.GetValue<byte>();
        public static implicit operator sbyte(ValueHolder vh) =>
            vh.GetValue<sbyte>();
        public static implicit operator short(ValueHolder vh) =>
            vh.GetValue<short>();
        public static implicit operator ushort(ValueHolder vh) =>
            vh.GetValue<ushort>();
        public static implicit operator int(ValueHolder vh) =>
            vh.GetValue<int>();
        public static implicit operator uint(ValueHolder vh) =>
            vh.GetValue<uint>();
        public static implicit operator long(ValueHolder vh) =>
            vh.GetValue<long>();
        public static implicit operator ulong(ValueHolder vh) =>
            vh.GetValue<ulong>();
        public static implicit operator float(ValueHolder vh) =>
            vh.GetValue<float>();
        public static implicit operator double(ValueHolder vh) =>
            vh.GetValue<double>();
        public static implicit operator char(ValueHolder vh) =>
            vh.GetValue<char>();
        public static implicit operator string(ValueHolder vh) =>
            vh.GetValue<string>();
        public static implicit operator decimal(ValueHolder vh) =>
            vh.GetValue<decimal>();
        public static implicit operator DateTime(ValueHolder vh) =>
            vh.GetValue<DateTime>();
        public static implicit operator DateTimeOffset(ValueHolder vh) =>
            vh.GetValue<DateTimeOffset>();
        public static implicit operator TimeSpan(ValueHolder vh) =>
            vh.GetValue<TimeSpan>();
        public static implicit operator Guid(ValueHolder vh) =>
            vh.GetValue<Guid>();
        public static implicit operator Uri(ValueHolder vh) =>
            vh.GetValue<Uri>();
        public static implicit operator Size(ValueHolder vh) =>
            vh.GetValue<Size>();
        public static implicit operator Point(ValueHolder vh) =>
            vh.GetValue<Point>();
        public static implicit operator Thickness(ValueHolder vh) =>
            vh.GetValue<Thickness>();
        public static implicit operator Color(ValueHolder vh) =>
            vh.GetValue<Color>();
#if WINDOWS_WPF
        public static implicit operator Vector(ValueHolder vh) =>
            vh.GetValue<Vector>();
        public static implicit operator Pen(ValueHolder vh) =>
            vh.GetValue<Pen>();
#endif
#if WINDOWS_WPF || WINDOWS_UWP
        public static implicit operator Brush(ValueHolder vh) =>
            vh.GetValue<Brush>();
        public static implicit operator Rect(ValueHolder vh) =>
            vh.GetValue<Rect>();
        public static implicit operator Matrix(ValueHolder vh) =>
            vh.GetValue<Matrix>();
        public static implicit operator Visibility(ValueHolder vh) =>
            vh.GetValue<Visibility>();
#endif
#if XAMARIN_FORMS
        public static implicit operator FontAttributes(ValueHolder vh) =>
            vh.GetValue<FontAttributes>();
#endif
        public static implicit operator ImageSource?(ValueHolder vh) =>
            vh.GetValue<ImageSource?>();
        public static implicit operator Command?(ValueHolder vh) =>
            vh.GetValue<Command?>();
    }
}
