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

using Seasar.Framework.Aop;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Aop.Impl;

namespace Seasar.Framework.Container.AutoRegister
{
    /// <summary>
    /// アスペクトを自動登録するためのクラスです。
    /// </summary>
    public class AspectAutoRegister : AbstractComponentTargetAutoRegister
    {
        private IMethodInterceptor interceptor;
        private string pointcut;

        /// <summary>
        /// インターセプタを設定します。
        /// </summary>
        public IMethodInterceptor Interceptor
        {
            set { interceptor = value; }
        }

        /// <summary>
        /// ポイントカットを設定します。
        /// </summary>
        public string Pointcut
        {
            set { pointcut = value; }
        }

        /// <summary>
        /// コンポーネント定義にアスペクトを登録します。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        protected override void Register(IComponentDef componentDef)
        {
            IAspectDef aspectDef;

            if (pointcut != null)
            {
                string[] methodNames = pointcut.Split(new char[] { ',' });
                aspectDef = new AspectDefImpl(interceptor,
                    new PointcutImpl(methodNames));
            }
            else
            {
                aspectDef = new AspectDefImpl(interceptor);
            }

            componentDef.AddAspeceDef(aspectDef);
        }
    }
}
