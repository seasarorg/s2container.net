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
            SingletonS2ContainerFactory.ConfigPath = "Seasar.Quill.Examples.test.dicon";
            SingletonS2ContainerFactory.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}