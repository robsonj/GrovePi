using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrovePi.Sensors
{
    public interface ITemperatureSensor
    {
        double TemperatureInCelcius();
    }

    public enum TemperatureSensorModel
    {
        OnePointZero = 3975,
        OnePointOne = 4250,
        OnePointTwo = 4250
    }

    /// <summary>
    /// The grove temperature only sensor.  Supported versions are 1.0, 1.1 and 1.2. Specify the TemperatureSensorModel accordingly.
    /// </summary>
    internal class TemperatureSensor : ITemperatureSensor
    {
        private readonly IGrovePi _device;
        private readonly TemperatureSensorModel _model;
        private readonly Pin _pin;


        internal TemperatureSensor(IGrovePi device, Pin pin, TemperatureSensorModel model)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
            _pin = pin;
            _model = model;
        }


        public double TemperatureInCelcius()
        {
            var result = (double)_device.AnalogRead(_pin);
            var resistance = (1023 - result) * 10000 / result;
            return 1 / (Math.Log(resistance / 10000) / (int)_model + 1 / 298.15) - 273.15;
        }
    }
}
