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

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EpoxyHello.Models;

public static class TheCatAPI
{
    private static readonly HttpClient httpClient = new HttpClient();

    public static async ValueTask<Cat[]> FetchTheCatsAsync(int cats)
    {
        // Uses The Cat API (https://thecatapi.com/)
        using (var response =
            await httpClient.
                GetAsync($"https://api.thecatapi.com/v1/images/search?limit={cats}").
                ConfigureAwait(false))
        {
            using (var stream =
                await response.Content.ReadAsStreamAsync().
                    ConfigureAwait(false))
            {
                var tr = new StreamReader(stream, Encoding.UTF8, true);
                var jr = new JsonTextReader(tr);

                var serializer = new JsonSerializer();

                return serializer.Deserialize<Cat[]>(jr)!;
            }
        }
    }

    public static async ValueTask<byte[]> FetchImageAsync(Uri url)
    {
        using (var response =
            await httpClient.
                GetAsync(url).
                ConfigureAwait(false))
        {
            using (var stream =
                await response.Content.ReadAsStreamAsync().
                    ConfigureAwait(false))
            {
                var ms = new MemoryStream();
                await stream.CopyToAsync(ms).
                    ConfigureAwait(false);

                return ms.ToArray();
            }
        }
    }
}
