using Seasar.Quill.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill
{
    /// <summary>
    /// インジェクション状態クラス
    /// </summary>
    public class QuillInjectionContext
    {
        /// <summary>
        /// インジェクション済の型コレクション
        /// </summary>
        private readonly AbstractInjectedTypes _injectedTypes;

        /// <summary>
        /// Quillコンテナ
        /// </summary>
        private readonly QuillContainer _container;

        /// <summary>
        /// インジェクション対象フィールド絞り込み条件
        /// </summary>
        private readonly BindingFlags _condition;

        /// <summary>
        /// インジェクション階層
        /// </summary>
        private int _injectionDepth = 0;
        
        /// <summary>
        /// Quillコンテナ
        /// </summary>
        public virtual QuillContainer Container { get { return _container; } }

        /// <summary>
        /// フィールド抽出条件
        /// </summary>
        public virtual BindingFlags Condition { get { return _condition; } }

        /// <summary>
        /// インジェクション階層
        /// </summary>
        public virtual int InjectionDepth { get { return _injectionDepth; } }
 
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="condition">フィールド抽出条件</param>
        /// <param name="container">Quillコンテナ</param>
        /// <param name="isThreadsafe">スレッドセーフフラグ（true:スレッドセーフ, false:非スレッドセーフ）</param>
        public QuillInjectionContext(
            BindingFlags condition = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
            QuillContainer container = null,
            bool isThreadsafe = false)
        {
            _condition = condition;
            _container = (container == null ? SingletonInstances.GetInstance<QuillContainer>() : container);
            if (isThreadsafe)
            {
                _injectedTypes = new ConcurrentInjectedTypes();
            }
            else
            {
                _injectedTypes = new InjectedTypes();
            }
        }

        /// <summary>
        /// インジェクション中か判定
        /// </summary>
        /// <returns></returns>
        public virtual bool IsInInjection()
        {
            return _injectionDepth > 0;
        }

        /// <summary>
        /// インジェクション開始
        /// </summary>
        /// <param name="targetType"></param>
        public virtual void BeginInjection(Type targetType)
        {
            _injectionDepth++;
            _injectedTypes.Add(targetType);
        }

        /// <summary>
        /// インジェクション終了
        /// </summary>
        public virtual void EndInjection()
        {
            if (_injectionDepth > 0)
            {
                _injectionDepth--;
            }
        }

        /// <summary>
        /// インジェクション状態のリセット
        /// </summary>
        public virtual void ResetInjection()
        {
            _injectionDepth = 0;
            _injectedTypes.Clear();
        }

        /// <summary>
        /// 既にインジェクション済の型か判定
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public virtual bool IsAlreadyInjected(Type targetType)
        {
            return _injectedTypes.Contains(targetType);
        }

        #region Injected types

        /// <summary>
        /// インジェクション済型コレクション抽象クラス
        /// </summary>
        private abstract class AbstractInjectedTypes
        {
            protected abstract IDictionary<Type, Type> GetInjectedTypes();

            public abstract void Add(Type injectedType);

            public virtual bool Contains(Type type)
            {
                return GetInjectedTypes().ContainsKey(type);
            }

            public virtual void Clear()
            {
                GetInjectedTypes().Clear();
            }
        }

        /// <summary>
        ///  インジェクション済な型コレクション（非スレッドセーフ）
        /// </summary>
        private class InjectedTypes : AbstractInjectedTypes
        {
            private readonly Dictionary<Type, Type> _injectedTypes = new Dictionary<Type, Type>();

            protected override IDictionary<Type, Type> GetInjectedTypes()
            {
                return _injectedTypes;
            }

            public override void Add(Type injectedType)
            {
                _injectedTypes[injectedType] = injectedType;
            }
        }

        /// <summary>
        ///  インジェクション済な型コレクション（スレッドセーフ）
        /// </summary>
        private class ConcurrentInjectedTypes : AbstractInjectedTypes
        {
            private readonly ConcurrentDictionary<Type, Type> _injectedTypes = new ConcurrentDictionary<Type, Type>();

            protected override IDictionary<Type, Type> GetInjectedTypes()
            {
                return _injectedTypes;
            }

            public override void Add(Type injectedType)
            {
                _injectedTypes.TryAdd(injectedType, injectedType);
            }
        }

        #endregion
    }
}
