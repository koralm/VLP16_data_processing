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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Lidar_processing_VLP16
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        sensor_VLP16 VLP16;
        UDP_receive UDP_FROM_VLP16;

        public MainWindow()
        {
            InitializeComponent();
            VLP16 = new sensor_VLP16();
            UDP_FROM_VLP16 = new UDP_receive();

            UDP_FROM_VLP16.UDP_initialize_start();
            UDP_FROM_VLP16.frame_received += collect_data;

            VLP16.frame_ready += update_gird;
        }

        //MOVE TO sensor_VLP_class or UDP_receive
        static int frames_number = 80;
        byte[] frame = new byte[(frames_number+1) * 1206];
        int i = 0;

        //private void collect_data()
        //{
        //    Array.Copy(UDP_FROM_VLP16.buffer_frame_static, 0, frame, i * 1206, 1206);
        //    if (i == frames_number)
        //    {
        //        VLP16.frame_VLP16_decode(frame);
        //        i = 0;
        //        Array.Clear(frame,0, (frames_number + 1) * 1206);
        //    }
        //    i++;
        //}

        private void collect_data()
        {
            VLP16.frame_VLP16_decode(UDP_FROM_VLP16.frameARR);
        }

        List<cloud_point> konik = new List<cloud_point>();

        private void update_gird()
        {
            konik = VLP16.cloudXC.cloudX.ToList();
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {

            
            var test = konik;

            //List<cloud_point> test = VLP16.cloudX;

            //var test = VLP16.azimuth_1DA;
            //var test1 = VLP16.X;

            //dataGrid.AutoGenerateColumns = false;
            //DataGridTextColumn col1 = new DataGridTextColumn();
            //col1.Binding = new Binding("ID");

            //DataGridTextColumn col2 = new DataGridTextColumn();
            //col2.Binding = new Binding("." + test1);

            //dataGrid.Columns.Add(col1);
            //dataGrid.Columns.Add(col2);

            //dataGrid.Items.Add(test);
            //dataGrid.Items.Add(test1);
            //dataGrid.ItemsSource = test;
            //dataGrid.Items.Clear();
            dataGrid.ItemsSource = test;
            dataGrid.Items.Refresh();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
        }
    }
}
