using System;
using System.Collections.Generic;

namespace Seasar.Quill.Container.Impl.InstanceManager
{
    /// <summary>
    /// Singletonインスタンス管理クラス
    /// </summary>
    public class SingletonInstanceManager : AbstractInstanceManager
    {
        /// <summary>
        /// 管理対象のコンポーネントマップ
        /// </summary>
        private readonly IDictionary<Type, object> _instanceHolder = new Dictionary<Type, object>();

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="t">取得する型</param>
        /// <param name="createInvoker">インスタンス生成委譲オブジェクト</param>
        /// <returns>インスタンス</returns>
        protected override object GetInstance(Type t, Func<IComponentCreater, object> createInvoker)
        {
            if (!_instanceHolder.ContainsKey(t))
            {
                _instanceHolder.Add(t, base.GetInstance(t, createInvoker));
            }
            return _instanceHolder[t];
        }
    }
}
