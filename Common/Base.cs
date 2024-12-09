using System;
using AIOAuto.Common.Constants;
using AIOAuto.Common.DTOs;
using AIOAuto.Common.Models;

namespace AIOAuto.Common
{
    internal static class Base
    {
        public static Random Rd = new Random();

        public static string Language = GetLanguage();

        private static string GetLanguage()
        {
            return new JsonSetting<ConfigGeneral>(JsonFile.ConfigGeneral).GetSettings().Language;
        }

        public static void SetLanguage(string lang = Lang.Vietnamese)
        {
            var json = new JsonSetting<ConfigGeneral>(JsonFile.ConfigGeneral);
            json.Update(nameof(ConfigGeneral.Language), lang);
            json.Save();
            Language = lang;
        }
    }
}