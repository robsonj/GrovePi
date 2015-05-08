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
    }

    internal sealed class GrovePi : IGrovePi
    {
        internal GrovePi(I2cDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            DirectAccess = device;
        }

        internal I2cDevice DirectAccess { get; }

        public string GetFirmwareVersion()
        {
            var buffer = new byte[] {(byte) Command.Version, Constants.Unused, Constants.Unused, Constants.Unused};
            DirectAccess.Write(buffer);
            DirectAccess.Read(buffer);
            return $"{buffer[1]}.{buffer[2]}.{buffer[3]}";
        }

        public byte DigitalRead(Pin pin)
        {
            var buffer = new byte[] {(byte) Command.DigitalRead, (byte) pin, Constants.Unused, Constants.Unused};
            DirectAccess.Write(buffer);

            var readBuffer = new byte[1];
            DirectAccess.Read(readBuffer);
            return readBuffer[0];
        }

        public void DigitalWrite(Pin pin, byte value)
        {
            var buffer = new byte[] {(byte) Command.DigitalWrite, (byte) pin, value, Constants.Unused};
            DirectAccess.Write(buffer);
        }

        public int AnalogRead(Pin pin)
        {
            var buffer = new byte[]
            {(byte) Command.DigitalRead, (byte) Command.AnalogRead, (byte) pin, Constants.Unused, Constants.Unused};
            DirectAccess.Write(buffer);

            var readBuffer = new byte[1];
            DirectAccess.Read(readBuffer);
            return readBuffer[1]*256 + readBuffer[2];
        }

        public void AnalogWrite(Pin pin, byte value)
        {
            var buffer = new byte[] {(byte) Command.AnalogWrite, (byte) pin, value, Constants.Unused};
            DirectAccess.Write(buffer);
        }

        public void PinMode(Pin pin, PinMode mode)
        {
            var buffer = new byte[] {(byte) Command.PinMode, (byte) pin, (byte) mode, Constants.Unused};
            DirectAccess.Write(buffer);
        }

        private enum Command
        {
            DigitalRead = 1,
            DigitalWrite = 2,
            AnalogRead = 3,
            AnalogWrite = 4,
            PinMode = 5,
            Version = 8
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