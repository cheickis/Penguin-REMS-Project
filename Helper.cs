using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
