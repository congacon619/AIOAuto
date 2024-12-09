using System;
using System.Windows.Forms;
using AIOAuto.Common.Constants;

namespace AIOAuto.Common
{
    public static class AppMessageBox
    {
        public static void ShowMessageBox(object s, MsgBoxLevel level = MsgBoxLevel.Info)
        {
            switch (level)
            {
                case MsgBoxLevel.Info:
                    MessageBox.Show(LanguageManager.GetValue(s.ToString()), @"MINSoftware", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    break;
                case MsgBoxLevel.Warning:
                    MessageBox.Show(LanguageManager.GetValue(s.ToString()), @"MINSoftware", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    break;
                case MsgBoxLevel.Error:
                    MessageBox.Show(LanguageManager.GetValue(s.ToString()), @"MINSoftware", MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public static DialogResult ShowMessageBoxWithQuestion(string content)
        {
            return MessageBox.Show(LanguageManager.GetValue(content), @"MINSoftware", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
        }
    }
}