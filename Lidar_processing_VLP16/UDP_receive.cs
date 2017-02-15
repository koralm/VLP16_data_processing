using System.Text;

//USING
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;

namespace Lidar_processing_VLP16
{
    public class UDP_receive
    {

        //instance
        private Socket sock { get; set; }

        //CONNECTION PARAMES
        private int PORT_NUMBER = 2368;
        private int BUFFER_SIZE = 1206 * 70;
        public byte[] buffer_frame = new byte[1206];
        public byte[] buffer_frame_static = new byte[1206];

        //received event
        public delegate void Event_recive_frame();
        public event Event_recive_frame frame_received;

        public void UDP_initialize_start()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            IPEndPoint VLP16_sensor = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            sock.Bind(VLP16_sensor);
            sock.ReceiveBufferSize = BUFFER_SIZE;
            ReceiveTest(sock);
        }


        public void ReceiveTest(Socket server)
        {
            server.BeginReceive(buffer_frame, 0, 1206, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), server);
        }

        private void Async_wait_for_frame(IAsyncResult result)
        {

            int bytesRead = sock.EndReceive(result);
            buffer_frame_static = buffer_frame;
            frame_received();
            sock.BeginReceive(buffer_frame, 0, 1206, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), sock);
        }

    }
}
