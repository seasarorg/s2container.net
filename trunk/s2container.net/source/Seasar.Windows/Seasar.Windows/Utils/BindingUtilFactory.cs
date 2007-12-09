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

using System;
using System.Data;

namespace Seasar.Windows.Seasar.Windows.Utils
{
    /// <summary>
    /// GridViewコントロールにバインドする実装クラス
    /// </summary>
    public sealed class BindingUtilFactory
    {
        private static volatile BindingUtilFactory _factory;
        private static object _lockRoot = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private BindingUtilFactory()
        {
            ;
        }

        /// <summary>
        /// インスタンス
        /// </summary>
        public static BindingUtilFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    lock (_lockRoot)
                    {
                        if (_factory == null)
                            _factory = new BindingUtilFactory();
                    }
                }
                return _factory;
            }
        }

        /// <summary>
        /// GridViewコントロールにバインドするクラスを生成する
        /// </summary>
        /// <param name="propertyType">プロパティType</param>
        /// <returns>バインドするクラス</returns>
        public IBindingUtil Create(Type propertyType)
        {
            if (propertyType.IsArray)
            {
                return (new BindingArrayUtil());
            }
            else if (propertyType == typeof (DataTable))
            {
                return (new BindingDataTableUtil());
            }
            else if (propertyType.IsGenericType)
            {
                if (propertyType.IsInterface)
                {
                    if (propertyType.Name == "IList")
                        return (new BindingGenericListUtil());
                    if (propertyType.Name == "IEnumerable")
                        return (new BindingArrayUtil());

                    throw new InvalidCastException(String.Format(SWFMessages.FSWF0006, propertyType.Name));
                }
                else
                {
                    Type interfaceType = propertyType.GetInterface("IList");
                    if (interfaceType != null)
                        return (new BindingGenericListUtil());

                    interfaceType = propertyType.GetInterface("IEnumerable");
                    if (interfaceType != null)
                        return (new BindingArrayUtil());

                    throw new InvalidCastException(String.Format(SWFMessages.FSWF0006, propertyType.Name));
                }
            }
            else
            {
                if (propertyType.IsInterface)
                {
                    if (propertyType.Name == "IBindingList")
                        return (new BindingBindingListUtil());
                    if (propertyType.Name == "IList")
                        return (new BindingListUtil());
                    if (propertyType.Name == "IEnumerable")
                        return (new BindingArrayUtil());

                    throw new InvalidCastException(String.Format(SWFMessages.FSWF0006, propertyType.Name));
                }
                else
                {
                    Type interfaceType = propertyType.GetInterface("IBindingList");
                    if (interfaceType != null)
                        return (new BindingBindingListUtil());

                    interfaceType = propertyType.GetInterface("IList");
                    if (interfaceType != null)
                        return (new BindingListUtil());

                    interfaceType = propertyType.GetInterface("IEnumerable");
                    if (interfaceType != null)
                        return (new BindingArrayUtil());

                    throw new InvalidCastException(String.Format(SWFMessages.FSWF0006, propertyType.Name));
                }
            }
        }
    }
}