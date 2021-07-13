using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;


namespace Penguin__REMS_Project
{
    class LidarGroupBox : GroupBox
    {
        #region Variable
        MetroTile lidarPicTile;
        MetroLabel metroLabel1 ;
        MetroLabel metroLabel2 ;
        MetroLabel metroLabel3 ;
        MetroLabel metroLabel4 ;
        MetroLabel mainIPLbl  ;
        MetroLabel mainTypeLbl;
        MetroLabel mainDataLbl ;
        PictureBox statusPicBx;
      
        #endregion
        public LidarGroupBox(String name, String ip, String type) {
            this.Name = name;
            this.Text = name;
            InitItem();
            InitTheGroup();
            SetItems( name, ip,  type);
        }

        #region Group Box Functions
        private void InitTheGroup() {

            this.Controls.Add(lidarPicTile);
            this.Controls.Add(mainDataLbl);
          
            this.Controls.Add(mainTypeLbl);
            this.Controls.Add(mainIPLbl);
            this.Controls.Add(metroLabel4);
            this.Controls.Add(metroLabel3);
            this.Controls.Add(metroLabel2);
            this.Controls.Add(metroLabel1);
            this.Controls.Add(statusPicBx);
            this.Location = new System.Drawing.Point(3, 3);
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(344, 242);
        }
        private void InitItem() {
             lidarPicTile = new MetroTile();
             metroLabel1 = new MetroLabel();
             metroLabel2 = new MetroLabel();
             metroLabel3 = new MetroLabel();
             metroLabel4 = new MetroLabel();
            mainIPLbl = new MetroLabel();
           mainTypeLbl = new MetroLabel();
            mainDataLbl = new MetroLabel();
           statusPicBx = new PictureBox();

        }
        #endregion

        #region GroupBox Items Functions

        private void SetItems(String name ,String ip, String type) {

            // 
            // metroTile1
            // 
            lidarPicTile.Location = new System.Drawing.Point(6, 19);
            lidarPicTile.Name = name + ":" + ip; ;
            lidarPicTile.Text = name+":"+ip;
            lidarPicTile.Size = new System.Drawing.Size(152, 217);
            lidarPicTile.Style = MetroFramework.MetroColorStyle.White;
            lidarPicTile.TabIndex = 0;
            
            


            //lidarPicTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;
             Helper.SetDeviceImage(lidarPicTile, this.Name); 
             lidarPicTile.UseTileImage = true;

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
            mainDataLbl.Text = "0 byte";

            // 
            // pictureBox1
            //

            SetStatusIcon(false);
            statusPicBx.Location = new System.Drawing.Point(265, 175);
            statusPicBx.Name = name + "PicBx";
            statusPicBx.Size = new System.Drawing.Size(48, 49);
            statusPicBx.TabIndex = 2;
        }


        public void SetStatusIcon(bool isConnected) {

            if (isConnected)
            {

                statusPicBx.Image = global::Penguin__REMS_Project.Properties.Resources.Green1;
            }
            else {


                statusPicBx.Image = global::Penguin__REMS_Project.Properties.Resources.Red1;

            }
            statusPicBx.Refresh();
        }


        #endregion

        #region GETTER


        public MetroTile LidarPicturTile {
            get => lidarPicTile;
        
        }

        public String IPString
        {
            get => mainIPLbl.Text;
        }
        #endregion

    }
}
