namespace Seasar.Quill.Examples
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.EmpButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.enameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.empNoTextBox = new System.Windows.Forms.TextBox();
            this.quillControl1 = new Seasar.Quill.QuillControl();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quillControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // EmpButton
            // 
            this.EmpButton.Location = new System.Drawing.Point(276, 20);
            this.EmpButton.Name = "EmpButton";
            this.EmpButton.Size = new System.Drawing.Size(76, 31);
            this.EmpButton.TabIndex = 3;
            this.EmpButton.Text = "社員検索";
            this.EmpButton.UseVisualStyleBackColor = true;
            this.EmpButton.Click += new System.EventHandler(this.EmpButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.enameTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.empNoTextBox);
            this.groupBox1.Controls.Add(this.EmpButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 100);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "社員検索";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "検索結果：社員名を表示します";
            // 
            // enameTextBox
            // 
            this.enameTextBox.Location = new System.Drawing.Point(202, 66);
            this.enameTextBox.Name = "enameTextBox";
            this.enameTextBox.ReadOnly = true;
            this.enameTextBox.Size = new System.Drawing.Size(150, 19);
            this.enameTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "社員コードを入力してください";
            // 
            // empNoTextBox
            // 
            this.empNoTextBox.Location = new System.Drawing.Point(186, 26);
            this.empNoTextBox.MaxLength = 4;
            this.empNoTextBox.Name = "empNoTextBox";
            this.empNoTextBox.Size = new System.Drawing.Size(84, 19);
            this.empNoTextBox.TabIndex = 4;
            // 
            // quillControl1
            // 
            this.quillControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("quillControl1.BackgroundImage")));
            this.quillControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.quillControl1.Location = new System.Drawing.Point(393, 124);
            this.quillControl1.Name = "quillControl1";
            this.quillControl1.Size = new System.Drawing.Size(21, 23);
            this.quillControl1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(375, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "© Copyright The Seasar Project and the others 2007, all rights reserved.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 149);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.quillControl1);
            this.Name = "Form1";
            this.Text = "Quill.Examples";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quillControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private QuillControl quillControl1;
        private System.Windows.Forms.Button EmpButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox empNoTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox enameTextBox;
        private System.Windows.Forms.Label label3;


    }
}

