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

using System.Windows.Forms;
using Seasar.Framework.Container;

namespace Seasar.Windows
{
    /// <summary>
    /// DIコンテナ用ApplicationContext派生クラス
    /// </summary>
    public class S2ApplicationContext : ApplicationContext
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        private IS2Container _container;

        /// <summary>
        /// DIコンテナ
        /// </summary>
        public IS2Container DIContainer
        {
            get { return _container; }
            set { _container = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">DIコンテナ</param>
        public S2ApplicationContext(IS2Container container)
        {
            _container = container;
        }

        /// <summary>
        /// スレッド終了処理
        /// </summary>
        protected override void ExitThreadCore()
        {
            // クリーンアップ処理
            _container.Destroy();

            // メインスレッドの終了処理
            base.ExitThreadCore();
        }
    }
}