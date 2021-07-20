using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Penguin__REMS_Project
{
    class ThreeDLidar : Lidar
    {
        #region VARIABLES
        private UdpClient listener_Lidar; // the lidar Listener
        private UdpState udpState;  // the udp state
        #endregion
        
        #region Constructor
        public ThreeDLidar(string name, string strIp, int vport , String type) : base(name, strIp, vport , type) { }
        #endregion
        
        #region Functions
        public override void ConnectToTheLidar()
        {
            try
            {
               
                this.listener_Lidar = new UdpClient(port);
                udpState = new UdpState
                {
                    e = lidarEndPoint,
                    u = listener_Lidar
                };
                isConnected = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show( ex.ToString(), "Connnection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public override void InitCommunication()
        {
            try
            {
                if (!isConnected) {
                   ConnectToTheLidar();
                    datasize = 0;
                }
               WriteFile = true;
                continuousReading = true;
                stopscan = false;
                
                listener_Lidar.BeginReceive(new AsyncCallback(ReceiveScanData), udpState);

            }
            catch (Exception e) // Must be check propertly to handle the exception
            {
                //MessageBox.Show(e.Message, "Connnection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             
            };
        }
        public void SetOrResetReadingData() {

            if (WriteFile)
            {
                WriteFile = false;
              
            }
            else {
                WriteFile = true;
              
            
            }
            if (stopscan)
            {
                stopscan = false;
               
            }
            else {
                stopscan = true;
                dataSizeCollection.Clear();
                datasize = 0;
            }
        
        
        }
        public override void DisconnectTheLidar()
        {
            isConnected = false;
        }
        private void ScanDataProcessInterface(byte[] scanDataBuffer)
        {
            DateTime _now = DateTime.Now;
            string NowString = _now.ToString("yyyyMMddHHmmssffff-");
            String data = NowString + BitConverter.ToString(scanDataBuffer);
          
            {
               
                pointCloudRawDataCollection.Add(data);
                UpdateDataSize(data);

                if (ScanTimes > 0xFFFFFFFFFFFFFFFF)
                    ScanTimes = 0;
                else
                    ScanTimes++;
                return;
            }
        }
        private void ReceiveScanData(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            Byte[] receiveBytes = u.EndReceive(ar, ref e);
            ScanDataProcessInterface(receiveBytes);
            if (stopscan == false)
            {
                UdpState s = new UdpState
                {
                    e = lidarEndPoint,
                    u = listener_Lidar
                };
                listener_Lidar.BeginReceive(new AsyncCallback(ReceiveScanData), s);
            }

        }
        public override string PullAFrame()
        {
             InitCommunication();
           
            Thread.Sleep(2);
      
            stopscan = true;
            return GetAFrameData();
        }
        public override void UpdateRawData(object sender, NotifyCollectionChangedEventArgs e)
        {
           UpateScanFile();
        }
        public override void StopScanning()
        {
            SetOrResetReadingData();
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
