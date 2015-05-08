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

            var readBuffer = new byte[1];
            _device.DirectAccess.Read(readBuffer);
            return readBuffer[1]*256 + readBuffer[2];
        }
    }
}