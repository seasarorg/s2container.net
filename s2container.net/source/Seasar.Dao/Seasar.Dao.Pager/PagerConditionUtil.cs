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
using System.Reflection;

namespace Seasar.Dao.Pager
{
    public sealed class PagerConditionUtil
    {
        private PagerConditionUtil()
        {
        }

        /// <summary>
        /// メソッドの引数にPagerConditionが含まれているかどうかを判定します。
        /// ただし、<seealso cref="IPagerCondition.Limit"/> が
        /// <seealso cref="PagerConditionConstants.NONE_LIMIT"/>の場合はfalseを返します。
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>true/false</returns>
        public static bool IsPagerDto(object[] args)
        {
            IPagerCondition condition = GetPagerDto(args);
            if (condition == null)
            {
                return false;
            }
            if (condition.Limit == PagerConditionConstants.NONE_LIMIT && condition.Offset == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// メソッド引数から<seealso cref="IPagerCondition"/>を取得します。
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>IPagerCondition</returns>
        public static IPagerCondition GetPagerDto(object[] args)
        {
            foreach (object arg in args)
            {
                if (arg is IPagerCondition)
                {
                    return arg as IPagerCondition;
                }
            }
            return null;
        }

        /// <summary>
        /// メソッド定義からページング用メソッド定義を取得します。優先順位は下記になります。
        /// 1. メソッド定義のNonPager属性(Pagear属性の取得を停止)
        /// 2. メソッド定義のPager属性
        /// 3. クラス定義のPager属性
        /// </summary>
        /// <returns>PagerAttribute</returns>
        public static PagerAttribute GetPagerAttribute(MethodInfo mi)
        {
            Attribute nonPager = Attribute.GetCustomAttribute(mi, typeof(NonPagerAttribute));
            if (nonPager != null)
             {
                return null;
            }

            PagerAttribute pager = Attribute.GetCustomAttribute(mi, typeof(PagerAttribute)) as PagerAttribute;
            if (pager == null)
            {
                pager = Attribute.GetCustomAttribute(mi.DeclaringType, typeof(PagerAttribute)) as PagerAttribute;
            }

            return pager;
        }

        internal static void SetCount(MethodInfo mi, object[] args, int count)
        {
            PagerAttribute pager = GetPagerAttribute(mi);
            if (pager == null)
            {
                return;
            }

            // Pager属性値よりページング用引数のインデックスを取得
            ParameterInfo[] parameters = mi.GetParameters();
            int ci = FindParameterIndex(pager.CountParameter, parameters);

            if (ci == -1)
            {
                return;
            }

            args[ci] = count;
        }

        /// <summary>
        /// メソッド定義からページング用メソッド定義を作成。優先順位は下記の通り。
        /// 1. メソッド定義のNonPager属性(Pager属性の取得を停止)
        /// 2. メソッド定義のPager属性
        /// 3. クラス定義のPager属性
        /// </summary>
        public static IPagerCondition CreatePagerDefinition(MethodInfo mi, object[] args)
        {
            // Pager属性が取得できなければnullを返却
            PagerAttribute pager = GetPagerAttribute(mi);
            if (pager == null)
            {
                return null;
            }

            // Pager属性値よりページング用引数のインデックスを取得
            ParameterInfo[] parameters = mi.GetParameters();
            int li = FindParameterIndex(pager.LimitParameter, parameters);
            int oi = FindParameterIndex(pager.OffsetParameter, parameters);
            int ci = FindParameterIndex(pager.CountParameter, parameters);

            // ページング用引数が取得できなかった場合、例外を発生
            if (li == -1)
            {
                throw new PagingParameterDefinitionException(pager.LimitParameter);
            }
            if (parameters[li].ParameterType != typeof(int))
            {
                throw new PagingParameterDefinitionException(pager.LimitParameter);
            }

            if (oi == -1)
            {
                throw new PagingParameterDefinitionException(pager.OffsetParameter);
            }
            if (parameters[oi].ParameterType != typeof(int))
            {
                throw new PagingParameterDefinitionException(pager.OffsetParameter);
            }

            if (ci == -1)
            {
                throw new PagingParameterDefinitionException(pager.OffsetParameter);
            }
            if (parameters[ci].ParameterType.Name != "Int32&" || !parameters[ci].IsOut)
            {
                throw new PagingParameterDefinitionException(pager.OffsetParameter);
            }

            return new DefaultPagerCondition(
                Convert.ToInt32(args[li]),
                Convert.ToInt32(args[oi])
                );
        }

        private static int FindParameterIndex(string parameterName, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameterName == parameters[i].Name)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}