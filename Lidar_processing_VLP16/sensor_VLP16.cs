using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lidar_processing_VLP16
{
    class sensor_VLP16
    {

        //Frame static variables
        const int BYTES_IN_FRAME = 1206;
        const int POINTS_IN_READ = 32;
        const int DATA_BLOCKS_IN_FRAME = 12;

        readonly int[,] FRAME_HEADER_POS = new int[,] { { 1, 2 }, { 101, 102 }, { 201, 202 }, { 301, 302 }, { 401, 402 }, { 501, 502 }, { 601, 602 }, { 701, 702 }, { 801, 802 }, { 901, 902 }, { 1001, 1002 }, { 1101, 1102 } };
        readonly int[,] FRAME_AZIMUTH_POS = new int[,] { { 3, 4 }, { 103, 104 }, { 203, 204 }, { 303, 304 }, { 403, 404 }, { 503, 504 }, { 603, 604 }, { 703, 704 }, { 803, 804 }, { 903, 904 }, { 1003, 1004 }, { 1103, 1104 } };
        readonly double[] SENSOR_VERTICAL_ANGLE = new double[] { -15, 1, -13, 3, -11, 5, -9, 7, -7, 9, -5, 11, -3, 13, -1, 15, -15, 1, -13, 3, -11, 5, -9, 7, -7, 9, -5, 11, -3, 13, -1, 15 };

        private double azimuth = 0;
        private double azimuthN = 0;
        private double azimuth_interpolated = 0;

        //TEST
        //public List<double> azimuth_1DA = new List<double>();
        public double[] azimuth_1DA = new double[DATA_BLOCKS_IN_FRAME * POINTS_IN_READ * 80];
        public double[] X = new double[DATA_BLOCKS_IN_FRAME * POINTS_IN_READ * 80];
        public double[] Y = new double[DATA_BLOCKS_IN_FRAME * POINTS_IN_READ * 80];
        public double[] Z = new double[DATA_BLOCKS_IN_FRAME * POINTS_IN_READ * 80];

        public cloud cloudX = new cloud();
        //public cloud_point point = new cloud_point(0, 0, 0, 0, 0);
        //public IList<cloud_point> cloudX = new List<cloud_point>();

        public void frame_VLP16_decode(byte[] VLP16_frame)
        {
            //check frame size [frames_number,frame_value]
            int frame_size = (VLP16_frame.Length / BYTES_IN_FRAME) - 1;

            //init tables and variables
            double[,] frame_decoded = new double[7, DATA_BLOCKS_IN_FRAME * POINTS_IN_READ * frame_size];
            
            for (int frame_index = 0; frame_index < frame_size; frame_index++)
            {

                //time stamp calc
                byte timestamp_HHbyte = VLP16_frame[(1204 + frame_index * 1206) ];
                byte timestamp_HLbyte = VLP16_frame[(1203 + frame_index * 1206) ];
                byte timestamp_LHbyte = VLP16_frame[(1202 + frame_index * 1206) ];
                byte timestamp_LLbyte = VLP16_frame[(1201 + frame_index * 1206) ];
                int time_stamp = (timestamp_HHbyte << 24) | (timestamp_HLbyte << 16) | (timestamp_LHbyte << 8) | timestamp_LLbyte;

                //Calc all data block
                for (int DataBlock_index = 0; DataBlock_index < DATA_BLOCKS_IN_FRAME; DataBlock_index++)
                {
                    //Calc azimuth ACTUAL
                    byte azimuth_Hbyte = VLP16_frame[((3 + DataBlock_index * 100)+BYTES_IN_FRAME*frame_index)];
                    byte azimuth_Lbyte = VLP16_frame[((2 + DataBlock_index * 100)+ BYTES_IN_FRAME * frame_index)];
                    int azimuthINT = (azimuth_Hbyte << 8) | azimuth_Lbyte;
                    azimuth = Convert.ToDouble(azimuthINT) / 100;

                    //Calc azimuth NEXT
                    if (DataBlock_index < 11)
                    {
                        byte azimuth_HbyteN = VLP16_frame[((3 + (DataBlock_index + 1) * 100) + BYTES_IN_FRAME * frame_index)];
                        byte azimuth_LbyteN = VLP16_frame[((2 + (DataBlock_index + 1) * 100) + BYTES_IN_FRAME * frame_index)];
                        int azimuthINTN = (azimuth_HbyteN << 8) | azimuth_LbyteN;
                        azimuthN = Convert.ToDouble(azimuthINTN) / 100;
                    } else if (DataBlock_index == 11)
                    {
                        byte azimuth_HbyteN = VLP16_frame[((3 + (DataBlock_index - 11) * 100) + BYTES_IN_FRAME * (frame_index + 1))];
                        byte azimuth_LbyteN = VLP16_frame[((2 + (DataBlock_index - 11) * 100) + BYTES_IN_FRAME * (frame_index + 1))];
                        int azimuthINTN = (azimuth_HbyteN << 8) | azimuth_LbyteN;
                        azimuthN = Convert.ToDouble(azimuthINTN) / 100;
                    }


                        azimuth_interpolated = azimuth_interpolation(azimuthN, azimuth);


                    for (int point_index = 0; point_index < POINTS_IN_READ; point_index++)
                    {

                        //Calc Distance
                        byte distance_Hbyte = VLP16_frame[((3 * point_index + 5) + BYTES_IN_FRAME * frame_index)];
                        byte distance_Lbyte = VLP16_frame[((3 * point_index + 4) + BYTES_IN_FRAME * frame_index)];
                        int distanceINT = (distance_Hbyte << 8) | distance_Lbyte;
                        double distance = Convert.ToDouble(distanceINT) * 2;

                        //Set azimuth
                        if (point_index > 15)
                        {
                            azimuth = azimuth_interpolated;
                        }

                        //Asing to 2d Array
                        frame_decoded[0, point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index;
                        frame_decoded[1, point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = azimuth;
                        frame_decoded[2, point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = distance;
                        frame_decoded[3, point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = time_stamp;
                        frame_decoded[4, point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = calc_X_cooridnates(distance, azimuth, point_index);
                        frame_decoded[5, point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = calc_Y_cooridnates(distance, azimuth, point_index);
                        frame_decoded[6, point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = calc_Z_cooridnates(distance, azimuth, point_index);

                        //TEST VARIABLES
                        //azimuth_1DA.Add(azimuth);
                        //azimuth_1DA[point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = azimuth;
                        //X[point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = calc_X_cooridnates(distance, azimuth, point_index);
                        //Y[point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = calc_Y_cooridnates(distance, azimuth, point_index);
                        //Z[point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index] = calc_Z_cooridnates(distance, azimuth, point_index);

                        //cloudX.cloud_add(new cloud_point(point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index, azimuth, calc_X_cooridnates(distance, azimuth, point_index), calc_Y_cooridnates(distance, azimuth, point_index), calc_Z_cooridnates(distance, azimuth, point_index)));
                        cloud_point point = new cloud_point(point_index + POINTS_IN_READ * DataBlock_index + POINTS_IN_READ * DATA_BLOCKS_IN_FRAME * frame_index, azimuth, calc_X_cooridnates(distance, azimuth, point_index), calc_Y_cooridnates(distance, azimuth, point_index), calc_Z_cooridnates(distance, azimuth, point_index));
                        cloudX.cloud_add(point);
                    }
                }
            }
        }

        private double azimuth_interpolation(double azimuthT, double azimuth_tempT)
        {
            double azimuth_interpolatedT = 0;

            if (azimuthT < azimuth_tempT)
            {
                azimuthT = azimuthT + 360;
            }

            azimuth_interpolatedT = azimuth_tempT + ((azimuthT - azimuth_tempT) / 2);

            if (azimuth_interpolatedT > 360)
            {
                azimuth_interpolatedT = azimuth_interpolatedT - 360;
            }

            return azimuth_interpolatedT;
        }

        private double calc_X_cooridnates(double distance, double azimuth, int i)
        {
            double coordinate_X = distance * Math.Cos(SENSOR_VERTICAL_ANGLE[i]) + Math.Sin(azimuth);

            return coordinate_X;
        }

        private double calc_Y_cooridnates(double distance, double azimuth, int i)
        {
            double coordinate_Y = distance * Math.Cos(SENSOR_VERTICAL_ANGLE[i]) + Math.Cos(azimuth);

            return coordinate_Y;
        }

        private double calc_Z_cooridnates(double distance, double azimuth, int i)
        {
            double coordinate_Z = distance * Math.Sin(SENSOR_VERTICAL_ANGLE[i]);

            return coordinate_Z;
        }

    }
}
