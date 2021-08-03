using System;
using System.Runtime.InteropServices;

namespace SpcLibrary.Win32API
{
    public static class Kernel32
    {
        #region ======== Misc ========
        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern uint GetLastError();

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool CloseHandle(IntPtr hObject);
        #endregion

        #region ======== I/O ========
        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool CancelSynchronousIo(IntPtr hObject);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool GetOverlappedResult(IntPtr hFile, ref System.Threading.NativeOverlapped lpOverlapped, ref uint lpNumberOfBytesTransferred, bool bWait);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool CancelIo(IntPtr hFile);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool CancelIoEx(IntPtr hFile, ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport(Win32DLL.Kernel32, SetLastError = true, EntryPoint = "CancelIoEx")]
        static public extern bool CancelIoEx2(IntPtr hFile, IntPtr lpOverlapped);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpInBuffer,
                                            uint nInBufferSize,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpOutBuffer,
                                            uint nOutBufferSize,
                                            ref uint lpBytesReturned,
                                            IntPtr lpOverlapped);

        #endregion

        #region ======== Pipe ========
        static public bool CreatePipe(out CAutoHandle read_pipe, out CAutoHandle write_pipe, SECURITY_ATTRIBUTES attribute, uint size)
        {
            IntPtr read = IntPtr.Zero;
            IntPtr write = IntPtr.Zero;
            bool ret = CreatePipe(out read, out write, attribute, size);

            read_pipe = new CAutoHandle(read);
            write_pipe = new CAutoHandle(write);
            return ret;
        }
        //hPipeToRead 與 hPipeToWrite 都是回傳值
        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool CreatePipe(out IntPtr hPipeToRead, out IntPtr hPipeToWrite, SECURITY_ATTRIBUTES attribute, uint buffer_size);

        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CreateNamedPipeW")]
        static public extern IntPtr CreateNamedPipe([MarshalAs(UnmanagedType.LPWStr)] string pipeName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, SECURITY_ATTRIBUTES lpSecurityAttributes);
        
        //[DllImport(Win32DLL.Kernel32, CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "CreateNamedPipe")]
        //static public extern IntPtr CreateNamedPipe2(string pipeName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, IntPtr lpSecurityAttributes);
        #endregion

        #region ======== File ========
        static public CAutoHandle CreateFile(string filename, ACCESS_TYPE access, FILE_SHARE_MODE share,
                    SECURITY_ATTRIBUTES security, FILE_DISPOSITION disposition, FILE_ATTR_AND_FLAG attribute,
                    CAutoHandle template)
        {
            IntPtr temp = (template == null) ? IntPtr.Zero : (IntPtr)template;
            IntPtr ret = CreateFile(filename, (uint)access, (uint)share, security, (uint)disposition, (uint)attribute, temp);
            return new CAutoHandle(ret);
        }

        static public CAutoHandle CreateFile(string filename, ACCESS_TYPE access, FILE_SHARE_MODE share,
                            FILE_DISPOSITION disposition, FILE_ATTR_AND_FLAG attribute)
        {
            return CreateFile(filename, access, share, null, disposition, attribute, IntPtr.Zero);
        }

        //參數是class的已經是reference type，直接塞物件進去，Marshal 會幫我們弄成pointer給 C function
        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CreateFileW")]
        static public extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPWStr)]string lpFileName, uint dwDesiredAccess, uint dwShareMode, SECURITY_ATTRIBUTES lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        static public CAutoHandle CreateEvent(bool reset, bool init, string name)
        {
            IntPtr ret = CreateEvent(null, reset, init, name);
            return new CAutoHandle(ret);
        }

        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Unicode, EntryPoint = "CreateEventW")]
        static public extern IntPtr CreateEvent(SECURITY_ATTRIBUTES securityAttributes, bool bManualReset, bool bInitialState, [MarshalAs(UnmanagedType.LPWStr)] string lpName);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool WriteFile(IntPtr hFile,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, 
                                            uint nNumberOfBytesToWrite, 
                                            ref uint lpNumberOfBytesWritten,
                                            OVERLAPPED lpOverlapped);
        
        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool WriteFileEx(IntPtr hFile,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, 
                                            uint nNumberOfBytesToWrite, 
                                            ref uint lpNumberOfBytesWritten, 
                                            OVERLAPPED lpOverlapped,
                                            DelegateIOCompletion lpCompletion);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool ReadFile(IntPtr hFile, 
                                    [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, 
                                    uint nNumberOfBytesToRead, 
                                    ref uint lpNumberOfBytesRead,
                                    OVERLAPPED lpOverlapped);
        
        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool ReadFileEx(IntPtr hFile,
                                    [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
                                    uint nNumberOfBytesToRead,
                                    ref uint lpNumberOfBytesRead,
                                    OVERLAPPED lpOverlapped,
                                    DelegateIOCompletion lpCompletion);
        #endregion
    }
}
