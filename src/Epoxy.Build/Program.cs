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

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Epoxy;

public static class Program
{
    public static int Main(string[] args)
    {
        var isTrace = false;
        void Message(LogLevels level, string message)
        {
            switch (level)
            {
                case LogLevels.Information:
                    Console.WriteLine($"Epoxy.Build: {message}");
                    break;
                case LogLevels.Trace when !isTrace:
                    break;
                default:
                    Console.WriteLine($"Epoxy.Build: {level.ToString().ToLowerInvariant()}: {message}");
                    break;
            }
        }

        try
        {
            var isDebug = false;

            var index = 0;
            while (true)
            {
                if (args[index] == "-t")
                {
                    isTrace = true;
                }
                else if (args[index] == "-d")
                {
                    isDebug = true;
                }
                else
                {
                    break;
                }
                index++;
            }

            var referencesBasePath = args[index++].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var targetAssemblyPath = args[index];

            if (isDebug)
            {
                Debugger.Launch();
            }

            var injector = new ViewModelInjector(referencesBasePath, Message);

            if (injector.Inject(targetAssemblyPath))
            {
                Message(
                    LogLevels.Information, 
                    $"Replaced injected assembly: Assembly={Path.GetFileName(targetAssemblyPath)}");
            }
            else
            {
                Message(LogLevels.Information,
                    $"Injection target isn't found: Assembly={Path.GetFileName(targetAssemblyPath)}");
            }

            return 0;
        }
        catch (Exception ex)
        {
            Message(LogLevels.Error, $"{ex.GetType().Name}: {ex.Message}");
            return Marshal.GetHRForException(ex);
        }
    }
}
