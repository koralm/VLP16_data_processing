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

        //DATA PACKET NUMBER
        const int DATA_PACKET_NUMBER = 77;
        const int BYTES_IN_FRAME = 1206;

        //CONNECTION PARAMES
        private int PORT_NUMBER = 2368;
        private int BUFFER_SIZE = BYTES_IN_FRAME * DATA_PACKET_NUMBER;
        private byte[] buffer_frame = new byte[BYTES_IN_FRAME];


        public byte[] frameARR = new byte[DATA_PACKET_NUMBER * BYTES_IN_FRAME];
        public byte[] frame_full_360 = new byte[DATA_PACKET_NUMBER * BYTES_IN_FRAME];
        //public List<byte[]> frameL = new List<byte[]>();

        //GENERATED EVENTS
        public delegate void Event_recive_frame();
        public event Event_recive_frame frame_received;

        public void UDP_initialize_start()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            IPEndPoint VLP16_sensor = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            sock.Bind(VLP16_sensor);
            sock.ReceiveBufferSize = BUFFER_SIZE;
            sock.BeginReceive(buffer_frame, 0, BYTES_IN_FRAME, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), null);
            //ReceiveTest(sock);
        }


        public void ReceiveTest(Socket server)
        {
            server.BeginReceive(buffer_frame, 0, BYTES_IN_FRAME, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), server);
        }

        private void Async_wait_for_frame(IAsyncResult result)
        {
            int bytesRead = sock.EndReceive(result);
            if (bytesRead > 0)
            {
                collect_frames(buffer_frame);
                sock.BeginReceive(buffer_frame, 0, BYTES_IN_FRAME, SocketFlags.None, new AsyncCallback(Async_wait_for_frame), sock);
            }
        }

        //Data packet index
        int data_packet_index = 0;

        private void collect_frames(byte[] frame)
        {
            //Check frames number
            if (data_packet_index >= (DATA_PACKET_NUMBER))
            {
                frame_full_360 = (byte[])frameARR.Clone();
                frame_received();
                Array.Clear(frameARR,0, frameARR.Length);
                data_packet_index = 0;
            }

            //Copy full matrix
            Array.Copy(frame, 0, frameARR, (data_packet_index * BYTES_IN_FRAME), BYTES_IN_FRAME);

            data_packet_index++;
        }


    }

}
