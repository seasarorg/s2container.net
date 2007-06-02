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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Seasar.Quill.Util;

namespace Seasar.Quill
{
    /// <summary>
    /// 親コンテナにDIを有効にする為のコントロールクラス
    /// </summary>
    public partial class QuillControl : UserControl, ISupportInitialize
    {
        /// <summary>
        /// QuillControlを初期化するコンストラクタ
        /// </summary>
        public QuillControl()
        {
            // デフォルトで非表示の状態とする
            this.Visible = false;

            // コンポーネントの初期化処理を行う
            InitializeComponent();
        }

        #region ISupportInitialize メンバ

        /// <summary>
        /// コントロールの初期化が開始されると呼び出されるメソッド
        /// </summary>
        public void BeginInit()
        {
        }

        /// <summary>
        /// コントロールの初期化が終了すると呼び出されるメソッド
        /// </summary>
        /// <remarks>
        /// QuillInjectorを使用してDIを行う。
        /// </remarks>
        public void EndInit()
        {
            if (DesignMode)
            {
                // デザインモードの場合はDIは行わない
                return;
            }

            Debug.WriteLine(MessageUtil.GetSimpleMessage("IQLL0001",
                new object[] { DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") } ));

            // QuilInjectorのインスタンスを取得する
            QuillInjector injector = QuillInjector.GetInstance();

            // 親コンテナに対してDIを行う
            injector.Inject(Parent);

            Debug.WriteLine(MessageUtil.GetSimpleMessage("IQLL0002",
                            new object[] { DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") }));
        }

        #endregion
    }
}
