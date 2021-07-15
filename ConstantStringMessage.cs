using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Penguin__REMS_Project
{
    class ConstantStringMessage
    {
        #region TELEGRAMM MSG
        public const String ONE_TELEGRAMM = "Pull a telegramm";
        #endregion
        #region FILES MSG
        public const String TXTEXTENSION = ".txt";
        public const String LIDARDEVICEFILE = "lidarConfig.txt";
        public const String CONFIGFILEHEADER = "Name  	       IP_ADRESSE      PORT   TYPE";
       
        #endregion
        #region DIALOGUE MSG
        public const String DEVICEEXISTINGINSYSTEMMSG = "This Device is already in the system \try to debug or change the IP ";
        public const String ADDDEVICECAPTIONMSG = "Device On the System";
        public const String LOADINGCONFIGFILEERRORCAPTIONMSG = "Config file";
        public const String LOADINGCONFIGFILEERRORMSG = "Can load the config file";
        #endregion


        #region ADD NEW LIDAR MSS
        public const String ADDLIDARFORMCAPTIONMSQ = "ADD Lidar Form";
        public const String ADDLIDARFORMERRORMSQ = "ALL Field of the form must be  filled";

        #endregion

        #region Scan Folder
       public  const String FOLDERPATH = @"C:\Scandatas\";
      public   const String FOLDERSUFFIX = "_ScanData_"; 

        #endregion


    }
}
