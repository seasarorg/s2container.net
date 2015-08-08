#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
    /// IMetaDefの設定をサポートします
    /// </summary>
    public sealed class MetaDefSupport
    {
        private readonly IList _metaDefs = ArrayList.Synchronized(new ArrayList());
        private IS2Container _container;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MetaDefSupport()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">S2Container</param>
        public MetaDefSupport(IS2Container container)
        {
            _container = container;
        }

        /// <summary>
        /// IMetaDefを追加します。
        /// </summary>
        /// <param name="metaDef"></param>
        public void AddMetaDef(IMetaDef metaDef)
        {
            if (_container != null)
            {
                metaDef.Container = _container;
            }
            _metaDefs.Add(metaDef);
        }

        /// <summary>
        /// IMetaDefの数
        /// </summary>
        public int MetaDefSize => _metaDefs.Count;

        /// <summary>
        /// 番号を指定してIMetaDefを取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns>IMetaDef</returns>
        public IMetaDef GetMetaDef(int index) => (IMetaDef) _metaDefs[index];

        /// <summary>
        /// 名前を指定してIMetaDefを取得します。
        /// </summary>
        /// <param name="name">IMetaDefの名前</param>
        /// <returns>IMetaDef</returns>
        public IMetaDef GetMetaDef(string name)
        {
            var enu = _metaDefs.GetEnumerator();
            while (enu.MoveNext())
            {
                var metaDef = (IMetaDef) enu.Current;
                if ((name == null && metaDef.Name == null) || name != null
                    && name.ToLower().CompareTo(metaDef.Name.ToLower()) == 0)
                {
                    return metaDef;
                }
            }
            return null;
        }

        /// <summary>
        /// 名前を指定してIMetaDefの配列を取得します。
        /// </summary>
        /// <param name="name">IMetaDefの名前</param>
        /// <returns>IMetaDefの配列</returns>
        public IMetaDef[] GetMetaDefs(string name)
        {
            var defs = new ArrayList();
            var enu = _metaDefs.GetEnumerator();
            while (enu.MoveNext())
            {
                var metaDef = (IMetaDef) enu.Current;
                if ((name == null && metaDef.Name == null) || name != null
                    && name.ToLower().CompareTo(metaDef.Name.ToLower()) == 0)
                {
                    defs.Add(metaDef);
                }
            }
            return (IMetaDef[]) defs.ToArray(typeof(IMetaDef));
        }

        /// <summary>
        /// S2Container
        /// </summary>
        public IS2Container Container
        {
            set
            {
                _container = value;
                var enu = _metaDefs.GetEnumerator();
                while (enu.MoveNext())
                {
                    var metaDef = (IMetaDef) enu.Current;
                    metaDef.Container = value;
                }
            }
        }
    }
}
