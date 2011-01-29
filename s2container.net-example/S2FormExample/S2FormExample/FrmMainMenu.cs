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
using System.Windows.Forms;

namespace Seasar.S2FormExample.Forms
{
    /// <summary>
    /// メインメニュー画面
    /// </summary>
    public partial class FrmMainMenu : Form
    {
        /// <summary>
        /// 画面ディスパッチャー
        /// </summary>
        protected IFormDispatcher dispatcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmMainMenu()
        {
            InitializeComponent();
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
                dispatcher.ShowDataList();
            }
            catch (Exception ex)
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
                dispatcher.ShowMasterList();
            }
            catch (Exception ex)
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
            if (MessageBox.Show("本当に終了しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}