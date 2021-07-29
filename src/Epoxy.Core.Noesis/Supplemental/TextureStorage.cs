////////////////////////////////////////////////////////////////////////////
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Noesis;

namespace Epoxy.Supplemental
{
    public sealed class TextureStorage : FileTextureProvider
    {
        private readonly SortedDictionary<int, Func<Uri, Stream?>> evaluators = new();
        private int index;

        private TextureStorage()
        {
        }

        public override Stream? OpenStream(Uri filename)
        {
            foreach (var entry in this.evaluators)
            {
                try
                {
                    if (entry.Value(filename) is { } stream)
                    {
                        return stream;
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        private int RegisterEvaluator(Func<Uri, Stream?> evaluator)
        {
            var index = this.index;
            this.index++;
            this.evaluators.Add(index, evaluator);
            return index;
        }

        private void UnregisterEvaluator(int index) =>
            this.evaluators.Remove(index);

        private static readonly TextureStorage provider = new TextureStorage();

        public static void Register(Uri filename, Func<Stream> creator) =>
            provider.RegisterEvaluator(fn => fn == filename ? creator() : null);

        public static void Register(params (Uri filename, Func<Stream> creator)[] entries)
        {
            var dict = entries.ToDictionary(entry => entry.filename, entry => entry.creator);
            provider.RegisterEvaluator(fn => dict.TryGetValue(fn, out var creator) ? creator() : null);
        }

        public static void Register(string fileSystemBasePath) =>
            provider.RegisterEvaluator(fn =>
            {
                var path = System.IO.Path.Combine(fileSystemBasePath, fn.LocalPath);
                return File.Exists(path) ?
                    new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read) : null;
            });

        public static void Register(Func<Uri, Stream?> evaluator) =>
            provider.RegisterEvaluator(evaluator);

        public static BitmapImage? LoadBitmapImage(Func<Stream> creator)
        {
            var uri = new Uri(Guid.NewGuid().ToString());
            var index = provider.RegisterEvaluator(fn => fn == uri ? creator() : null);
            try
            {
                return new BitmapImage(uri);
            }
            finally
            {
                provider.UnregisterEvaluator(index);
            }
        }

        public static BitmapImage? LoadBitmapImage(byte[] data) =>
            LoadBitmapImage(() => new MemoryStream(data));

        public static BitmapImage? LoadBitmapImage(Stream stream) =>
            LoadBitmapImage(() => stream);

        public static BitmapImage? LoadBitmapImage(string fileSystemPath) =>
            LoadBitmapImage(() => new FileStream(fileSystemPath, FileMode.Open, FileAccess.Read, FileShare.Read));
    }
}
