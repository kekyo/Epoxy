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

using Epoxy.Internal;
using System.Diagnostics;

namespace Epoxy.Infrastructure;

/// <summary>
/// The Model base class.
/// </summary>
[DebuggerDisplay("{PrettyPrint}")]
public abstract class ModelBase
{
    [DebuggerStepThrough]
    private protected ModelBase()
    { }

    /// <summary>
    /// Pretty printer for this class.
    /// </summary>
    public string PrettyPrint
    {
        [DebuggerStepThrough]
        get => InternalModelHelper.PrettyPrint(this, true);
    }

    /// <summary>
    /// Get pretty printing string.
    /// </summary>
    /// <returns>Pretty printing string</returns>
    [DebuggerStepThrough]
    public override string ToString() =>
        $"{this.GetPrettyTypeName()}: {this.PrettyPrint}";
}
