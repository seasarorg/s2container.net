#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
        /// MethodDefÇí«â¡ÇµÇ‹Ç∑ÅB
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
        /// IInitMethodDefÇÃêî
        /// </summary>
        public int InitMethodDefSize
        {
            get { return _methodDefs.Count; }
        }

        /// <summary>
        /// î‘çÜÇéwíËÇµÇƒIInitMethodDefÇéÊìæÇµÇ‹Ç∑ÅB
        /// </summary>
        /// <param name="index">IInitMethodDefÇÃî‘çÜ</param>
        /// <returns>IInitMethodDef</returns>
        public IInitMethodDef GetInitMethodDef(int index)
        {
            return (IInitMethodDef) _methodDefs[index];
        }

        /// <summary>
        /// S2Container
        /// </summary>
        public IS2Container Container
        {
            set
            {
                _container = value;
                IEnumerator enu = _methodDefs.GetEnumerator();
                while (enu.MoveNext())
                {
                    IInitMethodDef methodDef = (IInitMethodDef) enu.Current;
                    methodDef.Container = value;
                }
            }
        }
    }
}
