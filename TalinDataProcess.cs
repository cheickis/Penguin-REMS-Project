
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Collections.Specialized;

namespace Penguin__REMS_Project
{
    #region Delegate
    delegate void ObtainPositonDelegate(double x, double y, double z);
    delegate void ObtainOrientationDelegate(double r, double p, double y);

    #endregion
   
    class TalinDataProcess
    {
        #region Variable

        
        private ObservableCollection<String> dataSizeCollection;
        private ObservableCollection<String> logDataCollection;
      
        delegate void DataProcessDelegate(byte[] _data);

        public bool Write2File = false;
        public bool GetStatus = false; //
        private StreamWriter sw;
        public bool WriteRawData = false;
        public bool AllowRewriteConfigure = false;

        //for VCT information TT01x, Reading information need two layer command
        //First layer is command of TR01Hx, Second layer is command [10, 10, 60]
        //Then TT01x bytes will be read.
        private static ManualResetEvent receiveGeneralCommand = new ManualResetEvent(false);
        private string readConfiguration1;
        private string presetConfiguration1;
        private byte[] Configuration1PresetCommand;
        private bool Configuration1Presence = false;
        private static ManualResetEvent receiveConfiguration1 = new ManualResetEvent(false);
        private string readConfiguration2;
        private string presetConfiguration2;
        private byte[] Configuration2PresetCommand;
        private bool Configuration2Presence = false;
        private static ManualResetEvent receiveConfiguration2 = new ManualResetEvent(false);
        private string readBoreSight1;
        private string presetBoreSight1;
        private byte[] BoreSightPresetCommand;
        private bool BoresightPresence = false;
        private static ManualResetEvent receiveBoreSight1 = new ManualResetEvent(false);
        private string readLeverArm;
        private string presetLeverArm;
        private byte[] LeverArmPresetCommand;
        private bool LeverArmPresence = false;
        private static ManualResetEvent receiveLeverArm = new ManualResetEvent(false);
        private string readGravityReference;
        private string presetGravityReference;
        private byte[] GravityReferencePresetCommand;
        private static ManualResetEvent receiveGravityReference = new ManualResetEvent(false);
        private string CommPort = "COM7";
        private Boolean isConnected = false;
        private byte presetInitialZone;
        private byte[] presetInitialEasting = new byte[4];
        private byte[] presetInitialNorthing = new byte[4];
        private byte[] presetInitialElevation = new byte[4];
        private static ManualResetEvent receiveInitialPosition = new ManualResetEvent(false);

        private byte[] readStatus = new byte[64];
        private static ManualResetEvent receiveStatus = new ManualResetEvent(false);

        private const byte DLE = 0x10;
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const int positionaccuracy = -7;
        private const int angleaccuracy = -31;
        private const int offsetaccuracy = -7;
        private const int linearvelocityaccuracy = -20;
        private const int angularvelocityaccuracy = -12;
        private const int angularaccelerationaccuracy = -10;
        private const int lieanraccelerationaccuracy = -7;
        private const int headingerroraccuracy = -15;
        private const int rotationpointaccuracy = -15;
        private static readonly byte[] Prefix = new byte[] { DLE, STX };
        private static readonly byte[] Surfix = new byte[] { DLE, ETX };
        private bool requestCommandSend = false;
        private UInt16 ZoneID;
        private UInt16 PositionFormat;
        private double initialEast, initialNorth, initialElevation;
        private double initialRefereceGravity, initialMaterialDensity, initialDeflection1, initialDeflection2;

        public double elevation_display = 0;
        public int TalinMode = 0;
        private TalinStatus TS = new TalinStatus();

        private byte singlebyte = 0x00;
        private bool singlebyteexist = false;
        #endregion

        #region Structure
        #region TALIN STATUS
        public struct TalinStatus
        {
            public bool NavigationMode;
            public bool OutofTravelLock;
            public int  AlignmentTimeLeft;
            public bool Configuration1Presence;
            public bool BoreSightPresence;
            public bool AbleCompleteAlign;
            public bool InitialPositionReceived;

            public TalinStatus(byte MW2, byte AW11, byte SW12, byte ATG1, byte ATG2)
            {
                NavigationMode = ((MW2 & 0x40) > 0);
                OutofTravelLock = !((SW12 & 0x02) > 0);
                byte[] AlignTimebyte = { ATG1, ATG2 };
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(AlignTimebyte);
                }
                AlignmentTimeLeft = Convert.ToInt32(BitConverter.ToUInt16(AlignTimebyte, 0));
                Configuration1Presence = !((AW11 & 0x20) > 0);
                BoreSightPresence = !((AW11 & 0x40) > 0);
                AbleCompleteAlign = !((AW11 & 0x08) > 0);
                InitialPositionReceived = !((AW11 & 0x10) > 0);
            }

        }
        #endregion

        #region INUStatusDataAnalyze
        public struct INUStatus
        {
            public bool ZuptinProgress;
            public bool NavigationMode;
            public bool PowerUp;
            public bool PowerDown;

            public bool ZuptStopRequest;
            public bool PositionUpdateRequest;
            public bool PositionUpdateInProgress;
            public bool InertialAltitudeFixed;
            public bool VehicleInMotion;
            public bool INUMoving;
            public bool ZoneExtension;
            public bool TravelLockStatus;
            public bool ZuptEnable;
            public bool GPSEnable;
            public bool GPSDataSyncInProgress;

            public bool PreviuosShutdownAbnormal;
            public bool BoreSightNotPresent;
            public bool ConfigurationDataNotPresent;
            public bool InitialPositionNotReceived;
            public bool UnableCompleteAlignment;
            //public bool AlignInterrupt;
            public bool PositionUpdateInterrupt;
            public bool ZuptInterrupt;

            public bool GPSWarningPresent;
            public bool GPS1PPSFault;
            public bool GPSINSDatumDifferent;
            public bool GSPAntennaFault;
            public bool GPSUnkey;
            public bool GPSDataTransferFailure;
            public bool MotionDuringShutdown;
            public bool MotionDuringUpdateRequest;
            public bool MotionDuring_OutofTravelLock;
            public bool AltitudePositionUpdateRejected;
            public bool GPS_PositionUpdate_Different;
            public bool HorizontalPositionUpdateRejected;


            public uint TimeTag;  //MicroSecond
            public uint AlignTime2Go;
            public uint ShockTimes;
            public double EstimateHeadingError;
            public double EstimateHorizontalError;
            public double EstimateVerticalError;
            public double SphericalErrorProbability;
            public uint ZuptIntervalTimer;

