using System;

namespace gSmartHR.BLL.Library
{
    public static class Common
    {
        public static string EncryptionKey = "E546C8DF278CD5931069B522E695D4F2";
        public static string[] Pepper = {"pLgrgi", "GTn~sc", "hpvdCZ", "v*#Bc?", "kIGIc*", "$JHkcQ", ";RWlhL", "v*K?HT", "Jrs#F}", "?(IigV", "Z?RCt^", "S>n#j_", "ck!qgr", "Kma-~_", "PadPlo", "h_KaJj", ";[O#DK", "G$hHZv", "o%^qU;", "U)?^-s"};

        public static string RandomPepper()
        {
            Random random = new Random();
            return Pepper[random.Next(Pepper.Length)];
        }
    }
}
