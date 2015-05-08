namespace GrovePi.Sensors
{
    public interface IUltrasonicRangerSensor
    {
        int ReadDistance();
    }

    internal class UltrasonicRangerSensor : IUltrasonicRangerSensor
    {
        private const byte CommandAddress = 7;
        private readonly GrovePi _device;
        private readonly Pin _pin;

        internal UltrasonicRangerSensor(GrovePi device, Pin pin)
        {
            _device = device;
            _pin = pin;
        }

        public int ReadDistance()
        {
            var buffer = new byte[] {CommandAddress, (byte) _pin, Constants.Unused, Constants.Unused};
            _device.DirectAccess.Write(buffer);
            _device.DirectAccess.Read(buffer);
            return buffer[1]*256 + buffer[2];
        }
    }
}