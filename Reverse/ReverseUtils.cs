using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Reverse
{
    class ReverseUtils
    {
        /// <summary>
        /// 
        /// Length using foreach
        /// 
        /// Stopwatch : 
        /// 
        /// </summary>
        /// <param name="textToCount"></param>
        /// <returns></returns>
        public static int TextLength(string textToCount)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int lengthText = 0;
            foreach (char c in textToCount)
            {
                lengthText++;
            }
            sw.Stop();
            Console.WriteLine($"Length Foreach : {sw.Elapsed} ms");
            return lengthText;
        }

        public static unsafe int strLen(string s)
        {
            var sw3 = Stopwatch.StartNew();
            if (s == null) return 0;

            int length = 0;

            fixed (char* pStr = s)
            {
                char* pEnd = pStr;
                while (*pEnd++ != '\0')
                {
                    length = (int)((pEnd - pStr) - 1);
                }
            }
            sw3.Stop();
            Console.WriteLine($".Unsafe strLen intern : {sw3.Elapsed} ms");
            return length;
        }
    }
}
