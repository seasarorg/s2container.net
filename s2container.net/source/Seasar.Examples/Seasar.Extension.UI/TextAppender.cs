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
using System.IO;
using System.Windows.Forms;

namespace Seasar.Unit.UI
{
    /// <summary>
    /// テキストボックスに、文字列ストリームを流し込む為のクラスです。
    /// </summary>
    public class TextAppender : StringWriter
    {
        private delegate void WriteEventHandler(string s);

        private readonly TextBoxBase textBox;
        private WriteEventHandler WriteEvent;

        public TextAppender(TextBoxBase textBox)
        {
            this.textBox = textBox;
            this.textBox.HandleCreated += new EventHandler(OnHandleCreated);
            this.textBox.HandleDestroyed += new EventHandler(OnHandleDestroyed);

            WriteEvent = new WriteEventHandler(BufferText);
        }

        private void OnHandleCreated(object sender, EventArgs e)
        {
            textBox.AppendText(base.ToString()); // 既にバッファリングされている文字列を書き込む。
            WriteEvent = new WriteEventHandler(AppendText);
        }

        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            WriteEvent = new WriteEventHandler(DoNothing);
        }

        public override void Write(string s)
        {
            WriteEvent(s);
        }

        private void BufferText(string s)
        {
            base.Write(s);
        }

        private void AppendText(string s)
        {
            textBox.AppendText(s);
        }

        private void DoNothing(string s)
        {
        }

        public override void WriteLine(string s)
        {
            Write(s + base.NewLine);
        }

        public override void Write(char c)
        {
            Write(c.ToString());
        }
    }
}
