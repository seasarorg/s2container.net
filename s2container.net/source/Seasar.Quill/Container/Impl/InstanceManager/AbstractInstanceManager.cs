using Seasar.Quill.Exception;
using System;

namespace Seasar.Quill.Container.Impl.InstanceManager
{
    /// <summary>
    /// インスタンス管理抽象クラス
    /// </summary>
    public abstract class AbstractInstanceManager : IInstanceManager
    {
        /// <summary>
        /// コンポーネント生成オブジェクト配列
        /// </summary>
        private IComponentCreater[] _creators = null;

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="t">コンポーネントの型</param>
        /// <returns>インスタンス</returns>
        public virtual void SetComponentCreator(params IComponentCreater[] creators)
        {
            if (creators == null || creators.Length == 0)
            {
                throw new ArgumentNullException("creators");
            }
            _creators = creators;
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual object GetInstance(Type t)
        {
            return GetInstance(t, c => c.Create(t));
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="i">コンポーネントのインターフェース（スーパークラス）</param>
        /// <param name="impl">コンポーネントの型</param>
        /// <returns>インスタンス</returns>
        public virtual object GetInstance(Type i, Type impl)
        {
            return GetInstance(impl, c => c.Create(impl));
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="t">取得する型</param>
        /// <param name="createInvoker">インスタンス生成委譲オブジェクト</param>
        /// <returns>インスタンス</returns>
        protected virtual object GetInstance(Type t, Func<IComponentCreater, object> invokeCreateInstance)
        {
            Validate(t);
            foreach (IComponentCreater creator in _creators)
            {
                object instance = invokeCreateInstance(creator);
                if (instance != null)
                {
                    // 最初にインスタンスを生成できた方を適用
                    return instance;
                }
            }
            throw new ComponentCreatorNotFoundException(t.FullName);
        }

        /// <summary>
        /// 型の検証
        /// </summary>
        /// <param name="t">検証対象の型</param>
        protected virtual void Validate(Type t)
        {
            if (_creators == null)
            {
                throw new ArgumentNullException("ComponentCreator");
            }

            if (t == null)
            {
                throw new ArgumentNullException("TargetType");
            }
        }
    }
}
