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

namespace Epoxy

open Epoxy

[<ViewModel>]
type TargettedViewModel1() =

    [<DefaultValue>]
    val mutable Prop9Set: string
    [<DefaultValue>]
    val mutable Prop11Set: string
    [<DefaultValue>]
    val mutable Prop12Set: string

    ////////////////////////////////////

    member __.Prop1 = "ABC1"
    member val Prop2 = "ABC2"
        with get, set

    ////////////////////////////////////
    
    member val Prop9 = Unchecked.defaultof<string>
        with get, set

    member self.onProp9ChangedAsync value = async {
        do self.Prop9Set <- value
    }

    ////////////////////////////////////

    [<IgnoreInject>]
    member val Prop10 = "ABC10"
        with get, set

    ////////////////////////////////////

    member val Prop11 = Unchecked.defaultof<string>
        with get, set

    [<PropertyChanged("Prop11")>]
    member self.__onProp11ChangedAsync value = async {
        do self.Prop11Set <- value
    }

    ////////////////////////////////////

    member val Prop12 = Unchecked.defaultof<string>
        with get, set

    [<PropertyChanged("Prop12")>]
    member self.__onProp12ChangedAsync value = async {
        do self.Prop12Set <- value
    }

    member _.onProp12ChangedAsync value = async {
        return ()
    }
