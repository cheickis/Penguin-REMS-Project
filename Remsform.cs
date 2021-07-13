using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Forms;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MetroFramework.Controls;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace Penguin__REMS_Project
{
    public partial class Remsform : MetroForm
    {

        #region Variable
        static Lidar lidar;
        static List<Lidar> lidars;
        private object m_lockScan = new object();
        private bool m_isRunningScan = false;
        private bool m_isAbortRequestedScan = false;
        StreamWriter writer;
        Dictionary<String, Lidar> listOfLidarInStory;
        #endregion

        #region 3D LIDAR

        #endregion

        #region INTEL REALSENS

        #endregion

        #region SICK LIDAR MEMBER

        #endregion

        #region Observable COLLECTION

        private ObservableCollection<Lidar> lidarCollections;
        private ObservableCollection<TwoDLidar> twoDLidarCollections;
        private ObservableCollection<ThreeDLidar> treeDLidarCollection;
        private ObservableCollection<Realsens> realsensCollection;
        #endregion

        #region Queue

        Queue<Lidar> lidarQ;
        #endregion

        #region Constructor
        public Remsform()
        {
            InitializeComponent();
            InitCollection();
            OpenLidarConfigFile();
        }
        #endregion

        #region Collection Function 

        private void InitCollection()
        {
            listOfLidarInStory = new Dictionary<string, Lidar>();
            lidarCollections = new ObservableCollection<Lidar>();
            twoDLidarCollections = new ObservableCollection<TwoDLidar>();
            treeDLidarCollection = new ObservableCollection<ThreeDLidar>();
            realsensCollection = new ObservableCollection<Realsens>();

            twoDLidarCollections.CollectionChanged += upadteSickListForm;
            lidarQ = new Queue<Lidar>();
            lidars = new List<Lidar>();

        }

        private void upadteSickListForm(object sender, NotifyCollectionChangedEventArgs e)
        {
           // throw new NotImplementedException();
        }
        #endregion

        #region Scan Button handler
        private void StartBtn_Click(object sender, EventArgs e)
        {
            DateTime _now = DateTime.Now;
            string NowString = _now.ToString("yyyyMMddHHmmss");   //Timestamp.

            #region ParallelTasks
            // Perform three tasks in parallel :  2D lidar , 3D lidar, realsens, GPR ...
            Parallel.Invoke(() =>
            { 
                LidarsThreadHandlers(NowString);
            },  
            () =>
            {
                   ThreeDLidarHandler(NowString);
             }, 
             () =>
             {
                     TalonHandler();
                    },
              () =>
              {
                     GPRHandler();
               },
                 () =>
               {
                     RealsensHandler();
               }

             ); 

           
            #endregion
        }

        #endregion

        #region Two Lidar
        private void LidarsThreadHandlers(String timeStamp) {

            LidarsThread(timeStamp);
        }
        #endregion

        #region Three D Lidar
        private void ThreeDLidarHandler(String timeStamp) {
            if (treeDLidarCollection.Count != 0)
            {
                foreach (var item in treeDLidarCollection)
                {
                   


                }
            }
        }
        #endregion

        #region Realsens
        private void RealsensHandler()
        {

        }
        #endregion

        #region  Talon handler
        private void TalonHandler()
        {

        }
        #endregion
        
        #region  GPR (Ground prenetration radar handler
        private void GPRHandler()
        {

        }
        #endregion

        #region Lidar Config Tab Function
        private void ResetLidarBtn_Click(object sender, EventArgs e)
        {
            ResetAddLidarForm(); 
        }
        private void AddLidarBtn_Click(object sender, EventArgs e)
        {
             String type = lidarTypeCbx.Text;
             String lidarName = lidarNameCbx.Text;
             if (!lidarIPTxt.Equals("") && !lidarPortTxt.Equals("") && !lidarName.Equals("") && !type.Equals(""))
             {

                 String ip = lidarIPTxt.Text;
                 int port = Int32.Parse(lidarPortTxt.Text);
                 AddLidar( lidarName,ip, port, type);
                 pingNewLidarBtn.Visible = true;
                 pullFrameBtn.Visible = true;
             }
             else
             {
                ShowMessage(ConstantStringMessage.DEVICEEXISTINGINSYSTEMMSG, ConstantStringMessage.ADDDEVICECAPTIONMSG, MessageBoxButtons.OK);// need change the message 
            }
           
        }
        public void OpenLidarConfigFile() {

          
            try
            {
              
                using (StreamReader sr = new StreamReader(ConstantStringMessage.LIDARDEVICEFILE))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        i++;
                        if (i > 1)
                        {
                            LinerParser(line);
                        }
                       
                    }
                }
                writer = new StreamWriter(new FileStream(ConstantStringMessage.LIDARDEVICEFILE, FileMode.OpenOrCreate | FileMode.Append), Encoding.ASCII);
            }
            catch (Exception e)  // need to check for best pratices
            {
                writer = new StreamWriter(new FileStream(ConstantStringMessage.LIDARDEVICEFILE, FileMode.OpenOrCreate | FileMode.Append), Encoding.ASCII);
                //ShowMessage(ConstantStringMessage.LOADINGCONFIGFILEERRORMSG+e.Message, ConstantStringMessage.LOADINGCONFIGFILEERRORMSG, MessageBoxButtons.OK);
                writer.WriteLine(ConstantStringMessage.CONFIGFILEHEADER);
                writer.Flush();
            }

           
        }
     

        private void LinerParser(String line) {
            String[] result = line.Trim().Split(':');
            String ip, name, type;
            int port;
            if (result.Length == 4) {
                ip = result[1].Trim();
                name = result[0].Trim();
                port = Int32.Parse(result[2]);
                type = result[3].Trim();
                if (!listOfLidarInStory.ContainsKey(ip)) {

                    listOfLidarInStory.Add(ip, GetLidar( name, ip,  port, type));
                }
            }

        }
        #endregion

        #region Helper
        private void ResetAddLidarForm()
        {
            lidarIPTxt.Text = "";
            lidarPortTxt.Text = "";
           
        }
        private void AddLidar(String name, String ip, int port, String type) {

            if (!listOfLidarInStory.ContainsKey(ip))
            {
                String line =  name + " : " + ip + " : " +port+ " : "+type;
                writer.WriteLine(line);
                writer.Flush();
                listOfLidarInStory.Add(ip, GetLidar(name, ip, port, type));
            }
            else
            {

                ShowMessage(ConstantStringMessage.ADDLIDARFORMCAPTIONMSQ, ConstantStringMessage.ADDLIDARFORMERRORMSQ, MessageBoxButtons.OK);

            }

        }

        private Lidar GetLidar(String name, String ip, int port, string type) {

            if (type.Equals("2D"))
            {
                AddTwoDLidar(name, ip, port, type);
                return new TwoDLidar(name, ip, port, type);
            }
            else
            {
                AddThreeDLidar(name, ip, port, type);
                return new ThreeDLidar(name, ip, port, type);
            }
        }
        private void AddThreeDLidar(String name, String ip, int port, string type)
        {
            ThreeDLidar newThreeDLidar = new ThreeDLidar(name, ip, port , type);
            treeDLidarCollection.Add(newThreeDLidar);
            SetLidarCollectionAndForm(newThreeDLidar);
        }
        private void  AddTwoDLidar(String name, String ip, int port, string type)
        {

            TwoDLidar newtwoDLidar = new TwoDLidar(name, ip, port, type);
            twoDLidarCollections.Add(newtwoDLidar);
            SetLidarCollectionAndForm(newtwoDLidar);
        }
        private void AddLidarTile(String name, String type) {

            MetroTile lidarTile = new MetroTile();
            lidarTile.Name = name;
            lidarTile.Text = name;
            lidarTile.Size = new System.Drawing.Size(110, 157);


            if (type.Equals("2D") && name.Contains("LMS"))
            {
                lidarTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS1;
                lidarTile.UseTileImage = true;
                sickFLPanel.Controls.Add(lidarTile);
                sickFLPanel.Update();

            }
            else if (type.Equals("3D") && name.Equals("Leinshen C16"))
            {

                lidarTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.leishen2;
                lidarTile.UseTileImage = true;
                threeDLidarFLPanel.Controls.Add(lidarTile);
                threeDLidarFLPanel.Update();

            }
            else if (type.Equals("3D") && name.Equals("Velodyne VLP 16"))
            {
                lidarTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.leishen2;
                lidarTile.UseTileImage = true;
                threeDLidarFLPanel.Controls.Add(lidarTile);
                threeDLidarFLPanel.Update();
            }
            else {
                lidarTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.leishen2;//SICK MRS633
                lidarTile.UseTileImage = true;
                threeDLidarFLPanel.Controls.Add(lidarTile);
                threeDLidarFLPanel.Update();
            }
            /*lidarPicInfoTl.TileImage = lidarTile.TileImage;
            lidarPicInfoTl.Text = lidarTile.Text;
            lidarPicInfoTl.Update();
            lidarPicInfoTl.Refresh();*/

        }
        private void SetLidarCollectionAndForm(Lidar tlidar ) {
            ResetAddLidarForm();
            AddLidarTile(tlidar.Name, tlidar.Type);
            lidarQ.Enqueue(tlidar);
            lidar = tlidar;
            lidars.Add(lidar);
            
            AddNewGroupBox(tlidar.STRIPAdresse, tlidar.Name, tlidar.Type);
        }
        #endregion

        #region  Ping and  Pull
        private void PingNewLidarBtn_Click(object sender, EventArgs e)
        {
           

          
            if (lidar != null) {
                lidar.ConnectToTheLidar();
                lidarConfigLogview.Text = lidar.PingLidarResponse();
            }
        }

        private void PullFrameBtn_Click(object sender, EventArgs e)
        {


            if (lidar != null) {

               
                lidarConfigLogview.Clear();

                lidar.Request = ConstantStringMessage.ONE_TELEGRAMM;
                lidarConfigLogview.Text = lidar.PullAFrame();
                lidarConfigLogview.Update();
                lidarConfigLogview.Refresh();
            }

        }
              

        #endregion

        #region Handle Lidar Thread

        private void LidarsThread(String timeStamp)
        {
            lock (m_lockScan)
            {
                if (m_isRunningScan)
                {
                    m_isAbortRequestedScan = true;
                }
                else
                {
                    m_isAbortRequestedScan = false;
                    m_isRunningScan = true;
                    ThreadPool.QueueUserWorkItem(o => LidarsScanningBackgroundMethod(timeStamp));
                }
            }
        }
        private void LidarsScanningBackgroundMethod(String timeStamp)
        {
            try
            {
                StartLidarsCommunications(timeStamp);
            }
            finally
            {
                lock (m_lockScan)
                {
                    m_isRunningScan = false;
                }
            }
        }
        private void StartLidarsCommunications(String timeStamp)
        {
            SetSScansDatasFiles(timeStamp);
          
            while (true)
            {
                //ScanningTile.Text = " Scanning ...  ";
                LidarsStartScan();
                if (m_isAbortRequestedScan)
                {
                   
                    DialogResult dr = ShowMessage("Do you to stop Scanning? ", "Stop Scanning Message", MessageBoxButtons.YesNo);
                 
                    if (dr == DialogResult.Yes)
                    {
                        //ScanningTile.Text = " Stop Scanning  "; 
                       
                        CloseLidarsFiles();
                        return;
                    }
                    else
                    {
                        m_isAbortRequestedScan = false;

                    }
                }
               
            }

        }
    
        private void SetSScansDatasFiles(String NowString)
        {
          
            foreach (var item in lidars)
            {
                item.LidarFile = @"C:\Scandatas\" + item.Name + "_ScanData_" + NowString + ".txt";
                item.OpenFile();
                
            }
        }

        private void CloseLidarsFiles() {

            foreach (var item in lidars) {
                item.CloseFile();
            }
        }
        private void LidarsStartScan()
        {
            foreach (var item in lidars)
            {
                item.InitCommunication();
            }
            Thread.Sleep(500);
        }
        #endregion

        #region UI THREAD SAFE HELPER FUNCTION

        public DialogResult ShowMessage(string msg, string caption, MessageBoxButtons buttons)
        {
            if (InvokeRequired)
            {
                Func<DialogResult> m = () => MessageBox.Show(msg, caption, buttons);
                return (DialogResult)Invoke(m);
            }
            else
            {
                return MessageBox.Show(msg, caption, buttons);
            }
        }
        #endregion

        #region MAIN TAB UI 

      
        private void AddNewGroupBox(String ip, String name, String type)
        {

           this.mainfLPl.Controls.Add(AddGroupBox(ip, name, type));
        }

        private GroupBox AddGroupBox(String ip, String name, String type) {


            LidarGroupBox lidarGrpBox = new LidarGroupBox(name, ip, type);
           lidarGrpBox.LidarPicturTile.Click += new System.EventHandler(this.LidarGroupBox_On_Click);
            lidarGrpBox.Click += new System.EventHandler(this.LidarGroupBox_On_Click);
            return lidarGrpBox;
        }

        private void LidarGroupBox_On_Click(object sender, EventArgs e)
        {
            String ip = "";
            if (sender is LidarGroupBox )
            {
                LidarGroupBox  obj =(LidarGroupBox ) sender;
                lidarPicInfoTl.TileImage = obj.LidarPicturTile.TileImage;
                lidarPicInfoTl.Text = obj.Text;
                ip = obj.IPString;


            }
            else if (sender is MetroTile) {

                MetroTile obj = (MetroTile)sender;
                lidarPicInfoTl.TileImage = obj.TileImage;
                lidarPicInfoTl.Text = obj.Text;
                ip = obj.Text.Trim().Split(':')[1];
            }

            lidar = listOfLidarInStory[ip];



            lidarPicInfoTl.Update();
            lidarPicInfoTl.Refresh();
           



            lidarInfoPanel.Visible = true;
            lidarInfoPanel.Show();
            mainTb.Enabled = false;
        }



        #endregion

        private void CloseLidarPanelBtn_Click(object sender, EventArgs e)
        {
           
            lidarInfoPanel.Visible = false;
            mainTb.Enabled = true;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

        }
    }

}
