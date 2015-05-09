using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using GrovePi.Sensors;

namespace GrovePi
{
    public static class DeviceFactory
    {
        public static IBuildGroveDevices Build = new DeviceBuilder();
    }

    public interface IBuildGroveDevices
    {
        IGrovePi BuildGrovePi();
        IGrovePi BuildGrovePi(int address);
        ILed BuildLed(Pin pin);
        ITemperatureAndHumiditySensor BuildTemperatureAndHumiditySensor(Pin pin, Model model);
        IUltrasonicRangerSensor BuildUltraSonicSensor(Pin pin);
        IAccelerometerSensor BuildAccelerometerSensor(Pin pin);
        IRealTimeClock BuildRealTimeClock(Pin pin);
        ILedBar BuildLedBar(Pin pin);
        IFourDigitDisplay BuildFourDigitDisplay(Pin pin);
        IChainableRgbLed ChainableRgbLed(Pin pin);
        IRotaryAngleSensor BuildRotaryAngleSensor(Pin pin);
    }

    internal class DeviceBuilder : IBuildGroveDevices
    {
        private const string I2CName = "I2C1"; /* For Raspberry Pi 2, use I2C1 */
        private const byte GrovePiAddress = 0x04;
        private GrovePi _device;

        public IGrovePi BuildGrovePi()
        {
            return BuildGrovePiImpl(GrovePiAddress);
        }

        public IGrovePi BuildGrovePi(int address)
        {
            return BuildGrovePiImpl(address);
        }

        public ILed BuildLed(Pin pin)
        {
            return DoBuild(x => new Led(x, pin));
        }

        public ITemperatureAndHumiditySensor BuildTemperatureAndHumiditySensor(Pin pin, Model model)
        {
            return DoBuild(x => new TemperatureAndHumiditySensor(x, pin, model));
        }

        public IUltrasonicRangerSensor BuildUltraSonicSensor(Pin pin)
        {
            return DoBuild(x => new UltrasonicRangerSensor(x, pin));
        }

        public IAccelerometerSensor BuildAccelerometerSensor(Pin pin)
        {
            return DoBuild(x => new AccelerometerSensor(x, pin));
        }

        public IRealTimeClock BuildRealTimeClock(Pin pin)
        {
            return DoBuild(x => new RealTimeClock(x, pin));
        }

        public IRotaryAngleSensor BuildRotaryAngleSensor(Pin pin)
        {
            return DoBuild(x => new RotaryAngleSensor(x, pin));
        }

        public ILedBar BuildLedBar(Pin pin)
        {
            return DoBuild(x => new LedBar(x, pin));
        }

        public IFourDigitDisplay BuildFourDigitDisplay(Pin pin)
        {
            return DoBuild(x => new FourDigitDisplay(x, pin));
        }

        public IChainableRgbLed ChainableRgbLed(Pin pin)
        {
            return DoBuild(x => new ChainableRgbLed(x, pin));
        }

        private TSensor DoBuild<TSensor>(Func<GrovePi, TSensor> factory)
        {
            var device = BuildGrovePiImpl(GrovePiAddress);
            return factory(device);
        }

        private GrovePi BuildGrovePiImpl(int address)
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