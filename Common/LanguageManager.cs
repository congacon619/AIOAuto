using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AIOAuto.Common.Constants;
using AIOAuto.Common.Models;
using Timer = System.Timers.Timer;

namespace AIOAuto.Common
{
    public static class LanguageManager
    {
        private const string MissingKeysFilePath = @"logs\\MissingTranslate.txt";
        private static Dictionary<string, string> _translations = new Dictionary<string, string>();
        private static readonly HashSet<string> MissingKeys = new HashSet<string>();

        private static Timer _saveTimer;


        public static void Initialize()
        {
            LoadTranslations();
            _saveTimer = new Timer(5000);
            _saveTimer.Elapsed += (sender, e) => SaveMissingKeys();
            _saveTimer.AutoReset = true;
            _saveTimer.Start();
        }


        public static void LoadTranslations()
        {
            try
            {
                var settings = new JsonSetting<Dictionary<string, string>>($@"{Base.Language}_{JsonFile.Language}");
                _translations = settings.GetSettings();
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, $@"Error when load translates: {ex.Message}");
            }
        }

        public static string GetValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return key;

            if (_translations.TryGetValue(key, out var value)) return value;

            MissingKeys.Add(key);

            return key;
        }


        private static void SaveMissingKeys()
        {
            try
            {
                if (MissingKeys.Count == 0) return;
                Directory.CreateDirectory(Path.GetDirectoryName(MissingKeysFilePath) ??
                                          throw new InvalidOperationException());

                File.AppendAllLines(MissingKeysFilePath, MissingKeys);
                MissingKeys.Clear();
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, $@"Save missing keys failed: {ex.Message}");
            }
        }


        public static void Shutdown()
        {
            _saveTimer?.Stop();
            SaveMissingKeys();
        }

        private static void GetValue(Control control)
        {
            if (control.Name.ToLower().Contains("_hidden"))
                control.Visible = false;
            control.Text = GetValue(control.Name);
        }

        private static void GetValue(ToolStripItem control)
        {
            if (control.Name.ToLower().Contains("_hidden"))
                control.Visible = false;
            control.Text = GetValue(control.Name);
        }

        private static void GetValue(DataGridViewColumn control)
        {
            control.HeaderText = GetValue(control.Name);
        }


        private static IEnumerable<Control> GetAllControl(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            var enumerable = controls.ToList();
            return enumerable.SelectMany(GetAllControl)
                .Concat(enumerable);
        }

        public static void SetLanguage(Control control)
        {
            var c = GetAllControl(control);
            foreach (var item in c)
                switch (item)
                {
                    case DataGridView view:
                    {
                        foreach (DataGridViewColumn i in view.Columns)
                            GetValue(i);
                        break;
                    }
                    case MenuStrip strip:
                    {
                        foreach (ToolStripMenuItem i in strip.Items)
                        {
                            GetValue(i);
                            if (i.HasDropDownItems)
                                SetLanguage(i);
                        }

                        break;
                    }
                    case StatusStrip statusStrip:
                    {
                        foreach (ToolStripItem i in statusStrip.Items)
                            GetValue(i);
                        break;
                    }
                    default:
                        GetValue(item);
                        break;
                }

            GetValue(control);
        }

        public static void SetLanguage(ContextMenuStrip control)
        {
            foreach (ToolStripMenuItem i in control.Items)
            {
                GetValue(i);
                if (i.HasDropDownItems)
                    SetLanguage(i);
            }
        }

        private static void SetLanguage(ToolStripMenuItem control)
        {
            foreach (ToolStripMenuItem i in control.DropDownItems)
            {
                GetValue(i);
                if (i.HasDropDownItems)
                    SetLanguage(i);
            }
        }
    }
}