            public INUStatus(byte Mode1, byte Mode2, byte Status11, byte Status12, byte Status21, byte Status22, byte Alert11, byte Alert12, byte Alert21, byte Alert22, byte Alert32, uint tt, uint AlignT, uint ShockT, double HeadError, double HorizontalError, double VerticalError, double SphericalError, uint ZIT)
            {

                AlignTime2Go = AlignT;
                ShockTimes = ShockT;
                EstimateHeadingError = HeadError;
                EstimateHorizontalError = HorizontalError;
                EstimateVerticalError = VerticalError;
                SphericalErrorProbability = SphericalError;
                TimeTag = tt;
                ZuptIntervalTimer = ZIT;

                #region Readmode
                if ((Mode1 & 0x10) == 0x10)
                    PowerUp = true;
                else
                    PowerUp = false;

                if ((Mode1 & 0x02) == 2)
                    PowerDown = true;
                else
                    PowerDown = false;

                if ((Mode2 & 0x08) == 0x08)
                    ZuptinProgress = true;
                else
                    ZuptinProgress = false;

                if ((Mode2 & 0x40) == 0x40)
                    NavigationMode = true;
                else
                    NavigationMode = false;
                #endregion

                #region ReadStatus
                //Status word ------------------------------
                if ((Status11 & 0x80) == 0x80)
                    ZuptStopRequest = true;
                else
                    ZuptStopRequest = false;

                if ((Status11 & 0x40) == 0x40)
                    PositionUpdateRequest = true;
                else
                    PositionUpdateRequest = false;

                if ((Status11 & 0x04) == 0x04)
                    PositionUpdateInProgress = true;
                else
                    PositionUpdateInProgress = false;

                if ((Status12 & 0x40) == 0x40)
                    InertialAltitudeFixed = true;
                else
                    InertialAltitudeFixed = false;

                if ((Status12 & 0x20) == 0x20)
                    VehicleInMotion = true;
                else
                    VehicleInMotion = false;

                if ((Status12 & 0x10) == 0x10)
                    INUMoving = true;
                else
                    INUMoving = false;

                if ((Status12 & 0x04) == 0x04)
                    ZoneExtension = true;
                else
                    ZoneExtension = false;

                if ((Status12 & 0x02) == 0x02)
                    TravelLockStatus = true;
                else
                    TravelLockStatus = false;

                if ((Status21 & 0x80) == 0x80)
                    GPSEnable = true;
                else
                    GPSEnable = false;

                if ((Status21 & 0x40) == 0x40)
                    GPSDataSyncInProgress = true;
                else
                    GPSDataSyncInProgress = false;

                if ((Status22 & 0x01) == 0x01)
                    ZuptEnable = true;
                else
                    ZuptEnable = false;

                #endregion

                #region ReadAlert
                if ((Alert11 & 0x80) == 0x80)
                    PreviuosShutdownAbnormal = true;
                else
                    PreviuosShutdownAbnormal = false;

                if ((Alert11 & 0x40) == 0x40)
                    BoreSightNotPresent = true;
                else
                    BoreSightNotPresent = false;

                if ((Alert11 & 0x20) == 0x20)
                    ConfigurationDataNotPresent = true;
                else
                    ConfigurationDataNotPresent = false;

                if ((Alert11 & 0x10) == 0x10)
                    InitialPositionNotReceived = true;
                else
                    InitialPositionNotReceived = false;

                if ((Alert11 & 0x08) == 0x08)
                    UnableCompleteAlignment = true;
                else
                    UnableCompleteAlignment = false;

                if ((Alert11 & 0x02) == 0x02)
                    PositionUpdateInterrupt = true;
                else
                    PositionUpdateInterrupt = false;

                if ((Alert11 & 0x01) == 0x01)
                    ZuptInterrupt = true;
                else
                    ZuptInterrupt = false;


                if ((Alert12 & 0x01) == 0x01)
                    GPSWarningPresent = true;
                else
                    GPSWarningPresent = false;

                if ((Alert21 & 0x80) == 0x80)
                    GPS1PPSFault = true;
                else
                    GPS1PPSFault = false;

                if ((Alert21 & 0x40) == 0x40)
                    GPSINSDatumDifferent = true;
                else
                    GPSINSDatumDifferent = false;

                if ((Alert21 & 0x20) == 0x20)
                    GSPAntennaFault = true;
                else
                    GSPAntennaFault = false;

                if ((Alert21 & 0x10) == 0x10)
                    GPSUnkey = true;
                else
                    GPSUnkey = false;

                if ((Alert21 & 0x08) == 0x08)
                    MotionDuringShutdown = true;
                else
                    MotionDuringShutdown = false;

                if ((Alert21 & 0x02) == 0x02)
                    MotionDuringUpdateRequest = true;
                else
                    MotionDuringUpdateRequest = false;

                if ((Alert21 & 0x01) == 0x01)
                    MotionDuring_OutofTravelLock = true;
                else
                    MotionDuring_OutofTravelLock = false;

                if ((Alert22 & 0x80) == 0x80)
                    GPSDataTransferFailure = true;
                else
                    GPSDataTransferFailure = false;

                if ((Alert32 & 0x10) == 0x10)
                    GPS_PositionUpdate_Different = true;
                else
                    GPS_PositionUpdate_Different = false;

                if ((Alert32 & 0x08) == 0x08)
                    AltitudePositionUpdateRejected = true;
                else
                    AltitudePositionUpdateRejected = false;

                if ((Alert32 & 0x04) == 0x04)
                    HorizontalPositionUpdateRejected = true;
                else
                    HorizontalPositionUpdateRejected = false;

                #endregion
            }

        }

        public StringBuilder INUStatus4Display(INUStatus _status)
        {
            StringBuilder str = new StringBuilder();
            if (_status.TimeTag != 0)
                str.Append("Talin Timestamp (microsecond): " + _status.TimeTag.ToString());
            else
                str.Append("Data has error, Timestamp shall not be Zero.");
           
            str.Append(Environment.NewLine);
        

            if (_status.AlignTime2Go != 0)
                str.Append("Talin Align Time to GO (second): " + _status.AlignTime2Go.ToString() );
            else
                str.Append("Talin Aligned!.");

            str.Append(Environment.NewLine);

            if (_status.ConfigurationDataNotPresent == true)
                str.Append("Talin Vehicle Configuration not present!");
            else
                str.Append("Talin Vehicle Configuration present!.");

            str.Append(Environment.NewLine);

            if (_status.BoreSightNotPresent == true)
                str.Append("Talin Bore Sight not present!");
            else
                str.Append("Talin Bore Sight present!.");

            str.Append(Environment.NewLine);

            if (_status.InitialPositionNotReceived == true)
                str.Append("Talin Initial Position not Received!");
            else
                str.Append("Talin Initial Position Received!.");
            
            str.Append(Environment.NewLine);

            if (_status.ZuptIntervalTimer != 0)
                str.Append("Talin last Zupt is in" + _status.ZuptIntervalTimer.ToString() + " ago!");
            else
                str.Append("Talin just Zupted or data error!.");

            str.Append(Environment.NewLine);

            if (_status.ZuptEnable == true)
                str.Append("Talin Zupt is Enabled!");
            else
                str.Append("Talin Zupt is Disabled!.");

            str.Append(Environment.NewLine);

            if (_status.ZuptinProgress == true)
                str.Append("Talin Zupt is in progress!");
            else
                str.Append("Talin not Zupting!.");
            
            str.Append(Environment.NewLine);

            if (_status.ZuptIntervalTimer != 0)
                str.Append("Talin last Zupt is in" + _status.ZuptIntervalTimer.ToString() + " ago!");
            str.Append(Environment.NewLine);
            
            if (_status.TravelLockStatus == true)
                str.Append("Talin is in travel lock condition!");
            else
                str.Append("Talin is not in travel lock condition!!.");
            //str.Append(Environment.NewLine);
            
            if (_status.UnableCompleteAlignment == true)
                str.Append("Talin can not complete alignment! Stop motion of the INU.\n");
            str.Append(Environment.NewLine);
            
            if (_status.NavigationMode == true)
                str.Append("Talin Mode : In Navigation Mode.");
            else
                str.Append("Talin Mode : Not In Navigation Mode.");
            str.Append(Environment.NewLine);
            
            if (_status.PowerUp == true)
                str.Append("Talin Mode : In Power Up Mode.");
            else
                str.Append("Talin Mode : Not In Power Up Mode.");
            str.Append(Environment.NewLine);
            
            if (_status.PowerDown == true)
                str.Append("Talin Mode : In Power Down Mode.");
            else
                str.Append("Talin Mode : Not In Power Down Mode.");
            str.Append(Environment.NewLine);

            if (_status.ZuptinProgress == true)
                str.Append("Talin Mode : Zupt in Progress.");
            else
                str.Append("Talin Mode : Zupt not in Progress.");
            
            
            return str;

        }

