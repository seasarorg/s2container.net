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
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;

namespace Seasar.WindowsExample.Forms
{
    /// <summary>
    /// 部門登録画面
    /// </summary>
    public partial class FrmDepartmentEdit : Form
    {
        /// <summary>
        /// 部門ID
        /// </summary>
        private Nullable<int> _id;

        /// <summary>
        /// 部門登録サービス
        /// </summary>
        private IDepartmentEditService _service;

        /// <summary>
        /// ログ(log4net)
        /// </summary>
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 例外エラーメッセージ書式
        /// </summary>
        private const string EXCEPTION_MSG_FORMAT = "予期できないエラーが発生しました。詳細を確認してください。（{0}）";

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
        public Nullable<int> Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 部門登録サービス
        /// </summary>
        public IDepartmentEditService Service
        {
            get { return _service; }
            set { _service = value; }
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
                if ( MessageBox.Show("本当に登録しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                     DialogResult.No )
                    return;

                DepartmentDto data = new DepartmentDto();
                if ( !_SetInputData(data) ) return;

                if ( _service.ExecUpdate(data) > 0 )
                {
                    _InitializeControls();
                    MessageBox.Show("登録しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new ApplicationException("登録に失敗しました");
                }
            }
            catch ( Exception ex )
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
                if ( MessageBox.Show("本当に削除しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                     DialogResult.No )
                    return;

                if ( _id.HasValue )
                {
                    if ( _service.ExecDelete(_id.Value) > 0 )
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
            catch ( Exception ex )
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
                if ( _id.HasValue )
                {
                    DepartmentDto data = _service.GetData(_id.Value);
                    if ( data != null )
                    {
                        _ShowData(data);
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        throw new ApplicationException("部門データが見つかりませんでした");
                    }
                }
            }
            catch ( Exception ex )
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
        /// 部門データを表示する
        /// </summary>
        /// <param name="data">部門データ</param>
        private void _ShowData(DepartmentDto data)
        {
            txtCode.Text = data.Code;
            txtName.Text = data.Name;
            txtOrder.Text = data.ShowOrder.ToString();
        }

        /// <summary>
        /// 入力データをセットする
        /// </summary>
        /// <param name="data">セット部門先データ</param>
        /// <returns>登録の可否</returns>
        private bool _SetInputData(DepartmentDto data)
        {
            bool ret = true;

            // 部門Id
            if ( _id.HasValue )
                data.Id = _id.Value;
            else
                data.Id = null;

            // 部門コード
            if ( txtCode.Text != "" )
            {
                if ( Regex.IsMatch(txtCode.Text, @"^\d{4}") )
                {
                    data.Code = txtCode.Text;
                }
                else
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
            if ( txtName.Text != "" )
                data.Name = txtName.Text;
            else
            {
                MessageBox.Show("名前を入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ret = false;
            }

            // 表示順番
            if ( txtOrder.Text != "" )
            {
                if ( Regex.IsMatch(txtOrder.Text, @"^\d{1,4}") )
                {
                    data.ShowOrder = Convert.ToInt32(txtOrder.Text);
                }
                else
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