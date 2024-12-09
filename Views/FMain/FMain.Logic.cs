using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIOAuto.Common;
using AIOAuto.Common.Constants;
using AIOAuto.Properties;

namespace AIOAuto.Views.FMain
{
    partial class FMain
    {
        private readonly Bitmap _bmEn = Resources.united_kingdom1;
        private readonly Bitmap _bmVn = Resources.vietnam1;

        private static string GetInfoLabelText()
        {
            var info = LanguageManager.GetValue("infoLabel");
            var appVersion = Application.ProductVersion;
            return info.Replace("{}", appVersion);
        }

        private async Task ChangeLanguage()
        {
            try
            {
                picChangeLanguage.Enabled = false;
                Cursor = Cursors.WaitCursor;
                if (Base.Language == Lang.English)
                {
                    Base.SetLanguage();
                    picChangeLanguage.Image = _bmVn;
                }
                else
                {
                    Base.SetLanguage(Lang.English);
                    picChangeLanguage.Image = _bmEn;
                }

                LanguageManager.LoadTranslations();
                LanguageManager.SetLanguage(this);
                infoLabel.Text = GetInfoLabelText();
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, "Error changing language");
            }
            finally
            {
                await Task.Delay(500);
                picChangeLanguage.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }
}