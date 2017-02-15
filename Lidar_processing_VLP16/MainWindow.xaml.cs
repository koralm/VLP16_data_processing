using System;
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
            byte A = 0x50;
            byte B = 0x6C;

            int teset = 0;

            //textBox.Text =  B.ToString("X") + A.ToString("X");
            teset = (B << 8) | A;
            textBox.Text = Convert.ToDouble(teset).ToString();
        }
    }
}
