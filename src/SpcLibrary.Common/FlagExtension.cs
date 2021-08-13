using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpcLibrary.Common
{
    public static class FlagExtension
    {
    /// <summary>
    /// 將指定的數值轉換成enum型別的旗標清單，把每個flag用AND運算比對出來
    /// </summary>
    /// <typeparam name="T">必須是enum型別的type</typeparam>
    /// <param name="data">輸入值</param>
    /// <returns>enum型別的list，將符合data內數值的enum value列出來</returns>
        public static List<T> ToFlags<T>(this int data) where T : Enum
        {
            List<T> ret = new List<T>();

            var values = Enum.GetValues(typeof(T));

            foreach (var item in values)
            {
                if ((int)item == 0)
                {
                    if (data == 0)
                        ret.Add((T)item);
                }
                else if (((int)item & data) == (int)item)
                    ret.Add((T)item);
            }

            return ret;
        }
        public static List<T> ToFlags<T>(this Int16 data) where T : Enum
        {
            List<T> ret = new List<T>();

            var values = Enum.GetValues(typeof(T));

            foreach (var item in values)
            {
                if ((Int16)item == 0)
                {
                    if (data == 0)
                        ret.Add((T)item);
                }
                else if (((Int16)item & data) == (Int16)item)
                    ret.Add((T)item);
            }

            return ret;
        }
        public static List<T> ToFlags<T>(this byte data) where T : Enum
        {
            List<T> ret = new List<T>();

            var values = Enum.GetValues(typeof(T));

            foreach (var item in values)
            {
                if ((byte)item == 0)
                {
                    if (data == 0)
                        ret.Add((T)item);
                }
                else if (((byte)item & data) == (byte)item)
                    ret.Add((T)item);
            }

            return ret;
        }
    }
}
