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
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Dao.Interceptors;

namespace Seasar.Dao.Pager
{
    public class PagerS2DaoInterceptorWrapper : AbstractInterceptor
    {
        private readonly S2DaoInterceptor _interceptor;

        public PagerS2DaoInterceptorWrapper(S2DaoInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        public override object Invoke(IMethodInvocation invocation)
        {
            PagerContext pagerContext = PagerContext.GetContext();

            IPagerCondition condition = PagerConditionUtil.CreatePagerDefinition(
                (MethodInfo) invocation.Method, invocation.Arguments);
            if (condition == null)
            {
                if (PagerConditionUtil.IsPagerDto(invocation.Arguments))
                {
                    condition = PagerConditionUtil.GetPagerDto(invocation.Arguments);
                }
            }
            pagerContext.PushArgs(condition);
            object ret;
            try
            {
                ret = _interceptor.Invoke(invocation);
            }
            finally
            {
                pagerContext.PopArgs();
            }
            if (condition != null)
            {
                PagerConditionUtil.SetCount((MethodInfo) invocation.Method, invocation.Arguments, condition.Count);
            }
            return ret;
        }
    }
}
