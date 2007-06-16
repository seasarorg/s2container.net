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
using System.Collections;
using System.Reflection;
using Seasar.Dao.Impl;

namespace Seasar.Dao.Pager
{
    public class PagingDaoMetaData : DaoMetaDataImpl, IDaoMetaData
    {
        protected override string[] GetArgNames(MethodInfo mi)
        {
            PagerAttribute pageable = PagerConditionUtil.GetPagerAttribute(mi);
            if (pageable == null)
            {
                return base.GetArgNames(mi);
            }
            else
            {
                ArrayList list = new ArrayList();
                ParameterInfo[] parameters = mi.GetParameters();
                for (int i = 0; i < parameters.Length; ++i)
                {
                    if (parameters[i].Name != pageable.LimitParameter && parameters[i].Name != pageable.OffsetParameter)
                    {
                        list.Add(parameters[i].Name);
                    }
                }
                return list.ToArray(typeof(string)) as string[];
            }
        }

        protected override Type[] GetArgTypes(MethodInfo mi)
        {
            PagerAttribute pageable = PagerConditionUtil.GetPagerAttribute(mi);
            if (pageable == null)
            {
                return base.GetArgTypes(mi);
            }
            else
            {
                ArrayList list = new ArrayList();
                ParameterInfo[] parameters = mi.GetParameters();
                for (int i = 0; i < parameters.Length; ++i)
                {
                    if (parameters[i].Name != pageable.LimitParameter && parameters[i].Name != pageable.OffsetParameter)
                    {
                        list.Add(parameters[i].ParameterType);
                    }
                }
                return list.ToArray(typeof(Type)) as Type[];
            }
        }
    }
}