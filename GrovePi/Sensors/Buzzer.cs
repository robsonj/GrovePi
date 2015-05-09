using System;

namespace GrovePi.Sensors
{
    public interface IBuzzer
    {
        BuzzerStatus CurrentState { get; }
        IBuzzer ChangeState(BuzzerStatus newState);
    }

    public enum BuzzerStatus
    {
        Off = 0,
        On = 1
    }

    internal class Buzzer : IBuzzer
    {
        private readonly IGrovePi _device;
        private readonly Pin _pin;

        internal Buzzer(IGrovePi device, Pin pin)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            device.PinMode(_pin, PinMode.Output);
            _device = device;
            _pin = pin;
        }

        public IBuzzer ChangeState(BuzzerStatus newState)
        {
            _device.DigitalWrite(_pin, (byte) newState);
            return this;
        }

        public BuzzerStatus CurrentState => (BuzzerStatus) _device.DigitalRead(_pin);
    }
}