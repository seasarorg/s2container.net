#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using log4net;
using Nullables;
using Seasar.Windows.Utils;
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;

namespace Seasar.WindowsExample.Forms
{
    /// <summary>
    /// 社員登録用フォーム
    /// </summary>
    public class FrmEmployeeEdit : Form
    {
        private Label label3;
        private Label label4;
        private Button btnClose;
        private Button btnDelete;
        private Button btnUpdate;
        private TextBox txtName;
        private TextBox txtCode;
        private Label label5;
        private Label label2;
        private Label label1;
        private Label label6;
        private Label label7;
        private ComboBox cmbGender;
        private DateTimePicker dtpEntry;
        private ComboBox cmbDepart;

        /// <summary>
        /// ログ(log4net)
        /// </summary>
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 社員ID
        /// </summary>
        private NullableInt32 _id;

        /// <summary>
        /// 社員登録用サービス
        /// </summary>
        IEmployeeEditService _service;

        /// <summary>
        /// 例外エラーメッセージ書式
        /// </summary>
        private const string EXCEPTION_MSG_FORMAT = "予期できないエラーが発生しました。詳細を確認してください。（{0}）";

        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private Container components = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmEmployeeEdit()
        {
            //
            // Windows フォーム デザイナ サポートに必要です。
            //
            InitializeComponent();

            //
            // TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
            //
        }

