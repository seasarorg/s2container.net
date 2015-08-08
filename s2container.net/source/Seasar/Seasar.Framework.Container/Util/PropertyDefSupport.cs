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
using System.Collections.Specialized;

namespace Seasar.Framework.Container.Util
{
    public sealed class PropertyDefSupport
    {
        private readonly Hashtable _propertyDefs = Hashtable.Synchronized(CollectionsUtil.CreateCaseInsensitiveHashtable());
        private IS2Container _container;

        /// <summary>
        /// PropertyDef��ǉ����܂��B
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
        /// IPropertyDef�̐�
        /// </summary>
        public int PropertyDefSize => _propertyDefs.Count;

        /// <summary>
        /// �ԍ���w�肵��IPropertyDef��擾���܂��B
        /// </summary>
        /// <param name="index">IPropertyDef�̔ԍ�</param>
        /// <returns>IPropertyDef</returns>
        public IPropertyDef GetPropertyDef(int index)
        {
            var i = 0;
            var enu = _propertyDefs.Values.GetEnumerator();
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
        /// ���O��w�肵��IPropertyDef��擾���܂��B
        /// </summary>
        /// <param name="propertyName">IPropertyDef�̖��O</param>
        /// <returns>IPropertyDef</returns>
        public IPropertyDef GetPropertyDef(string propertyName) => (IPropertyDef) _propertyDefs[propertyName];

        /// <summary>
        /// �w�肳�ꂽ���O��IPropertyDef������Ă��邩���肵�܂��B
        /// </summary>
        /// <param name="propertyName">IPropertyDef�̖��O</param>
        /// <returns>���݂���Ȃ�true</returns>
        public bool HasPropertyDef(string propertyName) => _propertyDefs.ContainsKey(propertyName);

        /// <summary>
        /// S2Container
        /// </summary>
        public IS2Container Container
        {
            set
            {
                _container = value;
                var enu = _propertyDefs.Values.GetEnumerator();
                while (enu.MoveNext())
                {
                    var propertyDef = (IPropertyDef) enu.Current;
                    propertyDef.Container = value;
                }
            }
        }
    }
}
