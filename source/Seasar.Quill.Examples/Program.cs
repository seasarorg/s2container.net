using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
            // 定義(dicon)ファイルをセットする
            SingletonS2ContainerFactory.ConfigPath = "Seasar.Quill.Examples.App.dicon";

            // S2Containerを初期化する
            SingletonS2ContainerFactory.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // S2Containerの終了処理を行う
            SingletonS2ContainerFactory.Container.Destroy();
        }
    }
}