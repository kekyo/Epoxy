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

using Epoxy;

namespace Epoxy
{
    [ViewModel]
    public class TargettedViewModel1
    {
        public TargettedViewModel1()
        {
            //this.Prop1 = "ABC";
            //this.Prop2 = "ABC";
            //this.Prop3 = "ABC";
            this.Prop4 = "ABC";
        }

        //public string Prop1
        //{ get; }
        //public string Prop2
        //{ get; protected set; }
        //public string Prop3
        //{ get; internal set; }
        public string Prop4
        { get; set; }
    }
}
