using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GrovePi.Sensors
{

    public class TemperatureAndHumiditySensorValue
    {
        private float temperature;
        public float Temperature
        {
            get
            {
                return temperature;
            }
        }

        private float humidity;
        public float Humidity
        {
            get
            {
                return humidity;
            }
        }

        public TemperatureAndHumiditySensorValue(float temperature, float humidity)
        {
            this.temperature = temperature;
            this.humidity = humidity;
        }
    }

    public interface ITemperatureAndHumiditySensor
    {
        TemperatureAndHumiditySensorValue TemperatureAndHumidity();
    }

    public enum Model
    {
        ZERO,
        ONE,
        TWO
    }

    internal class TemperatureAndHumiditySensor : ITemperatureAndHumiditySensor
    {
        private readonly GrovePi _device;
        private readonly Model _model;
        private readonly Pin _pin;

        internal TemperatureAndHumiditySensor(GrovePi device, Pin pin, Model model)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
            _pin = pin;
            _model = model;
        }

        public TemperatureAndHumiditySensorValue TemperatureAndHumidity()
        {
            var buffer = new[] { (byte)40, (byte)_pin, (byte)_model, Constants.Unused };
            _device.DirectAccess.Write(buffer);

            // sleep
            //Task.Delay(100).Wait();
            Task.Delay(600).Wait();

            buffer = new byte[9];
            _device.DirectAccess.Read(buffer);
            
            float temperature = System.BitConverter.ToSingle(buffer, 1);
            float humidity = System.BitConverter.ToSingle(buffer, 5);

            return new TemperatureAndHumiditySensorValue(temperature, humidity);

        }
        
    }
}