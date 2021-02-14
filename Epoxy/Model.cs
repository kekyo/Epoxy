////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Epoxy
{
    [DebuggerDisplay("{PrettyPrint}")]
    public abstract class Model
    {
        internal static readonly object[] emptyArgs = new object[0];

        protected Model()
        { }

#if WINDOWS_UWP
        internal static bool IsPrimitive(Type type) =>
            type.GetTypeInfo().IsPrimitive;
#else
        internal static bool IsPrimitive(Type type) =>
            type.IsPrimitive;
#endif

        internal IEnumerable<(string name, object? value)> EnumerateFields() =>
            this.GetType().GetFields().
            Where(f => f.IsPublic && !f.IsStatic).
            Select(f => (f.Name, (object?)f.GetValue(this)));

        internal IEnumerable<(string name, object? value)> EnumerateProperties() =>
            this.GetType().GetProperties().
            Where(p =>
                p.CanRead &&
                (p.Name != "PrettyPrint") &&
                (p.GetIndexParameters().Length == 0) &&
                (IsPrimitive(p.PropertyType) || p.PropertyType == typeof(string)) &&
                p.GetGetMethod() is MethodInfo m &&
                m.IsPublic && !m.IsStatic).
            Select(p => (p.Name, (object?)p.GetValue(this, emptyArgs)));

        public virtual string PrettyPrint =>
            string.Join(
                ",",
                this.EnumerateFields().Concat(this.EnumerateProperties()).
                OrderBy(entry => entry.name).
                Select(entry => $"{entry.name}={entry.value ?? "(null)"}"));

        public override string ToString() =>
            $"{this.GetType().Name}: {this.PrettyPrint}";
    }
}
