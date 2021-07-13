using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace SpcLibrary.Win32API
{
    //AutoHandle 是對應windows native system 的 HANDLE 型別。
    //為了避免handle leak之類的毛病，所以將HANDLE包成 CAutoHandle，
    //讓他在GC自動回收或被呼叫Dispose時會自動釋放，省得使用者忘記close handle。
    public class CAutoHandle : IDisposable
    {
        private static readonly IntPtr InvalidValue = new IntPtr(-1);
        public bool IsInvalid
        {
            get { return ((Handle == IntPtr.Zero) || (Handle == InvalidValue)); }
        }

        private string AllocTrace = "";        //用來紀錄 "哪段code Allocate這個Handle的？"
        private IntPtr Handle = IntPtr.Zero;
        private bool IsDisposed = false;

        public static implicit operator IntPtr(CAutoHandle value)
        {
            return value.Handle;
        }

        public static implicit operator CAutoHandle(IntPtr value)
        {
            return new CAutoHandle(value);
        }

        public CAutoHandle()
        {
            AllocTrace = Environment.StackTrace;
        }
        public CAutoHandle(IntPtr init_handle) : this()
        { this.Handle = init_handle; }

        ~CAutoHandle() { Dispose(false); }
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed == false)
            {
                if (IntPtr.Zero != Handle)
                {
                    Kernel32.CloseHandle(Handle);
                    Handle = IntPtr.Zero;
                }
                IsDisposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
