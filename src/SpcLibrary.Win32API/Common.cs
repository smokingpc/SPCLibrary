using System;
using System.Runtime.InteropServices;

namespace SpcLibrary.Win32API
{
    public static class Win32DLL
    {
        public const string Kernel32 = "kernel32.dll";
        public const string User32 = "user32.dll";
        public const string Hid = "hid.dll";
        public const string SetupApi = "setupapi.dll";
        public const string WinMM = "winmm.dll";
    }

    public static class CONST
    {
        public const uint ACCESS_NONE = 0;
        //public const int INVALID_HANDLE_VALUE = -1;
        public static readonly IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

        public const int WAIT_TIMEOUT = 0x102;
        public const uint WAIT_OBJECT_0 = 0;
        public const uint WAIT_FAILED = 0xffffffff;

        public const int WAIT_INFINITE = -1;
        public const int MAX_RESOLUTION = 1;     //best resolution == 1 ms

        public const int MAX_PATH = 260;
    }


    #region ======== Callback Delegates for Win32API ========
    public delegate void DelegateIOCompletion(uint errorCode, uint numBytes, OVERLAPPED pOVERLAP);
    #endregion

    #region ======== Structures for Win32API ========
    [StructLayout(LayoutKind.Sequential)]
    public class SYSTEMTIME
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Milliseconds;

