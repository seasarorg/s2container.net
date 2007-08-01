using System;
using System.Windows.Forms;

namespace Seasar.S2FormExample.Forms
{
    /// <summary>
    /// 起動時スプラッシュウィンドウフォーム
    /// </summary>
    public partial class FrmSplash : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmSplash()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームを閉じるときの処理
        /// </summary>
        /// <remarks>閉じないように処理している</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSplash_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
#if DEBUG
            Console.Out.WriteLine("SplashClosing Cancel");
#endif
        }
    }
}