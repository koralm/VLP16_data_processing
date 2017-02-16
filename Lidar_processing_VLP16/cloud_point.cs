using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lidar_processing_VLP16
{
    class cloud_point
    {

        public int ID { get; set; }
        public double azimuth { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public cloud_point(int ID_in, double azimuth_in, double X_in, double Y_in, double Z_in)
        {
            ID = ID_in;
            azimuth = azimuth_in;
            X = X_in;
            Y = Y_in;
            Z = Z_in;
        }

    }
}
