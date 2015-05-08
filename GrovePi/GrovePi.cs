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
        void AnalogWrite(Pin pin, byte value);
        void PinMode(Pin pin, PinMode mode);
        int UltrasonicRead(Pin pin);
        byte[] AccelerometerRead();
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
            var buffer = new[] { (byte)Command.Version, Unused, Unused, Unused };
            _device.Write(buffer);
            _device.Read(buffer);
            return $"{buffer[1]}.{buffer[2]}.{buffer[3]}";
        }

        public byte DigitalRead(Pin pin)
        {
            var buffer = new[] { (byte)Command.DigitalRead, (byte)pin, Unused, Unused };
            _device.Write(buffer);

            var readBuffer = new byte[1];
            _device.Read(readBuffer);
            return readBuffer[0];
        }

        public void DigitalWrite(Pin pin, byte value)
        {
            var buffer = new[] { (byte)Command.DigitalWrite, (byte)pin, value, Unused };
            _device.Write(buffer);
        }

        public int AnalogRead(Pin pin)
        {
            var buffer = new[] { (byte)Command.DigitalRead, (byte)Command.AnalogRead, (byte)pin, Unused, Unused };
            _device.Write(buffer);

            var readBuffer = new byte[1];
            _device.Read(readBuffer);
            return readBuffer[1] * 256 + readBuffer[2];
        }

        public void AnalogWrite(Pin pin, byte value)
        {
            var buffer = new[] { (byte)Command.AnalogWrite, (byte)pin, value, Unused };
            _device.Write(buffer);
        }

        public void PinMode(Pin pin, PinMode mode)
        {
            var buffer = new[] { (byte)Command.PinMode, (byte)pin, (byte)mode, Unused };
            _device.Write(buffer);
        }

        public int UltrasonicRead(Pin pin)
        {
            var buffer = new[] { (byte)Command.UltrasonicRead, (byte)pin, Unused, Unused };
            _device.Write(buffer);

            var readBuffer = new byte[1];
            _device.Read(readBuffer);
            return readBuffer[1] * 256 + readBuffer[2];
        }

        public byte[] AccelerometerRead()
        {
            var buffer = new[] { (byte)Command.AccelerometerRead, Unused, Unused, Unused };
            _device.Write(buffer);

            var readBuffer = new byte[1];
            _device.Read(readBuffer);

            if (readBuffer[1] > 32)
                readBuffer[1] = (byte)-(readBuffer[1] - 224);
            if (readBuffer[2] > 32)
                readBuffer[2] = (byte)-(readBuffer[1] - 224);
            if (readBuffer[3] > 32)
                readBuffer[3] = (byte)-(readBuffer[1] - 224);

            return readBuffer;
        }

        private enum Command
        {
            DigitalRead = 1,
            DigitalWrite = 2,
            AnalogRead = 3,
            AnalogWrite = 4,
            PinMode = 5,
            UltrasonicRead = 7,
            Version = 8,
            AccelerometerRead = 20,
            //RtcGetTime = 30,
            //DhtProSensorTemp = 40,
            //LedBarInitialise = 50,
            //LedBarOrientation = 51,
            //LedBarLevel = 52,
            //LedBarSetOne = 53,
            //LedBarToggleOne = 54,
            //LedBarSet = 55,
            //LedBarGet = 56,
            //FourDigitInitialise = 70,
            //FourDigitBrightness = 71,
            //FourDigitValue = 72,
            //FourDigitValueZeros = 73,
            //FourDigitIndividualDigit = 74,
            //FourDigitIndividualLeds = 75,
            //FourDigitScore = 76,
            //FourDigitAnalogRead = 77,
            //FourDigitAllOn = 78,
            //FourDigitAllOff = 79,
            //StoreColor = 90,
            //ChainableRgbLedInit = 91,
            //ChainableRgbLedTest = 92,
            //ChainableRgbLedSetPattern = 93,
            //ChainableRgbLedSetModulo = 94,
            //ChainableRgbLedSetLevel = 95

        };
    }
}