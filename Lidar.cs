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
        protected Boolean isConnected;

        #region File Handler Variable
        protected StreamWriter sw_scan;
        protected bool continuousReading = true;
        protected bool WriteFile = false;
        protected byte[] ScanDataBuffer = new byte[1500];
        protected UInt64 ScanTimes = 0;
        protected bool stopscan = false;

        protected String req;
        #endregion
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
            isConnected = false;
        }
        #endregion

        #region  Function 
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
        public  bool OpenFile()
        {
            sw_scan = new StreamWriter(lidarFile);
            if (sw_scan != null)
                return true;
            else
                return false;
        }

        public  void CloseFile()
        {
            WriteFile = false;
            if (sw_scan != null)
                sw_scan.Close();
        }
      

        public void UpateScanFile() { 
        
         String lastFrame = pointCloudRawDataCollection.Last();

            if ((sw_scan != null) && (WriteFile == true) && (sw_scan.BaseStream != null) && (lastFrame != null))
            {
                lock (sw_scan)
                {
                    sw_scan.WriteLine(lastFrame);
                }
            
            }   
        }

        public String GetAFrameData() { 
        
         if (pointCloudRawDataCollection.Count() == 0) {
                return "No Frame received";
            }
            return pointCloudRawDataCollection.Last();
        
        }
        #endregion

        #region Abstract METHODE
        public abstract void ConnectToTheLidar();
        public abstract void InitCommunication();
        public abstract void DisconnectTheLidar();
        public abstract void UpdateRawData(object sender, NotifyCollectionChangedEventArgs e);
        public abstract String PullAFrame();
       /* public abstract bool OpenFile();
        public abstract void CloseFile();*/
        #endregion

        #region GETTER
        public string LidarFile
        {

            get => lidarFile;

            set => lidarFile = value;
        }

        public string Name
        {

            get => name;
            set => name = value;
        }

        public String Type
        {

            get => lidarType;
            set => lidarType = value;

        }


        public String Request {

            get => req;

            set => req = value;

        }

        public String STRIPAdresse {

            get => strIpAdress;
          
        }

        public Boolean IsConnected {

            get => isConnected;
        }
        #endregion
    }
}
