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
            return DoBuild(x => new Led(x, pin));
        }

        public static ITemperatureAndHumiditySensor BuildTemperatureAndHumiditySensor(Pin pin, Model model)
        {
            return DoBuild(x => new TemperatureAndHumiditySensor(x, pin, model));
        }

        public static IUltrasonicRangerSensor BuildUltraSonicSensor(Pin pin)
        {
            return DoBuild(x => new UltrasonicRangerSensor(x, pin));
        }

        public static IAccelerometerSensor BuildAccelerometer(Pin pin)
        {
            return DoBuild(x => new AccelerometerSensor(x, pin));
        }

        public static IRtcSensor BuildRtcSensor(Pin pin)
        {
            return DoBuild(x => new RtcSensor(x, pin));
        }

        public static ILedBar BuildLedBarSensor(Pin pin)
        {
            return DoBuild(x => new LedBar(x, pin));
        }

        public static IFourDigitDisplay BuildFourDigitDisplaySensor(Pin pin)
        {
            return DoBuild(x => new FourDigitDisplay(x, pin));
        }

        public static IChainableRgbLed ChainableRgbLed(Pin pin)
        {
            return DoBuild(x => new ChainableRgbLed(x, pin));
        }

        private static TSensor DoBuild<TSensor>(Func<GrovePi, TSensor> factory)
        {
            var device = BuildGrovePiImpl(GrovePiAddress);
            return factory(device);
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