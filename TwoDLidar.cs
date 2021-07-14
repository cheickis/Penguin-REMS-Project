using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


using System.Collections.Specialized;

namespace Penguin__REMS_Project
{
    class TwoDLidar : Lidar
    {
        #region  VARIABLES
        private String req = "";
        private const string READREQ = "\x02sEN LMDscandata 1\x03"; // request for one telegram 
        private const string READREQSUITE = "\x02sRN LMDscandata \x03"; // request polling data
        private TcpClient Tcp;
        private NetworkStream stream;
        private StreamWriter fileWriter, writer;
        #endregion
        public TwoDLidar(String name, String strIp, int vport, String type) : base( name, strIp, vport, type)

        {

        }
   
        /// <summary>
        /// Creates a new TCP-socket
        /// </summary>
        /// <param name="ip">IP adress of the MRS6124R
        /// </param>
        /// <param name="port">Port to the MRS6124R, default:211</param>
        public override void ConnectToTheLidar()
        {
            try
            {
             
                Tcp = new TcpClient();
                Tcp.Connect(ipAdr, port);
                stream = Tcp.GetStream();
                stream.ReadTimeout = 1000;
                writer = new StreamWriter(Tcp.GetStream(), Encoding.ASCII);
                isConnected = Tcp.Connected;
            }
            catch (Exception Ex)
            {
                Console.Write("Exception: " + Ex.Message);
                throw Ex;
            }
        }
        /// <summary>
        /// Reads incoming data from the TCP-socket
        /// </summary>
        /// <returns>ASCII string containing the read data</returns>
        private string ReadTCP()
        {
            String data = null;
            Byte[] bytes = new Byte[15000]; //big Array to be sure that we don't loose some datas
            while (!stream.DataAvailable) ;
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
               


                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                
                
                // STX and ETX in HEX indicate start and end of messages
                int startpoint = data.IndexOf('\x02');
                int endpoint = data.IndexOf('\x03');

                if (startpoint < 0)
                {
                    startpoint = 0;
                }
                if (endpoint < 1)
                {
                    endpoint = bytes.Length;
                }
                return data;

            }
            return null;
        }
        /// <summary>
        /// Reads incoming data from the TCP-socket
        /// </summary>
        /// <returns>Byte array containing read data</returns>
        private Byte[] ReadTCPbyte()
        {
            if (!stream.DataAvailable)
            {
                return null;
            }
            Byte[] bytes = new Byte[20000];
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                return bytes;
            }
            return null;
        }
       
     
        public override void DisconnectTheLidar()
        {
            stream.Close();

           if(Tcp.Connected)
               Tcp.Close();
            
        }

      
        public override string PullAFrame()
        {
             ReadScan();
             return GetAFrameData();
        }

        public override void UpdateRawData(object sender, NotifyCollectionChangedEventArgs e)
        {
           UpateScanFile();
        }

        public void SetNoStopReading() { 
        
         writer.Write(READREQ);
            writer.Flush();
            // Wait for an answer
            System.Threading.Thread.Sleep(200);  // Need to check 
        
        
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
               
                ReadScan();
            }catch(Exception ex) { 
               // Handle
           
            }
         }

        
        /// <summary>
        /// Read results of one measurement 
        /// </summary>
        /// <returns>RQAW DAta</returns>
        public void  ReadScan()
        {
         
            //Request results from SICK LMS511 
            writer.Write(READREQSUITE);
            writer.Flush();
            // Wait for an answer
          // System.Threading.Thread.Sleep(10);  // Need to check 
           
            DateTime _now = DateTime.Now;
            String NowString = _now.ToString("yyyyMMddHHmmssffff-");
            String  scanData = ReadTCP();  
            if (scanData!=null && scanData.Contains("sRA") == true)
            {
                String data = NowString + scanData;
                 pointCloudRawDataCollection.Add( data);
                 UpdateDataSize(data);
                
            }
       
        }

        public override void StopScanning()
        {
            WriteFile = false;
            continuousReading = false;
            dataSizeCollection.Clear();
            datasize = 0;
        }
    }
}
