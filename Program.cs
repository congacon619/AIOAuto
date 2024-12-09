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

            EnsureAdminRights();

            StopSuspiciousProcesses();

            try
            {
                Application.ApplicationExit += (sender, e) => { LanguageManager.Shutdown(); };
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FMain());
            }
            catch (Exception e)
            {
                AppMessageBox.ShowMessageBox(Default.UnknownErrorMsg, MsgBoxLevel.Error);
                AppLogger.ErrorDetail(e, "Run Program");
                Environment.Exit(0);
            }
        }

        private static void EnsureAdminRights()
        {
            if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) return;
            var processInfo = new ProcessStartInfo
            {
                FileName =
                    Process.GetCurrentProcess().MainModule?.FileName ?? throw new InvalidOperationException(),
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowMessageBox(Default.RunWithAdministratorErrorMsg, MsgBoxLevel.Error);
                AppLogger.ErrorDetail(ex, "EnsureAdminRights");
            }

            Environment.Exit(0);
        }


        private static void StopSuspiciousProcesses()
        {
            var nameProcess = new[] { "fiddler", "charles", "wireshark", "burp", "dnspy", "megadumper" };

            try
            {
                var suspiciousProcesses = Process.GetProcesses()
                    .Where(p =>
                    {
                        try
                        {
                            var processName = p.ProcessName.ToLower();
                            var windowTitle = p.MainWindowTitle?.ToLower() ?? "";
                            return nameProcess.Any(name => processName.Contains(name) || windowTitle.Contains(name));
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .ToList();

                suspiciousProcesses.ForEach(p =>
                {
                    try
                    {
                        p.Kill();
                        AppLogger.Warn($"Killed process: {p.ProcessName}");
                    }
                    catch (Exception ex)
                    {
                        AppLogger.ErrorDetail(ex, $"Failed to kill process {p.ProcessName}");
                    }
                });
            }
            catch (Exception ex)
            {
                AppLogger.ErrorDetail(ex, "StopSuspiciousProcesses");
            }
        }
    }
}