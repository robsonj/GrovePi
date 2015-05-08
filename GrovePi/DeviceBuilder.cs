using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using GrovePi.Sensors;

namespace GrovePi
{
    public static class DeviceBuilder
    {
        private const string I2CName = "I2C1"; /* For Raspberry Pi 2, use I2C1 */
        private const byte GrovePiAddress = 0x04;
        private static GrovePi _device;

        public static IGrovePi BuildGrovePi()
        {
            return BuildGrovePiImpl(GrovePiAddress);
        }

        public static IGrovePi BuildGrovePi(int address)
        {
            return BuildGrovePiImpl(address);
        }

        public static ILed BuildLed(Pin pin)
        {
            var device = BuildGrovePi();
            return new Led(device, pin);
        }

        public static ITemperatureAndHumiditySensor BuildTemperatureAndHumiditySensor(Pin pin, Model model)
        {
            var device = BuildGrovePi();
            return new TemperatureAndHumiditySensor(device, pin, model);
        }

        public static IUltrasonicRangerSensor BuildUltraSonicSensor(Pin pin)
        {
            var device = BuildGrovePiImpl(GrovePiAddress);
            return new UltrasonicRangerSensor(device, pin);
        }

        public static IAccelerometerSensor BuildAccelerometer(Pin pin)
        {
            var device = BuildGrovePiImpl(GrovePiAddress);
            return new AccelerometerSensor(device, pin);
        }

        public static IRtcSensor BuildRtc(Pin pin)
        {
            var device = BuildGrovePiImpl(GrovePiAddress);
            return new RtcSensor(device, pin);
        }
		
		public static ILedBarSensor BuildLedBarSensor(Pin pin)
        {
            var device = BuildGrovePiImpl(GrovePiAddress);
            return new LedBarSensor(device, pin);
        }

        private static GrovePi BuildGrovePiImpl(int address)
        {
            if (null != _device)
            {
                return _device;
            }

            /* Initialize the I2C bus */
            var settings = new I2cConnectionSettings(address)
            {
                BusSpeed = I2cBusSpeed.StandardMode
            };

            //Find the selector string for the I2C bus controller
            var aqs = I2cDevice.GetDeviceSelector(I2CName);

            _device = Task.Run(async () =>
            {
                //Find the I2C bus controller device with our selector string
                var dis = await DeviceInformation.FindAllAsync(aqs);
                // Create an I2cDevice with our selected bus controller and I2C settings
                var device = await I2cDevice.FromIdAsync(dis[0].Id, settings);
                return new GrovePi(device);
            }).Result;
            return _device;
        }
    }
}