using System;
using Windows.Devices.I2c;

namespace GrovePi
{
    public interface IGrovePi
    {
        string GetFirmwareVersion();
    }


    internal sealed class GrovePi : IGrovePi
    {
        private const byte VersionCommandAddress = 8;
        private readonly I2cDevice _device;

        internal GrovePi(I2cDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
        }

        /// <summary>
        ///     Read the firmware version
        /// </summary>
        public string GetFirmwareVersion()
        {
            var buffer = new byte[] {VersionCommandAddress, 0, 0, 0};
            _device.Write(buffer);
            _device.Read(buffer);
            return $"{buffer[1]}.{buffer[2]}.{buffer[3]}";
        }
    }
}