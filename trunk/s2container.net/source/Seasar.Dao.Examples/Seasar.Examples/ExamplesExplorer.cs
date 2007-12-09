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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Seasar.Extension.UI;
using Seasar.Extension.UI.Forms;
using Seasar.Framework.Util;

namespace Seasar.Examples
{
    /// <summary>
    /// ExamplesExplorer の概要の説明です。
    /// </summary>
    public class ExamplesExplorer : System.Windows.Forms.Form
    {
        private ExamplesContext examplesContext;
        private TextAppender ResultAppender;
        private TextAppender DiconAppender;
        private TextAppender CodeAppender;

        private System.Windows.Forms.TreeView MainTreeView;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage ResultConsole;
        private System.Windows.Forms.TabPage DiconConsole;
        private System.Windows.Forms.TabPage CodeConsole;
        private System.Windows.Forms.TextBox ResultView;
        private SyntaxHighlightingTextBox DiconView;
        private SyntaxHighlightingTextBox CodeView;
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ExamplesExplorer()
        {
            //
            // Windows フォーム デザイナ サポートに必要です。
            //
            InitializeComponent();
            this.examplesContext = new ExamplesContext();
            this.examplesContext.ResultViewChanged += new ResultViewChangedEventHandler(this.ResultViewChanged);
            
            ResultAppender = new TextAppender(ResultView);
            Console.SetOut(ResultAppender);
            Console.SetError(ResultAppender);

            DiconAppender = new TextAppender(DiconView);
            
            // TODO 色付けの定義がズラズラとココにあるのはチトイマイチ…。
            Font font = new Font("MS UI Gothic", 10,FontStyle.Bold);
            string[] keywords_tags = {"components", "include", "component", "description",
                                        "arg", "property", "meta", "initMethod", "destroyMethod", "aspect" };
            addHighlightDescriptors(keywords_tags, Color.Blue, font);

            string[] keywords_attributs = {"path", "instance", "class", "name",
                                         "autoBinding", "getter", "pointcut"};
            addHighlightDescriptors(keywords_attributs, Color.Magenta, font);

            string[] keywords_literals = {"singleton", "prototype", "outer", "request", "session",
                                          "auto", "constructor", "property", "none",
                                          "true", "false"};
            addHighlightDescriptors(keywords_literals, Color.Maroon, font);
            
            DiconView.HighlightDescriptors.Add(new HighlightDescriptor("<!--", "-->", Color.Green, font, DescriptorType.ToCloseToken, DescriptorRecognition.StartsWith));

            CodeAppender = new TextAppender(CodeView);
        }

