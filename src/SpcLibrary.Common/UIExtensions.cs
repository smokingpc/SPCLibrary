using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpcLibrary.Common.UI
{
    public static class UI
    {
        public static void SetText(this TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)(() => { SetText(tb, msg, append); }));
            }
            else
            {
                if (append)
                    tb.AppendText(msg);
                else
                    tb.Text = msg;
            }
        }
        //public static void SetLine(this TextBox tb, string msg, bool append = true)
        //{
        //    if (tb.InvokeRequired)
        //    {
        //        tb.Invoke((Action)(() => { SetText(tb, msg, append); }));
        //    }
        //    else
        //    {
        //        SetText(tb, msg + "\r\n", append);
        //    }
        //}
    }
}
