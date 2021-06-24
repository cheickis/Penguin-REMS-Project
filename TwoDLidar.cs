using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Penguin__REMS_Project
{
    class TwoDLidar : Lidar
    {
        private String req = "";

        public TwoDLidar(string strIp, int vport) : base(strIp, vport)
        {
        }
    }
}
