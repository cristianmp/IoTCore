using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceMeasureApp.Modules
{

    /// <summary>
    /// Available Gpio Pins. Refer: https://ms-iot.github.io/content/en-US/win10/samples/PinMappingsRPi2.htm
    /// </summary>
    public enum GpioPins : int
    {
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 29
        /// </summary>
        GpioPin_5 = 5,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 31
        /// </summary>
        GpioPin_6 = 6,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 32
        /// </summary>
        GpioPin_12 = 12,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 33
        /// </summary>
        GpioPin_13 = 13,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 36
        /// </summary>
        GpioPin_16 = 16,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 12
        /// </summary>
        GpioPin_18 = 18,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 15
        /// </summary>
        GpioPin_22 = 22,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 16
        /// </summary>
        GpioPin_23 = 23,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 18
        /// </summary>
        GpioPin_24 = 24,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 22
        /// </summary>
        GpioPin_25 = 25,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 37
        /// </summary>
        GpioPin_26 = 26,
        /// <summary>
        /// Raspberry Pi 2 - Header Pin Number : 13
        /// </summary>
        GpioPin_27 = 27
    }
}
