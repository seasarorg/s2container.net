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
    public sealed class InitMethodDefSupport
    {
        private readonly IList _methodDefs = ArrayList.Synchronized(new ArrayList());
        private IS2Container _container;

        /// <summary>
        /// MethodDef��ǉ����܂��B
        /// </summary>
        /// <param name="methodDef">MethodDef</param>
        public void AddInitMethodDef(IInitMethodDef methodDef)
        {
            if (_container != null)
            {
                methodDef.Container = _container;
            }
            _methodDefs.Add(methodDef);
        }

        /// <summary>
        /// IInitMethodDef�̐�
        /// </summary>
        public int InitMethodDefSize => _methodDefs.Count;

        /// <summary>
        /// �ԍ���w�肵��IInitMethodDef��擾���܂��B
        /// </summary>
        /// <param name="index">IInitMethodDef�̔ԍ�</param>
        /// <returns>IInitMethodDef</returns>
        public IInitMethodDef GetInitMethodDef(int index) => (IInitMethodDef) _methodDefs[index];

        /// <summary>
        /// S2Container
        /// </summary>
        public IS2Container Container
        {
            set
            {
                _container = value;
                var enu = _methodDefs.GetEnumerator();
                while (enu.MoveNext())
                {
                    var methodDef = (IInitMethodDef) enu.Current;
                    methodDef.Container = value;
                }
            }
        }
    }
}
