//
// Copyright (c) 2010-2017 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Text;

namespace Antmicro.Renode.Utilities.GDB.Commands
{
    internal class ReadGeneralRegistersCommand : Command
    {
        public ReadGeneralRegistersCommand(CommandsManager manager) : base(manager)
        {
        }

        [Execute("g")]
        public PacketData Execute()
        {
            var registers = new StringBuilder();
            foreach(var i in manager.Cpu.GetRegisters())
            {
                var value = manager.Cpu.GetRegisterUnsafe(i);
                foreach(var b in BitConverter.GetBytes(value))
                {
                    registers.AppendFormat("{0:x2}", b);
                }
            }

            return new PacketData(registers.ToString());
        }
    }
}