        public INUStatus InterpretStatusData(byte[] buffer)
        {
            byte[] TimeStampbyte = { buffer[4], buffer[5] };
            byte[] AlignTimebyte = { buffer[26], buffer[27] };
            byte[] HeadErrorbyte = { buffer[28], buffer[29] };
            byte[] HorizontalErrorbyte = { buffer[30], buffer[31], buffer[32], buffer[33] };
            byte[] VerticalErrorbyte = { buffer[34], buffer[35], buffer[36], buffer[37] };
            byte[] ShockCounterbyte = { buffer[38], buffer[39] };
            byte[] SphericalErrorbyte = { buffer[40], buffer[41], buffer[42], buffer[43] };
            byte[] ZuptIntervalbyte = { buffer[50], buffer[51] };
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(AlignTimebyte);
                Array.Reverse(TimeStampbyte);
                Array.Reverse(ZuptIntervalbyte);
                Array.Reverse(ShockCounterbyte);
            }
            uint shock = BitConverter.ToUInt16(ShockCounterbyte, 0);
            uint ZIT = BitConverter.ToUInt16(ZuptIntervalbyte, 0);
            double Head = TalinSingleWordsFloat_Double(HeadErrorbyte, headingerroraccuracy) * 180.0;
            double Horizontal = TalinDoubleWordsFloat_Double(HorizontalErrorbyte, positionaccuracy);
            double Vertical = TalinDoubleWordsFloat_Double(VerticalErrorbyte, positionaccuracy);
            double Spherical = TalinDoubleWordsFloat_Double(SphericalErrorbyte, positionaccuracy);
            uint t = BitConverter.ToUInt16(TimeStampbyte, 0);
            uint At = BitConverter.ToUInt16(AlignTimebyte, 0);
            byte mode1 = buffer[0];
            byte mode2 = buffer[1];
            byte status11 = buffer[10];
            byte status12 = buffer[11];
            byte status21 = buffer[12];
            byte status22 = buffer[13];
            byte Alert11 = buffer[18];
            byte Alert12 = buffer[19];
            byte Alert21 = buffer[20];
            byte Alert22 = buffer[21];
            byte Alert32 = buffer[23];


            INUStatus results = new INUStatus(mode1, mode2, status11, status12, status21, status22, Alert11, Alert12, Alert21, Alert22, Alert32, t, At, shock, Head, Horizontal, Vertical, Spherical, ZIT);

