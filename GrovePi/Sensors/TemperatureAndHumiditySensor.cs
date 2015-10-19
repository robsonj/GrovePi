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

    public enum TemperatureAndHumiditySensorModel
    {
        DHT11, // Temperature and Humidity Sensor
        DHT22  // Temperature and Humidity Sensor Pro (AM2302)
    }

    /// <summary>
    /// A grove temperature and humidity sensor. Currently supported sensor models can be specified by the TemperatureAndHumiditySensorModel
    /// </summary>
    internal class TemperatureAndHumiditySensor : ITemperatureAndHumiditySensor
    {
        private readonly GrovePi _device;
        private readonly TemperatureAndHumiditySensorModel _model;
        private readonly Pin _pin;


        internal TemperatureAndHumiditySensor(GrovePi device, Pin pin, TemperatureAndHumiditySensorModel model)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
            _pin = pin;
            _model = model;
        }

        public TemperatureAndHumiditySensorValue TemperatureAndHumidity()
        {
            switch(_model)
            {
                case TemperatureAndHumiditySensorModel.DHT11:
                    return ReadDHTValue((byte) 0);

                case TemperatureAndHumiditySensorModel.DHT22:
                    return ReadDHTValue((byte)1);

                default:
                    throw new NotSupportedException("Unsupported TemperatureAndHumidity Sensor");
            }
        }

        /// <summary>
        /// Reads data from a DHT sensor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private TemperatureAndHumiditySensorValue ReadDHTValue(byte model)
        {
            var buffer = new[] { (byte)40, (byte)_pin, model, Constants.Unused };
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