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
	/// 部門リスト用フォーム
	/// </summary>
	public class FrmDepartmentList : Form
	{
	    private Button btnClose;
	    private Label label1;
	    private Label label2;
	    private DataGrid gridList;
	    private Button btnNew;

        /// <summary>
        /// ログ(log4net)
        /// </summary>
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 部門リストサービス
        /// </summary>
        private IDepartmentListService _service;

        /// <summary>
        /// 画面ディスパッチャー
        /// </summary>
        private IFormDispatcher _dispatcher;

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
		public FrmDepartmentList(IFormDispatcher dispatcher)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//

            _dispatcher = dispatcher;

            // グリッドを初期化する
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
            this.btnClose = new System.Windows.Forms.Button();
            this.gridList = new System.Windows.Forms.DataGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnClose.Location = new System.Drawing.Point(368, 216);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "閉じる(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gridList
            // 
            this.gridList.DataMember = "";
            this.gridList.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridList.Location = new System.Drawing.Point(24, 16);
            this.gridList.Name = "gridList";
            this.gridList.ReadOnly = true;
            this.gridList.Size = new System.Drawing.Size(440, 184);
            this.gridList.TabIndex = 0;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
            this.label1.Location = new System.Drawing.Point(24, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 40);
            this.label1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.label2.Location = new System.Drawing.Point(24, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "部門一覧";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnNew.Location = new System.Drawing.Point(264, 216);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(96, 40);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "新規(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // FrmDepartmentList
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(488, 269);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmDepartmentList";
            this.Text = "部門リスト";
            this.Load += new System.EventHandler(this.FrmDepartmentList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// 部門リストサービス
        /// </summary>
	    public IDepartmentListService Service
	    {
	        get { return _service; }
	        set { _service = value; }
	    }

	    /// <summary>
        /// フォームをロードしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDepartmentList_Load(object sender, System.EventArgs e)
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
        private void btnNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                _dispatcher.ShowMasterEdit(null);
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
        /// 閉じるボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
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

                _dispatcher.ShowMasterEdit(id);

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
	            ts1.MappingName = typeof (DepartmentDto).Name;

	            DataGridColumnStyle style1 = new DataGridTextBoxColumn();
	            style1.MappingName = "Code";
	            style1.HeaderText = "コード";
	            style1.Width = 100;
	            ts1.GridColumnStyles.Add(style1);

	            DataGridColumnStyle style2 = new DataGridTextBoxColumn();
	            style2.MappingName = "Name";
	            style2.HeaderText = "名前 ";
	            style2.Width = 230;
	            ts1.GridColumnStyles.Add(style2);

	            gridList.TableStyles.Add(ts1);

	            DataTable dt = new DataTable(typeof (DepartmentDto).Name);
	            dt.Columns.Add(new DataColumn("Code"));
	            dt.Columns.Add(new DataColumn("Name"));

	            DataSet ds = new DataSet();
	            ds.Tables.Add(dt);
	            gridList.SetDataBinding(ds, typeof (DepartmentDto).Name);
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
                = Converter.ConvertPONOToDataSet(typeof ( DepartmentDto ), list);
            gridList.SetDataBinding(ds, typeof(DepartmentDto).Name);

        }
	}
}
