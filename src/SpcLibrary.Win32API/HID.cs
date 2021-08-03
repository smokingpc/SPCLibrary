using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SpcLibrary.Win32API
{
    [StructLayout(LayoutKind.Sequential)]
    public class HIDD_ATTRIBUTES
    {
        public int Size;
        public ushort VendorID;
        public ushort ProductID;
        public short VersionNumber;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class HIDP_CAPS
    {
        public short Usage;
        public short UsagePage;
        public short InputReportByteLength;
        public short OutputReportByteLength;
        public short FeatureReportByteLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public short[] Reserved;
        public short NumberLinkCollectionNodes;
        public short NumberInputButtonCaps;
        public short NumberInputValueCaps;
        public short NumberInputDataIndices;
        public short NumberOutputButtonCaps;
        public short NumberOutputValueCaps;
        public short NumberOutputDataIndices;
        public short NumberFeatureButtonCaps;
        public short NumberFeatureValueCaps;
        public short NumberFeatureDataIndices;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_VALUE_CAPS
    {
        public short UsagePage;
        public byte ReportID;
        public int IsAlias;
        public short BitField;
        public short LinkCollection;
        public short LinkUsage;
        public short LinkUsagePage;
        public int IsRange;
        public int IsStringRange;
        public int IsDesignatorRange;
        public int IsAbsolute;
        public int HasNull;
        public byte Reserved;
        public short BitSize;
        public short ReportCount;
        public short Reserved2;
        public short Reserved3;
        public short Reserved4;
        public short Reserved5;
        public short Reserved6;
        public int LogicalMin;
        public int LogicalMax;
        public int PhysicalMin;
        public int PhysicalMax;
        public short UsageMin;
        public short UsageMax;
        public short StringMin;
        public short StringMax;
        public short DesignatorMin;
        public short DesignatorMax;
        public short DataIndexMin;
        public short DataIndexMax;
    }

    public static class HID
    {
        public const short HIDP_INPUT = 0;
        public const short HIDP_OUTPUT = 1;
        public const short HIDP_FEATURE = 2;

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_FlushQueue(IntPtr hidDeviceObject);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_GetAttributes(IntPtr hidDeviceObject, HIDD_ATTRIBUTES attributes);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_GetFeature(IntPtr hidDeviceObject, [MarshalAs(UnmanagedType.LPArray)] byte[] lpReportBuffer, int reportBufferLength);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_GetInputReport(IntPtr hidDeviceObject, [MarshalAs(UnmanagedType.LPArray)] byte[] lpReportBuffer, int reportBufferLength);

        [DllImport(Win32DLL.Hid)]
        static public extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_GetNumInputBuffers(IntPtr hidDeviceObject, ref int numberBuffers);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_GetPreparsedData(IntPtr hidDeviceObject, ref IntPtr preparsedData);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_FreePreparsedData(IntPtr preparsedData);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_SetFeature(IntPtr hidDeviceObject, [MarshalAs(UnmanagedType.LPArray)] byte[] lpReportBuffer, int reportBufferLength);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_SetNumInputBuffers(IntPtr hidDeviceObject, int numberBuffers);

        [DllImport(Win32DLL.Hid)]
        static public extern bool HidD_SetOutputReport(IntPtr hidDeviceObject, [MarshalAs(UnmanagedType.LPArray)] byte[] lpReportBuffer, int reportBufferLength);

        [DllImport(Win32DLL.Hid)]
        static public extern int HidP_GetCaps(IntPtr preparsedData, HIDP_CAPS capabilities);

        [DllImport(Win32DLL.Hid)]
        static public extern int HidP_GetValueCaps(short reportType, ref byte valueCaps, ref short valueCapsLength, IntPtr preparsedData);

        [DllImport(Win32DLL.Hid, CharSet = CharSet.Auto)]
        public static extern bool HidD_GetProductString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);

        [DllImport(Win32DLL.Hid, CharSet = CharSet.Auto)]
        public static extern bool HidD_GetManufacturerString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);

        [DllImport(Win32DLL.Hid, CharSet = CharSet.Auto)]
        public static extern bool HidD_GetSerialNumberString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int reportBufferLength);
    }
}
