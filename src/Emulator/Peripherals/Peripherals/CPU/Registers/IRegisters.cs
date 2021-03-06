//
// Copyright (c) 2010-2017 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Collections.Generic;

namespace Antmicro.Renode.Peripherals.CPU.Registers
{
    public interface IRegisters
    {
        IEnumerable<int> Keys { get; }
    }

    public interface IRegisters<T> : IRegisters
    {
        T this[int index] { get; set; }
    }
}

