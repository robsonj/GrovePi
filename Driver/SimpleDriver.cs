using Windows.ApplicationModel.Background;
using GrovePi;
using GrovePi.Sensors;

namespace Driver
{
    public sealed class SimpleDriver : IBackgroundTask
    {
        private readonly IBuildGroveDevices _deviceFactory = DeviceFactory.Build;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //var distance = _deviceFactory
            //    .BuildUltraSonicSensor(Pin.DigitalPin2)
            //    .MeasureInCentimeters();
            //var tempInCelcius = _deviceFactory
            //    .BuildTemperatureAndHumiditySensor(Pin.DigitalPin3, Model.OnePointTwo)
            //    .TemperatureInCelcius();

            var level = _deviceFactory.BuildLightSensor(Pin.DigitalPin3)
                .SensorValue();
            _deviceFactory
                .BuildBuzzer(Pin.DigitalPin4)
                .ChangeState(BuzzerStatus.On)
                .ChangeState(BuzzerStatus.Off);
            

            _deviceFactory
                .BuildLed(Pin.DigitalPin4)
                .ChangeState(LedStatus.On)
                .ChangeState(LedStatus.Off)
                .ChangeState(LedStatus.On);
        }
    }
}