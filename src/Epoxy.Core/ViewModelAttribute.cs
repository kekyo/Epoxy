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

namespace Epoxy
{
    /// <summary>
    /// ViewModel marker attribute for ViewModel injector.
    /// </summary>
    /// <remarks>You can choose for using ViewModel implementation:
    /// * Applies ViewModel attribute (this class) onto your pure ViewModel type and places auto-implemented properties.
    /// * Your ViewModel type derives from ViewModel base class and use GetValue/SetValue methods.
    /// 
    /// See ViewModel injector guide: https://github.com/kekyo/Epoxy#viewmodel-injector-and-viewmodel-base-class
    /// </remarks>
    /// <example>
    /// <code>
    /// // Enable ViewModel injector on this class
    /// [ViewModel]
    /// public sealed class ImageData
    /// {
    ///     // Will inject (auto-implemented) properties:
    ///     public string Title { get; set; }
    ///     public Uri Url { get; private set; }
    ///     public ImageSource Image { get; internal set; }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class ViewModelAttribute :
        Attribute
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        public ViewModelAttribute()
        { }
    }
}
