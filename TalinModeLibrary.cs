using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Penguin__REMS_Project
{
    public class TalinModeLibrary
    {   //Following are a library for Talin required methods, variables
        private const int positionaccuracy = -7;

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

        #endregion

        public byte[] generatePositionUpdateCommand(string[] PositionArray, UInt16 VMS)
        {
            try
            {
                byte[] temp = new byte[65];

                UInt16 ZoneID = UInt16.Parse(PositionArray[5].Trim());
                string PFormat = PositionArray[4].Trim();
                UInt16 PositionFormat;
                if ((PFormat[0] == 'N') || (PFormat[0] == 'n'))
                {
                    PositionFormat = VMS;
                }
                else
                    PositionFormat = (UInt16)(0x8000 + VMS); //32770

                byte[] Initial_FormatBytes = BitConverter.GetBytes(PositionFormat);
                byte[] Initial_ZoneIDBytes = BitConverter.GetBytes(ZoneID);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(Initial_FormatBytes);
                    Array.Reverse(Initial_ZoneIDBytes);
                }

                byte[] Initial_EastingBytes = Decimal_TalinDoubleWordsFloat(PositionArray[6].Trim(), positionaccuracy);
                byte[] Initial_NorthingBytes = Decimal_TalinDoubleWordsFloat(PositionArray[7].Trim(), positionaccuracy);
                byte[] Initial_ElevationBytes = Decimal_TalinDoubleWordsFloat(PositionArray[8].Trim(), positionaccuracy);
                byte[] Initial_DatumIDBytes = new byte[6] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
                byte[] temp1 = Encoding.ASCII.GetBytes(PositionArray[9].Trim());
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
                string ModeType = PositionArray[3].Trim();
                if ((ModeType[0] == 'S') || (ModeType[0] == 's'))
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
                    byte[] Initial_GravityBytes = Decimal_TalinDoubleWordsFloat(PositionArray[10], -8);
                    double initialMaterialDensity = double.Parse(PositionArray[11]);
                    byte[] Initial_DensityBytes = BitConverter.GetBytes(Convert.ToUInt16(initialMaterialDensity * 10));
                    double initialDeflection1 = double.Parse(PositionArray[12]);
                    byte[] Initial_Deflection1Bytes = BitConverter.GetBytes(Convert.ToInt32(initialDeflection1 * 1000));
                    double initialDeflection2 = double.Parse(PositionArray[13]);
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
    }
}
