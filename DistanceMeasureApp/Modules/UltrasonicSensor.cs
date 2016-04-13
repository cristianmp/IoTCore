using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace DistanceMeasureApp.Modules
{
    public class UltrasonicSensor
    {
        GpioController gpio = GpioController.GetDefault();

        GpioPin TriggerPin;
        GpioPin EchoPin;

        public UltrasonicSensor(int TriggerPin, int EchoPin)
        {
            this.TriggerPin = gpio.OpenPin(TriggerPin);
            this.EchoPin = gpio.OpenPin(EchoPin);

            this.TriggerPin.SetDriveMode(GpioPinDriveMode.Output);
            this.EchoPin.SetDriveMode(GpioPinDriveMode.Input);

            this.TriggerPin.Write(GpioPinValue.Low);
        }

        public double GetDistance()
        {
            var mre = new ManualResetEventSlim(false);


            //Send a 10µs pulse to start the measurement
            TriggerPin.Write(GpioPinValue.High);
            mre.Wait(TimeSpan.FromTicks(100));
            TriggerPin.Write(GpioPinValue.Low);

            var time = PulseIn(EchoPin, GpioPinValue.High, 500);

            // multiply by speed of sound in milliseconds (34000) divided by 2 (cause pulse make rountrip)
            var distance = time * 17000;
            return distance;
        }

        private double PulseIn(GpioPin pin, GpioPinValue value, ushort timeout)
        {
            var sw = new Stopwatch();
            var sw_timeout = new Stopwatch();

            sw_timeout.Start();

            // Wait for pulse
            while (pin.Read() != value)
            {
                if (sw_timeout.ElapsedMilliseconds > timeout)
                    return 3.5;
            }
            sw.Start();

            // Wait for pulse end
            while (pin.Read() == value)
            {
                if (sw_timeout.ElapsedMilliseconds > timeout)
                    return 3.4;
            }
            sw.Stop();

            return sw.Elapsed.TotalSeconds;
        }
    }
}
