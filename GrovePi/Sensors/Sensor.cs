using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrovePi.Sensors
{
    public abstract class Sensor
    {
        protected readonly IGrovePi Device;
        protected readonly Pin Pin;

        internal Sensor(IGrovePi device, Pin pin, PinMode pinMode)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            device.PinMode(Pin, pinMode);
            Device = device;
            Pin = pin;
        }

        public SensorStatus CurrentState => (SensorStatus)Device.DigitalRead(Pin);
    }

    public enum SensorStatus
    {
        Off = 0,
        On = 1,
    }
}
