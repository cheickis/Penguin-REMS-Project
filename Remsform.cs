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
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Penguin__REMS_Project
{
    public partial class Remsform : MetroForm
    {

        #region Variables

        static Lidar lidar;
        static List<Lidar> lidars;
        private object m_lockScan = new object();
        private bool m_isRunningScan = false;
        private bool m_isAbortRequestedScan = false;
        StreamWriter writer;
        Dictionary<String, Lidar> listOfLidarInStory;

        Dictionary<String, LidarGroupBox> lidarGroupoxList;

        #region TALIN
        TalinModeLibrary TML = new TalinModeLibrary();
        static TalinDataProcess TDP = new TalinDataProcess("COM3"); // Initial Talin To COM3 as default
        //static string ComNum = "Com4";
        public byte[] TalinCommandArray = new byte[40];
        Helper.TalinModeConfigure TMC = new Helper.TalinModeConfigure();
        static bool Test_TalinInclude = true;
        string FileName = "";
        uint filecounter = 0;
        private System.Timers.Timer TT = new System.Timers.Timer(500);
        Thread TalinPositionUpdateThread;

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
        private ObservableCollection<String> talinLogCollection;

        #endregion

        #region Queue

        Queue<Lidar> lidarQ;
        #endregion
        #endregion

        #region Constructor
        public Remsform()
        {
            InitializeComponent();
            InitCollection();
            InitTalin();// Init Talin 
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
            lidarGroupoxList = new Dictionary<String, LidarGroupBox>();


            talinLogCollection = new ObservableCollection<string>();
            talinLogCollection.CollectionChanged  += UpdateTalinLog;

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

            filecounter++;
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
                UpdateLidarTxtBox();
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
            //SetSScansDatasFiles(timeStamp);
           // SetTalinDataFile(timeStamp);
            SetLidarsAndTalinDatasFiles(timeStamp);
            while (true)
            {
                //ScanningTile.Text = " Scanning ...  "; // Status message info Here
               // LidarsStartScan();
                StartTalinNavigation();
                if (m_isAbortRequestedScan)
                {
                   
                    DialogResult dr = ShowMessage("Do you to stop Scanning? ", "Stop Scanning Message", MessageBoxButtons.YesNo);
                 
                    if (dr == DialogResult.Yes)
                    {
                        //ScanningTile.Text = " Stop Scanning  "; 

                        //CloseLidarsFiles();
                        // CloseTalinFile();
                        CloseLidarAndTalinsFiles();
                        return;
                    }
                    else
                    {
                        m_isAbortRequestedScan = false;

                    }
                }
               
            }

        }



        private void SetLidarsAndTalinDatasFiles(String NowString) {
            SetTalinDataFile( NowString);
            SetSScansDatasFiles(NowString);
        }
        private void StartScanningAndNavigation() {

            LidarsStartScan();
            StartTalinNavigation();
        }

        private void CloseLidarAndTalinsFiles() {

            //CloseLidarsFiles();
            CloseTalinFile();

        }
        private void SetTalinDataFile(String NowString) {

            if (Test_TalinInclude == true)
            {
                String fileName = ConstantStringMessage.FOLDERPATH  + ConstantStringMessage.TalinFOLDERSUFFIX + NowString + ConstantStringMessage.TXTEXTENSION;
                TDP.StartWriteFile(fileName);
               
            }


        }

       private void StartTalinNavigation() {
            if (Test_TalinInclude == true)
            {
                if (Test_TalinInclude == true)
                {
                    TDP.startNavigation();
                   

                }
            }

        }
        private void CloseTalinFile()
        {

            if (Test_TalinInclude == true)
                TDP.EndWriteFile();
        }
        private void SetSScansDatasFiles(String NowString)
        {
          
            foreach (var item in lidars)
            {
                item.LidarFile = ConstantStringMessage.FOLDERPATH  + item.Name + ConstantStringMessage.FOLDERSUFFIX + NowString + ConstantStringMessage.TXTEXTENSION;
                item.OpenFile();
              
            }
        }

        private void CloseLidarsFiles() {

            foreach (var item in lidars) {
                item.CloseFile();
                item.StopScanning();
               
            }
        }
        private void LidarsStartScan()
        {
            foreach (var item in lidars)
            {
                
                lidar = item;
                UpdateLidarStatus();
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
                Func<DialogResult> m = () => System.Windows.Forms.MessageBox.Show(msg, caption, buttons);
                return (DialogResult)Invoke(m);
            }
            else
            {
                return System.Windows.Forms.MessageBox.Show(msg, caption, buttons);
            }
        }
        #endregion

        #region MAIN TAB UI 

      
        private void AddNewGroupBox(String ip, String name, String type)
        {
            LidarGroupBox lidarGrpBox = new LidarGroupBox(name, ip, type);
            lidarGrpBox.LidarPicturTile.Click += new System.EventHandler(this.LidarGroupBox_On_Click);
            lidarGrpBox.Click += new System.EventHandler(this.LidarGroupBox_On_Click);
            this.mainfLPl.Controls.Add(lidarGrpBox);

            if (!lidarGroupoxList.ContainsKey(lidar.STRIPAdresse)) {
                lidarGroupoxList.Add(lidar.STRIPAdresse, lidarGrpBox);


            }
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

            UpdateLidarTxtBox();


            lidarInfoPanel.Visible = true;
            lidarInfoPanel.Show();
            mainTb.Enabled = false;
        }


        private void UpdateLidarTxtBox() {


            lidarInfoTxtBox.Text = "";
            lidarInfoTxtBox.AppendText(lidar.ToString());
            lidarInfoTxtBox.AppendText(Environment.NewLine);

        }

        private void UpdateLidarStatus() {

            LidarGroupBox grpBx = lidarGroupoxList[lidar.STRIPAdresse];

            if (grpBx != null) lidar.DataLabel = grpBx.DataLabel;
            if (lidar.IsConnected && grpBx!=null) {

                grpBx.SetStatusIcon(lidar.IsConnected);
            }

           

        }
        #endregion

        private void CloseLidarPanelBtn_Click(object sender, EventArgs e)
        {
           
            lidarInfoPanel.Visible = false;
            lidarConfigLogview.Clear();
            mainTb.Enabled = true;
            UpdateLidarStatus();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

        }

        #region Talin_position_InOutput


    
        private void InitTalin() {

            if (TDP.Talin_Initial() == true )
            {
                SetTalinStatusIcon();
                talinLogCollection.Add("Talin is Connected!");

                ConnectToTalinBtn.Enabled = false;
                talinPortCbx.Enabled = false;
               
               /* bt_StartScan.Enabled = true;
                bt_StopScan.Enabled = false;*/
                bt_UpdatePosition.Enabled = true;
            }
            else
            {
                talinLogCollection.Add("Talin is not Connected!");
                
                if (Test_TalinInclude == false)
                {
                    ConnectToTalinBtn.Enabled = false;
                    talinPortCbx.Enabled = false;
                    bt_UpdatePosition.Enabled = false;
                    /*  bt_StartScan.IsEnabled = true;
                      bt_StopScan.IsEnabled = false;
                      bt_UpdatePosition.IsEnabled = false;
                      */
                }
                else
                {
                    /*bt_StartScan.IsEnabled = false;
                    bt_StopScan.IsEnabled = false;
                    bt_UpdatePosition.IsEnabled = false;*/
                }
            }
            FileName = DateTime.Now.ToString("yyyyMMdd");
            //Following code shall be load in the parent class.
            TMC.DatumID = "WGD";
            TMC.FileName = "TalinPositionList.txt";
            TMC.UseSurfaceMode = true;
            TMC.NorthSemishpere = true;
            TMC.ZoneID = 17;
            TMC.VMSType = 2;

            LoadPositionList();
            rb_surface.Checked = TMC.UseSurfaceMode;
            rb_underground.Checked = !(TMC.UseSurfaceMode);
            tb_DatumID.Text = TMC.DatumID;
            tb_ZoneID.Text = TMC.ZoneID.ToString();
            rb_NSphere.Checked = TMC.NorthSemishpere;
            rb_SSphere.Checked = !(TMC.NorthSemishpere);
            cmb_VMSType.SelectedIndex = TMC.VMSType;



        }

        private void tb_ZoneID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {   //For all text box required number
            Regex regex = new Regex(@"^[0-9]+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void tb_Float_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {   //For all text box required number
            Regex regex = new Regex(@"^[-+]?\d*\.?\d*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void tb_PositiveFloat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {   //For all text box required number
            Regex regex = new Regex(@"^\d*\.?\d*[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }



        private void LoadPositionList()
        {
            if (lstb_Position != null)
            { lstb_Position.Items.Clear(); }
            else
            { return; }

            using (StreamReader sr = new StreamReader(TMC.FileName)) //Shall be a database in future
            {
                //List<string> lines = new List<string>();
                string line;
                int i = 0;
                while (((line = sr.ReadLine()) != null) && (line != ""))
                {
                    string datastring;

                    datastring = line.Substring(21);
                    //a++;
                    i++;
                    lstb_Position.Items.Add(datastring);
                }
            }
        }

        private void LoadPosition()
        {
            //Select the item and load to Talin
            try
            {
                string results = lstb_Position.SelectedItem.ToString();
                string[] fields = results.Split('=', ',', ' ');
                if (fields.Length < 8)
                {
                    System.Windows.MessageBox.Show("The saved position NOT contains all required data", "Surface");
                    return;
                }
                string valuetoremove2 = "";
                bool gravityMode = false;
                fields = fields.Where(val => val != valuetoremove2).ToArray();
                tb_PositionName.Text = fields[0].Trim();
                string ModeString = fields[1].Trim();
                if ((ModeString[0] == 's') || (ModeString[0] == 'S'))
                {
                    rb_surface.Checked = true;
                }
                else
                {
                    rb_underground.Checked = true;
                    if (fields.Length < 12)
                    {
                        System.Windows.MessageBox.Show("The saved position NOT contains all required data", "Underground");
                        return;
                    }
                    gravityMode = true;
                }
                string temp1 = fields[2].Trim();
                if ((temp1[0] == 'N') || (temp1[0] == 'n'))
                {
                    rb_NSphere.Checked = true;
                }
                else
                {
                    if ((temp1[0] == 'S') || (temp1[0] == 's'))
                    {
                        rb_SSphere.Checked = true;
                    }
                    else
                    {
                        UInt16 format = UInt16.Parse(fields[2]);
                        if ((UInt16)(format & 0x8000) > 0)
                        {
                            rb_SSphere.Checked = true;
                        }
                        else
                        {
                            rb_NSphere.Checked = true;
                        }
                    }
                }

                cmb_VMSType.SelectedIndex = TMC.VMSType;

                tb_ZoneID.Text = fields[3].Trim();
                tb_Easting.Text = fields[4].Trim();
                tb_Northing.Text = fields[5].Trim();
                tb_Elevation.Text = fields[6].Trim();
                tb_DatumID.Text = fields[7].Trim();
                if (gravityMode)
                {
                    tb_ReferenceGravity.Text = fields[8].Trim();
                    tb_MaterialDensity.Text = fields[9].Trim();
                    tb_deflection1.Text = fields[10].Trim();
                    tb_deflection2.Text = fields[11].Trim();
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("The saved position format is wrong");
            }

        }

        private void lstb_Position_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadPosition();
        }

        private string[] getPositionData()
        {
            string[] results;
            if (rb_surface.Checked == true)
            {
                results = new string[10];
                results[3] = "Surface";
            }
            else
            {
                results = new string[14];
                results[3] = "Underground";
                results[10] = tb_ReferenceGravity.Text;
                results[11] = tb_MaterialDensity.Text;
                results[12] = tb_deflection1.Text;
                results[13] = tb_deflection2.Text;
            }
            results[2] = tb_PositionName.Text.Trim();
            //UInt16 Format = 0;
            if (rb_NSphere.Checked == true)
            {
                //Format = 0;
                results[4] = "North";
            }
            else
            {
                //Format = 0x8000;
                results[4] = "South";
            }
            //Format = (UInt16)(Format + cmb_VMSType.SelectedIndex);
            DateTime DT = DateTime.Now;
            results[0] = DT.ToString("yyyy-MM-dd");
            results[1] = DT.ToString("HH:mm:ss");
            //results[4] = Format.ToString();
            results[2] = tb_PositionName.Text;
            UInt16 temp;
            if ((UInt16.TryParse(tb_ZoneID.Text.Trim(), out temp) == false) || (temp == 0))
            {
                System.Windows.MessageBox.Show(" Zone Number must larger than zero.");
                return null;
            }
            if (temp > 60)
            {
                System.Windows.MessageBox.Show(" Zone Number must ssmaller than 60.");
                return null;
            }
            results[5] = temp.ToString();
            results[6] = tb_Easting.Text.Trim();
            results[7] = tb_Northing.Text.Trim();
            results[8] = tb_Elevation.Text.Trim();
            if (tb_DatumID.Text.Trim() == "WGS-48")
            {
                results[9] = "WGD";
            }
            else
            {
                results[9] = tb_DatumID.Text.Trim();
            }
            return results;

        }
      
        private void Bt_UpdatePosition_Click(object sender, RoutedEventArgs e)
        {
            string[] results = getPositionData();
            if (results == null)
            {
                System.Windows.MessageBox.Show("Input position format is incorrect. ", "Update Failed!");
                return;
            }
            if ((double.Parse(results[6]) == 0) || (double.Parse(results[7]) == 0) || (double.Parse(results[8]) == 0))
            {
                System.Windows.MessageBox.Show("Input Easting, Northing and Elevation value shall not be zero. ", "Update Failed!");
                return;
            }
            byte[] command = TML.generatePositionUpdateCommand(results, (UInt16)cmb_VMSType.SelectedIndex);
            TDP.PositionUpdateCommand = command;
            //System.Buffer.BlockCopy(command, 0, TalinCommandArray, 0, command.Length);
            TalinPositionUpdateThread = new Thread(TDP.Talin_InitialPosition);
            TalinPositionUpdateThread.Start();
            //TDP.Talin_InitialPosition(command);
            TT.Elapsed += TT_Elapsed;
            TT.Start();
        }

        private
        void TT_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (TalinDataProcess.AlignmentTime == 0)
            {

                    TalinPositionUpdateThread.Abort();
                   // bt_StartScan.IsEnabled = true;   // button start connexion
                    TT.Stop();
                 //  lb_Status.Content = " Talin Aligned. Scan can Start.";
                    talinLogCollection.Add( " Talin Aligned. Scan can Start.");

            }
            else
            {
                talinLogCollection.Add("Talin Alignment Time = " + TalinDataProcess.AlignmentTime.ToString());
              
            }
        }

        private void bt_SavePosition_Click(object sender, RoutedEventArgs e)
        {
            string[] results = getPositionData();
            if (results == null)
                return;
            string return_value = results[0] + " " + results[1];
            for (int i = 2; i < results.Length; i++)
            {
                return_value += (", " + results[i]);
            }

            string[] str = new string[lstb_Position.Items.Count + 1];
            bool rewrite = false;
            int j = 0;
            //Read the list
            using (StreamReader sr = new StreamReader(TMC.FileName)) //Shall be a database in future
            {
                //List<string> lines = new List<string>();
                string line;

                while (((line = sr.ReadLine()) != null) && (line != ""))
                {
                    string[] fields = line.Split(',', '=', ' ');
                    string valuetoremove2 = "";
                    fields = fields.Where(val => val != valuetoremove2).ToArray();
                    if (fields[2].Trim() == results[2]) //if same name rewrite the file
                    {
                        str[j] = (return_value + "\n");
                        rewrite = true;
                    }
                    else
                    {
                        str[j] = line;
                    }
                    j++;

                }
            }
            if (rewrite == false)  //add new one
            {
                str[j] = return_value;
            }
            else
            {
                j--;
            }
            using (StreamWriter sw = new StreamWriter(TMC.FileName))
            {
                for (int k = 0; k <= j; k++)
                    sw.WriteLine(str[k]);
            }

            LoadPositionList();

        }

        private void bt_DeletePosition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Since we load everything to the list even nonvalid data, following code can work
                int temp = lstb_Position.SelectedIndex;
                string[] str = new string[lstb_Position.Items.Count - 1];
                int i = 0;
                using (StreamReader sr = new StreamReader(TMC.FileName)) //Shall be a database in future
                {
                    //List<string> lines = new List<string>();
                    string line;
                    int j = 0;
                    while (((line = sr.ReadLine()) != null) && (line != ""))
                    {
                        string[] fields = line.Split(',', '=', ' ');
                        string valuetoremove2 = "";
                        fields = fields.Where(val => val != valuetoremove2).ToArray();
                        if (j != temp) //if same name rewrite the file
                        {
                            str[i] += line;
                            i++;
                            //if (i >= lstb_Position.Items.Count)
                            //    break;
                        }
                        j++;

                    }
                }
                using (StreamWriter sw = new StreamWriter(TMC.FileName))
                {
                    for (int k = 0; k < i; k++)
                        sw.WriteLine(str[k]);
                }

                LoadPositionList();
            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show("Failed to delete this position. Error \n" + er.ToString());
            }
        }

        public byte[] ReturnTalinCommand()  //The returned byte is for server-client application
        {
            return TalinCommandArray;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Test_TalinInclude == true)
                TDP.stopNavigation();
        }
        #endregion

        #region Talin  Config UI  Button Handler
        private void ConnectToTalinBtn_Click(object sender, EventArgs e)
        {
            String port = talinPortCbx.Text;

            if (!TDP.IsConnected) {

                TDP.TalinPort = port;
                TDP.Connect_Talin();
                EnableOrDiseableTalinConnectGroupoxItem();
            }
        }

        private void DisconnectedToTalinBtn_Click(object sender, EventArgs e)
        {
            TDP.Disconnect_Talin();

            talinLogCollection.Add("Talin is  disconnected!");
           
            EnableOrDiseableTalinConnectGroupoxItem();
        }

        private void EnableOrDiseableTalinConnectGroupoxItem() {

            Helper.EnableOrDisableButton(ConnectToTalinBtn);
            Helper.EnableOrDisableButton(DisconnectedToTalinBtn);
            Helper.EnableOrDisableButton(TalinStatusBtn);
            Helper.EnableOrDisableComboBox(talinPortCbx);
            SetTalinStatusIcon();


        }
    
      
        private void TalinStatusBtn_Click(object sender, EventArgs e)
        {



        }
        #endregion

        #region TALIN CONFIG UI HANDLER

        public void SetTalinStatusIcon() {
            talinPortGrpBx.Text = TDP.TalinPort;
            if (TDP.IsConnected) {

                talinStatusPicBox.Image = global::Penguin__REMS_Project.Properties.Resources.TalonGreenStatus;
                talinStatusGrpPicBx.Image = global::Penguin__REMS_Project.Properties.Resources.Green1;
                
                talinStatusPicBox.Refresh();
            }
            else
            {
                talinStatusPicBox.Image = global::Penguin__REMS_Project.Properties.Resources.RedTalon1;
                talinStatusGrpPicBx.Image = global::Penguin__REMS_Project.Properties.Resources.Red1;
                talinStatusPicBox.Refresh();
            }

        }


        public void UpdateTalinLog(object sender, NotifyCollectionChangedEventArgs e)
        {
            Helper.UpdateTxtBWithDataCollector(talinLogTxt,talinLogCollection);
        }


        
        #endregion


    }

}
