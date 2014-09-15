using Seasar.Quill.Exception;
using System;

namespace Seasar.Quill.Container.Impl
{
    /// <summary>
    /// Quillコンテナ実装クラス
    /// </summary>
    public class QuillContainer : IQuillContainer
    {
        /// <summary>
        /// インスタンス管理オブジェクト
        /// </summary>
        private IInstanceManager _instanceManager = null;

        /// <summary>
        /// 実装クラス走査オブジェクト
        /// </summary>
        private IImplTypeDetector[] _detectors = null;

        /// <summary>
        /// インスタンス生成管理オブジェクトの設定
        /// </summary>
        /// <param name="manager">インスタンス管理オブジェクト</param>
        public virtual void SetInstanceManager(IInstanceManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            _instanceManager = manager;
        }

        /// <summary>
        /// 実装型走査オブジェクトの設定
        /// </summary>
        /// <param name="detectors">実装型走査オブジェクト</param>
        public virtual void SetImplTypeDetector(params IImplTypeDetector[] detectors)
        {
            if (detectors == null || detectors.Length == 0)
            {
                throw new ArgumentNullException("detectors");
            }
            _detectors = detectors;
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <returns>コンポーネントのインスタンス</returns>
        public virtual T GetComponent<T>()
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <typeparam name="IF">コンポーネントインターフェース（スーパークラス含む）</typeparam>
        /// <typeparam name="IMPL">実装クラス</typeparam>
        /// <returns>コンポーネントのインスタンス</returns>
        public virtual IF GetComponent<IF, IMPL>() where IMPL : IF
        {
            return (IF)GetComponent(typeof(IF), typeof(IMPL));
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="t">取得する型</param>
        /// <returns>コンポーネントのインスタンス</returns>
        public virtual object GetComponent(Type t)
        {
            var implType = GetImplType(t);
            if (implType == null)
            {
                throw new ImplementTypeNotFoundException(t.FullName);
            }

            if (t == implType)
            {
                return _instanceManager.GetInstance(implType);    
            }
            return _instanceManager.GetInstance(t, implType);
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="i">コンポーネントインターフェース（スーパークラス含む）</param>
        /// <param name="t">実装クラス</param>
        /// <returns>コンポーネントのインスタンス</returns>
        public virtual object GetComponent(Type i, Type t)
        {
            return _instanceManager.GetInstance(i, t);
        }

        /// <summary>
        /// 実装クラスの取得
        /// </summary>
        /// <param name="baseType">走査元のクラス</param>
        /// <returns>実装クラス（見つからない場合はnull）</returns>
        protected virtual Type GetImplType(Type baseType)
        {
            foreach (var detector in _detectors)
            {
                var implType = detector.GetImplType(baseType);
                // 最初に有効な型が見つかった方を採用
                if (implType != null)
                {
                    return implType;
                }
            }

            // 実装型が見つからず、引数クラスがインスタンス生成可能であれば
            // 引数で指定された型を返す
            if (baseType.IsClass)
            {
                return baseType;
            }

            return null;
        }
    }
}
