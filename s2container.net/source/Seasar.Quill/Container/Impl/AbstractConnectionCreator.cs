using System;
using System.Data;

namespace Quill.Container.Impl {
    /// <summary>
    /// DBコネクション生成抽象クラス
    /// </summary>
    public abstract class AbstractConnectionCreator : IComponentCreator {
        /// <summary>
        /// インスタンス生成
        /// </summary>
        /// <param name="componentType">コンポーネント型(NotNull)</param>
        /// <returns>生成したインスタンス</returns>
        public virtual object Create(Type componentType) {
            if(componentType == null) {
                throw new ArgumentNullException("componentType");
            }

            if(!typeof(IDbConnection).IsAssignableFrom(componentType)) {
                throw new ArgumentException(componentType.FullName, "componentType");
            }

            return CreateConnection(componentType);
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public virtual void Dispose() {
            // 保持リソースがないため、実装なし
        }

        /// <summary>
        /// コネクションインスタンスの生成
        /// </summary>
        /// <param name="connectionType"></param>
        /// <returns></returns>
        protected abstract IDbConnection CreateConnection(Type connectionType);
    }
}