        private void addHighlightDescriptors(string[] keywords, Color color, Font font)
        {
            foreach(string word in keywords) 
            {
                DiconView.HighlightDescriptors.Add(new HighlightDescriptor(word, color, font, DescriptorType.Word, DescriptorRecognition.WholeWord));
            }
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
                ResultAppender.Close();
                DiconAppender.Close();
                CodeAppender.Close();
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
            this.MainTreeView = new System.Windows.Forms.TreeView();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ResultConsole = new System.Windows.Forms.TabPage();
            this.ResultView = new System.Windows.Forms.TextBox();
            this.DiconConsole = new System.Windows.Forms.TabPage();
            this.DiconView = new Seasar.Extension.UI.Forms.SyntaxHighlightingTextBox();
            this.CodeConsole = new System.Windows.Forms.TabPage();
            this.CodeView = new Seasar.Extension.UI.Forms.SyntaxHighlightingTextBox();
            this.MainTabControl.SuspendLayout();
            this.ResultConsole.SuspendLayout();
            this.DiconConsole.SuspendLayout();
            this.CodeConsole.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTreeView
            // 
            this.MainTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainTreeView.ImageIndex = -1;
            this.MainTreeView.Location = new System.Drawing.Point(0, 0);
            this.MainTreeView.Name = "MainTreeView";
            this.MainTreeView.SelectedImageIndex = -1;
            this.MainTreeView.Size = new System.Drawing.Size(168, 365);
            this.MainTreeView.TabIndex = 0;
            this.MainTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MainTreeView_AfterSelect);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.ResultConsole);
            this.MainTabControl.Controls.Add(this.DiconConsole);
            this.MainTabControl.Controls.Add(this.CodeConsole);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(168, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(564, 365);
            this.MainTabControl.TabIndex = 1;
            // 
            // ResultConsole
            // 
            this.ResultConsole.Controls.Add(this.ResultView);
            this.ResultConsole.Location = new System.Drawing.Point(4, 21);
            this.ResultConsole.Name = "ResultConsole";
            this.ResultConsole.Size = new System.Drawing.Size(556, 340);
            this.ResultConsole.TabIndex = 0;
            this.ResultConsole.Text = "ResultConsole";
            // 
            // ResultView
            // 
            this.ResultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultView.Location = new System.Drawing.Point(0, 0);
            this.ResultView.Multiline = true;
            this.ResultView.Name = "ResultView";
            this.ResultView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ResultView.Size = new System.Drawing.Size(556, 340);
            this.ResultView.TabIndex = 2;
            this.ResultView.Text = "";
            // 
            // DiconConsole
            // 
            this.DiconConsole.Controls.Add(this.DiconView);
            this.DiconConsole.Location = new System.Drawing.Point(4, 21);
            this.DiconConsole.Name = "DiconConsole";
            this.DiconConsole.Size = new System.Drawing.Size(556, 340);
            this.DiconConsole.TabIndex = 1;
            this.DiconConsole.Text = "DiconConsole";
            // 
            // DiconView
            // 
            this.DiconView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DiconView.Location = new System.Drawing.Point(0, 0);
            this.DiconView.Name = "DiconView";
            this.DiconView.Size = new System.Drawing.Size(556, 340);
            this.DiconView.TabIndex = 1;
            this.DiconView.Text = "ここに.diconファイルが出力される感じで。\n色分け出来ると見やすくて凄く良いんだけど…。";
            this.DiconView.WordWrap = false;
            // 
            // CodeConsole
            // 
            this.CodeConsole.Controls.Add(this.CodeView);
            this.CodeConsole.Location = new System.Drawing.Point(4, 21);
            this.CodeConsole.Name = "CodeConsole";
            this.CodeConsole.Size = new System.Drawing.Size(556, 340);
            this.CodeConsole.TabIndex = 2;
            this.CodeConsole.Text = "CodeConsole";
            // 
            // CodeView
            // 
            this.CodeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeView.Location = new System.Drawing.Point(0, 0);
            this.CodeView.Name = "CodeView";
            this.CodeView.Size = new System.Drawing.Size(556, 340);
            this.CodeView.TabIndex = 0;
            this.CodeView.Text = "ここにソースコードが出力される感じで。\n１つのnamespaceに全クラス、全インターフェース入れてしまう感じ。\nよって、テキストファイル１つをそのまま表示すれば" +
                "良いかな。";
            this.CodeView.WordWrap = false;
            // 
            // ExamplesExplorer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(732, 365);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.MainTreeView);
            this.Name = "ExamplesExplorer";
            this.Text = "ExamplesExplorer";
            this.MainTabControl.ResumeLayout(false);
            this.ResultConsole.ResumeLayout(false);
            this.DiconConsole.ResumeLayout(false);
            this.CodeConsole.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public void AddExampless(string title, IList exampless) 
        {
            TreeNode rootNode = new TreeNode(title);
            foreach(IExamplesHandler handler in exampless) 
            {
                rootNode.Nodes.Add(new ExecutableTreeNode(handler));
            }
            this.MainTreeView.Nodes.Add(rootNode);
        }

        private void ResultViewChanged(Control ctrl) 
        {
            this.ResultConsole.SuspendLayout();
            this.SuspendLayout();

            this.ResultConsole.Controls.Clear();
            this.ResultConsole.Controls.Add(ctrl);

            this.ResultConsole.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void MainTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try 
            {
                this.ResultView.Clear();
                this.CodeView.Clear();
                this.DiconView.Clear();
                if(e.Node is ExecutableTreeNode) 
                {
                    ExecutableTreeNode etn = e.Node as ExecutableTreeNode;
                    etn.ExamplesHandler.Main(examplesContext);
                    etn.ExamplesHandler.AppendDicon(this.DiconAppender);
                    this.DiconView.ProcessHighlighting();
                    etn.ExamplesHandler.AppendCode(this.CodeAppender);
                    this.CodeView.ProcessHighlighting();
                }

            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

    }
}
