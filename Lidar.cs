using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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
        protected String lidarType;
        protected ObservableCollection<String> pointCloudRawDataCollection;
        #endregion

        #region Constructor 
        public Lidar(String name ,String strIp, int vport,String type) {
            this.name = name;
            this.lidarType = type;
            this.strIpAdress = strIp;
            this.ipAdr = IPAddress.Parse(this.strIpAdress);
            this.port = vport;
            this.lidarEndPoint = new IPEndPoint(this.ipAdr, this.port);
            pointCloudRawDataCollection = new ObservableCollection<string>();
            pointCloudRawDataCollection.CollectionChanged += UpdateRawData;
        }

        public abstract void UpdateRawData(object sender, NotifyCollectionChangedEventArgs e);
        

        #endregion

        #region  Function 
        /***
         * 
         * function use principalement for the 3D lidar
         **/
        public abstract void ConnectToTheLidar();
        public abstract void DisconnectTheLidar();

        public String PingLidarResponse() {

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            int timeout = 120;
            PingReply reply = pingSender.Send(ipAdr, timeout);

            String status = "";
            if (reply.Status == IPStatus.Success)
            {
               
                status += "Address:  " + reply.Address.ToString();
                status += "\n RoundTrip time: " + reply.RoundtripTime;
                status += "\n Time to live: " + reply.Options.Ttl;
                status += "\n Don't fragment: " + reply.Options.DontFragment;
                status += "\n Buffer size:  " + reply.Buffer.Length;

                return status;
            }
            

            return "No response ";
        }

        public abstract String PullAFrame();
        public string LidarFile {

            get => lidarFile;

            set => lidarFile=value;
        }

        #endregion
    }
}
