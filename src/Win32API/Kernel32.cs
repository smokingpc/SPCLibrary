using System;
using System.Runtime.InteropServices;

namespace Win32API
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
        //hPipeToRead 與 hPipeToWrite 都是回傳值
        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool CreatePipe(ref IntPtr hPipeToRead, ref IntPtr hPipeToWrite, SECURITY_ATTRIBUTES attribute, uint buffer_size);

        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        static public extern IntPtr CreateNamedPipe(string pipeName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, ref SECURITY_ATTRIBUTES lpSecurityAttributes);
        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "CreateNamedPipe")]
        static public extern IntPtr CreateNamedPipe2(string pipeName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, IntPtr lpSecurityAttributes);
        #endregion

        #region ======== File ========
        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        static public extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, ref SECURITY_ATTRIBUTES lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "CreateFile")]
        static public extern IntPtr CreateFile2(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport(Win32DLL.Kernel32, CharSet = CharSet.Auto)]
        static public extern IntPtr CreateEvent(ref SECURITY_ATTRIBUTES securityAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool WriteFile(IntPtr hFile,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, 
                                            uint nNumberOfBytesToWrite, 
                                            ref uint lpNumberOfBytesWritten, 
                                            ref System.Threading.NativeOverlapped lpOverlapped);
        
        [DllImport(Win32DLL.Kernel32, SetLastError = true, EntryPoint = "WriteFile")]
        static public extern bool WriteFile2(IntPtr hFile,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
                                            uint nNumberOfBytesToWrite,
                                            ref uint lpNumberOfBytesWritten,
                                            IntPtr hOverlapped);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool WriteFileEx(IntPtr hFile,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, 
                                            uint nNumberOfBytesToWrite, 
                                            ref uint lpNumberOfBytesWritten, 
                                            ref System.Threading.NativeOverlapped lpOverlapped, 
                                            ref System.Threading.IOCompletionCallback lpCompletion);

        [DllImport(Win32DLL.Kernel32, SetLastError = true, EntryPoint = "WriteFileEx")]
        static public extern bool WriteFileEx2(IntPtr hFile,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
                                            uint nNumberOfBytesToWrite,
                                            ref uint lpNumberOfBytesWritten,
                                            IntPtr hOverlapped,
                                            IntPtr hCompletion);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool ReadFile(IntPtr hFile, 
                                    [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, 
                                    uint nNumberOfBytesToRead, 
                                    ref uint lpNumberOfBytesRead, 
                                    ref System.Threading.NativeOverlapped lpOverlapped);
        
        [DllImport(Win32DLL.Kernel32, SetLastError = true, EntryPoint = "ReadFile")]
        static public extern bool ReadFile2(IntPtr hFile,
                                    [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
                                    uint nNumberOfBytesToRead,
                                    ref uint lpNumberOfBytesRead,
                                    IntPtr hOverlapped);

        [DllImport(Win32DLL.Kernel32, SetLastError = true)]
        static public extern bool ReadFileEx(IntPtr hFile,
                                    [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
                                    uint nNumberOfBytesToRead,
                                    ref uint lpNumberOfBytesRead,
                                    ref System.Threading.NativeOverlapped lpOverlapped,
                                    ref System.Threading.IOCompletionCallback lpCompletion);

        [DllImport(Win32DLL.Kernel32, SetLastError = true, EntryPoint = "ReadFileEx")]
        static public extern bool ReadFileEx2(IntPtr hFile,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
                                            uint nNumberOfBytesToRead,
                                            ref uint lpNumberOfBytesRead,
                                            IntPtr hOverlapped,
                                            IntPtr hCompletion);
        #endregion


    }
}
