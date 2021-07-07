﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Forms;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MetroFramework.Controls;
using System.Threading;

namespace Penguin__REMS_Project
{
    public partial class Remsform : MetroForm
    {

        #region Variable
        static Lidar lidar;
        #endregion

        #region 3D LIDAR

        #endregion

        #region INTEL REALSENS

        #endregion

        #region SICK LIDAR MEMBER

        #endregion

        #region Observable COLLECTION
        private ObservableCollection<TwoDLidar> twoDLidarCollections;
        private ObservableCollection<ThreeDLidar> treeDLidarCollection;
        private ObservableCollection<Realsens> realsensCollection;
        #endregion

        #region Queue

        Queue<Lidar> lidarQ;
        #endregion



        public Remsform()
        {
            InitializeComponent();
            InitCollection();
        }

        #region Collection Function 

        private void InitCollection()
        {
            twoDLidarCollections = new ObservableCollection<TwoDLidar>();
            treeDLidarCollection = new ObservableCollection<ThreeDLidar>();
            realsensCollection = new ObservableCollection<Realsens>();

            twoDLidarCollections.CollectionChanged += upadteSickListForm;
            lidarQ = new Queue<Lidar>();

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
                TwoLidarHandlers(NowString);
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
        private void TwoLidarHandlers(String timeStamp) {


            if (twoDLidarCollections.Count != 0)
            {
                foreach (var item in twoDLidarCollections)
                {
                    


                }
            }
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

                // handle the error  herer

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
            AddLidarTile(name, type);
            ResetAddLidarForm();
            lidarQ.Enqueue(newThreeDLidar);
            lidar = newThreeDLidar;
        }
        private void  AddTwoDLidar(String name, String ip, int port, string type)
        {

            TwoDLidar newtwoDLidar = new TwoDLidar(name, ip, port, type);
            ResetAddLidarForm();
            twoDLidarCollections.Add(newtwoDLidar);
            AddLidarTile(name, type);
            lidarQ.Enqueue(newtwoDLidar);
            lidar = newtwoDLidar;

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

        #endregion

        #region  Ping and  Pull
        private void PingNewLidarBtn_Click(object sender, EventArgs e)
        {
             lidar = lidarQ.Peek();
            if (lidar != null) {
                lidar.ConnectToTheLidar();
                lidarConfigLogview.Text = lidar.PingLidarResponse();
            }
        }

        private void PullFrameBtn_Click(object sender, EventArgs e)
        {

            ThreeDLidar temp = new ThreeDLidar("Leinshen", "192.168.1.200", 2368, "3D");

            temp.ConnectToTheLidar();

            while (true) {
                temp.InitialScanner();

                Thread.Sleep(1000);

                lidarConfigLogview.Text = temp.PullAFrame();
            }

           /* if(lidar== null)
                lidar = lidarQ.Peek();
            lidarConfigLogview.Clear();

            lidar.ConnectToTheLidar();

           
            lidarConfigLogview.Text = lidar.PullAFrame();
            */

        }

        #endregion
    }

}
