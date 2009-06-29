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

using System;
using System.Collections.Generic;
using System.Reflection;
using Seasar.Framework.Log;
using Seasar.Quill.Attrs;
using Seasar.Quill.Exception;
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
    public class QuillInjector : IDisposable
    {
        private static readonly Logger _log = Logger.GetLogger(typeof(QuillInjector));

        // QuillInjectorのインスタンス
        private static QuillInjector quillInjector;

        // QuillInjector内で使用するQuillContainer
        protected QuillContainer container;

        /// <summary>
        /// インジェクション管理
        /// </summary>
        /// <remarks>
        /// 相互参照があるオブジェクトへのインジェクション時に
        /// 再帰呼び出しで無限ループとならないよう管理します。
        /// 無限ループ防止のみが目的のため、
        /// スレッドセーフにはしていません。
        /// </remarks>
        private readonly QuillInjectionContext _context;

        /// <summary>
        /// QuillInjector内で使用するQuillContainer
        /// (取得専用）
        /// </summary>
        public QuillContainer Container
        {
            get
            {
                return container;
            }
        }

        protected InjectionMap injectionMap;

        /// <summary>
        /// インターフェース型と実装型の対応情報
        /// </summary>
        public InjectionMap InjectionMap
        {
            set
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug(MessageUtil.GetMessage("IQLL0013", new object[] {}));
                }
                injectionMap = value;
            }
            get
            {
                return injectionMap;
            }
        }

        /// <summary>
        /// QuillInjectorを初期化するコンストラクタ
        /// </summary>
        /// <remarks>
        /// <see cref="GetInstance"/>からインスタンスを生成する
        /// </remarks>
        /// <seealso cref="Seasar.Quill.QuillInjector.GetInstance"/>
        protected QuillInjector()
        {
            // QuillInjector内で使用するQuillContainerを作成する
            container = new QuillContainer();
            //  デフォルトではInjectionMapは使わない
            injectionMap = null;

            _context = new QuillInjectionContext();
        }

        /// <summary>
        /// QuillInjectorのインスタンスを取得する
        /// </summary>
        /// <remarks>
        /// <para>
        /// QuillInjectorのコンストラクタのアクセス修飾子はprotectedに設定されている為、
        /// 直接QuillInjectorのインスタンスを生成することはできない。
        /// </para>
        /// <para>
        /// QuillInjectorのインスタンスを取得する場合は当メソッドを使用する。
        /// </para>
        /// <para>
        /// 基本的に同じインスタンスを返すが、DestroyメソッドによってQuillが持つ
        /// 参照が破棄されている場合は新しいQuillInjectorのインスタンスを作成する。
        /// </para>
        /// </remarks>
        /// <returns>QuillInjectorのインスタンス</returns>
        public static QuillInjector GetInstance()
        {
            if (quillInjector != null)
            {
                return quillInjector;
            }

            lock (typeof(QuillInjector))
            {
                if (quillInjector != null)
                {
                    return quillInjector;
                }
                quillInjector = new QuillInjector();
            }

            // QuillInjectorのインスタンスを返す
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
        /// <see cref="Seasar.Quill.Attrs.ImplementationAttribute"/>(属性)か
        /// InjectionMapに設定されている必要がある。
        /// </para>
        /// <para>
        /// Aspectを適用する場合はインターフェース・クラスもしくはメソッドに
        /// <see cref="Seasar.Quill.Attrs.AspectAttribute"/>(属性)
        /// 設定されている必要がある。
        /// </para>
        /// </remarks>
        /// <param name="target">
        /// この<code>target</code>のフィールドに必要なコンポーネントがセットされる
        /// </param>
        public virtual void Inject(object target)
        {
            if (container == null)
            {
                // Destroyされている場合は例外を発生する
                throw new QuillApplicationException("EQLL0018");
            }

            bool isFirstInjection = !_context.IsInInjection;
            if (isFirstInjection)
            {
                _context.BeginInjection();
            }

            try
            {
                Type targetType = target.GetType();
                if (_context.IsAlreadyInjected(targetType))
                {
                    return;
                }
                _context.AddInjectedType(targetType);

                // フィールドを取得する
                FieldInfo[] fields = targetType.GetFields(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                // フィールドの件数分、注入を行う
                foreach (FieldInfo field in fields)
                {
                    // フィールドにオブジェクトを注入する
                    InjectField(target, field);
                }
            }
            finally
            {
                if (isFirstInjection)
                {
                    _context.EndInjection();
                }
            }
        }

        /// <summary>
        /// QuillInjectorが持つ参照を破棄する
        /// </summary>
        public virtual void Destroy()
        {
            if (container == null)
            {
                return;
            }

            // QuillContainerが持つ参照を破棄する
            container.Destroy();

            container = null;
            quillInjector = null;
        }

        /// <summary>
        /// Inject済情報のクリア
        /// </summary>
        [Obsolete("ver.1.4.0から削除予定です。")]
        public void ClearInjected()
        {
            _context.EndInjection();
        }

        /// <summary>
        /// 指定するフィールドにDIする
        /// </summary>
        /// <remarks>QuillもしくはS2ContainerのコンポーネントをDIする</remarks>
        /// <param name="target">DIが行われるオブジェクト</param>
        /// <param name="field">DIが行われるフィールド情報</param>
        protected virtual void InjectField(object target, FieldInfo field)
        {
            // フィールドに設定されているバインディング属性を取得する
            BindingAttribute bindingAttr = AttributeUtil.GetBindingAttr(field);

            if (bindingAttr != null)
            {
                // バインディング属性が設定されている場合はS2Containerの
                // コンポーネントをDIする
                InjectField(target, field, bindingAttr);
                return;
            }

            //  インターフェースと実装型の対応情報に登録されている型であれば
            //  その型でDIする
            //  本番->Implementation,Mock->InjectionMapという
            //  使い分けもできるようにこちらを優先
            Type fieldType = field.FieldType;
            if(injectionMap != null && injectionMap.HasComponentType(fieldType))
            {
                InjectField(target, field, injectionMap.GetComponentType(fieldType));
                return;
            }

            // フィールドの型(Type)に設定されている実装を指定する属性を取得する
            ImplementationAttribute implAttr = 
                AttributeUtil.GetImplementationAttr(field.FieldType);

            if (implAttr != null)
            {
                // 実装を指定する属性が設定されている場合はQuillの
                // コンポーネントをDiする
                InjectField(target, field, implAttr);
            }
        }

        /// <summary>
        /// フィールドにバインディング属性で指定されたコンポーネントをInjectする
        /// </summary>
        /// <remarks>S2ContainerのコンポーネントをDIする</remarks>
        /// <param name="target">DIが行われるオブジェクト</param>
        /// <param name="field">DIが行われるフィールド情報</param>
        /// <param name="bindingAttr">DIするコンポーネントを指定するBinding属性</param>
        protected virtual void InjectField(
            object target, FieldInfo field, BindingAttribute bindingAttr)
        {
            // S2Containerからコンポーネントを取得する
            object component = SingletonS2ContainerConnector.GetComponent(
                bindingAttr.ComponentName, field.FieldType);

            // コンポーネントのTypeを取得する
            Type componentType = TypeUtil.GetType(component);

            if (!field.FieldType.IsAssignableFrom(componentType))
            {
                // 代入不可能なコンポーネントが指定されている場合は例外をスローする
                throw new QuillApplicationException("EQLL0014", new object[] {
                    field.FieldType.FullName, componentType.FullName });
            }

            // フィールドに値をセットする為のBindingFlags
            BindingFlags bindingFlags = BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField;

            // フィールドに実装クラスのインスタンスを注入する
            target.GetType().InvokeMember(field.Name, bindingFlags, null, target,
                new object[] { component });
        }

        /// <summary>
        /// フィールドにImplementation属性で指定されたコンポーネントをInjectする
        /// </summary>
        /// <remarks>QuillのコンポーネントをDIする</remarks>
        /// <param name="target">DIが行われるオブジェクト</param>
        /// <param name="field">DIが行われるフィールド情報</param>
        /// <param name="implAttr">実装クラスを指定する属性</param>
        protected virtual void InjectField(object target, FieldInfo field, ImplementationAttribute implAttr)
        {
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

            // フィールドに指定されたType(implType)のコンポーネントをInjectする
            InjectField(target, field, implType);
        }

        /// <summary>
        /// フィールドに指定されたType(implType)のコンポーネントをInjectする
        /// </summary>
        /// <remarks>QuillのコンポーネントをDIする</remarks>
        /// <param name="target">DIが行われるオブジェクト</param>
        /// <param name="field">DIが行われるフィールド情報</param>
        /// <param name="implType">実装クラスのType</param>
        protected virtual void InjectField(object target, FieldInfo field, Type implType)
        {
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

        

        #region IDisposable メンバ

        /// <summary>
        /// 保持しているQuillContainerのDisposeを呼び出す
        /// </summary>
        public virtual void Dispose()
        {
            // 保持しているQuillContainerのDisposeを呼び出す
            container.Dispose();
            _context.EndInjection();
        }

        #endregion

        #region QuillInjectionContext

        /// <summary>
        /// インジェクション管理クラス
        /// </summary>
        private sealed class QuillInjectionContext
        {
            [ThreadStatic]
            private static IDictionary<Type, Type> _alreadyInjectedTypeMap;

            /// <summary>
            /// インジェクション中（再帰）中か判定
            /// </summary>
            public bool IsInInjection
            {
                get { return _alreadyInjectedTypeMap != null; }
            }

            /// <summary>
            /// 既にインジェクション済の型か判定
            /// </summary>
            /// <param name="targetType"></param>
            /// <returns></returns>
            public bool IsAlreadyInjected(Type targetType)
            {
                return (IsInInjection && _alreadyInjectedTypeMap.ContainsKey(targetType));
            }

            /// <summary>
            /// インジェクション済の型を登録する
            /// </summary>
            /// <param name="targetType"></param>
            public void AddInjectedType(Type targetType)
            {
                if(IsInInjection)
                {
                    _alreadyInjectedTypeMap.Add(targetType, targetType);
                }
            }

            /// <summary>
            /// インジェクション中状態に遷移
            /// </summary>
            public void BeginInjection()
            {
                EndInjection();
                _alreadyInjectedTypeMap = new Dictionary<Type, Type>();
            }

            /// <summary>
            /// インジェクション終了
            /// </summary>
            public void EndInjection()
            {
                if(_alreadyInjectedTypeMap != null)
                {
                    _alreadyInjectedTypeMap.Clear();
                }
                _alreadyInjectedTypeMap = null;
            }
        }
        #endregion
    }

    
}
