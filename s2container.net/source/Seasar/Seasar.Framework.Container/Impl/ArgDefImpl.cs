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
using Seasar.Framework.Container.Util;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Impl
{
    /// <summary>
    /// 引数を定義します。
    /// </summary>
    public class ArgDefImpl : IArgDef
    {
        private object value;
        private IS2Container container;
        private string expression;
        private Type argType;
        private IComponentDef childComponentDef;
        private readonly MetaDefSupport metaDefSupport = new MetaDefSupport();

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
            this.value = value;
        }

        #region ArgDef メンバ

        public object Value
        {
            get
            {
                if (expression != null)
                {
                    if (container.HasComponentDef(expression))
                    {
                        return container.GetComponent(expression);
                    }
                    else if (IsCharacterString(expression))
                    {
                        return JScriptUtil.Evaluate(expression, container);
                    }
                    else if (expression.IndexOf(".") > 0)
                    {
                        int lastIndex = expression.LastIndexOf(".");
                        string enumTypeName = expression.Substring(0, lastIndex);
                        Type enumType = ClassUtil.ForName(enumTypeName,
                            AppDomain.CurrentDomain.GetAssemblies());
                        if (enumType != null && enumType.IsEnum)
                        {
                            return Enum.Parse(enumType, expression.Substring(lastIndex + 1));
                        }

                        Type classType = ClassUtil.ForName(expression,
                            AppDomain.CurrentDomain.GetAssemblies());
                        if (classType != null && classType.IsClass)
                        {
                            return classType;
                        }

                        return JScriptUtil.Evaluate(expression, container);
                    }
                    else
                    {
                        return JScriptUtil.Evaluate(expression, container);
                    }
                }
                if (childComponentDef != null)
                {
                    return childComponentDef.GetComponent(argType);
                }
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public IS2Container Container
        {
            get { return container; }
            set
            {
                container = value;
                if (childComponentDef != null)
                {
                    childComponentDef.Container = value;
                }
                metaDefSupport.Container = value;
            }
        }

        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public IComponentDef ChildComponentDef
        {
            set
            {
                if (container != null)
                {
                    value.Container = container;
                }
                childComponentDef = value;
            }
        }

        public Type ArgType
        {
            get { return argType; }
            set { argType = value; }
        }

        #endregion

        #region IMetaDefAware メンバ

        public void AddMetaDef(IMetaDef metaDef)
        {
            metaDefSupport.AddMetaDef(metaDef);
        }

        public int MetaDefSize
        {
            get { return metaDefSupport.MetaDefSize; }
        }

        public IMetaDef GetMetaDef(int index)
        {
            return metaDefSupport.GetMetaDef(index);
        }

        public IMetaDef GetMetaDef(string name)
        {
            return metaDefSupport.GetMetaDef(name);
        }

        public IMetaDef[] GetMetaDefs(string name)
        {
            return metaDefSupport.GetMetaDefs(name);
        }

        #endregion

        private bool IsCharacterString(string str)
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
