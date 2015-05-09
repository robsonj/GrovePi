using System;

namespace GrovePi.Sensors
{
    public interface IChainableRgbLed
    {
        IChainableRgbLed Initialise(int NumberOfLeds);
        IChainableRgbLed StoreColor(int red, int green, int blue);
        IChainableRgbLed Test(int NumberOfLeds, int TestColor);
        IChainableRgbLed SetPattern(int Pattern, int Led);
        IChainableRgbLed Mudulo(int Offset, int Divisor);
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

        public IChainableRgbLed Initialise(int NumberOfLeds)
        {
            var buffer = new byte[] {InitialiseCommandAddress, (byte) _pin, (byte) NumberOfLeds, Constants.Unused};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IChainableRgbLed StoreColor(int red, int green, int blue)
        {
            var buffer = new[] {StoreColorCommandAddress, (byte) red, (byte) green, (byte) blue};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IChainableRgbLed Test(int NumberOfLeds, int TestColor)
        {
            var buffer = new[] {TestCommandAddress, (byte) _pin, (byte) NumberOfLeds, (byte) TestColor};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IChainableRgbLed SetPattern(int Pattern, int Led)
        {
            var buffer = new[] {SetPatternCommandAddress, (byte) _pin, (byte) Pattern, (byte) Led};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IChainableRgbLed Mudulo(int Offset, int Divisor)
        {
            var buffer = new[] {SetModuloCommandAddress, (byte) _pin, (byte) Offset, (byte) Divisor};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IChainableRgbLed SetLevel(int Level, bool Reverse)
        {
            var buffer = new[] {(byte) Level, (byte) _pin, (byte) Level, Reverse ? (byte) 1 : (byte) 0};
            _device.DirectAccess.Write(buffer);
            return this;
        }
    }
}