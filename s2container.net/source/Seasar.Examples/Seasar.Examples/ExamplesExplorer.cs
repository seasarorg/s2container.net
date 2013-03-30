#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Drawing;
using System.Windows.Forms;
using Seasar.Unit.UI;
using Seasar.Unit.UI.Forms;

namespace Seasar.Examples
{
    public class ExamplesExplorer : Form
    {
        private readonly ExamplesContext _examplesContext;
        private readonly TextAppender _resultAppender;
        private readonly TextAppender _diconAppender;
        private readonly TextAppender _codeAppender;

        private TreeView MainTreeView;
        private TabControl MainTabControl;
        private TabPage ResultConsole;
        private TabPage DiconConsole;
        private TabPage CodeConsole;
        private TextBox ResultView;
        private SyntaxHighlightingTextBox DiconView;
        private SyntaxHighlightingTextBox CodeView;
        private ContextMenu FontContextMenu;
        private MenuItem ScalingUp;
        private MenuItem ScalingDown;
        private MenuItem SelectFont;
        private Splitter Splitter;
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private Container components = null;

        public ExamplesExplorer()
        {
            //
            // Windows フォーム デザイナ サポートに必要です。
            //
            InitializeComponent();
            _examplesContext = new ExamplesContext();
            _examplesContext.ResultViewChanged += new ResultViewChangedEventHandler(ResultViewChanged);

            _resultAppender = new TextAppender(ResultView);
            Console.SetOut(_resultAppender);
            Console.SetError(_resultAppender);

            _diconAppender = new TextAppender(DiconView);
            _codeAppender = new TextAppender(CodeView);

            SetUpHighlighting(new Font("MS UI Gothic", 10, FontStyle.Bold));
        }

        private void SetUpHighlighting(Font font)
        {
            // TODO 色付けの定義がズラズラとココにあるのはチトイマイチ…。
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
        }

        private void addHighlightDescriptors(string[] keywords, Color color, Font font)
        {
            foreach (string word in keywords)
            {
                DiconView.HighlightDescriptors.Add(new HighlightDescriptor(word, color, font, DescriptorType.Word, DescriptorRecognition.WholeWord));
            }
        }

        /// <summary>
        /// 使用されているリソースに後処理を実行します。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                _resultAppender.Close();
                _diconAppender.Close();
                _codeAppender.Close();
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
            this.MainTreeView = new TreeView();
            this.MainTabControl = new TabControl();
            this.ResultConsole = new TabPage();
            this.ResultView = new TextBox();
            this.DiconConsole = new TabPage();
            this.CodeConsole = new TabPage();
            this.FontContextMenu = new ContextMenu();
            this.ScalingUp = new MenuItem();
            this.ScalingDown = new MenuItem();
            this.SelectFont = new MenuItem();
            this.Splitter = new Splitter();
            this.DiconView = new SyntaxHighlightingTextBox();
            this.CodeView = new SyntaxHighlightingTextBox();
            this.MainTabControl.SuspendLayout();
            this.ResultConsole.SuspendLayout();
            this.DiconConsole.SuspendLayout();
            this.CodeConsole.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTreeView
            // 
            this.MainTreeView.Dock = DockStyle.Left;
            this.MainTreeView.Location = new Point(0, 0);
            this.MainTreeView.Name = "MainTreeView";
            this.MainTreeView.Size = new Size(168, 365);
            this.MainTreeView.TabIndex = 0;
            this.MainTreeView.AfterSelect += new TreeViewEventHandler(this.MainTreeView_AfterSelect);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.ResultConsole);
            this.MainTabControl.Controls.Add(this.DiconConsole);
            this.MainTabControl.Controls.Add(this.CodeConsole);
            this.MainTabControl.Dock = DockStyle.Fill;
            this.MainTabControl.Location = new Point(168, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new Size(564, 365);
            this.MainTabControl.TabIndex = 1;
            // 
            // ResultConsole
            // 
            this.ResultConsole.Controls.Add(this.ResultView);
            this.ResultConsole.Location = new Point(4, 21);
            this.ResultConsole.Name = "ResultConsole";
            this.ResultConsole.Size = new Size(556, 340);
            this.ResultConsole.TabIndex = 0;
            this.ResultConsole.Text = "ResultConsole";
            // 
            // ResultView
            // 
            this.ResultView.Dock = DockStyle.Fill;
            this.ResultView.Location = new Point(0, 0);
            this.ResultView.Multiline = true;
            this.ResultView.Name = "ResultView";
            this.ResultView.ScrollBars = ScrollBars.Horizontal;
            this.ResultView.Size = new Size(556, 340);
            this.ResultView.TabIndex = 2;
            this.ResultView.MouseDown += new MouseEventHandler(this.View_MouseDown);
            // 
            // DiconConsole
            // 
            this.DiconConsole.Controls.Add(this.DiconView);
            this.DiconConsole.Location = new Point(4, 21);
            this.DiconConsole.Name = "DiconConsole";
            this.DiconConsole.Size = new Size(556, 340);
            this.DiconConsole.TabIndex = 1;
            this.DiconConsole.Text = "DiconConsole";
            // 
            // CodeConsole
            // 
            this.CodeConsole.Controls.Add(this.CodeView);
            this.CodeConsole.Location = new Point(4, 21);
            this.CodeConsole.Name = "CodeConsole";
            this.CodeConsole.Size = new Size(556, 340);
            this.CodeConsole.TabIndex = 2;
            this.CodeConsole.Text = "CodeConsole";
            // 
            // FontContextMenu
            // 
            this.FontContextMenu.MenuItems.AddRange(new MenuItem[] {
            this.ScalingUp,
            this.ScalingDown,
            this.SelectFont});
            // 
            // ScalingUp
            // 
            this.ScalingUp.Index = 0;
            this.ScalingUp.Text = "拡大";
            this.ScalingUp.Click += new EventHandler(this.ScalingUp_Click);
            // 
            // ScalingDown
            // 
            this.ScalingDown.Index = 1;
            this.ScalingDown.Text = "縮小";
            this.ScalingDown.Click += new EventHandler(this.ScalingDown_Click);
            // 
            // SelectFont
            // 
            this.SelectFont.Index = 2;
            this.SelectFont.Text = "フォント選択";
            this.SelectFont.Click += new EventHandler(this.SelectFont_Click);
            // 
            // Splitter
            // 
            this.Splitter.Location = new Point(0, 0);
            this.Splitter.Name = "Splitter";
            this.Splitter.Size = new Size(3, 340);
            this.Splitter.TabIndex = 3;
            this.Splitter.TabStop = false;
            // 
            // DiconView
            // 
            this.DiconView.Dock = DockStyle.Fill;
            this.DiconView.Location = new Point(0, 0);
            this.DiconView.Name = "DiconView";
            this.DiconView.Size = new Size(556, 340);
            this.DiconView.TabIndex = 1;
            this.DiconView.Text = "";
            this.DiconView.WordWrap = false;
            this.DiconView.MouseDown += new MouseEventHandler(this.View_MouseDown);
            // 
            // CodeView
            // 
            this.CodeView.Dock = DockStyle.Fill;
            this.CodeView.Location = new Point(0, 0);
            this.CodeView.Name = "CodeView";
            this.CodeView.Size = new Size(556, 340);
            this.CodeView.TabIndex = 0;
            this.CodeView.Text = "";
            this.CodeView.WordWrap = false;
            this.CodeView.MouseDown += new MouseEventHandler(this.View_MouseDown);
            // 
            // ExamplesExplorer
            // 
            this.AutoScaleBaseSize = new Size(5, 12);
            this.ClientSize = new Size(732, 365);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.Splitter);
            this.Controls.Add(this.MainTreeView);
            this.Name = "ExamplesExplorer";
            this.Text = "ExamplesExplorer";
            this.MainTabControl.ResumeLayout(false);
            this.ResultConsole.ResumeLayout(false);
            this.ResultConsole.PerformLayout();
            this.DiconConsole.ResumeLayout(false);
            this.CodeConsole.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public void AddExamples(string title, IList exampless)
        {
            TreeNode rootNode = new TreeNode(title);
            foreach (IExamplesHandler handler in exampless)
            {
                rootNode.Nodes.Add(new ExecutableTreeNode(handler));
            }
            MainTreeView.Nodes.Add(rootNode);
        }

