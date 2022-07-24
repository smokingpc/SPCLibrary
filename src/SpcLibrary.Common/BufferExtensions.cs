using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpcCommon.Common.Extension
{
    public static class BufferExtensions
    {
    /// <summary>
    /// 將指定byte[] 轉換成 HEX 字串
    /// </summary>
    /// <param name="buffer">byte array原始資料</param>
    /// <param name="space">每個HEX數字之間要不要空格隔開？例如 A1 C9 與 A1C9 這樣的差異</param>
    /// <returns></returns>
        public static string ToHexString(this byte[] buffer, bool space=false)
        {
            string split = "";
            if (space == true)
                split = " ";
            
            string ret = "";
            foreach (var item in buffer)
            {
                ret = ret + split + item.ToString("X2");
            }

            ret = ret.Trim();
            return ret;
        }


        /// <summary>
        ///如果收到packet之類的buffer，可以透過這個function直接映射出struct或class的instance
        ///(取代new的工作，直接映射這個buffer。用在packet parsing很方便)
        ///
        ///用BufferHelper時，用來映射的 class / structure 必須明確宣告 layout
        ///要用Explicit或Sequential，並明定總長度
        ///這邊Mapping的class / structure裡的data member，只能用 Field
        ///每個 Field 在記憶體裡的 Offset 要用 FieldOffset 明確宣告(Explicit Layout)
        ///
        /// 註：其實這個函式也可以用在struct結構
        /// </summary>
        /// <typeparam name="T">最終回傳物件的型別，可以是class或struct</typeparam>
        /// <param name="buffer">原始的來源資料buffer，最後生成的物件會直接映射這個buffer，瞬間完成各欄位parsing</param>
        /// <param name="copy">true 表示要copy一份buffer，再把instance映射到新的buffer上，false 表示直接把物件映射到傳入的buffer，要注意這時候傳入的buffer在caller就別再使用了，除非你很清楚自己在做啥不然會搞亂記憶體。</param>
        /// <returns>型別為 T 的物件</returns>
        public static T ToClass<T>(this byte[] buffer, bool copy = true)
        {
            T result = default(T);

            byte[] map_buffer = null;
            if (copy)
            {
                map_buffer = new byte[buffer.Length];
                Array.Copy(buffer, 0, map_buffer, 0, buffer.Length);
            }
            else
                map_buffer = buffer;

            //GCHandleType.Pinned 允許這個buffer做出來的handle直接取用底下的實體記憶體位置
            //相當於允許直接使用底層的C指標
            GCHandle handle = GCHandle.Alloc(map_buffer, GCHandleType.Pinned);
            result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            //pin太久會造成C#的記憶體回收機制被打亂，所以趕緊用完趕緊釋放，不要一直占住不放
            handle.Free();
            return result;
        }

        /// <summary>
        ///如果收到packet之類的buffer，可以透過這個function直接映射出struct或class的instance
        ///(取代new的工作，直接映射這個buffer。用在packet parsing很方便)
        ///
        ///用BufferHelper時，用來映射的 class / structure 必須明確宣告 layout
        ///要用Explicit或Sequential，並明定總長度
        ///這邊Mapping的class / structure裡的data member，只能用 Field
        ///每個 Field 在記憶體裡的 Offset 要用 FieldOffset 明確宣告(Explicit Layout)
        ///
        /// 註：其實這個函式也可以用在struct結構
        /// </summary>
        /// <typeparam name="T">最終回傳物件的型別，可以是class或struct</typeparam>
        /// <param name="buffer">原始的來源資料buffer，最後生成的物件會直接映射這個buffer，瞬間完成各欄位parsing</param>
        /// <param name="offset">來源資料buffer要開始映射的element index</param>
        /// <param name="size">來源資料buffer要映射的長度</param>
        /// <returns>型別為 T 的物件</returns>
        public static T ToClass<T>(this byte[] buffer, int offset, int size)
        {
            T result = default(T);

            byte[] map_buffer = new byte[size];
            Array.Copy(buffer, offset, map_buffer, 0, size);

            //GCHandleType.Pinned 允許這個buffer做出來的handle直接取用底下的實體記憶體位置
            //相當於允許直接使用底層的C指標
            GCHandle handle = GCHandle.Alloc(map_buffer, GCHandleType.Pinned);
            result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            //pin太久會造成C#的記憶體回收機制被打亂，所以趕緊用完趕緊釋放，不要一直占住不放
            handle.Free();
            return result;
        }

        public static byte[] ToBytes<T>(this T obj) 
        {
            byte[] bytes = new byte[Marshal.SizeOf(typeof(T))];
            GCHandle pinStructure = GCHandle.Alloc(obj, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(pinStructure.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            catch
            {
                return null;
            }
            finally
            {
                pinStructure.Free();
            }
        }

        public static T FromBytes<T>(this byte[] buffer, bool copy = true)
        {
            return buffer.FromBytes<T>(0, copy);
        }
        public static T FromBytes<T>(this byte[] buffer, int offset, bool copy = true)
        {
            T result = default(T);
            int layout_size = Marshal.SizeOf<T>();

            if (buffer != null && (offset + layout_size) <= buffer.Length)
            {
                byte[] map_buffer = null;
                if (copy)
                {
                    map_buffer = new byte[layout_size];
                    Array.Copy(buffer, offset, map_buffer, 0, layout_size);
                }
                else
                    map_buffer = buffer;

                //GCHandleType.Pinned 允許這個buffer做出來的handle直接取用底下的實體記憶體位置
                //相當於允許直接使用底層的C指標
                GCHandle handle = GCHandle.Alloc(map_buffer, GCHandleType.Pinned);
                var test = handle.AddrOfPinnedObject();
                result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

                //pin太久會造成C#的記憶體回收機制被打亂，所以趕緊用完趕緊釋放，不要一直占住不放
                handle.Free();
            }

            return result;
        }
        public static T FromBytes<T>(this byte[] buffer, int offset, int t_size, bool copy = true)
        {
            T result = default(T);
            if (buffer != null && (offset + t_size) <= buffer.Length)
            {
                byte[] map_buffer = null;
                if (copy)
                {
                    map_buffer = new byte[t_size];
                    Array.Copy(buffer, offset, map_buffer, 0, t_size);
                }
                else
                    map_buffer = buffer;

                //GCHandleType.Pinned 允許這個buffer做出來的handle直接取用底下的實體記憶體位置
                //相當於允許直接使用底層的C指標
                GCHandle handle = GCHandle.Alloc(map_buffer, GCHandleType.Pinned);
                var test = handle.AddrOfPinnedObject();
                result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

                //pin太久會造成C#的記憶體回收機制被打亂，所以趕緊用完趕緊釋放，不要一直占住不放
                handle.Free();
            }

            return result;
        }

        //count == how many entries in list should returned?
        public static List<T> ArrayFromBytes<T>(this byte[] buffer, int offset, int count, bool copy = true)
        {
            //T result = default(T);
            List<T> result = new List<T>();
            int layout_size = Marshal.SizeOf<T>();

            for (int i = 0; i < count; i++)
            { 
                
            }

            return result;
        }
    }
}
