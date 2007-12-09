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

namespace Seasar.Framework.Container.Impl
{
    public class PropertyDefImpl : ArgDefImpl, IPropertyDef
    {
        private readonly string _propertyName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName"></param>
        public PropertyDefImpl(string propertyName)
        {
            _propertyName = propertyName;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public PropertyDefImpl(string propertyName, object value)
            : base(value)
        {
            _propertyName = propertyName;
        }

        #region PropertyDef メンバ

        public string PropertyName
        {
            get { return _propertyName; }
        }

        #endregion
    }
}
