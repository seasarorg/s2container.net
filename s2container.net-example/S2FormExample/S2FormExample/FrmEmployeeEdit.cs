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
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using log4net;
using Seasar.S2FormExample.Logics.Dto;
using Seasar.S2FormExample.Logics.Page;
using Seasar.S2FormExample.Logics.Service;
using Seasar.Windows;
using Seasar.Windows.Attr;

namespace Seasar.S2FormExample.Forms
{
    /// <summary>
    /// 社員登録画面
    /// </summary>
    [ControlModifier("txt", "")]
    [Control("cmbGender", "SelectedValue", "Gender")]
    [Control("dtpEntry", "Value", "Entry")]
    [Control("cmbDepart", "SelectedValue", "Depart")]
    public partial class FrmEmployeeEdit : S2Form
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
        /// 社員ID
        /// </summary>
        private int? _id;

        /// <summary>
        /// 画面登録用サービス
        /// </summary>
        protected IEmployeeEditService service;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmEmployeeEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 社員ID
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// フォームをロードしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEmployeeEdit_Load(object sender, EventArgs e)
        {
            logger.InfoFormat("{0}がロードされました", Name);

            _InitializeControls();
            if (_id.HasValue)
            {
                EmployeeEditPage data = service.GetData(_id.Value);
                if (data != null)
                {
                    DataSource = data;
                    btnDelete.Enabled = true;
                }
                else
                {
                    DataSource = null;
                    throw new ApplicationException("社員データが見つかりませんでした");
                }
            }
            else
            {
                DataSource = new EmployeeEditPage();
            }
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

                EmployeeEditPage data = (EmployeeEditPage) DataSource;
                data.Id = _id;
                if (service.ExecUpdate(data) > 0)
                {
                    _InitializeControls();
                    MessageBox.Show("登録しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    throw new ApplicationException("登録に失敗しました");
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                logger.Error(ex.StackTrace);
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
                logger.Error(ex.StackTrace);
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 閉じたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            logger.InfoFormat("{0}を終了", Name);
            Close();
        }

        /// <summary>
        /// コントロールを初期化する
        /// </summary>
        private void _InitializeControls()
        {
            txtCode.Text = "";
            txtName.Text = "";
            dtpEntry.Value = DateTime.Today;

            _InitializeGenderBox();
            _InializeDepartmentBox();

            btnDelete.Enabled = false;
        }

        /// <summary>
        /// 性別コンボボックスを初期化する
        /// </summary>
        private void _InitializeGenderBox()
        {
            IList<GenderDto> list = service.GetGenderAll();

            cmbGender.DataSource = list;
            cmbGender.ValueMember = "Id";
            cmbGender.DisplayMember = "Name";
            cmbGender.SelectedIndex = 0;
        }

        /// <summary>
        /// 部門コンボボックスを初期化する
        /// </summary>
        private void _InializeDepartmentBox()
        {
            IList<DepartmentDto> list = service.GetDepartmentAll();

            cmbDepart.DataSource = list;
            cmbDepart.ValueMember = "Id";
            cmbDepart.DisplayMember = "Name";
            cmbDepart.SelectedIndex = 0;
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
            // 社員コード
            if (!String.IsNullOrEmpty(txtCode.Text))
            {
                if (!Regex.IsMatch(txtCode.Text, @"^\d{6}"))
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

            // 社員名
            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("名前を入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ret = false;
            }

            return ret;
        }
    }
}