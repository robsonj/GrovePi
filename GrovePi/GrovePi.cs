using System;
using Windows.Devices.I2c;

namespace GrovePi
{
    public interface IGrovePi
    {
        string GetFirmwareVersion();
        byte DigitalRead(Pin pin);
        void DigitalWrite(Pin pin, byte value);
        int AnalogRead(Pin pin);
        void AnalogWrite(Pin pin);
        void PinMode(Pin pin, PinMode mode);
    }

    internal sealed class GrovePi : IGrovePi
    {
        private const byte Unused = 0;
        private readonly I2cDevice _device;

        internal GrovePi(I2cDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
        }

        public string GetFirmwareVersion()
        {
            var buffer = new[] {(byte) Command.Version, Unused, Unused, Unused};
            _device.Write(buffer);
            _device.Read(buffer);
            return $"{buffer[1]}.{buffer[2]}.{buffer[3]}";
        }

        public byte DigitalRead(Pin pin)
        {
            var buffer = new[] {(byte) Command.DigitalRead, (byte) pin, Unused, Unused};
            _device.Write(buffer);

            var readBuffer = new byte[1];
            _device.Read(readBuffer);
            return readBuffer[0];
        }

        public void DigitalWrite(Pin pin, byte value)
        {
            var buffer = new[] {(byte) Command.DigitalWrite, (byte) pin, value, Unused};
            _device.Write(buffer);
        }

        public int AnalogRead(Pin pin)
        {
            var buffer = new[] {(byte) Command.DigitalRead, (byte) Command.AnalogRead, (byte) pin, Unused, Unused};
            _device.Write(buffer);
            _device.Read(buffer);
            return buffer[1]*256 + buffer[2];
        }

        public void AnalogWrite(Pin pin)
        {
            var buffer = new[] {(byte) Command.AnalogWrite, (byte) pin, Unused, Unused};
            _device.Write(buffer);
        }

        public void PinMode(Pin pin, PinMode mode)
        {
            var buffer = new[] {(byte) Command.PinMode, (byte) pin, (byte) mode, Unused};
            _device.Write(buffer);
        }

        private enum Command
        {
            DigitalRead = 1,
            DigitalWrite = 2,
            AnalogRead = 3,
            AnalogWrite = 4,
            PinMode = 5,
            Version = 8
        };
    }
}