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
    public sealed class AspectDefSupport
    {
        private readonly IList _aspectDefs = ArrayList.Synchronized(new ArrayList());
        private IS2Container _container;

        /// <summary>
        /// IAspectDef��ǉ����܂��B
        /// </summary>
        /// <param name="aspectDef"></param>
        public void AddAspectDef(IAspectDef aspectDef)
        {
            if (_container != null)
            {
                aspectDef.Container = _container;
            }
            _aspectDefs.Add(aspectDef);
        }

        /// <summary>
        /// IAspectDef�̐�
        /// </summary>
        public int AspectDefSize => _aspectDefs.Count;

        /// <summary>
        /// �ԍ���w�肵��IAspectDef��Ԃ��܂��B
        /// </summary>
        /// <param name="index">�ԍ�</param>
        /// <returns>IAspectDef</returns>
        public IAspectDef GetAspectDef(int index) => (IAspectDef) _aspectDefs[index];

        /// <summary>
        /// S2Container
        /// </summary>
        public IS2Container Container
        {
            set
            {
                _container = value;
                var enu = _aspectDefs.GetEnumerator();
                while (enu.MoveNext())
                {
                    ((IAspectDef) enu.Current).Container = value;
                }
            }
        }
    }
}
