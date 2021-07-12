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
        StreamWriter writer, reader;
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
        }
        #endregion

        #region Collection Function 

        private void InitCollection()
        {
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
            /* String type = lidarTypeCbx.Text;
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
                 // handle the error  herer
             }*/
            OpenLidarConfigFile();
        }
        public void OpenLidarConfigFile() {

            writer = new StreamWriter(new FileStream(ConstantStringMessage.LIDARDEVICEFILE, FileMode.OpenOrCreate), Encoding.ASCII);
            try
            {
              
                using (StreamReader sr = new StreamReader(ConstantStringMessage.LIDARDEVICEFILE))
                {
                    string line;
                 
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

           
        }
        private void ParseTheConfigFile()
        {

        }
        private void AddNewLidarInTheConfigFile() { 
        

        }

        private void LoadPreviewLidar() { 
        
        
        
        
        }
        #endregion

        #region Helper
        private void ResetAddLidarForm()
        {
            lidarIPTxt.Text = "";
            lidarPortTxt.Text = "";
           
        }
        private void AddLidar(String name, String ip, int port, String type) {
            if (type.Equals("2D"))
            {
                AddTwoDLidar( name, ip, port, type);

            }
            else {
                AddThreeDLidar(name, ip, port, type);
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
            lidarPicInfoTl.TileImage = lidarTile.TileImage;
            lidarPicInfoTl.Text = lidarTile.Text;
            lidarPicInfoTl.Update();
            lidarPicInfoTl.Refresh();

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
             lidar = lidarQ.Last();
            if (lidar != null) {
                lidar.ConnectToTheLidar();
                lidarConfigLogview.Text = lidar.PingLidarResponse();
            }
        }

        private void PullFrameBtn_Click(object sender, EventArgs e)
        {


            if(lidar== null)
                lidar = lidarQ.Peek();
             lidarConfigLogview.Clear();

                 lidar.Request = ConstantStringMessage.ONE_TELEGRAMM;
                lidarConfigLogview.Text = lidar.PullAFrame();
                lidarConfigLogview.Update();
                lidarConfigLogview.Refresh();
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

        private void metroButton1_Click(object sender, EventArgs e)
        {
           
        }
        private void AddNewGroupBox(String ip, String name, String type)
        {

           this.mainfLPl.Controls.Add(AddGroupBox(ip, name, type));
        }

        private GroupBox AddGroupBox(String ip, String name, String type) {

            GroupBox   groupBox1 = new GroupBox();
             MetroTile metroTile1 = new MetroTile();
            MetroLabel metroLabel1 = new MetroLabel();
            MetroLabel metroLabel2 = new MetroLabel();
            MetroLabel metroLabel3 = new MetroLabel();
            MetroLabel metroLabel4 = new MetroLabel();
            MetroLabel mainIPLbl = new MetroLabel();
            MetroLabel mainTypeLbl = new MetroLabel();
            MetroLabel mainDataLbl = new MetroLabel();
            PictureBox statusPicBx = new PictureBox();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(mainDataLbl);
            groupBox1.Controls.Add(mainTypeLbl);
            groupBox1.Controls.Add(mainIPLbl);
            groupBox1.Controls.Add(metroLabel4);
            groupBox1.Controls.Add(metroLabel3);
            groupBox1.Controls.Add(metroLabel2);
            groupBox1.Controls.Add(metroLabel1);
            groupBox1.Controls.Add(metroTile1);
            groupBox1.Controls.Add(statusPicBx);

            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = " ";
            groupBox1.BackColor = System.Drawing.Color.White;
            groupBox1.Size = new System.Drawing.Size(344, 242); 
            groupBox1.Text = name;
            // 
            // metroTile1
            // 
            metroTile1.Location = new System.Drawing.Point(6, 19);
            metroTile1.Name = "metroTile1";
            metroTile1.Size = new System.Drawing.Size(152, 217);
            metroTile1.Style = MetroFramework.MetroColorStyle.White;
            metroTile1.TabIndex = 0;
            metroTile1.Text = "metroTile1";

            SetDeviceImage(metroTile1,  name);
            metroTile1.UseTileImage = true;
            // 
            // metroLabel1
            // 
            metroLabel1.AutoSize = true;
            metroLabel1.Location = new System.Drawing.Point(177, 19);
            metroLabel1.Name = "metroLabel1";
            metroLabel1.Size = new System.Drawing.Size(23, 19);
            metroLabel1.TabIndex = 1;
            metroLabel1.Text = "IP:";
            // 
            // metroLabel2
            // 
            metroLabel2.AutoSize = true;
            metroLabel2.Location = new System.Drawing.Point(175, 70);
            metroLabel2.Name = "metroLabel2";
            metroLabel2.Size = new System.Drawing.Size(39, 19);
            metroLabel2.TabIndex = 2;
            metroLabel2.Text = "Type:";
            // 
            // metroLabel3
            // 
            metroLabel3.AutoSize = true;
            metroLabel3.Location = new System.Drawing.Point(175, 125);
            metroLabel3.Name = "metroLabel3";
            metroLabel3.Size = new System.Drawing.Size(39, 19);
            metroLabel3.Text = "Data:";
            // 
            // metroLabel4
            // 
            metroLabel4.AutoSize = true;
            metroLabel4.Location = new System.Drawing.Point(175, 195);
            metroLabel4.Name = "statuslbl";
            metroLabel4.Size = new System.Drawing.Size(23, 19);
            metroLabel4.TabIndex = 1;
            metroLabel4.Text = "Status";
            // 
            // mainIPLbl
            // 
            mainIPLbl.AutoSize = true;
            mainIPLbl.Location = new System.Drawing.Point(245, 19);
            mainIPLbl.Name = "mainIPLbl";
            mainIPLbl.Text = ip;
            mainIPLbl.Size = new System.Drawing.Size(0, 0);
            
            // 
            // mainTypeLbl
            // 
            mainTypeLbl.AutoSize = true;
            mainTypeLbl.Location = new System.Drawing.Point(245, 70);
            mainTypeLbl.Name = "mainTypeLbl";
            mainTypeLbl.Text = type;
            mainTypeLbl.Size = new System.Drawing.Size(0, 0);
            mainTypeLbl.TabIndex = 5;
            // 
            // mainDataLbl
            // 
            mainDataLbl.AutoSize = true;
            mainDataLbl.Location = new System.Drawing.Point(248, 125);
            mainDataLbl.Name = "mainDataLbl";
            mainDataLbl.Size = new System.Drawing.Size(0, 0);

            // 
            // pictureBox1
            //
           
            statusPicBx.Image = global::Penguin__REMS_Project.Properties.Resources.Red1;
            statusPicBx.Location = new System.Drawing.Point(265, 175);
            statusPicBx.Name = name+"PicBx";
            statusPicBx.Size = new System.Drawing.Size(48, 49);
            statusPicBx.TabIndex = 2;
            statusPicBx.TabStop = false;






            return groupBox1;
        }
        private void SetDeviceImage(MetroTile vMetroTile, String name)
        {

                if (name.Contains("LMS511"))
                {
                    vMetroTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;
                }
                else if (name.Contains("MRS611"))
                {
                    vMetroTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;

                }
                else if (name.Contains("VLP"))
                {
                    vMetroTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;

                }
                else if (name.Contains("Leinshen"))
                {
                
                      vMetroTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.leishen_res;
                }
                else if (name.Contains("")) {

                    vMetroTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;

                }

        }
        #endregion




    }

}
