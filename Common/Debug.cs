using AIOAuto.Common.Constants;
using AIOAuto.Common.DTOs;
using AIOAuto.Common.Models;

namespace AIOAuto.Common
{
    public static class Debug
    {
        public static bool CkbLoginFail;
        public static bool CkbChangeInfoDevice;
        public static bool CkbCheckpoint;
        public static bool CkbChangedPass;
        public static bool CkbCheckSpam;

        public static void GetDebug()
        {
            var settings = new JsonSetting<ConfigDebug>(JsonFile.ConfigDebug).GetSettings();
            CkbCheckpoint = settings.CkbCheckpoint;
            CkbChangedPass = settings.CkbChangedPass;
            CkbCheckSpam = settings.CkbCheckSpam;
            CkbLoginFail = settings.CkbLoginFail;
            CkbChangeInfoDevice = settings.CkbChangeInfoDevice;
        }
    }
}