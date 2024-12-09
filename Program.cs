using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using AIOAuto.Common;
using AIOAuto.Common.Constants;
using AIOAuto.Views.FMain;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Debug = AIOAuto.Common.Debug;

namespace AIOAuto
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                var config = new LoggingConfiguration();
                var fileTarget = new FileTarget("file")
                {
                    FileName = "logs/${shortdate}.log",
                    Layout = "${longdate} [${level}] [${callsite}] ${message}"
                };
                var asyncFileTarget = new AsyncTargetWrapper(fileTarget)
                {
                    Name = "asyncFile",
                    QueueLimit = 5000
                };
                var consoleTarget = new ConsoleTarget("console")
                {
                    Layout = "${longdate} [${level}] [${callsite}] ${message}"
                };
                var asyncConsoleTarget = new AsyncTargetWrapper(consoleTarget)
                {
                    Name = "asyncConsole",
                    OverflowAction = AsyncTargetWrapperOverflowAction.Grow
                };
                config.AddTarget(asyncFileTarget);
                config.AddTarget(asyncConsoleTarget);
                config.AddRule(LogLevel.Debug, LogLevel.Fatal, asyncFileTarget);
                config.AddRule(LogLevel.Debug, LogLevel.Fatal, asyncConsoleTarget);

                LogManager.Configuration = config;

                LanguageManager.Initialize();

                Debug.GetDebug();

                EnsureAdminRights();

                StopSuspiciousProcesses();
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

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AppLogger.ErrorDetail(e.Exception, "Unhandled UI thread exception");
            MessageBox.Show(Default.UnknownErrorMsg);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            AppLogger.ErrorDetail(ex, "Unhandled non-UI thread exception");
            MessageBox.Show(Default.UnknownErrorMsg);
        }
    }
}