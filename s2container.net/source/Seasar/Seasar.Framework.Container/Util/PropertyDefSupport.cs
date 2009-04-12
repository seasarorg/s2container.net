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

using System.Collections;
using System.Collections.Specialized;

namespace Seasar.Framework.Container.Util
{
    public sealed class PropertyDefSupport
    {
        private readonly Hashtable _propertyDefs = Hashtable.Synchronized(CollectionsUtil.CreateCaseInsensitiveHashtable());
        private IS2Container _container;

        /// <summary>
        /// PropertyDefを追加します。
        /// </summary>
        /// <param name="propertyDef">IPropertyDef</param>
        public void AddPropertyDef(IPropertyDef propertyDef)
        {
            if (_container != null)
            {
                propertyDef.Container = _container;
            }
            _propertyDefs.Add(propertyDef.PropertyName, propertyDef);
        }

        /// <summary>
        /// IPropertyDefの数
        /// </summary>
        public int PropertyDefSize
        {
            get { return _propertyDefs.Count; }
        }

        /// <summary>
        /// 番号を指定してIPropertyDefを取得します。
        /// </summary>
        /// <param name="index">IPropertyDefの番号</param>
        /// <returns>IPropertyDef</returns>
        public IPropertyDef GetPropertyDef(int index)
        {
            int i = 0;
            IEnumerator enu = _propertyDefs.Values.GetEnumerator();
            while (enu.MoveNext())
            {
                if (i++ == index)
                {
                    return (IPropertyDef) enu.Current;
                }
            }
            return null;
        }

        /// <summary>
        /// 名前を指定してIPropertyDefを取得します。
        /// </summary>
        /// <param name="propertyName">IPropertyDefの名前</param>
        /// <returns>IPropertyDef</returns>
        public IPropertyDef GetPropertyDef(string propertyName)
        {
            return (IPropertyDef) _propertyDefs[propertyName];
        }

        /// <summary>
        /// 指定された名前のIPropertyDefを持っているか判定します。
        /// </summary>
        /// <param name="propertyName">IPropertyDefの名前</param>
        /// <returns>存在するならtrue</returns>
        public bool HasPropertyDef(string propertyName)
        {
            return _propertyDefs.ContainsKey(propertyName);
        }

        /// <summary>
        /// S2Container
        /// </summary>
        public IS2Container Container
        {
            set
            {
                _container = value;
                IEnumerator enu = _propertyDefs.Values.GetEnumerator();
                while (enu.MoveNext())
                {
                    IPropertyDef propertyDef = (IPropertyDef) enu.Current;
                    propertyDef.Container = value;
                }
            }
        }
    }
}
