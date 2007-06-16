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

using System.Reflection;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class RelationPropertyTypeImpl
        : PropertyTypeImpl, IRelationPropertyType
    {
        protected int _relationNo;
        protected string[] _myKeys;
        protected string[] _yourKeys;
        protected IBeanMetaData _beanMetaData;

        public RelationPropertyTypeImpl(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
        }

        public RelationPropertyTypeImpl(PropertyInfo propertyInfo, int relationNo,
            string[] myKeys, string[] yourKeys, IBeanMetaData beanMetaData)
            : base(propertyInfo)
        {
            _relationNo = relationNo;
            _myKeys = myKeys;
            _yourKeys = yourKeys;
            _beanMetaData = beanMetaData;
        }

        #region IRelationPropertyType ƒƒ“ƒo

        public int RelationNo
        {
            get { return _relationNo; }
        }

        public int KeySize
        {
            get
            {
                if (_myKeys.Length > 0)
                    return _myKeys.Length;
                else
                    return _beanMetaData.PrimaryKeySize;
            }
        }

        public string GetMyKey(int index)
        {
            if (_myKeys.Length > 0)
                return _myKeys[index];
            else
                return _beanMetaData.GetPrimaryKey(index);
        }

        public string GetYourKey(int index)
        {
            if (_yourKeys.Length > 0)
                return _yourKeys[index];
            else
                return _beanMetaData.GetPrimaryKey(index);
        }

        public bool IsYourKey(string columnName)
        {
            for (int i = 0; i < KeySize; ++i)
            {
                if (string.Compare(columnName, GetYourKey(i), true) == 0)
                    return true;
            }
            return false;
        }

        public IBeanMetaData BeanMetaData
        {
            get { return _beanMetaData; }
        }

        #endregion

    }
}
