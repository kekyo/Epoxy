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

namespace Epoxy.Advanced;

/// <summary>
/// Marks target inferface type for using GlobalService.
/// </summary>\
/// <remarks>The GlobalService will find target interface types when applied this attribute.</remarks>
[AttributeUsage(AttributeTargets.Interface)]
public sealed class GlobalServiceAttribute : Attribute
{
    /// <summary>
    /// The constructor.
    /// </summary>
    public GlobalServiceAttribute()
    { }
}

/// <summary>
/// GlobalService registering validation methods.
/// </summary>
public enum RegisteringValidations
{
    /// <summary>
    /// Strict method is totally declined when one or more interface type already registered.
    /// </summary>
    Strict,

    /// <summary>
    /// Unsafe partial method is ignored when interface type already registered.
    /// </summary>
    UnsafePartial,

    /// <summary>
    /// Unsafe override method is force overrided when interface type already registered.
    /// </summary>
    UnsafeOverride
}

/// <summary>
/// GlobalService accessor class.
/// </summary>
public sealed class GlobalServiceAccessor
{
    /// <summary>
    /// The constructor.
    /// </summary>
    internal GlobalServiceAccessor()
    {
    }
}
