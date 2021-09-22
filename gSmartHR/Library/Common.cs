using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace gSmartHR.Library
{
    public static class Common
    {
        public static string WebRootPath()
        {
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            location = location.Split(new string[] { "bin" }, StringSplitOptions.None)[0];
            return System.IO.Path.GetDirectoryName(location);
        }

        public static string RandomString(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
