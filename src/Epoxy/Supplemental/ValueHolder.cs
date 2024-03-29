﻿////////////////////////////////////////////////////////////////////////////
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
using System.Diagnostics;
using System.Threading;

#if WINDOWS_UWP || UNO
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

#if WINUI
using Windows.Foundation;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if MAUI
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
#endif

#if AVALONIA || AVALONIA11
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
#endif

namespace Epoxy.Supplemental;

/// <summary>
/// The variant value holder struct.
/// </summary>
/// <remarks>This struct is helper type,
/// will automatically convert some standard exactly matched types.</remarks>
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

        return (value != null) ? (TValue)value! : default!;
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
    public static implicit operator string?(ValueHolder vh) =>
        vh.GetValue<string?>();
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
    public static implicit operator Uri?(ValueHolder vh) =>
        vh.GetValue<Uri?>();
    public static implicit operator Size(ValueHolder vh) =>
        vh.GetValue<Size>();
    public static implicit operator Point(ValueHolder vh) =>
        vh.GetValue<Point>();
    public static implicit operator Thickness(ValueHolder vh) =>
        vh.GetValue<Thickness>();
    public static implicit operator Color(ValueHolder vh) =>
        vh.GetValue<Color>();
#if WINDOWS_WPF || AVALONIA || AVALONIA11
    public static implicit operator Vector(ValueHolder vh) =>
        vh.GetValue<Vector>();
    public static implicit operator Pen?(ValueHolder vh) =>
        vh.GetValue<Pen?>();
#endif
#if WINDOWS_WPF || WINDOWS_UWP || WINUI || UNO || AVALONIA || AVALONIA11 || OPENSILVER
    public static implicit operator Brush?(ValueHolder vh) =>
        vh.GetValue<Brush?>();
    public static implicit operator Rect(ValueHolder vh) =>
        vh.GetValue<Rect>();
    public static implicit operator Matrix(ValueHolder vh) =>
        vh.GetValue<Matrix>();
#endif
#if WINDOWS_WPF || WINDOWS_UWP || WINUI || UNO || OPENSILVER
    public static implicit operator Visibility(ValueHolder vh) =>
        vh.GetValue<Visibility>();
#endif
#if XAMARIN_FORMS || MAUI
    public static implicit operator FontAttributes(ValueHolder vh) =>
        vh.GetValue<FontAttributes>();
#endif
#if AVALONIA || AVALONIA11
    public static implicit operator DrawingImage?(ValueHolder vh) =>
        vh.GetValue<DrawingImage?>();
    public static implicit operator Bitmap?(ValueHolder vh) =>
        vh.GetValue<Bitmap?>();
    public static implicit operator CroppedBitmap?(ValueHolder vh) =>
        vh.GetValue<CroppedBitmap?>();
#else
    public static implicit operator ImageSource?(ValueHolder vh) =>
        vh.GetValue<ImageSource?>();
#endif
    public static implicit operator Command?(ValueHolder vh) =>
        vh.GetValue<Command?>();
    public static implicit operator Pile?(ValueHolder vh) =>
        vh.GetValue<Pile?>();
}
