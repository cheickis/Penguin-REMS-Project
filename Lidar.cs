using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Penguin__REMS_Project
{
    abstract class Lidar
    {
       #region Variable
        protected string strIpAdress; // the string ip address to be parse
        protected IPAddress ipAdr; // lidar ip address
        protected IPEndPoint lidarEndPoint; // lidar endpoint
        protected int port; // the lidar port
        protected string name;  // the lidar name
        protected string lidarFile;
        #endregion

       #region Constructor 
        public Lidar(String name ,String strIp, int vport) {

            this.name = name;
            this.strIpAdress = strIp;
            this.ipAdr = IPAddress.Parse(this.strIpAdress);
            this.port = vport;
            this.lidarEndPoint = new IPEndPoint(this.ipAdr, this.port);
        }

        #endregion

        #region  Function 
        /***
         * 
         * function use principalement for the 3D lidar
         **/
        public abstract void ConnectToTheLidar();
        public abstract void DisconnectTheLidar();
        public string LidarFile {

            get => lidarFile;

            set => lidarFile=value;
        }
        #endregion
    }
}
