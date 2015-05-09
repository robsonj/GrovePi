﻿using System;

namespace GrovePi.Sensors
{
    public interface IFourDigitDisplay
    {
        IFourDigitDisplay Initialise();
        IFourDigitDisplay SetBrightness(int brightness);
        IFourDigitDisplay SetIndividualSegment(int segment, int value);
        IFourDigitDisplay SetLedsOfSegment(int segment, int leds);
        IFourDigitDisplay SetScore(int left, int right);
        IFourDigitDisplay AllOn();
        IFourDigitDisplay AllOff();
    }

    public class FourDigitDisplay : IFourDigitDisplay
    {
        private const byte InitialiseCommandAddress = 70;
        private const byte BrightnessCommandAddress = 71;
        private const byte ValueCommandAddress = 72;
        private const byte ValueZerosCommandAddress = 73;
        private const byte IndividualDigitCommandAddress = 74;
        private const byte IndividualLedsCommandAddress = 75;
        private const byte ScoreCommandAddress = 76;
        private const byte AnalogReadCommandAddress = 77;
        private const byte AllOnCommandAddress = 78;
        private const byte AllOffCommandAddress = 79;

        private readonly GrovePi _device;
        private readonly Pin _pin;

        internal FourDigitDisplay(GrovePi device, Pin pin)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            _device = device;
            _pin = pin;
        }

        public IFourDigitDisplay Initialise()
        {
            var buffer = new byte[] { InitialiseCommandAddress, (byte)_pin, Constants.Unused, Constants.Unused };
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IFourDigitDisplay SetBrightness(int brightness)
        {
            brightness = Math.Min(brightness, 7);
            var buffer = new byte[] { InitialiseCommandAddress, (byte)_pin, (byte)brightness, Constants.Unused };
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IFourDigitDisplay SetIndividualSegment(int segment, int value)
        {
            var buffer = new byte[] { IndividualDigitCommandAddress, (byte)_pin, (byte)segment, (byte)value };
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IFourDigitDisplay SetLedsOfSegment(int segment, int leds)
        {
            var buffer = new byte[] { IndividualLedsCommandAddress, (byte)_pin, (byte)segment, (byte)leds };
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IFourDigitDisplay SetScore(int left, int right)
        {
            var buffer = new byte[] { ScoreCommandAddress, (byte)_pin, (byte)left, (byte)right };
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IFourDigitDisplay AllOn()
        {
            var buffer = new byte[] { AllOnCommandAddress, (byte)_pin, Constants.Unused, Constants.Unused };
            _device.DirectAccess.Write(buffer);
            return this;
        }

        public IFourDigitDisplay AllOff()
        {
            var buffer = new byte[] { AllOffCommandAddress, (byte)_pin, Constants.Unused, Constants.Unused };
            _device.DirectAccess.Write(buffer);
            return this;
        }
    }
}
