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

namespace Epoxy.Advanced

open Epoxy.Internal

open System.Diagnostics

/// <summary>
/// Design time utility class.
/// </summary>
[<DebuggerStepThrough>]
[<AbstractClass>]
[<Sealed>]
type public Designer =

    /// <summary>
    /// Get current design time execution mode.
    /// </summary>
    static member isDesignTime =
        InternalDesigner.IsDesignTime