        private void ResultViewChanged(Control ctrl)
        {
            SuspendLayout();
            ResultConsole.SuspendLayout();

            ResultConsole.Controls.Clear();
            ctrl.Dock = DockStyle.Fill;
            ctrl.Size = ResultView.Size;
            ResultConsole.Controls.Add(ctrl);

            ResultConsole.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void MainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                ResultView.Clear();
                CodeView.Clear();
                DiconView.Clear();
                if (e.Node is ExecutableTreeNode)
                {
                    ExecutableTreeNode etn = e.Node as ExecutableTreeNode;
                    ResultViewChanged(ResultView); // 何はともあれデフォルトの表示は出来る様にする。
                    etn.ExamplesHandler.Main(_examplesContext);
                    etn.ExamplesHandler.AppendDicon(_diconAppender);
                    DiconView.ProcessHighlighting();
                    etn.ExamplesHandler.AppendCode(_codeAppender);
                    CodeView.ProcessHighlighting();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #region font settings

        private void View_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Right == e.Button)
            {
                FontContextMenu.Show(sender as Control, new Point(e.X, e.Y));
            }
        }

        private void ScalingUp_Click(object sender, EventArgs e)
        {
            Control c = GetSourceControl(sender);
            if (c != null)
            {
                Scale(new Font(c.Font.FontFamily, c.Font.Size + 1));
            }
        }

        private void ScalingDown_Click(object sender, EventArgs e)
        {
            Control c = GetSourceControl(sender);
            if (c != null)
            {
                float size = c.Font.Size - 1;
                if (5 < size)
                {
                    Scale(new Font(c.Font.FontFamily, size));
                }
            }
        }

        private Control GetSourceControl(object sender)
        {
            Control result = null;
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                result = mi.GetContextMenu().SourceControl;
            }
            return result;
        }

        private void SelectFont_Click(object sender, EventArgs e)
        {
            FontDialog dialog = new FontDialog();
            dialog.AllowVerticalFonts = true;
            dialog.ScriptsOnly = true;
            dialog.ShowEffects = false;

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Scale(dialog.Font);
            }
        }

        private void Scale(Font font)
        {
            SuspendLayout();
            foreach (TabPage tp in MainTabControl.Controls)
            {
                tp.Controls[0].SuspendLayout();
            }

            foreach (TabPage tp in MainTabControl.Controls)
            {
                tp.Controls[0].Font = font;
            }

            Refresh();
            SetUpHighlighting(new Font(font.FontFamily, font.Size, FontStyle.Bold));
            DiconView.ProcessHighlighting();
            CodeView.ProcessHighlighting();

            foreach (TabPage tp in MainTabControl.Controls)
            {
                tp.Controls[0].ResumeLayout();
            }
            ResumeLayout();
        }

        #endregion
    }
}
