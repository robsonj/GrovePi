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

            var tempInCelcius = _deviceFactory
                .BuildTemperatureAndHumiditySensor(Pin.DigitalPin2, Model.OnePointTwo)
                .TemperatureInCelcius();
            //_deviceFactory.BuildBuzzer(Pin.DigitalPin2).ChangeState(SensorStatus.On);

            //var level = _deviceFactory.BuildLightSensor(Pin.DigitalPin3)
            //    .SensorValue();
            //_deviceFactory
            //    .BuildBuzzer(Pin.DigitalPin4)
            //    .ChangeState(SensorStatus.On)
            //    .ChangeState(SensorStatus.Off);
        }
    }
}