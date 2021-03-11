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

using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Epoxy
{
    public sealed class ViewModelInjectorTask : Task
    {
        [Required]
        public string? TargetAssemblyPath
        {
            get;
            set;
        }

        [Required]
        public string? EpoxyCorePath
        {
            get;
            set;
        }

        public override bool Execute()
        {
            var targetAssemblyPath = this.TargetAssemblyPath!;

            var injector = new ViewModelInjector(
                this.EpoxyCorePath!,
                message => base.Log.LogMessage(MessageImportance.High, message));

            if (injector.Inject(targetAssemblyPath, targetAssemblyPath + ".tmp"))
            {
                if (File.Exists(targetAssemblyPath + ".orig"))
                {
                    File.Delete(targetAssemblyPath + ".orig");
                }

                File.Move(targetAssemblyPath, targetAssemblyPath + ".orig");
                File.Move(targetAssemblyPath + ".tmp", targetAssemblyPath);

                base.Log.LogMessage
                    (MessageImportance.High,
                    $"Epoxy.Build: Replaced injected assembly: Assembly={Path.GetFileName(targetAssemblyPath)}");
            }

            return true;
        }
    }
}
