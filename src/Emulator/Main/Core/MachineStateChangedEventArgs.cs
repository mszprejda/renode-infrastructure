//
// Copyright (c) 2010-2017 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
namespace Antmicro.Renode.Core
{
    public class MachineStateChangedEventArgs
    {
        public MachineStateChangedEventArgs(State state)
        {
            CurrentState = state;
        }

        public State CurrentState { get; private set; }

        public enum State
        {
            Started,
            Paused,
            Disposed
        }
    }
}

