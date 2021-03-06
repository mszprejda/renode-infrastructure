//
// Copyright (c) 2010-2017 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Threading;
using Antmicro.Renode.Core;

namespace Antmicro.Renode.UnitTests.Mocks
{
    public class ActivelyAskingCPU : EmptyCPU
    {
        public ActivelyAskingCPU(Machine machine, long addressToAsk) : base(machine)
        {
            this.addressToAsk = addressToAsk;
            tokenSource = new CancellationTokenSource();
            finished = new ManualResetEventSlim();
        }

        public override void Start()
        {
            Resume();
        }

        public override void Resume()
        {
            finished.Reset();
            new Thread(() => AskingThread(tokenSource.Token))
            {
                IsBackground = true,
                Name = "AskingThread"
            }.Start();
        }

        public override void Pause()
        {
            tokenSource.Cancel();
            finished.Wait();
        }

        private void AskingThread(CancellationToken token)
        {
            while(!token.IsCancellationRequested)
            {
                machine.SystemBus.ReadDoubleWord(addressToAsk);
            }
            finished.Set();
        }

        private CancellationTokenSource tokenSource;
        private readonly ManualResetEventSlim finished;
        private readonly long addressToAsk;
    }
}

