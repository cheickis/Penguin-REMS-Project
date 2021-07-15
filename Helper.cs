using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Penguin__REMS_Project
{
     static class Helper
    {
        public static void SetDeviceImage(MetroTile vMetroTile, String name)
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
            else if (name.Contains(""))
            {

                vMetroTile.TileImage = global::Penguin__REMS_Project.Properties.Resources.isckLMS;

            }
            vMetroTile.Refresh();
        }

        public static string FormatBytes(Int64 bytes)
        {
            if (bytes >= 0x1000000000000000) { return ((double)(bytes >> 50) / 1024).ToString("0.### EB"); }
            if (bytes >= 0x4000000000000) { return ((double)(bytes >> 40) / 1024).ToString("0.### PB"); }
            if (bytes >= 0x10000000000) { return ((double)(bytes >> 30) / 1024).ToString("0.### TB"); }
            if (bytes >= 0x40000000) { return ((double)(bytes >> 20) / 1024).ToString("0.### GB"); }
            if (bytes >= 0x100000) { return ((double)(bytes >> 10) / 1024).ToString("0.### MB"); }
            if (bytes >= 0x400) { return ((double)(bytes) / 1024).ToString("0.###") + " KB"; }
            return bytes.ToString("0 Bytes");
        }

        public static void UpdateTxtBWithDataCollector(MetroTextBox txtB, ObservableCollection<string> obs)
        {

            if (txtB.InvokeRequired)
            {

                txtB.Invoke(new MethodInvoker(delegate {

                    if (obs.Count > 0)
                    {

                        txtB.AppendText(obs.ElementAt(obs.Count - 1));
                        txtB.AppendText(Environment.NewLine);
                    }

                }));
            }
            else if (obs.Count > 0)
            {

                txtB.AppendText(obs.ElementAt(obs.Count - 1));
                txtB.AppendText(Environment.NewLine);
            }

        }


        #region  Tatlin Structure
        public struct TalinModeConfigure
        {
            public int VMSType;
            public string DatumID;
            public int ZoneID;
            public bool NorthSemishpere;
            public bool UseSurfaceMode;
            public string FileName;// 
            public TalinModeConfigure(string ConfigureString)
            {
                string[] fields = ConfigureString.Split(',', '=', ' ');
                string valuetoremove2 = "";
                fields = fields.Where(val => val != valuetoremove2).ToArray();
                FileName = fields[0].Trim();
                UseSurfaceMode = Boolean.Parse(fields[1].Trim());
                NorthSemishpere = Boolean.Parse(fields[2].Trim());
                VMSType = int.Parse(fields[3].Trim());
                DatumID = fields[5].Trim();
                ZoneID = int.Parse(fields[4].Trim());
            }
        }
        #endregion

    }
}
