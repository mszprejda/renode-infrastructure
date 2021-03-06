//
// Copyright (c) 2010-2017 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;

namespace Antmicro.Renode.UserInterface.Tokenizer
{
    public class ConditionalEqualityToken : EqualityToken
    {
        public ConditionalEqualityToken(string value) : base(value)
        {
        }

        public override string ToString()
        {
            return string.Format("[ConditionalEqualityToken: Value={0}]", Value);
        }
    }
}

