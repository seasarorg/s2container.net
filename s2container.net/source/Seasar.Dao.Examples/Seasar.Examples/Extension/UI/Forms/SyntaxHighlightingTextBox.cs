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
using System.Windows.Forms;

namespace Seasar.Extension.UI.Forms
{
    /// <summary>
    /// SyntaxHighlightingTextBox の概要の説明です。
    /// </summary>
    public class SyntaxHighlightingTextBox : RichTextBox
    {
        //Members exposed via properties
        private readonly HighLightDescriptorCollection _highlightDescriptors = new HighLightDescriptorCollection();

        //Internal use members
        private bool _parsing = false;

        #region 公開プロパティ
        /// <summary>
        /// 強調表示する為の定義体コレクション
        /// </summary>
        public HighLightDescriptorCollection HighlightDescriptors
        {
            get { return _highlightDescriptors; }
        }

        #endregion

        #region Overriden methods

        /// <summary>
        /// テキストが変更された際に、色を付けます。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            Win32.POINT scrollPos = GetScrollPos();
            int cursorLoc = SelectionStart;
            int cursorLen = SelectionLength;

            if (_parsing) return;
            _parsing = true;
            base.OnTextChanged(e);
            // 複数回呼ばれパフォーマンスが低下するのでコメントアウト
            //ProcessHighlighting();
            _parsing = false;

            //Restore cursor and scrollbars location.
            SelectionStart = cursorLoc;
            SelectionLength = cursorLen;
            SetScrollPos(scrollPos);
        }

        public void ProcessHighlighting()
        {
            foreach (HighlightDescriptor desc in HighlightDescriptors)
            {
                int start = -1;
                string target = desc.Token;

                if (desc.DescriptorType == DescriptorType.ToCloseToken)
                {
                    continue; // 開始、終了のあるキーワードには、とりあえず対応しない。
                }
                do
                {
                    start = base.Find(target, ++start, RichTextBoxFinds.WholeWord);
                    if (-1 < start)
                    {
                        base.SelectionFont = desc.Font;
                        base.SelectionColor = desc.Color;
                    }
                } while (-1 < start);
            }
        }

        protected override void WndProc(ref Message m)
        {
            int msg = m.Msg;
            if ((msg == Win32.WM_PAINT || msg == Win32.WM_HSCROLL || msg == Win32.WM_VSCROLL) && _parsing)
            {
                m.Result = IntPtr.Zero;
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        #endregion

        #region Scrollbar positions functions
        /// <summary>
        /// Sends a win32 message to get the scrollbars' position.
        /// </summary>
        /// <returns>a POINT structore containing horizontal and vertical scrollbar position.</returns>
        private unsafe Win32.POINT GetScrollPos()
        {
            Win32.POINT res = new Win32.POINT();
            IntPtr ptr = new IntPtr(&res);
            Win32.SendMessage(Handle, Win32.EM_GETSCROLLPOS, 0, ptr);
            return res;
        }

        /// <summary>
        /// Sends a win32 message to set scrollbars position.
        /// </summary>
        /// <param name="point">a POINT conatining H/Vscrollbar scrollpos.</param>
        private unsafe void SetScrollPos(Win32.POINT point)
        {
            IntPtr ptr = new IntPtr(&point);
            Win32.SendMessage(Handle, Win32.EM_SETSCROLLPOS, 0, ptr);

        }

        #endregion
    }
}
