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
        }

        //MOVE TO sensor_VLP_class or UDP_receive
        static int frames_number = 80;
        byte[] frame = new byte[(frames_number+1) * 1206];
        int i = 0;

        private void collect_data()
        {
            Array.Copy(UDP_FROM_VLP16.buffer_frame_static, 0, frame, i * 1206, 1206);
            if (i == frames_number)
            {
                VLP16.frame_VLP16_decode(frame);
                i = 0;
                Array.Clear(frame,0, (frames_number + 1) * 1206);
            }
            i++;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string[] dupa = { "kon", "koc", "baca" };
            int[] dupaX = { 1, 2, 3, 5};

            dataGrid.AutoGenerateColumns = false;
            var col = new DataGridTextColumn();
            var binding = new Binding("kon");
            col.Binding = binding;
            dataGrid.Columns.Add(col);
            dataGrid.ItemsSource = dupaX;
   
        }

    }
}
