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

using Epoxy.Infrastructure;

using System;

namespace Epoxy
{
    /// <summary>
    /// The ValueConverter base class.
    /// </summary>
    /// <typeparam name="TFrom">Value conversion from this type.</typeparam>
    /// <typeparam name="TTo">Value conversion to this type.</typeparam>
    /// <remarks>You can easier implement for the XAML converter only override TryConvert method.</remarks>
    public abstract class ValueConverter<TFrom, TTo> :
        ValueConverterBase<TFrom, TTo>
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        protected ValueConverter()
        { }

        /// <summary>
        /// Converting value implment method.
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="result">To value</param>
        /// <returns>True if converts successfully.</returns>
        public abstract bool TryConvert(TFrom from, out TTo result);

        /// <summary>
        /// Reverse direction converting value implment method.
        /// </summary>
        /// <param name="to">To value</param>
        /// <param name="result">From value</param>
        /// <returns>True if reverse direction converts successfully.</returns>
        /// <remarks>This method is optional. Default implementation will raise an exception.</remarks>
        public virtual bool TryConvertBack(TTo to, out TFrom result) =>
            throw new NotImplementedException();

        private protected override sealed bool InternalTryConvert(TFrom from, out TTo result) =>
            this.TryConvert(from, out result);

        private protected override sealed bool InternalTryConvertBack(TTo to, out TFrom result) =>
            this.TryConvertBack(to, out result);
    }

    /// <summary>
    /// The ValueConverter base class.
    /// </summary>
    /// <typeparam name="TFrom">Value conversion from this type.</typeparam>
    /// <typeparam name="TParameter">Value conversion with this type parameter.</typeparam>
    /// <typeparam name="TTo">Value conversion to this type.</typeparam>
    /// <remarks>You can easier implement for the XAML converter only override TryConvert method.</remarks>
    public abstract class ValueConverter<TFrom, TParameter, TTo> :
        ValueConverterBase<TFrom, TParameter, TTo>
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        protected ValueConverter()
        { }

        /// <summary>
        /// Converting value implment method.
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="parameter">Parameter value</param>
        /// <param name="result">To value</param>
        /// <returns>True if converts successfully.</returns>
        public abstract bool TryConvert(TFrom from, TParameter parameter, out TTo result);

        /// <summary>
        /// Reverse direction converting value implment method.
        /// </summary>
        /// <param name="to">To value</param>
        /// <param name="parameter">Parameter value</param>
        /// <param name="result">From value</param>
        /// <returns>True if reverse direction converts successfully.</returns>
        /// <remarks>This method is optional. Default implementation will raise an exception.</remarks>
        public virtual bool TryConvertBack(TTo to, TParameter parameter, out TFrom result) =>
            throw new NotImplementedException();

        private protected override sealed bool InternalTryConvert(TFrom from, TParameter parameter, out TTo result) =>
            this.TryConvert(from, parameter, out result);

        private protected override sealed bool InternalTryConvertBack(TTo to, TParameter parameter, out TFrom result) =>
            this.TryConvertBack(to, parameter, out result);
    }
}
