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
#if NET_4_0
using System;
using System.IO;
using Seasar.Framework.Util;

namespace Seasar.Unit.Core
{
    /// <summary>
    /// テスト実行共通処理ユーティリティクラス
    /// </summary>
    public class S2TestUtils
    {
        /// <summary>
        /// 特定の名前のメソッドを指定した属性が付加されている場合と同じように呼び出す
        /// </summary>
        /// <typeparam name="TAttr"></typeparam>
        /// <param name="fixture"></param>
        /// <param name="targetMethodName"></param>
        public static void CallAsHavingAttribute<TAttr>(object fixture, string targetMethodName) where TAttr : Attribute
        {
            var method = fixture.GetExType().GetMethod(targetMethodName);
            if (method != null)
            {
                var attribute = Attribute.GetCustomAttribute(method, typeof(TAttr)) as TAttr;
                if (attribute == null)
                {
                    MethodUtil.Invoke(method, fixture, null);
                }
            }
        }

        /// <summary>
        /// 名称に特定のキーワードを含むメソッドを呼び出す
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="testMethodName"></param>
        /// <param name="specificKeyword"></param>
        public static void CallForSpecificMethod(object fixture,
            string testMethodName, string specificKeyword)
        {
            var targetName = _GetTargetName(testMethodName);
            if (targetName.Length > 0)
            {
                var method = fixture.GetExType().GetMethod(specificKeyword + targetName);
                if (method != null)
                {
                    MethodUtil.Invoke(method, fixture, null);
                }
            }
        }

        public static bool IsMatchExpectedException(Type expectedExceptionType, Exception e)
        {
            if (expectedExceptionType == e.GetExType())
            {
                return true;
            }
            else if (e.InnerException != null)
            {
                return IsMatchExpectedException(expectedExceptionType, e.InnerException);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// テストメソッド名の前後から文字列「test」（大文字小文字区別なし）
        /// を取り除いたものを取得する
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private static string _GetTargetName(string methodName)
        {
            if (methodName.ToLower().StartsWith("test"))
            {
                methodName = methodName.Substring(4);
            }

            if (methodName.ToLower().EndsWith("test"))
            {
                methodName = methodName.Substring(0, methodName.Length - 4);
            }

            return methodName;
        }

        

        public static string ConvertPath(Type type, string path)
        {
            if (ResourceUtil.GetResourceNoException(path, type.Assembly) != null)
            {
                return path;
            }
            if (path.IndexOf('/') > 0)
            {
                return path;
            }
            if (path.IndexOf(Path.DirectorySeparatorChar) > 0)
            {
                return path;
            }
            var prefix = type.FullName.Replace('.', '/');
            var pos = (prefix.LastIndexOf('/') + 1);
            prefix = prefix.Substring(0, pos);
            return prefix + path;
        }

        public static string NormalizeName(string name) => name.TrimEnd('_').TrimStart('_');
    }
}
#endif
