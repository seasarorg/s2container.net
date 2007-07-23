#region Copyright

/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;

namespace Seasar.WindowsExample.Forms
{
    /// <summary>
    /// 社員一覧画面
    /// </summary>
    public partial class FrmEmployeeList : Form
    {
        /// <summary>
        /// 例外エラーメッセージ書式
        /// </summary>
        private const string EXCEPTION_MSG_FORMAT = "予期できないエラーが発生しました。詳細を確認してください。（{0}）";

        /// <summary>
        /// ログ(log4net)
        /// </summary>
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 画面ディスパッチャー
        /// </summary>
        private IFormDispatcher _dispatcher;

        /// <summary>
        /// 社員一覧サービス
        /// </summary>
        private IEmployeeListService _service;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmEmployeeList(IFormDispatcher dispatcher)
        {
            InitializeComponent();

            this._dispatcher = dispatcher;
        }

        /// <summary>
        /// 社員一覧サービス
        /// </summary>
        public IEmployeeListService Service
        {
            get { return _service; }
            set { _service = value; }
        }

        /// <summary>
        /// 新規ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                _dispatcher.ShowDataEdit(null);

                _ShowList();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 出力ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutput_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("保存先を指定してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                _InitializeSaveDialog();

                if (dlgSave.ShowDialog(this) == DialogResult.OK)
                {
                    if (_service.OutputCSV(dlgSave.FileName) > 0)
                        MessageBox.Show("出力しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("出力するデータがありませんでした", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 閉じるボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            logger.InfoFormat("{0}を終了", Name);
            Close();
        }

        /// <summary>
        /// フォームをロードしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEmployeeList_Load(object sender, EventArgs e)
        {
            try
            {
                logger.InfoFormat("{0}がロードされました", Name);

                _ShowList();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// グリッドをダブルクリックしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Nullable<int> id = (Nullable<int>) gridList.CurrentRow.Cells["ColumnId"].Value;
                _dispatcher.ShowDataEdit(id);

                _ShowList();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Gridに表示する
        /// </summary>
        private void _ShowList()
        {
            IList<EmployeeDto> list = _service.GetAll();

            gridList.DataSource = list;
        }

        /// <summary>
        /// 保存ダイアログを初期化する
        /// </summary>
        private void _InitializeSaveDialog()
        {
            dlgSave.DefaultExt = "*.csv";
            dlgSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            dlgSave.Title = "CSV出力";
            dlgSave.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル (*.*)|*.*";
            dlgSave.AddExtension = true;
            dlgSave.OverwritePrompt = true;
            dlgSave.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\社員一覧.csv";
            dlgSave.RestoreDirectory = true;
        }
    }
}