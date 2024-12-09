using System;
using System.Windows.Forms;
using AIOAuto.Common;
using AIOAuto.Common.Constants;

namespace AIOAuto.Views
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
            LanguageManager.SetLanguage(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Base.Language == Lang.English)
                Base.SetLanguage();
            else
                Base.SetLanguage(Lang.English);
            LanguageManager.LoadTranslations();
            LanguageManager.SetLanguage(this);
        }
    }
}