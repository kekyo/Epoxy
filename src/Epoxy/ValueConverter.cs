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

using Epoxy.Infrastructure;

using System;

namespace Epoxy
{
    public abstract class ValueConverter<TFrom, TTo> :
        ValueConverterBase<TFrom, TTo>
    {
        protected ValueConverter()
        { }

        public abstract bool TryConvert(TFrom from, out TTo result);

        public virtual bool TryConvertBack(TTo to, out TFrom result) =>
            throw new NotImplementedException();

        private protected override sealed bool InternalTryConvert(TFrom from, out TTo result) =>
            this.TryConvert(from, out result);

        private protected override sealed bool InternalTryConvertBack(TTo to, out TFrom result) =>
            this.TryConvertBack(to, out result);
    }

    public abstract class ValueConverter<TFrom, TParameter, TTo> :
        ValueConverterBase<TFrom, TParameter, TTo>
    {
        protected ValueConverter()
        { }

        public abstract bool TryConvert(TFrom from, TParameter parameter, out TTo result);

        public virtual bool TryConvertBack(TTo to, TParameter parameter, out TFrom result) =>
            throw new NotImplementedException();

        private protected override sealed bool InternalTryConvert(TFrom from, TParameter parameter, out TTo result) =>
            this.TryConvert(from, parameter, out result);

        private protected override sealed bool InternalTryConvertBack(TTo to, TParameter parameter, out TFrom result) =>
            this.TryConvertBack(to, parameter, out result);
    }
}
