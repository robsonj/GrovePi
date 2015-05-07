using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace GrovePi
{
    public static class DeviceBuilder
    {
        private const string I2CName = "I2C1"; /* For Raspberry Pi 2, use I2C1 */
        private const byte GrovePiAddress = 0x04;

        public static IGrovePi BuildGrovePi()
        {
            return BuildGrovePi(GrovePiAddress);
        }

        public static IGrovePi BuildGrovePi(int address)
        {
            /* Initialize the I2C bus */
            var settings = new I2cConnectionSettings(address)
            {
                BusSpeed = I2cBusSpeed.StandardMode
            };

            //Find the selector string for the I2C bus controller
            var aqs = I2cDevice.GetDeviceSelector(I2CName);

            return Task.Run(async () =>
            {
                //Find the I2C bus controller device with our selector string
                var dis = await DeviceInformation.FindAllAsync(aqs);
                // Create an I2cDevice with our selected bus controller and I2C settings
                var device = await I2cDevice.FromIdAsync(dis[0].Id, settings);
                return (IGrovePi) new GrovePi(device);
            }).Result;
        }
    }
}