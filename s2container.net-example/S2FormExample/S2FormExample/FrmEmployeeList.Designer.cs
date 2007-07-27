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

namespace Seasar.S2FormExample.Forms
{
    partial class FrmEmployeeList
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEmployeeList));
            this.gridList = new System.Windows.Forms.DataGridView();
            this.ColumnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDepart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEntryDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeptNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.quillControl1 = new Seasar.Quill.QuillControl();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGenderId = new System.Windows.Forms.TextBox();
            this.lblGenderName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.quillControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridList
            // 
            this.gridList.AllowUserToAddRows = false;
            this.gridList.AllowUserToDeleteRows = false;
            this.gridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCode,
            this.ColumnName,
            this.ColumnDepart,
            this.ColumnId,
            this.ColumnGender,
            this.ColumnEntryDay,
            this.ColumnDeptNo,
            this.ColumnDepartment});
            this.gridList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridList.Location = new System.Drawing.Point(31, 55);
            this.gridList.MultiSelect = false;
            this.gridList.Name = "gridList";
            this.gridList.ReadOnly = true;
            this.gridList.RowTemplate.Height = 21;
            this.gridList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridList.Size = new System.Drawing.Size(457, 146);
            this.gridList.TabIndex = 0;
            this.gridList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridList_CellDoubleClick);
            // 
            // ColumnCode
            // 
            this.ColumnCode.DataPropertyName = "Code";
            this.ColumnCode.HeaderText = "コード";
            this.ColumnCode.Name = "ColumnCode";
            this.ColumnCode.ReadOnly = true;
            this.ColumnCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            this.ColumnName.FillWeight = 150F;
            this.ColumnName.HeaderText = "名前";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnName.Width = 150;
            // 
            // ColumnDepart
            // 
            this.ColumnDepart.DataPropertyName = "DeptName";
            this.ColumnDepart.FillWeight = 150F;
            this.ColumnDepart.HeaderText = "部門";
            this.ColumnDepart.Name = "ColumnDepart";
            this.ColumnDepart.ReadOnly = true;
            this.ColumnDepart.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDepart.Width = 150;
            // 
            // ColumnId
            // 
            this.ColumnId.DataPropertyName = "Id";
            this.ColumnId.HeaderText = "ID";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            this.ColumnId.Visible = false;
            // 
            // ColumnGender
            // 
            this.ColumnGender.DataPropertyName = "Gender";
            this.ColumnGender.HeaderText = "Gender";
            this.ColumnGender.Name = "ColumnGender";
            this.ColumnGender.ReadOnly = true;
            this.ColumnGender.Visible = false;
            // 
            // ColumnEntryDay
            // 
            this.ColumnEntryDay.DataPropertyName = "EntryDay";
            this.ColumnEntryDay.HeaderText = "EntryDay";
            this.ColumnEntryDay.Name = "ColumnEntryDay";
            this.ColumnEntryDay.ReadOnly = true;
            this.ColumnEntryDay.Visible = false;
            // 
            // ColumnDeptNo
            // 
            this.ColumnDeptNo.DataPropertyName = "DeptNo";
            this.ColumnDeptNo.HeaderText = "DeptNo";
            this.ColumnDeptNo.Name = "ColumnDeptNo";
            this.ColumnDeptNo.ReadOnly = true;
            this.ColumnDeptNo.Visible = false;
            // 
            // ColumnDepartment
            // 
            this.ColumnDepartment.DataPropertyName = "Department";
            this.ColumnDepartment.HeaderText = "Department";
            this.ColumnDepartment.Name = "ColumnDepartment";
            this.ColumnDepartment.ReadOnly = true;
            this.ColumnDepartment.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClose.Location = new System.Drawing.Point(398, 227);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 46);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "閉じる(&C)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOutput.Location = new System.Drawing.Point(302, 227);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(90, 46);
            this.btnOutput.TabIndex = 15;
            this.btnOutput.Text = "出力(&O)";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNew.Location = new System.Drawing.Point(206, 227);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(90, 46);
            this.btnNew.TabIndex = 14;
            this.btnNew.Text = "新規(&N)";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(31, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 46);
            this.label1.TabIndex = 17;
            this.label1.Text = "社員一覧";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // quillControl1
            // 
            this.quillControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("quillControl1.BackgroundImage")));
            this.quillControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.quillControl1.Location = new System.Drawing.Point(4, 249);
            this.quillControl1.Name = "quillControl1";
            this.quillControl1.Size = new System.Drawing.Size(21, 23);
            this.quillControl1.TabIndex = 18;
            this.quillControl1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "性別";
            // 
            // txtGenderId
            // 
            this.txtGenderId.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtGenderId.Location = new System.Drawing.Point(75, 28);
            this.txtGenderId.Name = "txtGenderId";
            this.txtGenderId.Size = new System.Drawing.Size(43, 19);
            this.txtGenderId.TabIndex = 20;
            this.txtGenderId.Leave += new System.EventHandler(this.txtGenderId_Leave);
            // 
            // lblGenderName
            // 
            this.lblGenderName.AutoSize = true;
            this.lblGenderName.Location = new System.Drawing.Point(124, 31);
            this.lblGenderName.Name = "lblGenderName";
            this.lblGenderName.Size = new System.Drawing.Size(35, 12);
            this.lblGenderName.TabIndex = 21;
            this.lblGenderName.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "01:男性　02:女性　99:全員";
            // 
            // FrmEmployeeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 285);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblGenderName);
            this.Controls.Add(this.txtGenderId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.quillControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.gridList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmEmployeeList";
            this.Text = "社員一覧";
            this.Load += new System.EventHandler(this.FrmEmployeeList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.quillControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDepart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEntryDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeptNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDepartment;
        private Seasar.Quill.QuillControl quillControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGenderId;
        private System.Windows.Forms.Label lblGenderName;
        private System.Windows.Forms.Label label3;
    }
}