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

namespace Seasar.Framework.Xml.Impl
{
    public class AttributesImpl : IAttributes
    {
        private readonly Hashtable _values = new Hashtable();
        private readonly IList _qNames = new ArrayList();

        public void AddAttribute(string qName, string value)
        {
            _qNames.Add(qName);
            _values[qName] = value;
        }

        #region IAttributes ÉÅÉìÉo

        public string this[string qName]
        {
            get { return (string) _values[qName]; }
        }

        string IAttributes.this[int index]
        {
            get { return (string) _values[_qNames[index]]; }
        }

        public int Count
        {
            get { return _qNames.Count; }
        }

        public string GetQName(int index)
        {
            return (string) _qNames[index];
        }

        #endregion
    }
}
