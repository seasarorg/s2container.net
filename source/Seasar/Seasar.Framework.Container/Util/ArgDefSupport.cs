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

using System.Collections;

namespace Seasar.Framework.Container.Util
{
    /// <summary>
    /// IArgDefの設定をサポートします。
    /// </summary>
    public class ArgDefSupport
    {
        private readonly IList _argDefs = ArrayList.Synchronized(new ArrayList());
        private IS2Container _container;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argDef">IArgDef</param>
        public void AddArgDef(IArgDef argDef)
        {
            if (_container != null)
            {
                argDef.Container = _container;
            }
            _argDefs.Add(argDef);
        }

        /// <summary>
        /// IArgDefの数
        /// </summary>
        public int ArgDefSize
        {
            get { return _argDefs.Count; }
        }

        /// <summary>
        /// 番号を指定してIArgDefを取得します。
        /// </summary>
        /// <param name="index">番号</param>
        /// <returns>IArgDef</returns>
        public IArgDef GetArgDef(int index)
        {
            return (IArgDef) _argDefs[index];
        }

        /// <summary>
        /// S2Container
        /// </summary>
        public IS2Container Container
        {
            set
            {
                _container = value;
                IEnumerator enu = _argDefs.GetEnumerator();
                while (enu.MoveNext())
                {
                    IArgDef argDef = (IArgDef) enu.Current;
                    argDef.Container = value;
                }
            }
        }
    }
}