            return results;

        }
        #endregion

        #endregion

        #region Constructor

        public TalinDataProcess(String port) {
            this.CommPort = port;

            dataSizeCollection = new ObservableCollection<string>();
            logDataCollection = new ObservableCollection<string>();
           
            
        }

        

        #endregion

        #region TalinCommandRegion

        private static readonly byte[] TalinShutDown = new byte[] { 0x41, 0x00, 0x3C, 0x00, 0x00, 0x00, 0x0A };

        private static readonly byte[] TalinTravelLock = new byte[] { 0x41, 0x00, 0x3C, 0x00, 0x00, 0x00, 0x3D };

        private static readonly byte[] TalinOutTravelLock = new byte[] { 0x41, 0x00, 0x3C, 0x00, 0x00, 0x00, 0x3C };

        private static readonly byte[] OrientationRequestOnly = new byte[] { 0x02, 0x41, 0x8B };
        private static readonly byte[] OrientationRequestAll = new byte[] { 0x02, 0x40, 0xE0 };  //in 12 Hz
        private static readonly byte[] OrientationStopAll = new byte[] { 0x02, 0x00, 0xE0 };  //in 12 Hz Stop
        private static readonly byte[] PositionRequestOnly = new byte[] { 0x03, 0x41, 0x4E };
        private static readonly byte[] PositionRequestAll = new byte[] { 0x03, 0x40, 0xD5 };     //in 12 Hz
        private static readonly byte[] PositionStopAll = new byte[] { 0x03, 0x00, 0xD5 };     //in 12 Hz
        private static readonly byte[] PositionUpdateNorth = new byte[] { 0x00, 0x28, 0xB8, 0x00, 0x00, 0x02 };
        private static readonly byte[] PositionUpdateSouth = new byte[] { 0x00, 0x28, 0xB8, 0x00, 0x80, 0x02 };
        // 0x0028 = 40 Position update 
        // 0xB800 - Data Validity (Bit: 1011 1000 0000 0000 in sequence Grid Zone Validity, Grid Square Invalidity, Horizontal Validate,
        //                                 Altitude Validate, Datum ID invalidity, Position Uncertainty inValid, Lever Arm Invalid, then spares. 
        // 0x00 - bit in sequency: Hemisphere - Northern; Zone - Normal; Altitude ref - MSL (mean sea level); spares (3); Ox80 is for southern.
        // 0x02 - Coordinate ReferenceUTM format, UTM is 2 . 


        //Gravity Mode
        //Gravity Mode Position request
        private static readonly byte[] GravityModePositionRequest = new byte[] { 0x00, 0x5A, 0xFF, 0xE6, 0x00, 0x00, 0x00, 0x00 };
        //0x005A = Mode Command 90 for gravity mode
        //0xFF = User Defined Sync code
        //0xE6 = Data Code 230
        //Others are data validity code and Configuration Code Number

        //Update the gravity mode position - Question: it is used for build Gravity model numbers or initial position?
        private static readonly byte[] GravityModePositionUpdate = new byte[] { 0x00, 0xE6, 0xC0, 0x00, 0x00, 0x00, 0xFF, 0xFF };

        private static readonly byte[] GravityModeAllPositionUpdate = new byte[] { 0x00, 0xE6, 0xF8, 0x00, 0x00, 0x00, 0xFF, 0xFF };
        // 0x00E6 = 230 - Data ID Code;
        // 0xC000 - Data Validity : C000 - only update reference positions
        //                          F800 - update the reference position/gravity reference/Material density reference/Deflection of vertical
        // 0x0000 - Configuration Code Number: 0000 use current configuration
        // 0xFFFF - Model number: 0000 for surface and 0xFFFF for underground; Maybe - 0x0001 for underground and 0x0000 for surface.

        private static readonly byte[] VehicleConfiguration1Request = new byte[] { 0x41, 0x00, 0x50, 0x00, 0x0A };
        // 0x41 message ID
        // 0x50 5HZ;
        // 0x60 reading first word to first offset word for now
        // 0x0050 = 80 - Data ID Code;
        // 0x00 - Sync Code to setect which information is request
        // 0x0A = 10 - Data Code
        // 0x00 - Current Configuration
        private static readonly byte[] VehicleConfiguration2Request = new byte[] { 0x41, 0x00, 0x50, 0x00, 0x14 };
        // 0x0E - Sync Code to setect which information is request
        // 0x014 = 20 - Data Code
        private static readonly byte[] VehicleBoreSight1Request = new byte[] { 0x41, 0x00, 0x50, 0x00, 0xA1 };
        // 0x41 message ID
        // 0x50 5HZ;
        // 0x60 reading first word to first offset word for now
        // 0x0050 = 80 - Data ID Code;
        // 0x0F - Sync Code to setect which information is request
        // 0x0A = 10 - Data Code
        // 0x00 - Current Configuration
        private static readonly byte[] TalinGeneralReading = new byte[] { 0x10, 0x02, 0x01, 0x10, 0x10, 0x60, 0x7F, 0x10, 0x03 };
        private static readonly byte[] ReadLeverArm = new byte[] { 0x41, 0x00, 0x50, 0x00, 0x8C, 0x00, 0x00 }; //F8, 00,00,00

        //private static readonly byte[] ReadRotationPoint1 = new byte[] { 0x41, 0x00, 0x50, 0x00, 0x97, 0x00, 0x00 };
        //private static readonly byte[] ReadRotationPoint2 = new byte[] { 0x41, 0x00, 0x50, 0x00, 0x98, 0x00, 0x00 };

        private static readonly byte[] ReadGravityModeReference = new byte[] { 0x41, 0x00, 0x50, 0x00, 0xE6, 0x00, 0x00 };

        private static readonly byte[] GravityModePositionCommand = new byte[] { 0x41, 0x00, 0x50, 0x00, 0x3C };
        private static readonly byte[] ReadGravityModePosition = new byte[] { 0x01, 0x40, 0x60 };
        private static readonly byte[] ReadStatus = new byte[] { 0x04, 0x10, 0x10, 0x60 };
        private static readonly byte[] ReadStatus1Hz = new byte[] { 0x04, 0x20, 0x60 };
        private static readonly byte[] ReadStatusStop = new byte[] { 0x04, 0x00, 0x60 };
        #endregion

        #region dataconvertingregion
        /// <summary>
        /// Convert a string to a double with assigned accuracy for Talin
        /// </summary>
        /// <param name="tempstr"> the string contain a decimal</param>
        /// <returns></returns>
        private byte[] Decimal_TalinDoubleWordsFloat(string tempstr, int accuracyvalue)
        {
            byte[] tempbyte = new byte[4];
            try
            {
                double tempdbl = Convert.ToDouble(tempstr) * Math.Pow(2, -accuracyvalue);
                Int32 tempint = Convert.ToInt32(tempdbl);
                tempbyte = BitConverter.GetBytes(tempint);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(tempbyte);
                }
                return tempbyte;
            }
            catch (Exception er)
            {
                byte[] temp = new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF };
                return temp;
            }

        }

        /// <summary>
        /// Convert a string with 4 bytes to a double with assigned accuracy for Talin
        /// </summary>
        /// <param name="temp"> the double number</param>
        /// <returns></returns>
        private byte[] Decimal_TalinDoubleWordsFloat(double temp, int accuracyvalue)
        {
            double tempdbl = temp * Math.Pow(2, -accuracyvalue);
            Int32 tempint = Convert.ToInt32(tempdbl);
            byte[] tempbyte = BitConverter.GetBytes(tempint);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(tempbyte);
            }
            return tempbyte;
        }
        /// <summary>
        /// Convert a double to double number with assigned accuracy for Talin.
        /// </summary>
        /// <param name="data"> 4 bytes contains a number</param>
        /// <returns></returns>
        private double TalinDoubleWordsFloat_Double(byte[] data, int accuracyvalue)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
            double tempdbl = BitConverter.ToInt32(data, 0) * Math.Pow(2, accuracyvalue);
            return tempdbl;
        }

        /// <summary>
        /// Convert a string with 2 bytes to accuracy double for Talin
        /// </summary>
        /// <param name="tempstr"> the string contain a decimal</param>
        /// <returns></returns>
        private byte[] Decimal_TalinSingleWordFloat(string tempstr, int accuracyvalue)
        {
            byte[] tempbyte = new byte[4];
            try
            {
                double tempdbl = Convert.ToDouble(tempstr) * Math.Pow(2, -accuracyvalue);
                Int32 tempint = Convert.ToInt32(tempdbl);
                tempbyte = BitConverter.GetBytes(tempint);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(tempbyte);
                }
                return tempbyte;
            }
            catch (Exception err)
            {
                byte[] temp = new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF };
                return temp;
            }
        }
        /// <summary>
        /// Convert a double number to 2 bytes floating number for Talin with assigned accuracy
        /// </summary>
        /// <param name="temp"> the double number </param>
        /// <returns></returns>
        private byte[] Decimal_TalinSingleWordFloat(double temp, int accuracyvalue)
        {
            double tempdbl = temp * Math.Pow(2, -accuracyvalue);
            Int32 tempint = Convert.ToInt32(tempdbl);
            byte[] tempbyte = BitConverter.GetBytes(tempint);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(tempbyte);
            }
            //byte[] results = new byte[2] { tempbyte[3], tempbyte[4] };
            return tempbyte;
        }
        /// <summary>
        /// Convert a double number to 2 bytes floating number for Talin with assigned accuracy.
        /// </summary>
        /// <param name="data"> 4 bytes contains a number</param>
        /// <returns></returns>
        private double TalinSingleWordsFloat_Double(byte[] data, int accuracyvalue)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
            double tempdbl = BitConverter.ToInt16(data, 0) * Math.Pow(2, accuracyvalue);
            return tempdbl;
        }

        private byte[] Hexstring2byte(string buffer)
        {
            string str = buffer;
            string[] fields = str.Split('-');
            byte[] _command = new byte[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                _command[i] = Convert.ToByte(fields[i], 16);
            }
            return _command;
        }

        private byte[] ReceiveDataRemoveHex10(byte[] Command, int bytestart)
        {
            byte[] temp = new byte[Command.Length];
            int j = 0;
            for (int i = 0; i < Command.Length; i++)
            {
                temp[j] = Command[i];
                j++;
                if (((i > bytestart) && (i < Command.Length - 3)) && ((Command[i] == 0x10) && (Command[i + 1] == 0x10)))
                {
                    i++;
                }
            }

            byte[] results = new byte[j];
            System.Buffer.BlockCopy(temp, 0, results, 0, j);
            return results;
        }

        #endregion

        #region File Handler
        public void StartWriteFile(string FileName)
        {
            sw = new StreamWriter(FileName);
           
        }
        public void EndWriteFile()
        {
            sw.Close();
            Write2File = false;
        }
        #endregion
       
        #region Talin_InitialProcess
        private bool readPresetData()
        {
            try
            {
                StreamReader sr = new StreamReader("TalinConfig.cfg");
                int j = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] fields = line.Split('=', ',');
                    string valuetoremove2 = " ";
                    fields = fields.Where(val => val != valuetoremove2).ToArray();
                    if (j == 0)
                    {
                        CommPort = fields[1].Trim();
                    }
                    if (j == 1)
                    {
                        presetConfiguration1 = fields[1].Trim();
                    }
                    if (j == 2)
                    {
                        Configuration1PresetCommand = Hexstring2byte(fields[1].Trim());
                    }

                    if (j == 3)
                    {
                        presetBoreSight1 = fields[1].Trim();
                    }
                    if (j == 4)
                    {
                        BoreSightPresetCommand = Hexstring2byte(fields[1].Trim());
                    }
                    if (j == 5)
                    {
                        presetLeverArm = fields[1].Trim();
                    }
                    if (j == 6)
                    {
                        LeverArmPresetCommand = Hexstring2byte(fields[1].Trim());
                    }
         
                    j++;
                }
                if (j < 6)
                {
                   
                    return false;
                }
                sr.Close();
                return true;
            }
            catch
            {
               
                return false;
            }


        }
        private byte[] ReadConfig4WriteCommand(byte[] readConfig)
        {
            byte[] results;
            byte[] temp = new byte[90];
            byte dataID = readConfig[7];
            int L = readConfig.Length;

            temp[2] = 0x41;
            temp[3] = 0x00;
            temp[4] = dataID;
            int ValidDataStart = 12;  //10-02-01-10-10-60-00-DataID-Reserve-Reserve

            switch (dataID)
            {
                case 0x0A: //Configuration 1
                    temp[5] = 0xFF;  //Data Valid bytes
                    temp[6] = 0xC0;
                    break;
                case 0x14: //Configuration 2
                    temp[5] = 0x2C;
                    temp[6] = 0xF8;
                    break;
                case 0xA0: //BoreSight
                    ValidDataStart = 14;  //Four reserves
                    temp[5] = 0xF0;
                    temp[6] = 0x00;
                    break;
                case 0x8C: //Lever Arm
                    temp[5] = 0x60;   //If GPS is include change to 0x70
                    temp[6] = 0x00;
                    break;
                default:
                    results = new byte[2];
                    return results;
            }
            int ValidDataLenght = L - 3 - ValidDataStart;  // ...-checksum-10-03
            int bytepointer = 7;
            System.Buffer.BlockCopy(readConfig, ValidDataStart, temp, bytepointer, ValidDataLenght);
            bytepointer += ValidDataLenght;
            //Get CheckSum
            temp[bytepointer++] = CalculateChecksum(temp);

            temp[0] = DLE;
            temp[1] = STX;
            temp[bytepointer++] = DLE;
            temp[bytepointer++] = ETX;
            results = new byte[bytepointer];
            System.Buffer.BlockCopy(temp, 0, results, 0, bytepointer);
            return results;
        }
        private bool writePresetFile()
        {
            try
            {
                StreamWriter sr = new StreamWriter("TalinConfig.cfg");
                sr.WriteLine("Comm Port = " + CommPort);
                sr.WriteLine("Vehicle Configuration 1 Setup = " + presetConfiguration1);
                sr.WriteLine("Vehicle Configuration 1 Command = " + BitConverter.ToString(Configuration1PresetCommand));
                sr.WriteLine("Bore Sight 1 Setup = " + presetBoreSight1);
                sr.WriteLine("Bore Sight 1 Command = " + BitConverter.ToString(BoreSightPresetCommand));
                sr.WriteLine("Lever Arm Setup = " + presetConfiguration1);
                sr.WriteLine("Lever Arm Command = " + LeverArmPresetCommand);
                  sr.Close();
                return true;
            }
            catch (Exception er)
            {
               
                return false;
            }
        }


        
        public bool Talin_Initial()
        {
            try
            {
               

                if (Connect_Talin(CommPort) != true)
                { return false; }
                
                return true;
            }
            catch (Exception er)
            {
                
                return false;
            }


        }
        #endregion

        #region InitialPosition
        public byte[] PositionUpdateCommand;
        public void Talin_InitialPosition()
        {
            try
            {

                int results = 0x00;
                if (PositionUpdateCommand == null)
                {
                    return; //0xFF;
                }
                if (PositionUpdateCommand[0] == 0x00)
                {
                   
                    return;// 0xFF;
                }
                //Send Out of Travel Control Command:

                byte[] OrginazedCommand;

                OrginazedCommand = SerialSendCommand(PositionUpdateCommand);
                SerialSend(OrginazedCommand);
                receiveGeneralCommand.WaitOne(new TimeSpan(0, 0, 15), true);


                OrginazedCommand = SerialSendCommand(TalinOutTravelLock);
                SerialSend(OrginazedCommand);
                receiveGeneralCommand.WaitOne(new TimeSpan(0, 0, 15), true);

                StartReadStatus(true);
                return;// results;
            }
            catch (Exception e)
            {
                return;// 0xFF;
            }
        }
       

        private byte[] GeneratePositionUpdateCommand(string PositionArray)
        {
            try
            {

                byte[] temp = new byte[65];
                string[] fields = PositionArray.Split(',');
                string valuetoremove2 = "";
                fields = fields.Where(val => val != valuetoremove2).ToArray();
                ZoneID = UInt16.Parse(fields[4].Trim());
                PositionFormat = UInt16.Parse(fields[3].Trim());
                byte[] Initial_FormatBytes = BitConverter.GetBytes(PositionFormat);
                byte[] Initial_ZoneIDBytes = BitConverter.GetBytes(ZoneID);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(Initial_FormatBytes);
                    Array.Reverse(Initial_ZoneIDBytes);
                }

                byte[] Initial_EastingBytes = Decimal_TalinDoubleWordsFloat(fields[5].Trim(), positionaccuracy);
                byte[] Initial_NorthingBytes = Decimal_TalinDoubleWordsFloat(fields[6].Trim(), positionaccuracy);
                byte[] Initial_ElevationBytes = Decimal_TalinDoubleWordsFloat(fields[7].Trim(), positionaccuracy);
                byte[] Initial_DatumIDBytes = new byte[6] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
                byte[] temp1 = Encoding.ASCII.GetBytes(fields[8].Trim());
                int L = temp1.Length;
                if (L > 6)
                    L = 6;
                System.Buffer.BlockCopy(temp1, 0, Initial_DatumIDBytes, 0, L);
                byte[] positionarray = new byte[24];

                System.Buffer.BlockCopy(Initial_FormatBytes, 0, positionarray, 0, 2);
                System.Buffer.BlockCopy(Initial_ZoneIDBytes, 0, positionarray, 2, 2);
                System.Buffer.BlockCopy(Initial_EastingBytes, 0, positionarray, 6, 4);
                System.Buffer.BlockCopy(Initial_NorthingBytes, 0, positionarray, 10, 4);
                System.Buffer.BlockCopy(Initial_ElevationBytes, 0, positionarray, 14, 4);
                System.Buffer.BlockCopy(Initial_DatumIDBytes, 0, positionarray, 18, 6);
                temp[0] = 0x41;   //

                int i = 0;
                string ModeType = fields[2].Trim();
                if (ModeType[0] == 'S')
                {
                    temp[1] = 0x00;
                    temp[2] = 0x28;
                    temp[3] = 0xB8;
                    temp[4] = 0x00;
                    System.Buffer.BlockCopy(positionarray, 0, temp, 5, 24);
                    i = 29;
                }
                else
                {
                    byte[] Initial_GravityBytes = Decimal_TalinDoubleWordsFloat(fields[9], -8);
                    initialMaterialDensity = double.Parse(fields[10]);
                    byte[] Initial_DensityBytes = BitConverter.GetBytes(Convert.ToUInt16(initialMaterialDensity * 10));
                    initialDeflection1 = double.Parse(fields[11]);
                    byte[] Initial_Deflection1Bytes = BitConverter.GetBytes(Convert.ToInt32(initialDeflection1 * 1000));
                    initialDeflection2 = double.Parse(fields[12]);
                    byte[] Initial_Deflection2Bytes = BitConverter.GetBytes(Convert.ToInt32(initialDeflection2 * 1000));
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(Initial_DensityBytes);
                        Array.Reverse(Initial_Deflection1Bytes);
                        Array.Reverse(Initial_Deflection2Bytes);
                    }
                    temp[1] = 0xF8;
                    temp[2] = 0x00;
                    temp[3] = 0x00;  // Configuration Code
                    temp[4] = 0x00;
                    temp[5] = 0x00;  //Gravity -Underground Mode
                    temp[6] = 0x01;
                    System.Buffer.BlockCopy(positionarray, 0, temp, 7, 24);
                    System.Buffer.BlockCopy(Initial_GravityBytes, 0, temp, 31, 4);
                    System.Buffer.BlockCopy(Initial_DensityBytes, 0, temp, 35, 2);
                    System.Buffer.BlockCopy(Initial_Deflection1Bytes, 0, temp, 37, 4);
                    System.Buffer.BlockCopy(Initial_Deflection2Bytes, 0, temp, 41, 4);
                    i = 45;
                }
                byte[] Command = new byte[i];
                System.Buffer.BlockCopy(temp, 0, Command, 0, i);
                // Change 0x10 to 0x10 0x10.
                byte[] results = CommandAddHex10(Command);
                // return command body
                return results;
            }
            catch (Exception er)
            {
                return null;
            }

        }
        #endregion

        #region Status
        public bool StartReadStatus(bool start)
        {
           
            //Read Status in 1Hz;
            try
             {
                GetTalinStatus = start;
                //Send Out of Travel Control Command:
                byte[] OrginazedCommand;
                byte[] Command = ReadStatus1Hz;
                if (!start)
                {
                    Command[1] = 0x00;
                    OrginazedCommand = SerialSendCommand(Command);
                    SerialSend(OrginazedCommand);
                    return true;
                }
                OrginazedCommand = SerialSendCommand(Command);
                SerialSend(OrginazedCommand);

                receiveStatus.WaitOne(new TimeSpan(0, 0, 15));

                
                return true;
             }
             catch (Exception er)
             {
                 return false;
             } 

            
        }
        public static int AlignmentTime = 600;
        private int StatusDisplay(TalinStatus TS)
        {
            int results = 0x00;
            if (!TS.AbleCompleteAlign)
            {
                results = 0xFF;
            }
            else
            {
                results = results | 0x02;
              
                if (TS.AlignmentTimeLeft != 0)
                {
                  
                }
                else
                {
                }
                AlignmentTime = TS.AlignmentTimeLeft;
               
                if (TS.OutofTravelLock)
                {
                  
                    results = results | 0x04;
                }
                else
                {
                    results = results & 0xFB;
                }
              
                if (TS.NavigationMode)
                {
                    results = results | 0x08;
                }
                else
                {
                    
                    results = results & 0xF7;
                }
                if (TS.InitialPositionReceived)
                {
                    results = results | 0x10;
                }
                else
                {
                    results = results & 0xEF;
                }
            }
            return results;
        }

        #endregion

        #region Navigation
        public void startNavigation()
        {
            Write2File = true;

            startPositioning(TalinMode);
            startOrientation();
            ////Console.WriteLine
        }
        public void stopNavigation()
        {
            Write2File = false;

            byte[] OrginazedCommand;

            OrginazedCommand = SerialSendCommand(PositionStopAll);
            ////Console.WriteLine(BitConverter.ToString(OrginazedCommand));
            SerialSend(OrginazedCommand);

            OrginazedCommand = SerialSendCommand(OrientationStopAll);
            ////Console.WriteLine(BitConverter.ToString(OrginazedCommand));
            SerialSend(OrginazedCommand);

            OrginazedCommand = SerialSendCommand(ReadStatusStop);
            SerialSend(OrginazedCommand);

        }
        private bool startPositioning(int Mode)
        {
            try
            {
                if (Mode == 0) //Surface
                {
                    byte[] OrginazedCommand;

                    OrginazedCommand = SerialSendCommand(PositionRequestAll);
                    // //Console.WriteLine(BitConverter.ToString(OrginazedCommand));
                    SerialSend(OrginazedCommand);
                    return true;
                }
                return true;
            }
            catch (Exception er)
            {
                return false;
            }
        }
        private bool startOrientation()
        {
            try
            {
                byte[] OrginazedCommand;

                OrginazedCommand = SerialSendCommand(OrientationRequestAll);
                ////Console.WriteLine(BitConverter.ToString(OrginazedCommand));
                SerialSend(OrginazedCommand);
                return true;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        #endregion
        
        #region SerialPort

        private byte[] CommandAddHex10(byte[] Command)
        {
            byte[] temp = new byte[Command.Length * 2];
            int bytepointer = 0;
            for (int i = 0; i < Command.Length; i++)
            {
                temp[bytepointer] = Command[i];
                bytepointer++;
                if (Command[i] == 0x10)
                {
                    temp[bytepointer] = 0x10;
                    bytepointer++;
                }
            }
            byte[] results = new byte[bytepointer];
            System.Buffer.BlockCopy(temp, 0, results, 0, bytepointer);
            return results;
        }
        /// <summary>
        /// Orginaze comands to Talin through Serial port
        /// </summary>
        /// <param name="Command">Orginazed commands exclude the prefix, sumcheck and surfix</param>
        /// <returns></returns>
        private byte[] SerialSendCommand(byte[] Command)
        {
            byte[] _command;
            _command = Command;
            byte[] results = new byte[_command.Length + 5];

            System.Buffer.BlockCopy(Prefix, 0, results, 0, Prefix.Length);   //Add the 0x10 0x02
            System.Buffer.BlockCopy(_command, 0, results, Prefix.Length, _command.Length);  //Add Command Body

            results[Prefix.Length + _command.Length] = CalculateChecksum(_command);  //Add Check Sum

            System.Buffer.BlockCopy(Surfix, 0, results, Prefix.Length + _command.Length + 1, Surfix.Length); //Add 0x10 0x03 for end 

            return results;
        }

        private static byte CalculateChecksum(byte[] data)
        {
            byte checksum = 0;
            int result = 0;
            foreach (byte value in data)
            {
                result += value;
            }
            checksum = (byte)(-1 * (result));
            return checksum;
        }

        static int bufferSize = 400;
        static byte[] _buffer = new byte[bufferSize];
        static int offset = 0;
        SerialPort sp;
        int baudrate = 19200;
        string ComNo = "";

        public bool Connect_Talin(string ComNum)
        {
            ComNo = ComNum;
            if (sp != null)
                sp.Close();

            sp = new SerialPort(ComNo, baudrate, Parity.None, 8, StopBits.One);
            try
            {
                sp.Open();
            }
            catch (Exception err)
            {
                isConnected = false;
                return false;
            }
            sp.ReceivedBytesThreshold = 1;
            sp.DataReceived += Sp_DataReceived;
            sp.ErrorReceived += Sp_ErrorReceived;
            sp.ReadTimeout = 200;
            sp.WriteTimeout = 200;
            isConnected = true;
            return true;
        }

        public void Connect_Talin()
        {
            ComNo = CommPort ;
            if (sp != null)
                sp.Close();

            sp = new SerialPort(ComNo, baudrate, Parity.None, 8, StopBits.One);
            try
            {
                sp.Open();
            }
            catch (Exception err)
            {
                isConnected = false; 
            }
            sp.ReceivedBytesThreshold = 1;
            sp.DataReceived += Sp_DataReceived;
            sp.ErrorReceived += Sp_ErrorReceived;
            sp.ReadTimeout = 200;
            sp.WriteTimeout = 200;

            isConnected = true;
        }
        public void Disconnect_Talin()
        {
            if (sp != null) {

                sp.Close();
                isConnected = false;
            }
                
        }
        public void ShutdownTalin()
        {
            byte[] OrginazedCommand = SerialSendCommand(TalinShutDown);
            SerialSend(OrginazedCommand);
            receiveGeneralCommand.WaitOne(new TimeSpan(0, 0, 15), true);
          
        }
        /// <summary>
        /// Send comand to Talin through Serial port
        /// </summary>
        /// <param name="Command">Orginazed commands exclude the prefix, sumcheck and surfix</param>
        /// <returns></returns>
        private bool SerialSend(byte[] Command)
        {
            try
            {
                sp.Write(Command, offset, Command.Length);
                return true;
            }
            catch (TimeoutException err)
            {
                
                return false;
            }
            catch (NullReferenceException er)
            {
                
                return false;
            }
            catch (Exception e)
            {
               
                return false;
            }
        }

        void Sp_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            SerialPort _sp = (SerialPort)sender;
            _sp.DiscardInBuffer();
        }

        static int ReadingArraySize = bufferSize * 2;
        byte[] ReadingArrayBuilder = new byte[ReadingArraySize];

        int ReadArrayBuilderOffset = 0;
        bool ReadArrayBuilderEnd = false;
        void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            try
            {
                int sByteRead = sp.BytesToRead;
                if (sByteRead == 0)   //nothing to read return;
                    return;
                offset = 0;
                if (sByteRead > bufferSize)
                    sByteRead = bufferSize;

                sp.Read(_buffer, offset, sByteRead); //Read to buffer

                //Sometimes, port only read one port, it shall be added into the first byte of the next reading
                if ((sByteRead == 1) && (singlebyteexist == false))
                {
                    singlebyte = _buffer[0];
                    singlebyteexist = true;
                    return;
                }

                byte[] results;
                if (singlebyteexist == false)
                {

                    results = new byte[sByteRead];
                    Array.Copy(_buffer, results, sByteRead);
                    //Reinitial the buffer
                }
                else
                {
                    results = new byte[sByteRead + 1];
                    Array.Copy(_buffer, 0, results, 1, sByteRead);
                    results[0] = singlebyte;
                    singlebyte = 0x00;
                    singlebyteexist = false;
                }
                _buffer = new byte[bufferSize];  //Reset buffer
                //ReadArrayBuilderEnd = false;
                //Make sure we obtain 0x10, 0x03 as the end.

                if ((results[0] == 0x10) && (results[1] == 0x02) && (results[results.Length - 2] == 0x10) && (results[results.Length - 1] == 0x03))
                {
                    ReadArrayBuilderEnd = true;
                    System.Buffer.BlockCopy(results, 0, ReadingArrayBuilder, ReadArrayBuilderOffset, results.Length);
                    //load command to process
                    ReadArrayBuilderOffset += results.Length;
                }
                else if ((results[0] == 0x10) && (results[1] == 0x02))
                {  //Array starts
                    ReadingArrayBuilder = new byte[ReadingArraySize];
                    ReadArrayBuilderOffset = 0;
                    ReadArrayBuilderEnd = false;
                    System.Buffer.BlockCopy(results, 0, ReadingArrayBuilder, ReadArrayBuilderOffset, results.Length);
                    ReadArrayBuilderOffset = results.Length;
                    return;
                }
                else if ((results[results.Length - 2] == 0x10) && (results[results.Length - 1] == 0x03))
                { // Array end started
                    ReadArrayBuilderEnd = true;
                    System.Buffer.BlockCopy(results, 0, ReadingArrayBuilder, ReadArrayBuilderOffset, results.Length);
                    ReadArrayBuilderOffset += results.Length;
                    //load command to process
                }
                else if ((ReadArrayBuilderOffset > 120) | (results.Length > 120))
                {
                    bool NotFindEnd = true;
                    System.Buffer.BlockCopy(results, 0, ReadingArrayBuilder, ReadArrayBuilderOffset, results.Length);
                    ReadArrayBuilderOffset += results.Length;
                    bool getRidRemainData = false;
                    if (results.Length == bufferSize)
                        getRidRemainData = true;  //in this case, since data lost we cannot properly restart build another data array
                    int i = ReadArrayBuilderOffset;
                    while ((i > 1) && (NotFindEnd))
                    {
                        if ((ReadingArrayBuilder[i - 2] == 0x10) && (ReadingArrayBuilder[i - 1] == 0x03) && (ReadingArrayBuilder[i] == 0x10) && (ReadingArrayBuilder[i + 1] == 0x02))
                        {
                            NotFindEnd = false; // Find the end;
                            byte[] finalResults1 = new byte[i];
                            System.Buffer.BlockCopy(ReadingArrayBuilder, 0, finalResults1, 0, i);
                            int remainLength = ReadArrayBuilderOffset - i;
                            byte[] remainResults = new byte[remainLength];
                            System.Buffer.BlockCopy(ReadingArrayBuilder, i, remainResults, 0, remainLength);
                            DataProcessDelegate _dataProcessDelegate1 = new DataProcessDelegate(dataprocess4save);
                            _dataProcessDelegate1(finalResults1);
                            //Start new Data Array
                            ReadArrayBuilderEnd = true;
                            if (!getRidRemainData)  //if get rid of data.
                            {
                                ReadingArrayBuilder = new byte[ReadingArraySize];
                                ReadArrayBuilderOffset = 0;
                                ReadArrayBuilderEnd = false;
                                System.Buffer.BlockCopy(remainResults, 0, ReadingArrayBuilder, 0, remainLength);
                                ReadArrayBuilderOffset = remainLength;
                                System.Buffer.BlockCopy(results, 0, ReadingArrayBuilder, ReadArrayBuilderOffset, results.Length);
                                ReadArrayBuilderOffset = results.Length;
                            }

                            return;
                        }
                        i--;
                    }
                }
                else if (ReadArrayBuilderEnd == false)
                {
                    System.Buffer.BlockCopy(results, 0, ReadingArrayBuilder, ReadArrayBuilderOffset, results.Length);
                    ReadArrayBuilderOffset += results.Length;
                    return;
                }
                else
                {
                    return;
                    //throw new Exception("No reading Valid Data");
                }

                byte[] finalResults = new byte[ReadArrayBuilderOffset];
                System.Buffer.BlockCopy(ReadingArrayBuilder, 0, finalResults, 0, ReadArrayBuilderOffset);


                DataProcessDelegate _dataProcessDelegate = new DataProcessDelegate(dataprocess4save);
                _dataProcessDelegate(finalResults);
            }
            catch (Exception err)
            {
                sp.DiscardInBuffer();
                return;
            }

        }

        /// <summary>
        /// Handling the read data
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        private void DataProcess4Display(byte[] _data)
        {
            //int results;
            int Length = _data.Length;
            //StringBuilder sb = new StringBuilder();
            int dataStartlocation = 0;
            int dataEndlocation = 64;  //32 word for all received message
            int dataOffset = 0;
            int dataGroup = _data[2];
            StringBuilder sb = new StringBuilder();
            //
            if ((_data[0] != 0x10) || (_data[1] != 0x02) || (_data[Length - 2] != 0x10) || (_data[Length - 1] != 0x03)) //(DataReading Error)
            {
                sb.Append("0\n Invalid Data.\n Data Obtained:");
                sb.Append(BitConverter.ToString(_data));
                return;
            }
            if ((_data[3] != 0x00) || (_data[4] != 0x00))  //Protocol Error
            {
                sb.Append("0\n Protocol error.\n Data Obtained:");
                sb.Append(BitConverter.ToString(_data));
                return;
            }
            else
            {
                if ((dataGroup == 0x41) && (Length < 10)) //The update command is proper Received
                {
                    receiveGeneralCommand.Set();
                    sb.Append("41\n Request Command is send.\n Data Obtained:");
                    sb.Append(BitConverter.ToString(_data));
                    return;
                }
            }
            if (Length < 12)  //No Data Include
            {
                sb.Append("-1\n No valid Data include.\n Data Obtained:");
                sb.Append(BitConverter.ToString(_data));
                return;
            }
            if ((_data[5] == 0x10) && (_data[6] == 0x10))
            {
                dataEndlocation = (_data[7] & 0x3F);
                dataStartlocation = ((_data[6] & 0x0F) << 2) + (_data[7] >> 6);
                dataOffset = 8;
                sb.Append(dataGroup.ToString() + "\n");
            }
            else
            {
                dataEndlocation = (_data[6] & 0x3F);
                dataStartlocation = ((_data[5] & 0x0F) << 2) + (_data[6] >> 6);
                dataOffset = 7;
                sb.Append(dataGroup.ToString() + "\n");
            }
            //Length = (dataEndlocation - dataStartlocation + 1) * 2 + dataOffset+3;
            byte[] Temp = new byte[Length - 5];
            if ((_data[Length - 3] == 0x10) && (_data[Length - 4] == 0x10))
                System.Buffer.BlockCopy(_data, 2, Temp, 0, Length - 6);
            else
                System.Buffer.BlockCopy(_data, 2, Temp, 0, Length - 5);

            byte CheckSumValue = CalculateChecksum(Temp);
            
            if ((CheckSumValue != _data[Length - 3])) // && (CheckSumValue != _data[(dataEndlocation - dataStartlocation + 1) * 2 + dataOffset-1]))  //status reading crazy
            {
                sb.Append("CheckSum Value is not match. Received Data may be broken.\n Data Obtained:");
                sb.Append(BitConverter.ToString(_data));

                if ((dataGroup != 0x04) && (dataGroup != 1))
                {
                    return;    //Since there are repeat data obtained from Talin for all readings. This cause the checksum error So ignored this error for status and Vehicle Configuration Setup now.
                }
            }
           
            byte[] RegulatedData;
            RegulatedData = ReceiveDataRemoveHex10(_data, dataOffset);   //Change the "0x10, 0x10" to 0x10
            Length = RegulatedData.Length;

            byte[] FullReadFrame = new byte[64];   //Only save the databody for position, status and orientation
            if (((dataEndlocation - dataStartlocation + 1) * 2) <= (Length - dataOffset - 3))
                System.Buffer.BlockCopy(RegulatedData, dataOffset, FullReadFrame, (dataStartlocation - 1) * 2, (dataEndlocation - dataStartlocation + 1) * 2);
            else  //Data Length is different from requested (something wrong);
            {
                sb.Append("The obtained Data length is not match the requested data length.\n Data Obtained:");
                sb.Append(BitConverter.ToString(_data));

                return;
            }

           
            DateTime _now = DateTime.Now;
            string NowString = _now.ToString("yyyyMMddHHmmssffff");
            String dataValue = NowString + "-" + BitConverter.ToString(Temp);
            switch (dataGroup)
            {
                case 3:

                    byte[] altitudebyte = { FullReadFrame[24], FullReadFrame[25], FullReadFrame[26], FullReadFrame[27] };
                    double altitude = TalinDoubleWordsFloat_Double(altitudebyte, positionaccuracy);

                    AddUpdateFileAndCollection(dataValue);
                 

                    break;
                case 2:
                   
                    AddUpdateFileAndCollection(dataValue);
                  
                    break;
                case 4:
                    if (Write2File == true) {  //Not Optimal
                        AddUpdateFileAndCollection(dataValue);
                    }
                    else
                    {

                        byte mode1 = FullReadFrame[0];
                        byte mode2 = FullReadFrame[1];
                        byte status11 = FullReadFrame[10];
                        byte status12 = FullReadFrame[11];
                        byte status21 = FullReadFrame[12];
                        byte status22 = FullReadFrame[13];
                        byte Alert11 = FullReadFrame[18];
                        byte Alert12 = FullReadFrame[19];
                        byte Alert21 = FullReadFrame[20];
                        byte Alert22 = FullReadFrame[21];
                        byte Alert32 = FullReadFrame[23];
                        TS = new TalinStatus(mode2, Alert11, status12, FullReadFrame[26], FullReadFrame[27]);
                        StatusDisplay(TS);


                        if (GetTalinStatus) {

                            INUStatus SResults = InterpretStatusData(FullReadFrame);
                            StringBuilder Str = INUStatus4Display(SResults);
                            TalinLogCollection.Add(Str.ToString());
                            GetTalinStatus = false;
                        }


                       
                       
                        receiveStatus.Set();
                    }
                    break;
                case 1:
                    if (FullReadFrame[1] == 10)  //Vehicle Configuration 1
                    {
                        readConfiguration1 = BitConverter.ToString(_data);
                       
                        receiveConfiguration1.Set();
                    }
                    if (FullReadFrame[1] == 20)  //Vehicle Configuration 2
                    {
                        readConfiguration2 = BitConverter.ToString(_data);
                        receiveConfiguration2.Set();
                    }
                    if (FullReadFrame[1] == 161)  //Bore Sight
                    {
                        readBoreSight1 = BitConverter.ToString(_data);
                      
                        receiveBoreSight1.Set();
                    }
                    if (FullReadFrame[1] == 140)  //Lever Arm
                    {
                        readLeverArm = BitConverter.ToString(_data);
                   
                        receiveLeverArm.Set();
                    }
                    if (FullReadFrame[1] == 151)  //Rotation Point
                    {

                    }
                    if (FullReadFrame[1] == 230)  //Gravity Reference
                    {
                        readGravityReference = BitConverter.ToString(_data);
                        receiveGravityReference.Set();
                    }
                    if (FullReadFrame[1] == 60)  //Gravity Modeposition
                    {

                    }
                    break;
                default:
                    break;
            }
        }

        private void AddUpdateFileAndCollection(String dataValue) {
            if (Write2File == true)
            {
                sw.WriteLine(dataValue);
                dataSizeCollection.Add(dataValue);
            }
          
        }
        private void dataprocess4save(byte[] _data)
        {
            int[] ReadingGroup_Index = new int[50];
            int j = 1;
            ReadingGroup_Index[0] = 0;
            for (int i = 0; i < _data.Length - 4; i++)
            {
                if ((_data[i] == 0x10) && (_data[i + 1] == 0x03) && (_data[i + 2] == 0x10) && (_data[i + 3] == 0x02))
                {
                    ReadingGroup_Index[j] = i + 2;
                    j++;
                }
            }

            ReadingGroup_Index[j] = _data.Length;
            // One reading may content different data, the following code is seperating the data
            for (int k = 0; k < j; k++)
            {
                int SingleLineDataLength = ReadingGroup_Index[k + 1] - ReadingGroup_Index[k];
                byte[] data = new byte[SingleLineDataLength];

                System.Buffer.BlockCopy(_data, ReadingGroup_Index[k], data, 0, SingleLineDataLength);
                DataProcess4Display(data);
            }

        }

        #endregion

        #region GETTER
         
        public String TalinPort {

            get => CommPort;
            set => CommPort = value;
        
        }


        public bool IsConnected {
            get => isConnected;
        }

        public ObservableCollection<String> TalinLogCollection {

            get => logDataCollection;
        }

        public ObservableCollection<String> DataSizeCollection
        {

            get => dataSizeCollection;
        }

        public bool GetTalinStatus {

            get => GetStatus;
            set => GetStatus = value;
        }
        #endregion

      


    }
}