using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Penguin__REMS_Project
{
    class Lidar
    {
        private string strIpAdress;
        private IPAddress ipAdr;
        private IPEndPoint lidarEndPoint;
        private int port;


        public Lidar(String strIp, int vport) {
            this.strIpAdress = strIp;
            this.ipAdr = IPAddress.Parse(this.strIpAdress);
            this.port = vport;
            try
            {
                this.lidarEndPoint = new IPEndPoint(this.ipAdr,this.port);

            }
            catch { 

            }
          
            
        }
          
    }
}
