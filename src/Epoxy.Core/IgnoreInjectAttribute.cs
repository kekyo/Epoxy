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
    /// Will ignore ViewModel injection, marking on this property.
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
    ///     // Will not inject this property:
    ///     [IgnoreInject]
    ///     private int Score { get; set; }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreInjectAttribute :
        Attribute
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        public IgnoreInjectAttribute()
        { }
    }
}
