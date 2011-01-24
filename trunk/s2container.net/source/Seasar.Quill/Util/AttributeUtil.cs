#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Reflection;
using Seasar.Quill.Attrs;
using Seasar.Quill.Exception;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// Quillで用意されている属性を扱うクラス
    /// </summary>
    public static class AttributeUtil
    {
        #region ImplementationAttribute

        /// <summary>
        /// 実装クラスを指定する為に設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.ImplementationAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するクラスもしくはインターフェースのType</param>
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

        #endregion

        #region AspectAttribute

        /// <summary>
        /// Aspectを指定する為にクラスやインターフェースに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.AspectAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するType</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static AspectAttribute[] GetAspectAttrs(Type type)
        {
            if (!type.IsPublic && !type.IsNestedPublic)
            {
                // メソッドを宣言するクラスがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0016", new object[] {
                    type.FullName });
            }

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

            if (!method.DeclaringType.IsPublic && !method.DeclaringType.IsNestedPublic)
            {
                // メソッドを宣言するクラスがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0016", new object[] {
                    method.DeclaringType.FullName });
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
        public static AspectAttribute[] GetAspectAttrsByMember(MemberInfo member)
        {
            // Aspectを指定する属性を取得する
            AspectAttribute[] attrs =
                (AspectAttribute[])Attribute.GetCustomAttributes(
                member, typeof(AspectAttribute));

            // Aspectを格納するリスト
            List<AspectAttribute> attrList = 
                new List<AspectAttribute>(attrs);

            // Aspectのリストを並び替える
            attrList.Sort(new AspectAttributeComparer());

            // Aspectを指定する属性を返す
            return attrList.ToArray();
        }

        #endregion

        #region TransactionAttribute

        /// <summary>
        /// Transactionを指定する為にクラスやインターフェースに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.TransactionAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するType</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static TransactionAttribute GetTransactionAttr(Type type)
        {
            if ( !type.IsPublic && !type.IsNestedPublic )
            {
                // メソッドを宣言するクラスがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0016", new object[] {
                    type.FullName });
            }
            
            // Aspectを指定する属性を取得して返す
            return GetTransactionAttrByMember(type);
        }

        /// <summary>
        /// Transactionを指定する為にクラスやインターフェースに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.TransactionAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するType</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static TransactionAttribute[] GetTransactionAttrs(Type type)
        {
            if (!type.IsPublic && !type.IsNestedPublic)
            {
                // メソッドを宣言するクラスがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0016", new object[] {
                    type.FullName });
            }

            // Aspectを指定する属性を取得して返す
            return GetTransactionAttrsByMember(type);
        }

        /// <summary>
        /// Aspectを指定する為にメソッドに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.TransactionAttribute"/>)を取得する
        /// </summary>
        /// <param name="method">属性を確認するメソッド</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static TransactionAttribute GetTransactionAttrByMethod(MethodInfo method)
        {
            // Aspectを指定する属性を取得する
            TransactionAttribute attr = GetTransactionAttrByMember(method);
            if ( attr == null )
            {
                // Aspect属性が指定されていない場合はnullを返す
                return null;
            }

            ValidateMethodInfo(method);

            // Aspectを指定する属性を返す
            return attr;
        }

        /// <summary>
        /// Aspectを指定する為にメソッドに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.TransactionAttribute"/>)を取得する
        /// </summary>
        /// <param name="method">属性を確認するメソッド</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static TransactionAttribute[] GetTransactionAttrsByMethod(MethodInfo method)
        {
            // Aspectを指定する属性を取得する
            TransactionAttribute[] attrs = GetTransactionAttrsByMember(method);
            if (attrs == null || attrs.Length == 0)
            {
                // Aspect属性が指定されていない場合はnullを返す
                return null;
            }

            ValidateMethodInfo(method);

            // Aspectを指定する属性を返す
            return attrs;
        }

        /// <summary>
        /// Aspectを指定する為にメンバに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.TransactionAttribute"/>)を取得する
        /// </summary>
        /// <param name="member">属性を確認するメンバ</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static TransactionAttribute GetTransactionAttrByMember(MemberInfo member)
        {
            // Aspectを指定する属性を取得する
            TransactionAttribute attr =
                (TransactionAttribute)Attribute.GetCustomAttribute(
                member, typeof(TransactionAttribute));

            return attr;
        }

        /// <summary>
        /// Aspectを指定する為にメンバに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.TransactionAttribute"/>)を取得する
        /// </summary>
        /// <param name="member">属性を確認するメンバ</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static TransactionAttribute[] GetTransactionAttrsByMember(MemberInfo member)
        {
            // Aspectを指定する属性を取得する
            TransactionAttribute[] attrs =
                (TransactionAttribute[])Attribute.GetCustomAttributes(
                member, typeof(TransactionAttribute));

            return attrs;
        }

        #endregion

        #region S2DaoAttribute

        /// <summary>
        /// DaoInterceptorを指定する為にクラスやインターフェースに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.S2DaoAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するType</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static S2DaoAttribute GetS2DaoAttr(Type type)
        {
            if (!type.IsPublic && !type.IsNestedPublic)
            {
                // メソッドを宣言するクラスがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0016", new object[] {
                    type.FullName });
            }

            // Aspectを指定する属性を取得して返す
            return GetS2DaoAttrByMember(type);
        }

        /// <summary>
        /// Aspectを指定する為にメソッドに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.S2DaoAttribute"/>)を取得する
        /// </summary>
        /// <param name="method">属性を確認するメソッド</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static S2DaoAttribute GetS2DaoAttrByMethod(MethodInfo method)
        {
            // Aspectを指定する属性を取得する
            S2DaoAttribute attr = GetS2DaoAttrByMember(method);
            if (attr == null)
            {
                // Aspect属性が指定されていない場合はnullを返す
                return null;
            }

            ValidateMethodInfo(method);

            // Aspectを指定する属性を返す
            return attr;
        }

        /// <summary>
        /// Aspectを指定する為にメンバに設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.S2DaoAttribute"/>)を取得する
        /// </summary>
        /// <param name="member">属性を確認するメンバ</param>
        /// <returns>Aspectが指定された属性の配列</returns>
        public static S2DaoAttribute GetS2DaoAttrByMember(MemberInfo member)
        {
            // Aspectを指定する属性を取得する
            S2DaoAttribute attr =
                (S2DaoAttribute)Attribute.GetCustomAttribute(
                member, typeof(S2DaoAttribute));

            return attr;
        }

        #endregion

        #region BindingAttribute

        /// <summary>
        /// S2ContainerのコンポーネントのBindingを指定する為にフィールドに設定されている
        /// 属性(<see cref="Seasar.Quill.Attrs.BindingAttribute"/>)を取得する
        /// </summary>
        /// <param name="field">フィールド</param>
        /// <returns>Bindingコンポーネントが指定されたBinding属性</returns>
        public static BindingAttribute GetBindingAttr(FieldInfo field)
        {
            if (field.IsStatic)
            {
                // staticフィールドの場合は例外をスローする
                throw new QuillApplicationException("EQLL0015", new string[] {
                    field.DeclaringType.FullName, field.Name});
            }

            // バインディングコンポーネントを指定する属性を取得する
            BindingAttribute bindingAttr =
                (BindingAttribute)Attribute.GetCustomAttribute(
                field, typeof(BindingAttribute));

            // バインディング属性が指定されていない場合はnullを返す
            if (bindingAttr == null || bindingAttr.ComponentName == null)
            {
                return null;
            }

            // Binding属性を返す
            return bindingAttr;
        }

        #endregion

        #region MockAttribute

        /// <summary>
        /// Mockを指定する為に設定されている属性
        /// (<see cref="Seasar.Quill.Attrs.MockAttribute"/>)を取得する
        /// </summary>
        /// <param name="type">属性を確認するクラスのType</param>
        /// <returns>Mockクラスが指定された属性</returns>
        public static MockAttribute GetMockAttr(Type type)
        {
            // 実装クラスを指定する属性を取得する
            MockAttribute mockAttr = (MockAttribute)Attribute.GetCustomAttribute(
                type, typeof(MockAttribute));

            if (mockAttr == null)
            {
                // Mock属性が指定されていない場合はnullを返す
                return null;
            }

            // Mock属性に指定されたMockクラスのType
            Type mockType = mockAttr.MockType;

            if (mockType == null)
            {
                // クラスのMock属性にクラスが指定されていない場合は例外をスローする
                throw new QuillApplicationException("EQLL0019",
                    new object[] {  });
            }

            if (mockType.IsInterface)
            {
                // Mock属性にインターフェースが指定されている場合は例外をスローする
                throw new QuillApplicationException("EQLL0020", 
                    new object[] { mockType.FullName });
            }

            if (mockType.IsAbstract)
            {
                // Mock属性に抽象クラスが指定されている場合は例外をスローする
                throw new QuillApplicationException("EQLL0021",
                    new object[] { mockType.FullName });
            }

            if (!type.IsAssignableFrom(mockType))
            {
                // 代入不可能なクラスが指定されている場合は例外をスローする
                throw new QuillApplicationException("EQLL0022",
                    new object[] { type.FullName, mockType.FullName });
            }

            // Mockクラスを指定する属性を返す
            return mockAttr;
        }

        #endregion

        #region AspectAttributeを比較するクラス

        /// <summary>
        /// AspectAttributeを比較するクラス
        /// </summary>
        private class AspectAttributeComparer : IComparer<AspectAttribute>
        {
            /// <summary>
            /// 2つのAspectAttributeを比較する
            /// （並び順は<see cref="Seasar.Quill.Attrs.AspectAttribute.Ordinal"/>で
            /// 決定する)
            /// <para>
            /// xとyが等しい場合は0, xがyより大きい場合は正の値,
            /// xがyより小さい場合は負の値を返す
            /// </para>
            /// </summary>
            /// <param name="x">比較対象の第1オブジェクト</param>
            /// <param name="y">比較対象の第2オブジェクト</param>
            /// <returns>xとyが等しい場合は0, xがyより大きい場合は正の値,
            /// xがyより小さい場合は負の値を返す</returns>
            public int Compare(AspectAttribute x, AspectAttribute y)
            {
                // xとyが等しい場合は0, xがyより大きい場合は正の値,
                // xがyより小さい場合は負の値を返す
                return x.Ordinal - y.Ordinal;
            }
        }

        #endregion

        #region Validate

        /// <summary>
        /// メソッドにAspectを適用できるか検証
        /// </summary>
        /// <param name="method">検証対象のメソッド情報</param>
        private static void ValidateMethodInfo(MethodInfo method)
        {
            if ( !method.DeclaringType.IsPublic && !method.DeclaringType.IsNestedPublic )
            {
                // メソッドを宣言するクラスがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0016", new object[] {
                    method.DeclaringType.FullName });
            }

            if ( method.IsStatic )
            {
                // メソッドがstaticの場合は例外をスローする
                throw new QuillApplicationException("EQLL0005", new object[] {
                    method.DeclaringType.FullName, method.Name });
            }

            if ( !method.IsPublic )
            {
                // メソッドがpublicではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0006", new object[] {
                    method.DeclaringType.FullName, method.Name });
            }

            if ( !method.IsVirtual )
            {
                // メソッドがvirtualかインターフェースのメソッド
                // ではない場合は例外をスローする
                throw new QuillApplicationException("EQLL0007", new object[] {
                    method.DeclaringType.FullName, method.Name });
            }
        }

        #endregion
    }
}
