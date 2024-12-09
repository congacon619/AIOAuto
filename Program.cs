using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using AIOAuto.Common;
using AIOAuto.Common.Constants;
using AIOAuto.Views;
using NLog;
using NLog.Config;
using NLog.Targets;
using Debug = AIOAuto.Common.Debug;

namespace AIOAuto
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget("file")
            {
                FileName = "logs/${shortdate}.log",
                Layout = "${longdate} [${level}] [${callsite}] ${message}"
            };
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = "${longdate} [${level}] [${callsite}] ${message}"
            };
            config.AddTarget(fileTarget);
            config.AddTarget(consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
            LogManager.Configuration = config;

            LanguageManager.Initialize();

            Debug.GetDebug();


            try
            {
                var nameProcess = new[] { "fiddler", "charles", "wireshark", "burp", "dnspy", "megadumper" };
                Process.GetProcesses().Where(p => nameProcess.Any(p.ProcessName.ToLower().Contains)).ToList()
                    .ForEach(y => y.Kill());
                Process.GetProcesses().Where(p => nameProcess.Any(p.MainWindowTitle.ToLower().Contains)).ToList()
                    .ForEach(y => y.Kill());


                if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator) ==
                    false) AppMessageBox.ShowMessageBox(Default.RunWithAdministrator, MsgBoxLevel.Warning);


                Application.ApplicationExit += (sender, e) => { LanguageManager.Shutdown(); };
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FMain());
            }
            catch (Exception e)
            {
                AppMessageBox.ShowMessageBox(Default.UnknownErrorMsg, MsgBoxLevel.Error);
                AppLogger.ErrorDetail(e, "Run Program2");
                Environment.Exit(0);
            }
        }
    }
}