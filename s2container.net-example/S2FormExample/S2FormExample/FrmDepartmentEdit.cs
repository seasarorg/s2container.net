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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using log4net;
using Seasar.S2FormExample.Logics.Page;
using Seasar.S2FormExample.Logics.Service;
using Seasar.Windows;
using Seasar.Windows.Attr;

namespace Seasar.S2FormExample.Forms
{
    /// <summary>
    /// 部門登録画面
    /// </summary>
    [ControlModifier("txt", "")]
    public partial class FrmDepartmentEdit : S2Form
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
        /// 部門ID
        /// </summary>
        private int? _id;

        /// <summary>
        /// 部門登録サービス
        /// </summary>
        protected IDepartmentEditService service;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmDepartmentEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 部門ID
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 登録ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("本当に登録しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.No)
                    return;

                if (!_SetInputData()) return;

                DepartmentEditPage data = (DepartmentEditPage) this.DataSource;
                data.Id = _id;
                if (service.ExecUpdate(data) > 0)
                {
                    _InitializeControls();
                    MessageBox.Show("登録しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.DataSource = null;
                    throw new ApplicationException("登録に失敗しました");
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
        /// 削除ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("本当に削除しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.No)
                    return;

                if (_id.HasValue)
                {
                    if (service.ExecDelete(_id.Value) > 0)
                    {
                        MessageBox.Show("削除しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    else
                    {
                        throw new ApplicationException("削除に失敗しました");
                    }
                }
                else
                {
                    MessageBox.Show("削除対象を選んでください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private void FrmDepartmentEdit_Load(object sender, EventArgs e)
        {
            logger.InfoFormat("{0}がロードされました", Name);

            try
            {
                _InitializeControls();
                if (_id.HasValue)
                {
                    DepartmentEditPage data = service.GetData(_id.Value);
                    if (data != null)
                    {
                        this.DataSource = data;
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        throw new ApplicationException("部門データが見つかりませんでした");
                    }
                }
                else
                {
                    this.DataSource = new DepartmentEditPage();
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
        /// コントロールを初期化する
        /// </summary>
        private void _InitializeControls()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtOrder.Text = "0";

            btnDelete.Enabled = false;
        }

        /// <summary>
        /// 入力データをチェックする
        /// </summary>
        /// <returns>登録の可否</returns>
        private bool _SetInputData()
        {
            bool ret = true;

            // コントロールからDataSourceでバインドしたオブジェクトへ反映。ControlAttributeでも可能。
            Validate();
            // 部門コード
            if (!String.IsNullOrEmpty(txtCode.Text))
            {
                if (!Regex.IsMatch(txtCode.Text, @"^\d{4}"))
                {
                    MessageBox.Show("コードに数字以外の文字があります", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ret = false;
                }
            }
            else
            {
                MessageBox.Show("コードを入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ret = false;
            }

            // 部門名
            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("名前を入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ret = false;
            }

            // 表示順番
            if (!String.IsNullOrEmpty(txtOrder.Text))
            {
                if (!Regex.IsMatch(txtOrder.Text, @"^\d{1,4}"))
                {
                    MessageBox.Show("表示順番に数字以外の文字があります", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ret = false;
                }
            }
            else
            {
                MessageBox.Show("表示順番を入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ret = false;
            }

            return ret;
        }
    }
}