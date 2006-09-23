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

using System.Windows.Forms;

namespace Seasar.WindowsExample.Forms
{
	/// <summary>
	/// メインメニュー画面
	/// </summary>
	public class FrmMainMenu : Form
	{
	    private Button btnEmployee;
	    private Button btnDepartment;
	    private Button button1;
	    private Button button2;
	    private Button btnClose;

	    /// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// 画面ディスパッチャー
        /// </summary>
        IFormDispatcher _dispatcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public FrmMainMenu(IFormDispatcher dispatcher)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
            _dispatcher = dispatcher;
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.btnEmployee = new System.Windows.Forms.Button();
            this.btnDepartment = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEmployee
            // 
            this.btnEmployee.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnEmployee.Location = new System.Drawing.Point(8, 16);
            this.btnEmployee.Name = "btnEmployee";
            this.btnEmployee.Size = new System.Drawing.Size(144, 56);
            this.btnEmployee.TabIndex = 0;
            this.btnEmployee.Text = "社員マスタ編集(&E)";
            this.btnEmployee.Click += new System.EventHandler(this.btnEmployee_Click);
            // 
            // btnDepartment
            // 
            this.btnDepartment.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnDepartment.Location = new System.Drawing.Point(152, 16);
            this.btnDepartment.Name = "btnDepartment";
            this.btnDepartment.Size = new System.Drawing.Size(144, 56);
            this.btnDepartment.TabIndex = 1;
            this.btnDepartment.Text = "部門マスタ編集(&D)";
            this.btnDepartment.Click += new System.EventHandler(this.btnDepartment_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 56);
            this.button1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(152, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 56);
            this.button2.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
            this.btnClose.Location = new System.Drawing.Point(8, 128);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(288, 56);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "閉じる(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmMainMenu
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(306, 199);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDepartment);
            this.Controls.Add(this.btnEmployee);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMainMenu";
            this.Text = "メインメニュー";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FrmMainMenu_Closing);
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// 社員一覧を表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmployee_Click(object sender, System.EventArgs e)
        {
            _dispatcher.ShowDataList();
        }

        /// <summary>
        /// 部門一覧を表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDepartment_Click(object sender, System.EventArgs e)
        {
            _dispatcher.ShowMasterList();
        }

        /// <summary>
        /// フォームを閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// フォームを閉じるまえの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMainMenu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ( MessageBox.Show("本当に終了しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

	}
}
