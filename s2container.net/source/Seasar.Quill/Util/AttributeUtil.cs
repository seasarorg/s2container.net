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
using Seasar.Quill.Attrs;
using System.Reflection;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// Quillで用意されている属性を扱うクラス
    /// </summary>
    public static class AttributeUtil
    {
        /// <summary>
        /// 実装クラスを指定する為に設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.ImplementationAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するType</param>
        /// <returns>実装クラスが指定された属性</returns>
        public static ImplementationAttribute GetImplementationAttr(Type type)
        {
            // 実装クラスを指定する属性を取得する
            ImplementationAttribute implAttr =
                (ImplementationAttribute)Attribute.GetCustomAttribute(
                type, typeof(ImplementationAttribute));

            if(implAttr == null)
            {
                // Implementation属性が指定されていない場合はnullを返す
                return null;
            }

            // Implementation属性に指定された実装クラスのType
            Type implType = implAttr.ImplementationType;

            if (!type.IsInterface && implType != null)
            {
                // クラスのImplementation属性に実装クラスが指定されている場合は
                // 例外をスローする
                throw new QuillApplicationException("EQLL0001",
                    new object[] { type.FullName });
            }

            if (implType != null && implType.IsInterface)
            {
                // Implementation属性の実装クラスにインターフェースが
                // 指定されている場合は例外をスローする
                throw new QuillApplicationException("EQLL0002", new object[] {
                    type.FullName, implType.FullName });
            }

            if (implType != null && implType.IsAbstract)
            {
                // Implementation属性の実装クラスに抽象クラスが
                // 指定されている場合は例外をスローする
                throw new QuillApplicationException("EQLL0003", new object[] {
                    type.FullName, implType.FullName });
            }

            if (implType != null && !type.IsAssignableFrom(implType))
            {
                // 代入不可能な実装クラスが指定されている場合は例外をスローする
                throw new QuillApplicationException("EQLL0004", new object[] {
                    type.FullName, implType.FullName });
            }

            // 実装クラスを指定する属性を返す
            return implAttr;
        }

        /// <summary>
        /// Aspectを指定する為にクラスやインターフェースに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.AspectAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するType</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static AspectAttribute[] GetAspectAttrs(Type type)
        {
            // Aspectを指定する属性を取得して返す
            return GetAspectAttrsByMember(type);
        }

        /// <summary>
        /// Aspectを指定する為にメソッドに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.AspectAttribute"/>)を取得する
        /// </summary>
        /// <param name="method">属性を確認するメソッド</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static AspectAttribute[] GetAspectAttrsByMethod(MethodInfo method)
        {
            // Aspectを指定する属性を取得する
            AspectAttribute[] attrs = GetAspectAttrsByMember(method);

            if (attrs.Length == 0)
            {
                // Aspect属性が指定されていない場合は配列サイズ0のAspect属性の配列を返す
                return attrs;
            }

            if (method.IsStatic)
            {
                // メソッドがstaticの場合は例外をスローする
                throw new QuillApplicationException("EQLL0005", new object[] {
                    method.DeclaringType.FullName, method.Name });
            }

            if (!method.IsPublic)
            {
                // メソッドがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0006", new object[] {
                    method.DeclaringType.FullName, method.Name });
            }

            if (!method.IsVirtual)
            {
                // メソッドがvirtualかインターフェースのメソッド
                // ではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0007", new object[] {
                    method.DeclaringType.FullName, method.Name });
            }

            // Aspectを指定する属性を返す
            return attrs;
        }

        /// <summary>
        /// Aspectを指定する為にメンバに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.AspectAttribute"/>)を取得する
        /// </summary>
        /// <param name="member">属性を確認するメンバ</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        private static AspectAttribute[] GetAspectAttrsByMember(MemberInfo member)
        {
            // Aspectを指定する属性を取得する
            AspectAttribute[] attrs =
                (AspectAttribute[])Attribute.GetCustomAttributes(
                member, typeof(AspectAttribute));

            // Aspectを指定する属性を返す
            return attrs;
        }
    }
}
