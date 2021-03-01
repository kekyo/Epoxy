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

namespace Epoxy.Supplemental
{
    public sealed class DelegatedValueConverter<TFrom, TTo> :
        ValueConverterBase<TFrom, TTo>
    {
        private readonly Func<TFrom, TTo> convert;
        private readonly Func<TTo, TFrom>? convertBack;

        internal DelegatedValueConverter(Func<TFrom, TTo> convert) =>
            this.convert = convert;

        internal DelegatedValueConverter(Func<TFrom, TTo> convert, Func<TTo, TFrom> convertBack)
        {
            this.convert = convert;
            this.convertBack = convertBack;
        }

        private protected override bool InternalTryConvert(TFrom from, out TTo result)
        {
            result = this.convert(from);
            return true;
        }

        private protected override bool InternalTryConvertBack(TTo to, out TFrom result)
        {
            if (this.convertBack != null)
            {
                result = this.convertBack(to);
                return true;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    public sealed class DelegatedValueConverter<TFrom, TParameter, TTo> :
        ValueConverterBase<TFrom, TParameter, TTo>
    {
        private readonly Func<TFrom, TParameter, TTo> convert;
        private readonly Func<TTo, TParameter, TFrom>? convertBack;

        internal DelegatedValueConverter(Func<TFrom, TParameter, TTo> convert) =>
            this.convert = convert;

        internal DelegatedValueConverter(Func<TFrom, TParameter, TTo> convert, Func<TTo, TParameter, TFrom> convertBack)
        {
            this.convert = convert;
            this.convertBack = convertBack;
        }

        private protected override bool InternalTryConvert(TFrom from, TParameter parameter, out TTo result)
        {
            result = this.convert(from, parameter);
            return true;
        }

        private protected override bool InternalTryConvertBack(TTo to, TParameter parameter, out TFrom result)
        {
            if (this.convertBack != null)
            {
                result = this.convertBack(to, parameter);
                return true;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
