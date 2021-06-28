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
    class Lidar
    {
        #region Variable
        private string strIpAdress; // the string ip address to be parse
        private IPAddress ipAdr; // lidar ip address
        private IPEndPoint lidarEndPoint; // lidar endpoint
        private int port; // the lidar port
        private string name;  // the lidar name
        private  UdpClient listener_Lidar; // the lidar Listener
        private UdpState udpState;  // the udp state

        public StreamWriter sw_scan;
        public bool continuousReading = true;
        public bool WriteFile = false;
        public byte[] ScanDataBuffer = new byte[1500];
        private UInt64 ScanTimes = 0;
        public bool stopscan = false;

        #endregion


       #region Constructor 
        public Lidar(String name ,String strIp, int vport) {

            this.name = name;
            this.strIpAdress = strIp;
            this.ipAdr = IPAddress.Parse(this.strIpAdress);
            this.port = vport;
            try
            {
                this.lidarEndPoint = new IPEndPoint(this.ipAdr,this.port);
                this.listener_Lidar = new UdpClient();
                udpState = new UdpState();
                udpState.e = lidarEndPoint;
                udpState.u = listener_Lidar;

            }
            catch { 

                // add the handling erro message here

            }
            
        }
        #endregion
        
       #region Functions
       
         private void ScanDataProcessInterface(byte[] scanDataBuffer)
        {
            if ((sw_scan != null) && (WriteFile == true) && (sw_scan.BaseStream!=null))
            {
                DateTime _now = DateTime.Now;
                string NowString = _now.ToString("yyyyMMddHHmmssffff-");   //Timestamp.
                sw_scan.WriteLine(NowString +BitConverter.ToString(scanDataBuffer));
                if (ScanTimes > 0xFFFFFFFFFFFFFFFF)
                    ScanTimes = 0;
                else
                    ScanTimes++;
                return;
            }
        }

        public bool OpenFile(string FileName)
        {
            sw_scan = new StreamWriter(FileName);
            if (sw_scan != null)
                return true;
            else
                return false;
        }

        public void CloseFile()
        {
            WriteFile = false;
            if (sw_scan != null)
                sw_scan.Close();
        }


       public void initialScanner()
        {

            try
            {
                WriteFile = true;
                continuousReading = true;
                listener_Lidar.BeginReceive(new AsyncCallback(ReceiveScanData), s);
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e.Message);
            };

        }
       

        private void ReceiveScanData(IAsyncResult ar)
        { 
            UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            Byte[] receiveBytes = u.EndReceive(ar, ref e);
            ScanDataProcessInterface(receiveBytes);
            if (stopscan == false)
            {
                UdpState s = new UdpState();
                s.e = LidarEndPoint;
                s.u = listener_Lidar;
                listener_Lidar.BeginReceive(new AsyncCallback(ReceiveScanData), s);
            }

        }
       #endregion

        #region Inner Class Udp State
        public class UdpState
        {

            public IPEndPoint e;
            public UdpClient u;

        }
        #endregion

          
    }
}
