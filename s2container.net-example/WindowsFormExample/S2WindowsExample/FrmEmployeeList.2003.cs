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
using System.Windows.Forms;
using log4net;
using Nullables;
using Seasar.Windows.Utils;
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;

namespace Seasar.WindowsExample.Forms
{
	/// <summary>
	/// 社員リスト用フォーム
	/// </summary>
	public class FrmEmployeeList : Form
	{
	    private Button btnNew;
	    private Label label2;
	    private Label label1;
	    private Button btnClose;
	    private Button btnOutput;
	    private SaveFileDialog dlgSave;
        private DataGrid gridList;

	    /// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
	    private Container components = null;

        /// <summary>
        /// 社員リストサービス
        /// </summary>
        IEmployeeListService _service;

	    /// <summary>
        /// 画面ディスパッチャー
        /// </summary>
        IFormDispatcher _dispatcher;

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
        /// <param name="dispatcher">画面ディスパッチャー</param>
		public FrmEmployeeList(IFormDispatcher dispatcher)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
            _dispatcher = dispatcher;

            // グリッドを設定
            _InitGrid();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.btnNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.gridList = new System.Windows.Forms.DataGrid();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnNew.Location = new System.Drawing.Point(160, 184);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(96, 40);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "新規(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(192)));
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.label2.Location = new System.Drawing.Point(24, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "社員一覧";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(192)));
            this.label1.Location = new System.Drawing.Point(24, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 40);
            this.label1.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnClose.Location = new System.Drawing.Point(368, 184);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 40);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "閉じる(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnOutput.Location = new System.Drawing.Point(264, 184);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(96, 40);
            this.btnOutput.TabIndex = 9;
            this.btnOutput.Text = "出力(&O)";
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // gridList
            // 
            this.gridList.DataMember = "";
            this.gridList.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridList.Location = new System.Drawing.Point(16, 16);
            this.gridList.Name = "gridList";
            this.gridList.ReadOnly = true;
            this.gridList.Size = new System.Drawing.Size(448, 152);
            this.gridList.TabIndex = 10;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // FrmEmployeeList
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(486, 237);
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmEmployeeList";
            this.Text = "社員リスト";
            this.Load += new System.EventHandler(this.FrmEmployeeList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// 社員リストサービス
        /// </summary>
	    public IEmployeeListService Service
	    {
	        get { return _service; }
	        set { _service = value; }
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
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                if ( dlgSave.ShowDialog(this) == DialogResult.OK )
                {
                    if ( _service.OutputCSV(dlgSave.FileName) > 0 )
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
        /// グリッドをダブルクリックしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridList_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                DataSet ds = (DataSet)gridList.DataSource;
                DataRow row = ds.Tables[0].Rows[gridList.CurrentRowIndex];
                logger.Debug("ID:" + row["Id"]);
                
                NullableInt32 id = (NullableInt32) row["Id"];

                _dispatcher.ShowDataEdit(id);

                _ShowList();

            }
            catch ( Exception ex )
            {
                MessageBox.Show(this, ex.Message, Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// グリッドを初期化する
        /// </summary>
        private void _InitGrid()
        {
            try
            {
                // Gridに表示用テーブルを作成する。
                // MappingNameはDTOのプロパティ名を使用すること。
                // PONOから生成されるDataSetではDTOのプロパティ名を使用しているから。

                DataGridTableStyle ts1 = new DataGridTableStyle();
                ts1.MappingName = typeof (EmployeeDto).Name;

                DataGridColumnStyle style1 = new DataGridTextBoxColumn();
                style1.MappingName = "Code";
                style1.HeaderText = "コード";
                style1.Width = 100;
                ts1.GridColumnStyles.Add(style1);

                DataGridColumnStyle style2 = new DataGridTextBoxColumn();
                style2.MappingName = "Name";
                style2.HeaderText = "名前 ";
                style2.Width = 150;
                ts1.GridColumnStyles.Add(style2);

                DataGridColumnStyle style3 = new DataGridTextBoxColumn();
                style3.MappingName = "DeptName";
                style3.HeaderText = "部門 ";
                style3.Width = 150;
                ts1.GridColumnStyles.Add(style3);

                gridList.TableStyles.Add(ts1);

                DataTable dt = new DataTable(typeof (EmployeeDto).Name);
                dt.Columns.Add(new DataColumn("Code"));
                dt.Columns.Add(new DataColumn("Name"));
                dt.Columns.Add(new DataColumn("DeptName"));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                gridList.SetDataBinding(ds, typeof (EmployeeDto).Name);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT ,ex.Message), Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Gridに表示する
        /// </summary>
        private void _ShowList()
        {
            IList list = _service.GetAll();
            
            DataSet ds
                = Converter.ConvertPONOToDataSet(typeof ( EmployeeDto ), list);
            gridList.SetDataBinding(ds, typeof(EmployeeDto).Name);

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
