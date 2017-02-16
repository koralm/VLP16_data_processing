using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lidar_processing_VLP16
{
    class cloud
    {
        public IList<cloud_point> cloudX { get; set; }

        public cloud()
        {
            cloudX = new List<cloud_point>();
        }

        public void cloud_add(cloud_point point)
        {
            cloudX.Add(point);
            if (cloudX.Count > (79 * 12 * 32))
            {
                cloudX.Clear();
            }
        }
    }
}
