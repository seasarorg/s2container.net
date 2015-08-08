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
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// フィールド（インスタンス変数）情報を取り扱うユーティリティ
    /// </summary>
    public static class FieldUtil
    {
        /// <summary>
        /// 読み取り専用か判定する
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsReadOnly(FieldInfo field)
        {
            return (field.IsInitOnly || field.IsLiteral);
        }

        /// <summary>
        /// オブジェクトのプロパティに値をセットする
        /// </summary>
        /// <param name="target">対象オブジェクト</param>
        /// <param name="targetType">対象オブジェクト型</param>
        /// <param name="fieldName">プロパティ名</param>
        /// <param name="value">セットする値</param>
        /// <param name="fieldType">プロパティ型</param>
        public static void SetValue(object target, Type targetType, string fieldName, Type fieldType, object value)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (String.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException(nameof(fieldName));

            const CSharpBinderFlags binderFlags = CSharpBinderFlags.None;
            const CSharpArgumentInfoFlags argumentFlags = CSharpArgumentInfoFlags.None;

            var binder = Binder.SetMember(binderFlags, fieldName, targetType,
                new[]
                {
                    CSharpArgumentInfo.Create(argumentFlags, null),
                    CSharpArgumentInfo.Create(argumentFlags, null),
                });

            var callsite = CallSite<Func<CallSite, object, object, object>>.Create(binder);
            callsite.Target(callsite, target, value);
        }

        /// <summary>
        /// オブジェクトから値を取得する
        /// </summary>
        /// <param name="target">対象オブジェクト</param>
        /// <param name="targetType">対象オブジェクト型</param>
        /// <param name="fieldName">プロパティ名</param>
        /// <returns>取得した値</returns>
        public static object GetValue(object target, Type targetType, string fieldName)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (String.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException(nameof(fieldName));

            const CSharpBinderFlags binderFlags = CSharpBinderFlags.None;
            const CSharpArgumentInfoFlags argumentFlags = CSharpArgumentInfoFlags.None;
            var binder = Binder.GetMember(binderFlags, fieldName, targetType,
                new[]
                {
                    CSharpArgumentInfo.Create(argumentFlags, null)
                });

            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            return callsite.Target(callsite, target);
        }
    }
}
