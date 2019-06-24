using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
    public static class UI
    {
        public static void SetTextBoxText(TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)(() => { SetTextBoxText(tb, msg, append); }));
            }
            else
            {
                if (append)
                    tb.AppendText(msg + "\r\n");
                else
                    tb.Text = msg + "\r\n";
            }
        }
    }
}
