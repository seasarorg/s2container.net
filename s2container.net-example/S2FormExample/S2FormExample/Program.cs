#region Copyright

/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

#endregion

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using log4net.Util;
using Seasar.Quill;

namespace Seasar.S2FormExample.Forms
{
    internal static class Program
    {
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
                if (os.Platform == PlatformID.Win32NT && os.Version.Major >= 5)
                {
                    mutex = new Mutex(false, @"Global\" + Application.ProductName);
                }
                else
                {
                    mutex = new Mutex(false, Application.ProductName);
                }

                if (mutex.WaitOne(0, false))
                {
                    // 起動済がない場合

                    logger.Info("起動");

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmMainMenu());

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
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Main",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (Exception e)
            {
                logger.Debug("エラー:" + e.Message, e);
                logger.Error(e.StackTrace);
            }

            logger.Info("終了");
        }
    }
}