        /// <summary>
        /// 使用されているリソースに後処理を実行します。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if ( disposing )
            {
                if ( components != null )
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード 

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.dtpEntry = new System.Windows.Forms.DateTimePicker();
            this.cmbDepart = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor =
                System.Drawing.Color.FromArgb(( (System.Byte) ( 128 ) ), ( (System.Byte) ( 255 ) ),
                                              ( (System.Byte) ( 128 ) ));
            this.label3.Location = new System.Drawing.Point(8, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(304, 40);
            this.label3.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.BackColor =
                System.Drawing.Color.FromArgb(( (System.Byte) ( 128 ) ), ( (System.Byte) ( 255 ) ),
                                              ( (System.Byte) ( 128 ) ));
            this.label4.Font =
                new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.label4.Location = new System.Drawing.Point(8, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(296, 24);
            this.label4.TabIndex = 4;
            this.label4.Text = "社員";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.btnClose.Location = new System.Drawing.Point(216, 264);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 40);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "閉じる(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.btnDelete.Location = new System.Drawing.Point(112, 264);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(88, 40);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "削除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.btnUpdate.Location = new System.Drawing.Point(8, 264);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(88, 40);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "登録(&R)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtName
            // 
            this.txtName.AutoSize = false;
            this.txtName.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.txtName.Location = new System.Drawing.Point(128, 104);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(176, 24);
            this.txtName.TabIndex = 18;
            this.txtName.Text = "NNNNNNNNNNNNNNNNNN";
            // 
            // txtCode
            // 
            this.txtCode.AutoSize = false;
            this.txtCode.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.txtCode.Location = new System.Drawing.Point(128, 72);
            this.txtCode.MaxLength = 6;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(80, 24);
            this.txtCode.TabIndex = 17;
            this.txtCode.Text = "9999";
            // 
            // label5
            // 
            this.label5.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.label5.Location = new System.Drawing.Point(24, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "性別";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.label2.Location = new System.Drawing.Point(24, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 24);
            this.label2.TabIndex = 15;
            this.label2.Text = "社員名";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.label1.Location = new System.Drawing.Point(24, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "コード";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.label6.Location = new System.Drawing.Point(24, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 24);
            this.label6.TabIndex = 20;
            this.label6.Text = "入社日";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.label7.Location = new System.Drawing.Point(24, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 24);
            this.label7.TabIndex = 21;
            this.label7.Text = "部門";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.cmbGender.Location = new System.Drawing.Point(128, 136);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(96, 23);
            this.cmbGender.TabIndex = 22;
            // 
            // dtpEntry
            // 
            this.dtpEntry.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.dtpEntry.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEntry.Location = new System.Drawing.Point(128, 168);
            this.dtpEntry.Name = "dtpEntry";
            this.dtpEntry.ShowCheckBox = true;
            this.dtpEntry.Size = new System.Drawing.Size(128, 22);
            this.dtpEntry.TabIndex = 23;
            // 
            // cmbDepart
            // 
            this.cmbDepart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepart.Font =
                new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ( (System.Byte) ( 128 ) ));
            this.cmbDepart.Location = new System.Drawing.Point(128, 200);
            this.cmbDepart.Name = "cmbDepart";
            this.cmbDepart.Size = new System.Drawing.Size(96, 23);
            this.cmbDepart.TabIndex = 24;
            // 
            // FrmEmployeeEdit
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(320, 311);
            this.Controls.Add(this.cmbDepart);
            this.Controls.Add(this.dtpEntry);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmEmployeeEdit";
            this.Text = "社員";
            this.Load += new System.EventHandler(this.FrmEmployeeEdit_Load);
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// 社員ID
        /// </summary>
        public NullableInt32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 社員登録用サービス
        /// </summary>
        public IEmployeeEditService Service
        {
            get { return _service; }
            set { _service = value; }
        }

        /// <summary>
        /// フォームをロードしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEmployeeEdit_Load(object sender, System.EventArgs e)
        {
            try
            {
                logger.InfoFormat("{0}がロードされました", Name);

                _InitializeControls();
                if ( _id.HasValue )
                {
                    EmployeeDto data = _service.GetData(_id.Value);
                    if ( data != null )
                    {
                        _ShowData(data);
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        throw new ApplicationException("社員データが見つかりませんでした");
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
        /// 更新ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                if ( MessageBox.Show("本当に登録しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                EmployeeDto data = new EmployeeDto();
                if ( !_SetInputData(data) ) return;

                if ( _service.ExecUpdate(data) > 0 )
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
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 削除ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                if ( MessageBox.Show("本当に削除しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
            catch(Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 閉じるときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            logger.InfoFormat("{0}を終了", Name);
            Close();
        }

        /// <summary>
        /// コントロールを初期化する
        /// </summary>
        public void _InitializeControls()
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
            IList list = _service.GetGenderAll();

            DataSet ds =
                Converter.ConvertPONOToDataSet(typeof (GenderDto), list);
            cmbGender.DataSource = ds.Tables[typeof (GenderDto).Name];
            cmbGender.ValueMember = "Id";
            cmbGender.DisplayMember = "Name";
            cmbGender.SelectedIndex = 0;
        }

        /// <summary>
        /// 部門コンボボックスを初期化する
        /// </summary>
        private void _InializeDepartmentBox()
        {
            IList list = _service.GetDepartmentAll();
            DataSet ds =
                Converter.ConvertPONOToDataSet(typeof (DepartmentDto), list);
            cmbDepart.DataSource = ds.Tables[typeof (DepartmentDto).Name];
            cmbDepart.ValueMember = "Id";
            cmbDepart.DisplayMember = "Name";
            cmbDepart.SelectedIndex = 0;
        }
        
        /// <summary>
        /// 入力データをセットする
        /// </summary>
        /// <param name="data">セット社員データ</param>
        /// <returns>登録の可否</returns>
        private bool _SetInputData(EmployeeDto data)
        {
            bool ret = true;
            
            if ( _id.HasValue )
                data.Id = _id;
            else 
                data.Id = null;
            
            // 社員コード
            if (txtCode.Text != "")
            {
                if ( Regex.IsMatch(txtCode.Text, @"^\d{6}") )
                {
                    data.Code = txtCode.Text;
                }
                else
                {
                    MessageBox.Show("コードに数字以外の文字があります", Text, MessageBoxButtons.OK,  MessageBoxIcon.Exclamation);
                    ret = false;
                }
            }
            else
            {
                MessageBox.Show("コードを入力してください", Text, MessageBoxButtons.OK,  MessageBoxIcon.Exclamation);
                ret = false;
            }
            
            // 社員名
            if ( txtName.Text != "" )
            {
                data.Name = txtName.Text;
            }
            else
            {
                MessageBox.Show("名前を入力してください", Text, MessageBoxButtons.OK,  MessageBoxIcon.Exclamation);
                ret = false;
            }
            
            // 性別
            data.Gender = Convert.ToInt32(cmbGender.SelectedValue);
            
            // 入社日
            if ( dtpEntry.Checked )
                data.EntryDay = new NullableDateTime(dtpEntry.Value);
            else
                data.EntryDay = null;
            // 部門
            data.DeptNo = (NullableInt32) cmbDepart.SelectedValue;

            return ret;
        }
        
        /// <summary>
        /// データを表示する
        /// </summary>
        /// <param name="data">表示データ</param>
        public void _ShowData(EmployeeDto data)
        {
            txtCode.Text = data.Code;
            txtName.Text = data.Name;
            cmbGender.SelectedValue = data.Gender;
            if ( data.EntryDay.HasValue )
                dtpEntry.Value = data.EntryDay.Value;
            else
                dtpEntry.Checked = false;
            cmbDepart.SelectedValue = data.DeptNo;
        }
    }
}