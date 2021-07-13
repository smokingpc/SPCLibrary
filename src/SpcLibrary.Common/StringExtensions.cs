using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpcCommon.Common.Extension
{
    public static class StringExtensions
    {
    
    /// <summary>
    /// 將HEX String轉為 Byte Array
    /// </summary>
    /// <param name="data">輸入的HEX字串，可以用空格或逗號分隔</param>
    /// <returns></returns>
        public static byte[] ToBuffer(this string data)
        {
            List<byte> ret = new List<byte>();
            var tokens = data.Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries );
            foreach (var item in tokens)
            {
                ret.Add(byte.Parse(item));
            }

            return ret.ToArray();
        }
    }
}
