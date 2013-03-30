#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Windows.Forms;
using log4net;
using log4net.Config;
using log4net.Util;
using Seasar.Framework.Container.Factory;

namespace Seasar.Quill.Examples
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // log4netの初期化
            FileInfo info = new FileInfo(SystemInfo.AssemblyShortName(
                Assembly.GetExecutingAssembly()) + ".exe.config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);

            // 定義(dicon)ファイルをセットする
            SingletonS2ContainerFactory.ConfigPath = "Seasar.Quill.Examples.App.dicon";

            // S2Containerを初期化する
            SingletonS2ContainerFactory.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //  継承フォームを動かしたい場合はコメントアウトを外して
            //  こちらを呼び出す
            //Application.Run(new ExtendedForm());

            // S2Containerの終了処理を行う
            SingletonS2ContainerFactory.Container.Destroy();
        }
    }
}
