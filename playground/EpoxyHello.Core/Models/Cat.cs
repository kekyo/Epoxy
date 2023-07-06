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

using Newtonsoft.Json.Linq;
using System;

namespace EpoxyHello.Models;

// https://developers.thecatapi.com/view-account/ylX4blBYT9FaoVd6OhvR?report=bOoHBz-8t

public sealed class BleedInformation
{
    public readonly string Id;
    public readonly string? Name;
    public readonly JToken? Weight;
    public readonly string? Temperament;
    public readonly string? Origin;
    public readonly string? Country_codes;
    public readonly string? Country_code;
    public readonly string? Life_span;
    public readonly string? Description;
    public readonly string? Alt_names;
    public readonly int? Intelligence;
    public readonly Uri? Wikipedia_url;

    public BleedInformation(
        string id, string? name, JToken? weight, string? temperament, string? origin,
        string? country_codes, string? country_code, string? life_span, string? description,
        string? alt_names, int? intelligence, Uri? wikipedia_url)
    {
        this.Id = id;
        this.Name = name;
        this.Weight = weight;
        this.Temperament = temperament;
        this.Origin = origin;
        this.Country_codes = country_codes;
        this.Country_code = country_code;
        this.Life_span = life_span;
        this.Description = description;
        this.Alt_names = alt_names;
        this.Intelligence = intelligence;
        this.Wikipedia_url = wikipedia_url;
    }
}

public sealed class Cat
{
    public readonly string Id;
    public readonly Uri? Url;
    public readonly int? Width;
    public readonly int? Height;
    public readonly BleedInformation[] Bleeds;
    public readonly JToken? Favourite;

    public Cat(
        string id, Uri? url, int? width, int? height,
        BleedInformation[]? bleeds, JToken? favourite)
    {
        this.Id = id;
        this.Url = url;
        this.Width = width;
        this.Height = height;
        this.Bleeds = bleeds ?? Array.Empty<BleedInformation>();
        this.Favourite = favourite;
    }
}
