////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
// Copyright (c) 2020 Kouji Matsui (@kozy_kekyo, @kekyo2)
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
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EpoxyHello.Wpf.Models
{
    public static class Reddit
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async ValueTask<RedditPost[]> FetchNewPostsAsync(string name)
        {
            // Uses Reddit with Json API
            using (var response =
                await httpClient.
                    GetAsync($"https://www.reddit.com/{name}/new.json").
                    ConfigureAwait(false))
            {
                using (var stream =
                    await response.Content.ReadAsStreamAsync().
                        ConfigureAwait(false))
                {
                    var tr = new StreamReader(stream, Encoding.UTF8, true);
                    var jr = new JsonTextReader(tr);

                    var serializer = new JsonSerializer();

                    var root = serializer.Deserialize<JObject>(jr);

                    return root!["data"]!["children"]!.
                        Select(child => child["data"]!).
                        Where(data => Path.GetExtension(((Uri)data["url"]!).AbsolutePath) switch { ".jpg" => true, ".png" => true, _ => false }).
                        Select(data => new RedditPost((string)data["title"]!, (Uri)data["url"]!, (int)data["score"]!)).
                        ToArray();
                }
            }
        }

        public static async ValueTask<ImageSource> FetchImageAsync(Uri url)
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

                    ms.Position = 0;
                    var bitmap = new WriteableBitmap(BitmapFrame.Create(ms));
                    bitmap.Freeze();

                    return bitmap;
                }
            }
        }
    }
}
