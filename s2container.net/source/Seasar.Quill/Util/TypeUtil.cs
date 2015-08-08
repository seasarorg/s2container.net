﻿#region Copyright
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
using System.Runtime.Remoting;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Util;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// Typeを扱うユーティリティクラス
    /// </summary>
    public static class TypeUtil
    {
        /// <summary>
        /// 指定されたオブジェクトのTypeを取得する
        /// </summary>
        /// <remarks>
        /// オブジェクトが透過プロキシの場合はAopProxyからTypeを取得する
        /// </remarks>
        /// <param name="obj">オブジェクト</param>
        /// <returns>Type</returns>
        public static Type GetType(object obj)
        {
            if (RemotingServices.IsTransparentProxy(obj))
            {
                // 透過プロキシの場合はAopProxyからTypeを取得する
                var aopProxy = RemotingServices.GetRealProxy(obj) as AopProxy;
                return aopProxy?.TargetType;
            }
            else
            {
                // 透過プロキシではない場合は通常の方法でTypeを取得する
                return obj.GetExType();
            }
        }

        /// <summary>
        /// 名称に名前空間を含むかどうか判定
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasNamespace(string name) => name.Contains(".");
    }
}
