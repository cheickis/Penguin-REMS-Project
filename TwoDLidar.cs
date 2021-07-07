using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("\tInit connection to SICK LMS 511");
                Tcp = new TcpClient();
                Tcp.Connect(ipAdr, port);
                stream = Tcp.GetStream();
                // fileWriter = new StreamWriter(new FileStream("../../data.txt", FileMode.Create), Encoding.ASCII);
                fileWriter = new StreamWriter(new FileStream(lidarFile, FileMode.Create), Encoding.ASCII);
                stream.ReadTimeout = 1000;
                writer = new StreamWriter(Tcp.GetStream(), Encoding.ASCII);
                Console.WriteLine("\tConnection to SICK LMS 511 ok");

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
                // Translate data bytes to a ASCII string.
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
                //return data.Substring(startpoint, endpoint);
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
        private void UpdateDataFile(String data)
        {
            // Change decimal separator from , to .
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            // Backup the current culture
            System.Globalization.CultureInfo defaultCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            ///fileWriter.WriteLine(DateTime.Now.ToString("hh:mm:ss.ff ") + data);
            fileWriter.WriteLine(data);
           // Console.WriteLine("Data was update");
            // Restore the , as default decimal separator
            System.Threading.Thread.CurrentThread.CurrentCulture = defaultCulture;

        }
        public void CloseFile()
        {

            fileWriter.Close();
        }
        public Boolean IsContainTheOpposite()
        {
            return false;
        }
        public override void DisconnectTheLidar()
        {
            throw new NotImplementedException();
        }

      
        public override string PullAFrame()
        {
            throw new NotImplementedException();
        }

        public override void UpdateRawData(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
