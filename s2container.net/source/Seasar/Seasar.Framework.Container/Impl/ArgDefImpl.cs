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
using Seasar.Framework.Container.Util;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Impl
{
    /// <summary>
    /// 引数を定義します。
    /// </summary>
    public class ArgDefImpl : IArgDef
    {
        private object _value;
        private IS2Container _container;
        private IComponentDef _childComponentDef;
        private readonly MetaDefSupport _metaDefSupport = new MetaDefSupport();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArgDefImpl()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">値</param>
        public ArgDefImpl(object value)
        {
            _value = value;
        }

        #region ArgDef メンバ

        public object Value
        {
            get
            {
                if (Expression != null)
                {
                    if (_container.HasComponentDef(Expression))
                    {
                        return _container.GetComponent(Expression);
                    }
                    else if (_IsCharacterString(Expression))
                    {
                        return JScriptUtil.Evaluate(Expression, _container);
                    }
                    else if (Expression.IndexOf('.') > 0)
                    {
                        var lastIndex = Expression.LastIndexOf('.');
                        var enumTypeName = Expression.Substring(0, lastIndex);
                        var enumType = ClassUtil.ForName(enumTypeName,
                            AppDomain.CurrentDomain.GetAssemblies());
                        if (enumType != null && enumType.IsEnum)
                        {
                            return Enum.Parse(enumType, Expression.Substring(lastIndex + 1));
                        }

                        var classType = ClassUtil.ForName(Expression,
                            AppDomain.CurrentDomain.GetAssemblies());
                        if (classType != null && classType.IsClass)
                        {
                            return classType;
                        }

                        return JScriptUtil.Evaluate(Expression, _container);
                    }
                    else
                    {
                        return JScriptUtil.Evaluate(Expression, _container);
                    }
                }
                if (_childComponentDef != null)
                {
                    return _childComponentDef.GetComponent(ArgType);
                }
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public IS2Container Container
        {
            get { return _container; }
            set
            {
                _container = value;
                if (_childComponentDef != null)
                {
                    _childComponentDef.Container = value;
                }
                _metaDefSupport.Container = value;
            }
        }

        public string Expression { get; set; }

        public IComponentDef ChildComponentDef
        {
            set
            {
                if (_container != null)
                {
                    value.Container = _container;
                }
                _childComponentDef = value;
            }
        }

        public Type ArgType { get; set; }

        #endregion

        #region IMetaDefAware メンバ

        public void AddMetaDef(IMetaDef metaDef)
        {
            _metaDefSupport.AddMetaDef(metaDef);
        }

        public int MetaDefSize => _metaDefSupport.MetaDefSize;

        public IMetaDef GetMetaDef(int index) => _metaDefSupport.GetMetaDef(index);

        public IMetaDef GetMetaDef(string name) => _metaDefSupport.GetMetaDef(name);

        public IMetaDef[] GetMetaDefs(string name) => _metaDefSupport.GetMetaDefs(name);

        #endregion

        private bool _IsCharacterString(string str)
        {
            if (str == null)
            {
                return false;
            }
            if (str.StartsWith("\"") && str.EndsWith("\"") && str.Length >= 2)
            {
                return true;
            }
            return false;
        }
    }
}
