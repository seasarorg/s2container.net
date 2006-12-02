using System;
using System.Windows.Forms;

namespace Seasar.WindowsExample.Forms
{
    /// <summary>
    /// メインメニュー画面
    /// </summary>
    public partial class FrmMainMenu : Form
    {
        /// <summary>
        /// 画面ディスパッチャー
        /// </summary>
        private IFormDispatcher _dispatcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dispatcher">画面ディスパッチャー</param>
        public FrmMainMenu(IFormDispatcher dispatcher)
        {
            InitializeComponent();

            this._dispatcher = dispatcher;
        }

        /// <summary>
        /// 社員一覧を表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                _dispatcher.ShowDataList();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 部門一覧を表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                _dispatcher.ShowMasterList();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// フォームを閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// フォームをロードする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMainMenu_Load(object sender, EventArgs e)
        {
            ;
        }

        /// <summary>
        /// フォームを閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( MessageBox.Show("本当に終了しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                 == DialogResult.No )
            {
                e.Cancel = true;
            }
        }
    }
}