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

open Epoxy.Infrastructure

open System

/// <summary>
/// The ValueConverter base class.
/// </summary>
/// <typeparam name="TFrom">Value conversion from this type.</typeparam>
/// <typeparam name="TTo">Value conversion to this type.</typeparam>
/// <remarks>You can easier implement for the XAML converter only override TryConvert method.</remarks>
[<AbstractClass>]
type public ValueConverter<'TFrom, 'TTo>() =
    inherit ValueConverterBase<'TFrom, 'TTo>()

    /// <summary>
    /// Converting value implment method.
    /// </summary>
    /// <returns>To value if converts successfully.</returns>
    abstract convert: 'TFrom -> 'TTo option

    /// <summary>
    /// Reverse direction converting value implment method.
    /// </summary>
    /// <returns>From value if reverse direction converts successfully.</returns>
    /// <remarks>This method is optional. Default implementation will raise an exception.</remarks>
    abstract convertBack: 'TTo -> 'TFrom option
    default __.convertBack _ =
        raise(NotImplementedException())

    override self.InternalTryConvert(from, result) =
        match self.convert from with
        | Some r ->
            result <- r
            true
        | None ->
            result <- Unchecked.defaultof<'TTo>
            false

    override self.InternalTryConvertBack(_to, result) =
        match self.convertBack _to with
        | Some r ->
            result <- r
            true
        | None ->
            result <- Unchecked.defaultof<'TFrom>
            false

/// <summary>
/// The ValueConverter base class.
/// </summary>
/// <typeparam name="'TFrom">Value conversion from this type.</typeparam>
/// <typeparam name="'TParameter">Value conversion with this type parameter.</typeparam>
/// <typeparam name="'TTo">Value conversion to this type.</typeparam>
/// <remarks>You can easier implement for the XAML converter only override TryConvert method.</remarks>
[<AbstractClass>]
type public ValueConverter<'TFrom, 'TParameter, 'TTo>() =
    inherit ValueConverterBase<'TFrom, 'TParameter, 'TTo>()

    /// <summary>
    /// Converting value implment method.
    /// </summary>
    /// <returns>To value if converts successfully.</returns>
    abstract convert: 'TFrom -> 'TParameter -> 'TTo option

    /// <summary>
    /// Reverse direction converting value implment method.
    /// </summary>
    /// <returns>From value if reverse direction converts successfully.</returns>
    /// <remarks>This method is optional. Default implementation will raise an exception.</remarks>
    abstract convertBack: 'TTo -> 'TParameter -> 'TFrom option
    default __.convertBack _ _ =
        raise(NotImplementedException())

    override self.InternalTryConvert(from, parameter, result) =
        match self.convert from parameter with
        | Some r ->
            result <- r
            true
        | None ->
            result <- Unchecked.defaultof<'TTo>
            false

    override self.InternalTryConvertBack(_to, parameter, result) =
        match self.convertBack _to parameter with
        | Some r ->
            result <- r
            true
        | None ->
            result <- Unchecked.defaultof<'TFrom>
            false
