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
        public const byte StoreColorCommandAddress = 90;
        public const byte InitialiseCommandAddress = 91;
        public const byte TestCommandAddress = 92;
        public const byte SetPatternCommandAddress = 93;
        public const byte SetModuloCommandAddress = 94;
        public const byte SetLevelCommmandAddress = 95;

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
            var buffer = new byte[] { InitialiseCommandAddress, (byte)_pin, (byte)NumberOfLeds, Constants.Unused };
            _device.DirectAccess.Write(buffer);
        }

        public void StoreColor(int red, int green, int blue)
        {
            var buffer = new byte[] { StoreColorCommandAddress, (byte)red, (byte)green, (byte)blue };
            _device.DirectAccess.Write(buffer);
        }

        public void Test(int NumberOfLeds, int TestColor)
        {
            var buffer = new byte[] { TestCommandAddress, (byte)_pin, (byte)NumberOfLeds, (byte)TestColor };
            _device.DirectAccess.Write(buffer);
        }

        public void SetPattern(int Pattern, int Led)
        {
            var buffer = new byte[] { SetPatternCommandAddress, (byte)_pin, (byte)Pattern, (byte)Led };
            _device.DirectAccess.Write(buffer);
        }

        public void Mudulo(int Offset, int Divisor)
        {
            var buffer = new byte[] { SetModuloCommandAddress, (byte)_pin, (byte)Offset, (byte)Divisor };
            _device.DirectAccess.Write(buffer);
        }

        public void SetLevel(int Level, bool Reverse)
        {
            var buffer = new byte[] { (byte)Level, (byte)_pin, (byte)Level, Reverse ? (byte)1 : (byte)0 };
            _device.DirectAccess.Write(buffer);
        }
    }
}
