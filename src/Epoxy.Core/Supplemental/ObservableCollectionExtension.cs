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

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Epoxy.Supplemental;

public static class ObservableCollectionExtension
{
    public static ObservableCollection<TValue> ToObservableCollection<TValue>(
        this IEnumerable<TValue> enumerable)
    {
        var collection = new ObservableCollection<TValue>();

        foreach (var value in enumerable)
        {
            collection.Add(value);
        }

        return collection;
    }

    public static void AddRange<TValue>(
        this ObservableCollection<TValue> collection,
        IEnumerable<TValue> values)
    {
        foreach (var value in values)
        {
            collection.Add(value);
        }
    }
}
