#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class BooleanToIntParameterCollection : MarshalByRefObject, IDataParameterCollection
    {
        private readonly ArrayList _list = new ArrayList();

        internal BooleanToIntParameterCollection()
        {
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get { return _list.Count; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BooleanToIntParameter this[int index]
        {
            get { return (BooleanToIntParameter) _list[index]; }
            set { _list[index] = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BooleanToIntParameter this[string parameterName]
        {
            get
            {
                foreach (BooleanToIntParameter p in _list)
                {
                    if (p.ParameterName.Equals(parameterName))
                    {
                        return p;
                    }
                }
                throw new IndexOutOfRangeException("The specified name does not exist: " + parameterName);
            }
            set
            {
                if (!Contains(parameterName))
                {
                    throw new IndexOutOfRangeException("The specified name does not exist: " + parameterName);
                }
                this[IndexOf(parameterName)] = value;
            }

        }

        int ICollection.Count
        {
            get { return _list.Count; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection.IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        object IList.this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        object IDataParameterCollection.this[string name]
        {
            get { return this[name]; }
            set
            {
                if (!(value is BooleanToIntParameter))
                {
                    throw new InvalidCastException("Only BooleanToIntParameter objects can be used.");
                }
                this[name] = (BooleanToIntParameter) value;
            }

        }

        public int Add(object value)
        {
            if (!(value is BooleanToIntParameter))
            {
                throw new InvalidCastException("The parameter was not an BooleanToIntParameter.");
            }
            Add((BooleanToIntParameter) value);
            return IndexOf(value);
        }

        public BooleanToIntParameter Add(BooleanToIntParameter parameter)
        {
            _list.Add(parameter);
            return parameter;
        }

        int IList.Add(object value)
        {
            if (!(value is IDataParameter))
            {
                throw new InvalidCastException();
            }
            _list.Add(value);
            return _list.IndexOf(value);
        }

        void IList.Clear()
        {
            _list.Clear();
        }

        bool IList.Contains(object value)
        {
            return _list.Contains(value);
        }

        bool IDataParameterCollection.Contains(string value)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                IDataParameter parameter = (IDataParameter) _list[i];
                if (parameter.ParameterName == value)
                {
                    return true;
                }
            }

            return false;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            _list.ToArray().CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        int IList.IndexOf(object value)
        {
            return _list.IndexOf(value);
        }

        int IDataParameterCollection.IndexOf(string name)
        {
            return _list.IndexOf(((IDataParameterCollection) this)[name]);
        }

        void IList.Insert(int index, object value)
        {
            _list.Insert(index, value);
        }

        void IList.Remove(object value)
        {
            _list.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            _list.Remove(_list[index]);
        }

        void IDataParameterCollection.RemoveAt(string name)
        {
            _list.Remove(((IDataParameterCollection) this)[name]);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(object value)
        {
            if (!(value is BooleanToIntParameter))
            {
                throw new InvalidCastException("The parameter was not an BooleanToIntParameter.");
            }
            return Contains(((BooleanToIntParameter) value).ParameterName);
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            if (!(value is BooleanToIntParameter))
            {
                throw new InvalidCastException("The parameter was not an BooleanToIntParameter.");
            }
            return IndexOf(((BooleanToIntParameter) value).ParameterName);
        }

        public int IndexOf(string parameterName)
        {
            for (int i = 0; i < Count; i += 1)
            {
                if (this[i].ParameterName.Equals(parameterName))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, object value)
        {
            _list.Insert(index, value);
        }

        public void Remove(object value)
        {
            _list.Remove(value);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void RemoveAt(string parameterName)
        {
            RemoveAt(IndexOf(parameterName));
        }
    }
}
