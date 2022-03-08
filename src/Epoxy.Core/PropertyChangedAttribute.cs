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

namespace Epoxy
{
    /// <summary>
    /// The method is targetted for receiving property changed in ViewModel injection.
    /// </summary>
    /// <remarks>See ViewModel injector guide: https://github.com/kekyo/Epoxy#viewmodel-injector-and-viewmodel-base-class</remarks>
    /// <example>
    /// <code>
    /// // Enable ViewModel injector on this class
    /// [ViewModel]
    /// public sealed class ImageData
    /// {
    ///     // Will inject (auto-implemented) property:
    ///     public string Title { get; set; }
    ///     
    ///     // Will receive property changed.
    ///     [PropertyChanged(nameof(Title))]
    ///     private ValueTask OnTitleChanged(string value)
    ///     {
    ///         // Your owned task...
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PropertyChangedAttribute :
        Attribute
    {
        /// <summary>
        /// Target property name.
        /// </summary>
        public readonly string PropertyName;

        /// <summary>
        /// The constructor.
        /// </summary>
        public PropertyChangedAttribute(string propertyName) =>
            this.PropertyName = propertyName;
    }
}
