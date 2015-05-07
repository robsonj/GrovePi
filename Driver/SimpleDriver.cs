using Windows.ApplicationModel.Background;
using GrovePi;

namespace Driver
{
    public sealed class SimpleDriver : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var grovePi = DeviceBuilder.BuildGrovePi();
            var version = grovePi.GetFirmwareVersion();
            var test = grovePi.AnalogRead(2);
        }
    }
}