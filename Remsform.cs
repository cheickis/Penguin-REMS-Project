using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFramework;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MetroFramework.Controls;

namespace Penguin__REMS_Project
{
    public partial class Remsform : MetroForm
    {

        #region 3D LIDAR

        #endregion

        #region INTEL REALSENS

        #endregion

        #region SICK LIDAR MEMBER

        #endregion

        #region Observable COLLECTION
        private ObservableCollection<TwoDLidar> sickCollections;
        private ObservableCollection<ThreeDLidar> treeDLidarCollection;
        private ObservableCollection<Realsens> realsensCollection;
        #endregion

        public Remsform()
        {
            InitializeComponent();
            InitCollection();
        }

        #region Collection Function 

        private void InitCollection()
        {
            sickCollections = new ObservableCollection<TwoDLidar>();
            treeDLidarCollection = new ObservableCollection<ThreeDLidar>();
            realsensCollection = new ObservableCollection<Realsens>();

            sickCollections.CollectionChanged += upadteSickListForm;


        }

        private void upadteSickListForm(object sender, NotifyCollectionChangedEventArgs e)
        {
           // throw new NotImplementedException();
        }
        #endregion

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }


        #region ADD New SICK FUNCTION

        private void ResetSickAddFormBtn_Click(object sender, EventArgs e)
        {
            ResertAddNewSickForm();
        }

        private void AddSickBtn_Click(object sender, EventArgs e)
        {
            string name = txtSickName.Text;
            int port = Int32.Parse(sickPortTxt.Text);
            String ipAdd = sickIpTxt.Text;
            TwoDLidar newSick = new TwoDLidar(name, ipAdd, port);
            ResertAddNewSickForm();

            sickCollections.Add(newSick);
            AddNewSickTile(name);
        }

        private void ResertAddNewSickForm()
        {
            sickPortTxt.Text = "";
            sickPortTxt.Text = "";
            txtSickName.Text = "";

        }

        private void AddNewSickTile(String name) {

             MetroTile newMetroSick   = new MetroTile();
             newMetroSick.Name = name;
             newMetroSick.Text = name;
             newMetroSick.Size = new System.Drawing.Size(110, 157);
             newMetroSick.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS1;
             newMetroSick.UseTileImage = true;
             sickFLPanel.Controls.Add(newMetroSick);
             sickFLPanel.Update();
        }
        #endregion


        private void metroTile1_Click(object sender, EventArgs e)
        {

        }
    }
  
}
