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

using System;
using System.Reflection;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class RelationPropertyTypeImpl
        : PropertyTypeImpl, IRelationPropertyType
    {
        protected int relationNo;
        protected string[] myKeys;
        protected string[] yourKeys;
        protected IBeanMetaData beanMetaData;

        public RelationPropertyTypeImpl(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
        }

        public RelationPropertyTypeImpl(PropertyInfo propertyInfo, int relationNo,
            string[] myKeys, string[] yourKeys, IBeanMetaData beanMetaData)
            : base(propertyInfo)
        {
            this.relationNo = relationNo;
            this.myKeys = myKeys;
            this.yourKeys = yourKeys;
            this.beanMetaData = beanMetaData;
        }

        #region IRelationPropertyType �����o

        public virtual int RelationNo => relationNo;

        public virtual int KeySize
        {
            get
            {
                if (myKeys.Length > 0)
                    return myKeys.Length;
                else
                    return beanMetaData.PrimaryKeySize;
            }
        }

        public virtual string GetMyKey(int index)
        {
            if (myKeys.Length > 0)
                return myKeys[index];
            else
                return beanMetaData.GetPrimaryKey(index);
        }

        public virtual string GetYourKey(int index)
        {
            if (yourKeys.Length > 0)
                return yourKeys[index];
            else
                return beanMetaData.GetPrimaryKey(index);
        }

        public virtual bool IsYourKey(string columnName)
        {
            for (var i = 0; i < KeySize; ++i)
            {
                if (String.Compare(columnName, GetYourKey(i), StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }
            return false;
        }

        public virtual IBeanMetaData BeanMetaData => beanMetaData;

        #endregion
    }
}
