using System;

namespace GrovePi.Sensors
{
    public interface IBuzzer
    {
        SensorStatus CurrentState { get; }
        IBuzzer ChangeState(BuzzerStatus newState);
    }

    public enum BuzzerStatus
    {
        Off = 0,
        On = 1
    }

    internal class Buzzer : Sensor, IBuzzer
    {

        internal Buzzer(IGrovePi device, Pin pin) : base(device,pin,PinMode.Output)
        {
        }

        public IBuzzer ChangeState(BuzzerStatus newState)
        {
            Device.DigitalWrite(Pin, (byte) newState);
            return this;
        }
    }
}