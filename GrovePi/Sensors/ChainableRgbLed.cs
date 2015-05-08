using System;

namespace GrovePi.Sensors
{
    public interface IChainableRgbLed
    {
        void Initialise(int NumberOfLeds);
        void StoreColor(int red, int green, int blue);
        void Test(int NumberOfLeds, int TestColor);
        void SetPattern(int Pattern, int Led);
        void Mudulo(int Offset, int Divisor);
    }

    public class ChainableRgbLed : IChainableRgbLed
    {
        private readonly GrovePi _device;
        private readonly Pin _pin;

        internal ChainableRgbLed(GrovePi device, Pin pin)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
            _pin = pin;
        }

        public void Initialise(int NumberOfLeds)
        {
            var buffer = new byte[] { CommandAddress.Initialise, (byte)_pin, (byte)NumberOfLeds, Constants.Unused };
            _device.DirectAccess.Write(buffer);
        }

        public void StoreColor(int red, int green, int blue)
        {
            var buffer = new byte[] { CommandAddress.StoreColor, (byte)red, (byte)green, (byte)blue };
            _device.DirectAccess.Write(buffer);
        }

        public void Test(int NumberOfLeds, int TestColor)
        {
            var buffer = new byte[] { CommandAddress.Test, (byte)_pin, (byte)NumberOfLeds, (byte)TestColor };
            _device.DirectAccess.Write(buffer);
        }

        public void SetPattern(int Pattern, int Led)
        {
            var buffer = new byte[] { CommandAddress.SetPattern, (byte)_pin, (byte)Pattern, (byte)Led };
            _device.DirectAccess.Write(buffer);
        }

        public void Mudulo(int Offset, int Divisor)
        {
            var buffer = new byte[] { CommandAddress.SetPattern, (byte)_pin, (byte)Offset, (byte)Divisor };
            _device.DirectAccess.Write(buffer);
        }
    }

    class CommandAddress
    {
        public const byte StoreColor = 90;
        public const byte Initialise = 91;
        public const byte Test = 92;
        public const byte SetPattern = 93;
        public const byte SetModulo = 94;
        public const byte SetLevel = 95;
    }
}
