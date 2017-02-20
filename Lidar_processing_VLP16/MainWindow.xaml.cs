using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//USING
using System.Threading;

using SciChart.Charting3D.Model;
using SciChart.Examples.ExternalDependencies.Data;
using System.Timers;
using System.Windows.Media;

namespace Lidar_processing_VLP16
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        sensor_VLP16 VLP16_decoder;
        UDP_receive UDP_CONNECTION;

        public MainWindow()
        {
            InitializeComponent();
            VLP16_decoder = new sensor_VLP16();
            UDP_CONNECTION = new UDP_receive();

            OnStart();

            //INITIALIZE UDP COMMUNICATION
            UDP_CONNECTION.UDP_initialize_start();
            UDP_CONNECTION.frame_received += collect_data;

            //EVENT FRAME PROCESSED
            VLP16_decoder.frame_ready += update_gird;
        }



        private void collect_data()
        {
            VLP16_decoder.frame_VLP16_decode(UDP_CONNECTION.frame_full_360);
        }

        List<cloud_point> konik = new List<cloud_point>();


        private void update_gird()
        {
            konik = VLP16_decoder.cloudX.cloudX.ToList();


            //UPDATE PLOT
            using (_xyzData.SuspendUpdates())
            {
                var random = new Random(14);
                Color? randomColor = Color.FromArgb(0xFF, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));
                for (int i = 0; i < _xyzData.Count; i++)
                {
                    try
                    {
                        double currentX = VLP16_decoder.X[i];
                        double currentY = VLP16_decoder.Z[i];
                        double currentZ = VLP16_decoder.Y[i];

                        _xyzData.Update(i, currentX, currentY, currentZ, new PointMetadata3D(randomColor));
                    }
                    catch (Exception ex) { }
                }
            }

        }




        private void button_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = konik.ToList();
            dataGrid.Items.Refresh();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
        }

        /////
        // DARWAING CHART TEST
        private XyzDataSeries3D<double> _xyzData;
        private System.Timers.Timer _timer;
        private int _pointCount = 384 * 77;

        private void OnStart()
        {
            //StartButton.IsChecked = true;
            //PauseButton.IsChecked = false;
            //ResetButton.IsChecked = false;

            //if (ScatterRenderableSeries3D.DataSeries == null)
            // {
            _xyzData = new XyzDataSeries3D<double>();
            ScatterRenderableSeries3D.DataSeries = _xyzData;
            //}

            //if (_timer == null)
            //{
            //    _timer = new System.Timers.Timer(50);
            //     _timer.Elapsed += OnTimerTick;
            // }

            //  _timer.Start();

            // First load, fill with some random values                    
            if (_xyzData.Count == 0)
            {
                for (int i = 0; i < _pointCount; i++)
                {
                    double x = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                    double y = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                    double z = DataManager.Instance.GetGaussianRandomNumber(50, 15);

                    _xyzData.Append(x, y, z);
                }

                return;
            }
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            //lock (_timer)
            //{
            //    Random r = new Random();

            //    // First load, fill with some random values                    
            //    if (_xyzData.Count == 0)
            //    {
            //        for (int i = 0; i < _pointCount; i++)
            //        {
            //            double x = DataManager.Instance.GetGaussianRandomNumber(50, 15);
            //            double y = DataManager.Instance.GetGaussianRandomNumber(50, 15);
            //            double z = DataManager.Instance.GetGaussianRandomNumber(50, 15);

            //            _xyzData.Append(x, y, z);
            //        }

            //        return;
            //    }

            // Subsequent load, update point positions using a sort of brownian motion by using random
            // numbers between -0.5, +0.5



            //}
        }

        private void OnPause()
        {
            _timer.Stop();

            StartButton.IsChecked = false;
            PauseButton.IsChecked = true;
            ResetButton.IsChecked = false;
        }

        private void OnReset()
        {
            _timer.Stop();

            StartButton.IsChecked = false;
            PauseButton.IsChecked = false;
            ResetButton.IsChecked = true;

            using (sciChart.SuspendUpdates())
            {
                ScatterRenderableSeries3D.DataSeries = null;
                sciChart.InvalidateElement();

                ScatterRenderableSeries3D.GetSceneEntity().Update();
                ScatterRenderableSeries3D.GetSceneEntity().RootSceneEntity.Update();
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            OnStart();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            OnPause();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            OnReset();
        }

        private void CreateRealTime3DPointCloudChart_OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= OnTimerTick;
                _timer = null;
            }
        }
    }
}
