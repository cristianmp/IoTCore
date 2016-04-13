using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;

namespace DistanceMeasureApp.Modules
{
    public class DistanceSensor
    {
        private const int MIN_DISTANCE = 5;
        private const int MAX_DISTANCE = 400;

        private const int MaxFlag = -1;
        private const int MinFlag = -2;

        private readonly int TicksPerMicrosecond = (int)(TimeSpan.TicksPerMillisecond / 1000);

        GpioController gpio = GpioController.GetDefault();

        GpioPin Trigger;
        GpioPin Echo;

        /// <summary>
        /// Number of errors that can be accumulated in the GetDistanceInCentimeters function before the function returns an error value;
        /// </summary>
        public int AcceptableErrorRate = 10;

        /// <summary>
        /// Error that will be returned if the sensor fails to get an accurate reading after the AcceptableErrorRate has been reached.
        /// </summary>
        public readonly int SENSOR_ERROR = -1;

        /// <summary>Constructor</summary>
        /// <param name="triggerPin">The socket pin that this module is plugged in to.</param>
        /// <param name="echoPin">The socket pin that this module is plugged in to.</param>
        public DistanceSensor(int triggerPin, int echoPin)
        {
            this.Trigger = gpio.OpenPin(triggerPin);
            this.Echo = gpio.OpenPin(echoPin);

            this.Trigger.SetDriveMode(GpioPinDriveMode.Output);
            this.Echo.SetDriveMode(GpioPinDriveMode.Input);

            this.Trigger.Write(GpioPinValue.Low);
        }

        /// <summary>
        /// Takes a number of measurements, averaging the distance, and returns the detected distance in centimeters.
        /// </summary>
        /// <param name="numMeasurements">The number of measurements to take and average before returning a value.</param>
        /// <returns>Distance that the module has detected an object in front of it. Will return SENSOR_ERROR if the number of errors 
        /// was reached, which can be caused by an object either being too close or too far from the sensor.</returns>
        public long GetDistance(int numMeasurements = 1)
        {
            long measuredValue = 0;
            long measuredAverage = 0;
            int errorCount = 0;

            for (int i = 0; i < numMeasurements; i++)
            {
                measuredValue = GetDistanceHelper();

                if (measuredValue != MaxFlag || measuredValue != MinFlag)
                {
                    measuredAverage += measuredValue;
                }
                else
                {
                    errorCount++;
                    i--;

                    if (errorCount > AcceptableErrorRate)
                    {
                        return SENSOR_ERROR;
                    }
                }

            }

            measuredAverage /= numMeasurements;
            return measuredAverage;
        }

        private long GetDistanceHelper()
        {
            var mre = new ManualResetEventSlim(false);

            long start = 0;
            long microseconds = 0;
            long time = 0;
            long distance = 0;
            var sw = new Stopwatch();

            //Send a 10µs pulse to start the measurement
            Trigger.Write(GpioPinValue.High);
            mre.Wait(TimeSpan.FromTicks(100));
            Trigger.Write(GpioPinValue.Low);

            int error = 0;
            while (Echo.Read() != GpioPinValue.High)
            {
                error++;
                if (error > 1000)
                    break;
                mre.Wait(TimeSpan.FromTicks(0));
            }

            start = DateTime.Now.Ticks;
            sw.Start();

            while (Echo.Read() == GpioPinValue.High)
                mre.Wait(TimeSpan.FromTicks(0));

            time = (DateTime.Now.Ticks - start);
            sw.Stop();
            microseconds = sw.Elapsed.Ticks / TicksPerMicrosecond;

            distance = (microseconds / 58);
            distance += 2;

            if (distance < MAX_DISTANCE)
            {
                if (distance >= MIN_DISTANCE)
                    return distance;
                else
                    return MinFlag;
            }
            else
            {
                return MaxFlag;
            }
        }
    }
}
