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
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Seasar.S2FormExample.Logics.Page;
using Seasar.S2FormExample.Logics.Service;
using Seasar.Windows;
using Seasar.Windows.Attr;

namespace Seasar.S2FormExample.Forms
{
    /// <summary>
    /// 部門一覧画面
    /// </summary>
    [ControlModifier("txt", "")]
    [Control("gridList", "DataSource", "List")]
    public partial class FrmDepartmentList : S2Form
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
        protected IFormDispatcher dispatcher;

        /// <summary>
        /// 部門一覧サービス
        /// </summary>
        protected IDepartmentListService service;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmDepartmentList()
        {
            InitializeComponent();

            _InitializeGridView();
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
                dispatcher.ShowMasterEdit(null);
                this.DataSource = service.GetAll();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private void FrmDepartmentList_Load(object sender, EventArgs e)
        {
            try
            {
                logger.InfoFormat("{0}がロードされました", Name);

                this.DataSource = service.GetAll();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                int? id = ((DepartmentListPage) this.DataSource).List[gridList.CurrentRow.Index].Id;
                dispatcher.ShowMasterEdit(id);

                this.DataSource = service.GetAll();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// GridViewを初期化する
        /// </summary>
        private void _InitializeGridView()
        {
            gridList.RowCount = 0;
        }

    }
}