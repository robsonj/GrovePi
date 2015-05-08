using System;

namespace GrovePi.Sensors
{
    public interface ITemperatureAndHumiditySensor
    {
        double Temperature();
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

        public double Temperature()
        {
            var result = (double)_device.AnalogRead(_pin);
            var resistance = (1023 - result)*10000/result;
            return (float) (1/(Math.Log(resistance/10000)/(int) _model + 1/298.15) - 273.15);
        }
    }
}

//186 # Read temp in Celsius from Grove Temperature Sensor 
//187 def temp(pin, model = '1.0'): 
//188 	# each of the sensor revisions use different thermistors, each with their own B value constant 
//189 	if model == '1.2': 
//190 		bValue = 4250  # sensor v1.2 uses thermistor ??? (assuming NCP18WF104F03RC until SeeedStudio clarifies) 
//191 	elif model == '1.1': 
//192 		bValue = 4250  # sensor v1.1 uses thermistor NCP18WF104F03RC 
//193 	else: 
//194 		bValue = 3975  # sensor v1.0 uses thermistor TTC3A103*39H 
//195 	a = analogRead(pin)
//196 	resistance = (float)(1023 - a) * 10000 / a 
//197 	t = (float)(1 / (math.log(resistance / 10000) / bValue + 1 / 298.15) - 273.15) 
//198 	return t