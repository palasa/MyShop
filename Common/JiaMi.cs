using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace Common
{
    public static class JiaMi
    {
        public static string Md5( string str)
        {
            var input = Encoding.Default.GetBytes(str);
            System.Security.Cryptography.MD5 md5 = MD5.Create("MD5");
            var output = md5.ComputeHash(input);
            string o = BitConverter.ToString(output);
            o = o.Replace("-", "");
            return o;
        }
    }
}
