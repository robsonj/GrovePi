using System;

namespace GrovePi.Sensors
{
    public interface ITemperatureAndHumiditySensor
    {
        double TemperatureInCelcius();
    }

    public enum Model
    {
        OnePointZero = 3975,
        OnePointOne = 4250,
        OnePointTwo = 4250
    }

    internal class TemperatureAndHumiditySensor : ITemperatureAndHumiditySensor
    {
        private readonly IGrovePi _device;
        private readonly Pin _pin;
        private readonly Model _model;

        internal TemperatureAndHumiditySensor(IGrovePi device, Pin pin, Model model)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
            _pin = pin;
            _model = model;
        }

        public double TemperatureInCelcius()
        {
            var result = (double)_device.AnalogRead(_pin);
            var resistance = (1023 - result)*10000/result;
            return (float) (1/(Math.Log(resistance/10000)/(int) _model + 1/298.15) - 273.15);
        }
    }
}