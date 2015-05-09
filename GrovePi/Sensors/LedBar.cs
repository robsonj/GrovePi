using System;

namespace GrovePi.Sensors
{
    public interface ILedBar
    {
        ILedBar Initialize(Orientation orientation);
        ILedBar SetOrientation(Orientation orientation);
        ILedBar SetLevel(int level);
        ILedBar SetLed(int level, int led, State state);
        ILedBar ToggleLed(int led);
    }

    internal class LedBar : ILedBar
    {
        private const byte InitialiseCommandAddress = 50;
        private const byte OrientationCommandAddress = 51;
        private const byte LevelCommandAddress = 52;
        private const byte SetOneCommandAddress = 53;
        private const byte ToggleOneCommandAddress = 54;
        //private const byte SetCommandAddress = 55;
        //private const byte GetCommandAddress = 56;

        private readonly GrovePi _device;
        private readonly Pin _pin;

        internal LedBar(GrovePi device, Pin pin)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
            _pin = pin;
        }

        public ILedBar Initialize(Orientation orientation)
        {
            var buffer = new byte[] {InitialiseCommandAddress, (byte) _pin, (byte) orientation, Constants.Unused};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public ILedBar SetOrientation(Orientation orientation)
        {
            var buffer = new byte[] {OrientationCommandAddress, (byte) _pin, (byte) orientation, Constants.Unused};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public ILedBar SetLevel(int level)
        {
            level = Math.Min(level, 10);
            var buffer = new byte[] {LevelCommandAddress, (byte) _pin, (byte) level, Constants.Unused};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public ILedBar SetLed(int level, int led, State state)
        {
            var buffer = new[] {SetOneCommandAddress, (byte) _pin, (byte) led, (byte) state};
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public ILedBar ToggleLed(int led)
        {
            var buffer = new byte[] {ToggleOneCommandAddress, (byte) _pin, (byte) led, Constants.Unused};
            _device.DirectAccess.Write(buffer);
            return this;
        }
    }

    public enum Orientation
    {
        RedToGreen = 0,
        GreenToRed = 1
    }

    public enum State
    {
        On = 0,
        Off = 1
    }
}