﻿////////////////////////////////////////////////////////////////////////////
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

using System.ComponentModel;
using System.Diagnostics;

namespace Epoxy.Internal
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerStepThrough]
    public readonly struct Unit
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals(Unit unit) => true;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) => obj is Unit;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => 0;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() => "()";
    }
}
