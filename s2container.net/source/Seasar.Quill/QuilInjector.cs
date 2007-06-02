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
using Seasar.Quill.Attrs;
using Seasar.Quill.Util;

namespace Seasar.Quill
{
    /// <summary>
    /// DependencyInjectionを行うクラス
    /// </summary>
    /// <remarks>
    /// <see cref="Seasar.Quill.QuillInjector.GetInstance"/>
    /// でQuillInjectorのインスタンスを作成して、
    /// <see cref="Seasar.Quill.QuillInjector.Inject"/>を使用してDIを行う。
    /// </remarks>
    public class QuillInjector
    {
        // QuillInjectorの唯一のインスタンス
        private static QuillInjector quillInjector = new QuillInjector();

        // QuillInjector内で使用するQuillContainer
        protected QuillContainer container;

        /// <summary>
        /// QuillInjectorを初期化するコンストラクタ
        /// </summary>
        /// <remarks>
        /// アクセス修飾子がprivateに設定されているのでQuillInjector内からしか呼ばれない。
        /// <see cref="GetInstance"/>から呼び出される。
        /// </remarks>
        /// <seealso cref="Seasar.Quill.QuillInjector.GetInstance"/>
        private QuillInjector()
        {
            // QuillInjector内で使用するQuillContainerを作成する
            container = new QuillContainer();
        }

        /// <summary>
        /// QuillInjectorのインスタンスを取得する
        /// </summary>
        /// <remarks>
        /// <para>
        /// QuillInjectorのコンストラクタのアクセス修飾子はprivateに設定されている為、
        /// 直接QuillInjectorのインスタンスを生成することはできない。
        /// </para>
        /// <para>
        /// QuillInjectorのインスタンスを取得する場合は当メソッドを使用する。
        /// </para>
        /// <para>
        /// 当メソッドで取得するQuillInjectorのインスタンスはアプリケーション内で
        /// 唯一のインスタンスとなる。
        /// </para>
        /// </remarks>
        /// <returns>QuillInjectorのインスタンス</returns>
        public static QuillInjector GetInstance()
        {
            // 唯一であるQuillInjectorのインスタンスを返す
            return quillInjector;
        }

        /// <summary>
        /// 引数で渡される<code>target</code>のオブジェクトのフィールドに
        /// 必要なコンポーネントをセット（DI）する
        /// </summary>
        /// <remarks>
        /// <para>
        /// DIを行うことのできるフィールドの条件はインスタンスメンバとする。
        /// （パブリックメンバ、非パブリックメンバは問わない）
        /// </para>
        /// <para>
        /// DIを行う場合はフィールドの型に
        /// <see cref="Seasar.Quill.Attrs.ImplementationAttribute"/>(属性)が
        /// 設定されている必要がある。
        /// </para>
        /// <para>
        /// Aspectを適用する場合はインターフェース・クラスもしくはメソッドに
        /// <see cref="Seasar.Quill.Attrs.AspectAttribute"/>(属性)が
        /// 設定されている必要がある。
        /// </para>
        /// </remarks>
        /// <param name="target">
        /// この<code>target</code>のフィールドに必要なコンポーネントがセットされる
        /// </param>
        public virtual void Inject(object target)
        {
            // フィールドを取得する
            FieldInfo[] fields = target.GetType().GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // フィールドの件数分、注入を行う
            foreach (FieldInfo field in fields)
            {
                // フィールドにオブジェクトを注入する
                InjectField(target, field);
            }
        }

        /// <summary>
        /// 指定するフィールドにDIを行う
        /// </summary>
        /// <param name="target">DIが行われるオブジェクト</param>
        /// <param name="field">DIが行われるフィールド情報</param>
        protected virtual void InjectField(object target, FieldInfo field)
        {
            // 実装クラスを指定する属性を取得する
            ImplementationAttribute implAttr = 
                AttributeUtil.GetImplementationAttr(field.FieldType);

            if (implAttr == null)
            {
                // 実装クラスを指定する属性が指定されていない場合は注入は行わない
                return;
            }

            // 実装クラスのTypeを取得する
            Type implType;

            if (!field.FieldType.IsInterface || implAttr.ImplementationType == null)
            {
                // インターフェース型でない、もしくは実装クラスが指定されていない
                // インターフェースの場合はフィールドのTypeを実装クラスのTypeとする
                implType = field.FieldType;
            }
            else
            {
                // インターフェース型で実装クラスが指定されている場合は
                // 属性に定義されたTypeを実装クラスのTypeとする
                implType = implAttr.ImplementationType;
            }

            // 実装クラスのインスタンスを取得する
            QuillComponent component = container.GetComponent(field.FieldType, implType);

            // 再帰的に実装クラスのインスタンスにDIを行う
            Inject(component.GetComponentObject(implType));

            // フィールドに値をセットする為のBindingFlags
            BindingFlags bindingFlags = BindingFlags.Public | 
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField;

            // フィールドに実装クラスのインスタンスを注入する
            target.GetType().InvokeMember(field.Name, bindingFlags, null, target,
                new object[] { component.GetComponentObject(field.FieldType) });
        }
    }
}
