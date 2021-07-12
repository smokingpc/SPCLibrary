﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpcCommon
{
    public static class UI
    {
        public static void SetText(TextBox tb, string msg, bool append = true)
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
        public static void SetLine(TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)(() => { SetText(tb, msg, append); }));
            }
            else
            {
                SetText(tb, msg + "\r\n", append);
            }
        }
    }
}
