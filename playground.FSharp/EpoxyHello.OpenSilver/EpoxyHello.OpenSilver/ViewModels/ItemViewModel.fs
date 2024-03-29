﻿////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) 2019-2021 Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
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

namespace EpoxyHello.Wpf.ViewModels

open Epoxy
open System.Windows.Media

type ItemViewModel() =
    inherit ViewModel()

    member __.Title
        with get(): string = __.getValue()
        and set (value: string) = __.setValue value

    member __.Image
        with get(): ImageSource = __.getValue()
        and set (value: ImageSource) = __.setValue value

    member __.Score
        with get(): int = __.getValue()
        and set (value: int) = __.setValue value
