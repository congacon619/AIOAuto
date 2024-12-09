using System;
using System.Windows.Forms;
using AIOAuto.Common;

namespace AIOAuto.Views.FMain
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
            LanguageManager.SetLanguage(this);
            infoLabel.Text = GetInfoLabelText();
        }

        private async void picChangeLanguage_Click(object sender, EventArgs e)
        {
            await ChangeLanguage();
        }
    }
}