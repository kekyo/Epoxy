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

using System.Diagnostics;
using System.Linq;

using Epoxy.Internal;

namespace Epoxy
{
    [DebuggerDisplay("{PrettyPrint}")]
    public abstract class Model
    {
        internal static readonly object[] emptyArgs = new object[0];

        protected Model()
        { }

        public virtual string PrettyPrint =>
            string.Join(
                ",",
                this.EnumerateFields().Concat(this.EnumerateProperties()).
                OrderBy(entry => entry.Key).
                Select(entry => $"{entry.Key}={entry.Value ?? "(null)"}"));

        public override string ToString() =>
            $"{this.GetPrettyTypeName()}: {this.PrettyPrint}";
    }
}