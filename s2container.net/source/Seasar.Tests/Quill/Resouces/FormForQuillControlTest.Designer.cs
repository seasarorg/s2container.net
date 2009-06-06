#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

namespace Seasar.Tests.Quill.Resouces
{
    partial class FormForQuillControlTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormForQuillControlTest));
            this.testQuillControl = new Seasar.Quill.QuillControl();
            ((System.ComponentModel.ISupportInitialize)(this.testQuillControl)).BeginInit();
            this.SuspendLayout();
            // 
            // testQuillControl
            // 
            this.testQuillControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("testQuillControl.BackgroundImage")));
            this.testQuillControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.testQuillControl.Location = new System.Drawing.Point(54, 96);
            this.testQuillControl.Name = "testQuillControl";
            this.testQuillControl.Size = new System.Drawing.Size(21, 23);
            this.testQuillControl.TabIndex = 0;
            this.testQuillControl.Visible = false;
            // 
            // FormForQuillControlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.testQuillControl);
            this.Name = "FormForQuillControlTest";
            this.Text = "FormForQuillControlTest";
            this.Load += new System.EventHandler(this.testQuillControlForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.testQuillControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Seasar.Quill.QuillControl testQuillControl;
    }
}