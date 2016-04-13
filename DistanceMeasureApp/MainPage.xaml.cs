using DistanceMeasureApp.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DistanceMeasureApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly DistanceSensor _distance = new DistanceSensor((int)GpioPins.GpioPin_5, (int)GpioPins.GpioPin_6);
        private readonly DispatcherTimer _timer;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, object e)
        {
            distanceLabel.Text = String.Format("Distance: {0} cm", ((long)_distance.GetDistance()).ToString());
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }
    }
}
