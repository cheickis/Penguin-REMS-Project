using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Penguin__REMS_Project
{
    class TwoDLidar : Lidar
    {
        private String req = "";
        private const string READREQ = "\x02sEN LMDscandata 1\x03"; // request for one telegram 
        private const string READREQSUITE = "\x02sRN LMDscandata \x03"; // request polling data
        private TcpClient Tcp;
        private NetworkStream stream;
        private StreamWriter writer, fileWriter;
        public TwoDLidar(String name, string strIp, int vport) : base(name,strIp, vport)
        {
        }
           /// <summary>
            /// Creates a new TCP-socket
            /// </summary>
            /// <param name="ip">IP adress of the MRS6124R
            /// </param>
            /// <param name="port">Port to the MRS6124R, default:211</param>
            public void ConnectToSICKLMS5XX(string ip, int port = 2111)
        {
            try
            {
                Console.WriteLine("\tInit connection to SICK LMS 511");
                Tcp = new TcpClient();
                Tcp.Connect(System.Net.IPAddress.Parse(ip), port);
                stream = Tcp.GetStream();
                fileWriter = new StreamWriter(new FileStream("../../data.txt", FileMode.Create), Encoding.ASCII);
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
        private string readTCP()
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
        private Byte[] readTCPbyte()
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

        private void updateDataFile(String data)
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
        /// <summary>
        /// Read results of one measurement from the MRS6124R
        /// </summary>
        /// <returns>MeasurementResult struct containing the info</returns>
      /*  public MeasurementResult ReadScan()
        {
            MeasurementResult res = new MeasurementResult();
            res.distanceAndAngle = new Dictionary<double, double>();
                
               // Console.WriteLine("Time out TCP {0}", Tcp.ReceiveTimeout);
            // Request results from SICK LMS511 
            writer.Write(READREQSUITE);
            writer.Flush();
            // Wait for an answer
            System.Threading.Thread.Sleep(200);
            String _m = readTCP();
            if (_m.Contains("sRA") == true)
            {
                updateDataFile(DateTime.Now.ToString("hh:mm:ss.ff ") + _m);
                
            }
          //  Console.WriteLine(_m);
           // Console.Clear();
             return res;
        }
       */
        

        public Boolean IsContainTheOpposite()
        {
            return false;
        }
    }
}
