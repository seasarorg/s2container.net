using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using log4net.Util;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Windows;

namespace Seasar.WindowsExample.Forms
{
    internal static class Program
    {
        /// <summary>
        /// DIコンテナ設定ファイル(変更)
        /// </summary>
        private const string PATH = "Example.dicon";

        /// <summary>
        /// ログ(log4net)
        /// </summary>
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                FileInfo info = new FileInfo(
                    string.Format("{0}.exe.config", SystemInfo.AssemblyShortName(
                                                        Assembly.GetExecutingAssembly())));
                // アセンブリがdllの場合は".dll.config"

                XmlConfigurator.Configure(LogManager.GetRepository(), info);

                logger.Info("二重起動チェック");
                Mutex mutex;
                OperatingSystem os = Environment.OSVersion;
                if ( os.Platform == PlatformID.Win32NT && os.Version.Major >= 5 )
                {
                    mutex = new Mutex(false, @"Global\" + Application.ProductName);
                }
                else
                {
                    mutex = new Mutex(false, Application.ProductName);
                }

                if ( mutex.WaitOne(0, false) )
                {
                    // 起動済がない場合

                    logger.Info("起動");

                    Application.EnableVisualStyles();

                    SingletonS2ContainerFactory.ConfigPath = PATH;
                    SingletonS2ContainerFactory.Init();
                    IS2Container container = SingletonS2ContainerFactory.Container;

                    ApplicationContext context
                        = (ApplicationContext) container.GetComponent(typeof (S2ApplicationContext));
                    Application.Run(context);

                    mutex.ReleaseMutex();
                }
                else
                {
                    logger.Info("二重起動済み");
                    MessageBox.Show("このアプリケーションはすでに起動しています", "Main",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                GC.KeepAlive(mutex);
                mutex.Close();
            }
            catch ( ApplicationException ex )
            {
                MessageBox.Show(ex.Message, "Main",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch ( Exception e )
            {
                logger.Debug("エラー:" + e.Message, e);
            }
            logger.Info("終了");
        }
    }
}