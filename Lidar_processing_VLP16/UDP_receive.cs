using System.Text;
using System.Windows.Documents;

//USING
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Lidar_processing_VLP16
{
    public class UDP_receive
    {

        //instance
        private Socket sock;

        //CONNECTION PARAMES
        private int PORT_NUMBER = 2368;
        private int BUFFER_SIZE = 1206 * 70;
        public byte[] buffer_frame = new byte[1206];
        public byte[] buffer_frame_static = new byte[1206];
        public byte[] frameARR = new byte[(80) * 1206];

        private byte[] frame = new byte[(80 + 1) * 1206];
        private List<byte[]> frameL = new List<byte[]>();
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
            sock.BeginReceive(buffer_frame, 0, 1206, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), null);
            //ReceiveTest(sock);
        }


        public void ReceiveTest(Socket server)
        {
            server.BeginReceive(buffer_frame, 0, 1206, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), server);
        }

        private void Async_wait_for_frame(IAsyncResult result)
        {
            int bytesRead = sock.EndReceive(result);
            if (bytesRead > 0)
            {
                collect_frames(buffer_frame);
                sock.BeginReceive(buffer_frame, 0, 1206, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), sock);
            }
        }

        void collect_frames(byte[] frame)
        {
            frameL.Add(frame);
            Array.Copy(frame, 0, frameARR, ((frameL.Count - 1) * 1206), 1206);

            if (frameL.Count == 80)
            {
                frame_received();
                frameL.Clear();
            }

        }
    }

}
