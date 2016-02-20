using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Quill.Exception;
using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Container.Impl {
    /// <summary>
    /// 既定のコンポーネント生成クラス
    /// </summary>
    public class ComponentCreators : IComponentCreator {
        /// <summary>
        /// コンポーネント型と生成処理の紐づけDictionary
        /// </summary>
        private readonly IDictionary<Type, Func<Type, object>> _creatorMap
            = new Dictionary<Type, Func<Type, object>>();

        /// <summary>
        /// コンポーネントの生成
        /// </summary>
        /// <param name="componentType">コンポーネント型(NotNull)</param>
        /// <returns>生成したコンポーネント</returns>
        public virtual object Create(Type componentType) {
            if(componentType == null) {
                throw new ArgumentNullException("componentType");
            }

            QM.OutputLog("CompornentCreators#Create", EnumMsgCategory.INFO,
                    string.Format("ComponentType:[{0}], Creator:[{1}]", 
                    componentType.FullName,
                    _creatorMap.ContainsKey(componentType) ? "Defined" : "Default"));

            if(_creatorMap.ContainsKey(componentType)) {
                return _creatorMap[componentType](componentType);
            }

            // 特に生成処理が指定されていない型はそのままインスタンスを生成
            if(HasNoArgumentConstructor(componentType)) {
                return Activator.CreateInstance(componentType);
            }
            throw new QuillException(string.Format("{0}, componentType=[{1}]",
                QMsg.IllegalConstructor.Get(), componentType));
        }

        /// <summary>
        /// コンポーネント生成処理の追加（上書き）
        /// </summary>
        /// <param name="targetType">指定した生成処理を適用する型</param>
        /// <param name="func">生成処理</param>
        public virtual void AddCreator(Type targetType, Func<Type, object> func) {
            _creatorMap[targetType] = func;
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public virtual void Dispose() {
            _creatorMap.Clear();
        }

        /// <summary>
        /// コンポーネント型に引数なしコンストラクタが存在するか判定
        /// </summary>
        /// <param name="componentType">コンポーネント型</param>
        /// <returns>true:引数なしコンストラクタあり, false:なし</returns>
        protected virtual bool HasNoArgumentConstructor(Type componentType) {
            var constructors = componentType.GetConstructors();
            var noArgConstructors = constructors.Where(ci => ci.GetParameters().Length == 0);
            return noArgConstructors.Count() > 0;
        }
    }
}
