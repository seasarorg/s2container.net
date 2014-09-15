using Seasar.Quill.Attr;
using Seasar.Quill.Container;
using System;
using System.Linq;
using System.Reflection;

namespace Seasar.Quill.Injection.Impl
{
    /// <summary>
    /// Quillインジェクション抽象クラス
    /// </summary>
    public class QuillInjector : IQuillInjector
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        public IQuillContainer Container { get; set; }

        /// <summary>
        /// インジェクション状態
        /// </summary>
        public IQuillInjectionContext Context { get; set; }

        /// <summary>
        /// インジェクション実行
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        public virtual void Inject(object target)
        {
            if (target == null)
            {
		        throw new ArgumentNullException("root");
            }

            bool isFirstInjection = !Context.IsInInjection;
            if (isFirstInjection)
            {
                Context.BeginInjection();
            }

            try
            {
                if (Context.IsAlreadyInjected(target.GetType()))
                {
                    return;
                }

                this.InjectFields(target);
            }
            finally
            {
                if (isFirstInjection)
                {
                    Context.EndInjection();
                }
            }
        }

        /// <summary>
        /// インジェクション済のコンポーネント生成
        /// </summary>
        /// <typeparam name="T">コンポーネントの型</typeparam>
        /// <returns>コンポーネントのインスタンス</returns>
        public virtual T CreateInjectedInstance<T>()
        {
            var root = Container.GetComponent<T>();
            if (root != null)
            {
                Inject(root);
            }
            return root;
        }

        /// <summary>
        /// インジェクション済のコンポーネント生成
        /// </summary>
        /// <typeparam name="I">コンポーネントのインターフェース（スーパークラス）</typeparam>
        /// <typeparam name="T">コンポーネントの型</typeparam>
        /// <returns>コンポーネントのインスタンス</returns>
        public virtual I CreateInjectedInstance<I, T>() where T : I
        {
            var root = Container.GetComponent<I, T>();
            if (root != null)
            {
                Inject(root);
            }
            return root;
        }

        /// <summary>
        /// 各フィールドへのインジェクション実施
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        protected virtual void InjectFields(object target)
        {
            var fields = this.GetFields(target);
            if (fields == null || fields.Length == 0)
            {
                // フィールド情報を取得できなかった場合はインジェクション終了
                return;
            }

            var targetFields = fields.Where(this.IsInjectTarget);
            targetFields.AsParallel().ForAll(fi =>
            {
                var component = this.GetComponent(fi);
                this.SetField(target, fi, component);
            });
        }

        /// <summary>
        /// オブジェクト内のフィールド情報を取得
        /// </summary>
        /// <param name="root">インジェクション対象のオブジェクト</param>
        /// <returns>取得したフィールド情報配列</returns>
        protected virtual FieldInfo[] GetFields(object root)
        {
            return root.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// インジェクションを行うか判定
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        protected virtual bool IsInjectTarget(FieldInfo fi)
        {
            // デフォルトはImplementation属性をもつコンポーネントのみインジェクション対象とする
            var attr = fi.GetCustomAttribute<ImplementationAttribute>();
            return (attr != null);
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="fi">フィールド情報</param>
        /// <returns>コンポーネントのインスタンス</returns>
        protected virtual object GetComponent(FieldInfo fi)
        {
            return Container.GetComponent(fi.FieldType);
        }

        /// <summary>
        /// フィールド設定
        /// </summary>
        /// <param name="target">設定対象オブジェクト</param>
        /// <param name="fi">フィールド情報</param>
        /// <param name="component">設定するコンポーネント</param>
        protected virtual void SetField(object target, FieldInfo fi, object component)
        {
            // nullでない時のみ設定
            if (component != null)
            {
                // コンポーネントを設定し、再帰的に処理していく
                fi.SetValue(target, component);
                Inject(component);
            }
        }   
    }
}