        public SYSTEMTIME(DateTime dt)
        {
            Year = (ushort)dt.Year;
            Month = (ushort)dt.Month;
            DayOfWeek = (ushort)dt.DayOfWeek;
            Day = (ushort)dt.Day;
            Hour = (ushort)dt.Hour;
            Minute = (ushort)dt.Minute;
            Second = (ushort)dt.Second;
            Milliseconds = (ushort)dt.Millisecond;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public class OVERLAPPED
    {
        public int Internal;
        public int InternalHigh;
        public int Offset;
        public int OffsetHigh;
        public int hEvent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SECURITY_ATTRIBUTES
    {
        public int Length;
        public IntPtr SecurityDescriptor;
        public bool InheritHandle;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GUID
    {
        int Data1 = 0;
        Int16 Data2 = 0;
        short Data3 = 0;
        UInt64 Data4 = 0;

        public static implicit operator System.Guid(GUID value)
        {
            System.Guid ret = new Guid(value.Data1, value.Data2, value.Data3, BitConverter.GetBytes(value.Data4));
            return ret;
        }

        public static implicit operator GUID(System.Guid value)
        {
            GUID ret = new GUID(value.ToByteArray());
            return ret;
        }

        public GUID() { }
        public GUID(System.Guid data) : this(data.ToByteArray()){ }
        public GUID(byte[] data) :this()
        { Parse(data); }

        public void Parse(byte[] buffer)
        {
            int index = 0;
            Data1 = (Int32)BitConverter.ToInt32(buffer, index);
            index += sizeof(UInt32);

            Data2 = BitConverter.ToInt16(buffer, index);
            index += sizeof(UInt16);

            Data3 = (Int16)BitConverter.ToInt16(buffer, index);
            index += sizeof(UInt16);

            Data4 = (UInt64)BitConverter.ToUInt64(buffer, index);
        }

        public byte[] ToBytes()
        {
            return ((System.Guid)this).ToByteArray();
        }
    }
    #endregion

    #region ======== Enumerations for Win32API ========
    public enum ACCESS_TYPE : uint
    {
        DELETE = 0x00010000,
        READ_CONTROL = 0x00020000,
        WRITE_DAC = 0x00040000,
        WRITE_OWNER = 0x00080000,
        SYNCHRONIZE = 0x00100000,
        STANDARD_RIGHTS_REQUIRED = 0x000F0000,
        STANDARD_RIGHTS_READ = READ_CONTROL,
        STANDARD_RIGHTS_WRITE = READ_CONTROL,
        STANDARD_RIGHTS_EXECUTE = READ_CONTROL,
        STANDARD_RIGHTS_ALL = 0x001F0000,

        GENERIC_READ = 0x80000000,
        GENERIC_WRITE = 0x40000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_ALL = 0x10000000,
    }

    public enum ACCESS_RIGHTS : uint
    {
        FILE_READ_DATA = 0x0001,
        FILE_LIST_DIRECTORY = 0x0001,
        FILE_WRITE_DATA = 0x0002,
        FILE_ADD_FILE = 0x0002,
        FILE_APPEND_DATA = 0x0004,
        FILE_ADD_SUBDIRECTORY = 0x0004,
        FILE_CREATE_PIPE_INSTANCE = 0x0004,
        FILE_READ_EA = 0x0008,
        FILE_WRITE_EA = 0x0010,
        FILE_EXECUTE = 0x0020,
        FILE_TRAVERSE = 0x0020,
        FILE_DELETE_CHILD = 0x0040,
        FILE_READ_ATTRIBUTES = 0x0080,
        FILE_WRITE_ATTRIBUTES = 0x0100,

        FILE_ALL_ACCESS = ACCESS_TYPE.STANDARD_RIGHTS_REQUIRED | ACCESS_TYPE.SYNCHRONIZE | 0x1FF,
        FILE_GENERIC_READ = ACCESS_TYPE.STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | ACCESS_TYPE.SYNCHRONIZE,
        FILE_GENERIC_WRITE = ACCESS_TYPE.STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | ACCESS_TYPE.SYNCHRONIZE,
        FILE_GENERIC_EXECUTE = ACCESS_TYPE.STANDARD_RIGHTS_EXECUTE | FILE_READ_ATTRIBUTES | FILE_EXECUTE | ACCESS_TYPE.SYNCHRONIZE,
    }

    public enum FILE_SHARE_MODE : uint
    {
        SHARE_READ = 0x00000001,
        SHARE_WRITE = 0x00000002,
        SHARE_DELETE = 0x00000004,
    }

    public enum FILE_DISPOSITION : uint
    {
        CREATE_NEW = 1,
        CREATE_ALWAYS = 2,
        OPEN_EXISTING = 3,
        OPEN_ALWAYS = 4,
        TRUNCATE_EXISTING = 5,
    }

    //把 Win32的 File Attribute 與 File Flag 合併定義
    public enum FILE_ATTR_AND_FLAG : uint
    {
        READONLY = 0x00000001,
        HIDDEN = 0x00000002,
        SYSTEM = 0x00000004,
        DIRECTORY = 0x00000010,
        ARCHIVE = 0x00000020,
        DEVICE = 0x00000040,
        NORMAL = 0x00000080,
        TEMPORARY = 0x00000100,
        SPARSE_FILE = 0x00000200,
        REPARSE_POINT = 0x00000400,
        COMPRESSED = 0x00000800,
        OFFLINE = 0x00001000,
        NOT_CONTENT_INDEXED = 0x00002000,
        ENCRYPTED = 0x00004000,
        VIRTUAL = 0x00010000,

        FLAG_WRITE_THROUGH = 0x80000000,
        FLAG_OVERLAPPED = 0x40000000,
        FLAG_NO_BUFFERING = 0x20000000,
        FLAG_RANDOM_ACCESS = 0x10000000,
        FLAG_SEQUENTIAL_SCAN = 0x08000000,
        FLAG_DELETE_ON_CLOSE = 0x04000000,
        FLAG_BACKUP_SEMANTICS = 0x02000000,
        FLAG_POSIX_SEMANTICS = 0x01000000,
        FLAG_OPEN_REPARSE_POINT = 0x00200000,
        FLAG_OPEN_NO_RECALL = 0x00100000,
        FLAG_FIRST_PIPE_INSTANCE = 0x00080000,
    }

    public enum REG_TYPE
    {
        REG_NONE = 0,                       // No value type
        REG_SZ = 1,                         // Unicode nul terminated string
        REG_EXPAND_SZ = 2,                  // Unicode nul terminated string, with environment variable references
        REG_BINARY = 3,                     // Free form binary
        REG_DWORD = 4,                      // 32-bit number
        REG_DWORD_LITTLE_ENDIAN = 4,        // 32-bit number (same as REG_DWORD)
        REG_DWORD_BIG_ENDIAN = 5,           // 32-bit number
        REG_LINK = 6,                       // Symbolic Link (unicode)
        REG_MULTI_SZ = 7,                   // Multiple Unicode strings
        REG_RESOURCE_LIST = 8,              // Resource list in the resource map
        REG_FULL_RESOURCE_DESCRIPTOR = 9,   // Resource list in the hardware description
        REG_RESOURCE_REQUIREMENTS_LIST = 10,
        REG_QWORD = 11,                     // 64-bit number
        REG_QWORD_LITTLE_ENDIAN = 11,       // 64-bit number (same as REG_QWORD)
    }

    public enum PIPE_OPEN_MODE : uint
    {
        PIPE_ACCESS_INBOUND = 0x00000001,
        PIPE_ACCESS_OUTBOUND = 0x00000002,
        PIPE_ACCESS_DUPLEX = 0x00000003,
    }

    public enum PIPE_MODE : uint
    {
        PIPE_WAIT = 0x00000000,
        PIPE_NOWAIT = 0x00000001,
        PIPE_READMODE_BYTE = 0x00000000,
        PIPE_READMODE_MESSAGE = 0x00000002,
        PIPE_TYPE_BYTE = 0x00000000,
        PIPE_TYPE_MESSAGE = 0x00000004,
        PIPE_ACCEPT_REMOTE_CLIENTS = 0x00000000,
        PIPE_REJECT_REMOTE_CLIENTS = 0x00000008,
    }

    public enum PIPE_END_FLAG : uint
    {
        PIPE_CLIENT_END = 0x00000000,
        PIPE_SERVER_END = 0x00000001,
    }



    #endregion
}
