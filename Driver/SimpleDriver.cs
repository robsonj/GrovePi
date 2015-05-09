using System;
using System.Threading;
using Windows.ApplicationModel.Background;
using GrovePi;

namespace Driver
{
    public sealed class SimpleDriver : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
        }

        private static void LedBlink(ILed device, int durationInSeconds, int frequencyInMillseconds)
        {
            var autoEvent = new AutoResetEvent(false);
            using (new Timer(x => OnTimer(device), null, 0, frequencyInMillseconds))
            {
                autoEvent.WaitOne(TimeSpan.FromSeconds(durationInSeconds));
                device.ChangeState(LedStatus.Off);
            }
        }

        private static void OnTimer(ILed device)
        {
            var result = device.CurrentState;
            var newStatus = LedStatus.Off;
            if (LedStatus.Off == result)
            {
                newStatus = LedStatus.On;
            }
            device.ChangeState(newStatus);
        }
    }
}