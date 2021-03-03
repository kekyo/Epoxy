////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.Models

open Newtonsoft.Json;
open Newtonsoft.Json.Linq;
open System;
open System.IO;
open System.Net.Http;
open System.Text;

module public Reddit =
    let private httpClient = new HttpClient()

    let fetchNewPostsAsync (name: string) = async {
        // Uses Reddit with Json API
        use! response = httpClient.GetAsync($"https://www.reddit.com/{name}/new.json")
        use! stream = response.Content.ReadAsStreamAsync()

        let tr = new StreamReader(stream, Encoding.UTF8, true)
        let jr = new JsonTextReader(tr)

        let serializer = new JsonSerializer()

        let root = serializer.Deserialize<JObject>(jr)

        return root.["data"].["children"]
            |> Seq.map (fun child -> child.["data"])
            |> Seq.filter (fun data ->
                match Path.GetExtension((data.["url"].ToObject<Uri>()).AbsolutePath) with
                | ".jpg" -> true
                | ".png" -> true
                | _ -> false)
            |> Seq.map (fun data -> new RedditPost(data.["title"].ToObject<string>(), data.["url"].ToObject<Uri>(), data.["score"].ToObject<int>()))
            |> Seq.toArray
    }

    let fetchImageAsync(url: Uri) = async {
        use! response = httpClient.GetAsync url
        use! stream = response.Content.ReadAsStreamAsync()

        let ms = new MemoryStream()
        do! stream.CopyToAsync(ms)

        return ms.ToArray();
    }
