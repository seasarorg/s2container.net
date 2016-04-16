using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Quill.Exception;
using Quill.Message;
using Quill.Util;
using QM = Quill.QuillManager;

namespace Quill.Inject.Impl {
    /// <summary>
    /// Quill Injection実行クラス
    /// </summary>
    public class QuillInjector : IQuillInjector {
        /// <summary>
        /// Injection済の型セット
        /// </summary>
        private readonly ISet<Type> _injectedTypes = new HashSet<Type>();

        /// <summary>
        /// Injection実行
        /// </summary>
        /// <param name="target">Injection対象オブジェクト</param>
        public virtual void Inject(object target) {
            if(target == null) {
                throw new QuillException("Injection target is null.");
            }

            var targetType = target.GetType();

            if(_injectedTypes.Contains(targetType)) {
                QM.OutputLog(GetType(), EnumMsgCategory.INFO,
                    string.Format("{0} targetType=[{1}]",
                        QMsg.AlreadyInjected.Get(), targetType)); 
                return;
            }

            var fieldInfos = GetFields(target);
            // 処理対象フィールドの取得後、先にインジェクション済型に追加しておく
            // （無限ループを避けるため）
            _injectedTypes.Add(targetType);

            ForEachFields(fieldInfos, fieldInfo => {
                InjectToField(target, fieldInfo, targetType);
            });
        }

        private static void InjectToField(object target, FieldInfo fieldInfo, Type targetType) {
            var fieldType = fieldInfo.FieldType;

            // フィールドの型とインジェクション対象の型が同じ場合は
            // 自分自身のインスタンスを設定
            var component = target;
            if(fieldType != targetType) {
                component = QM.Container.GetComponent(fieldType, withInjection: true);
            }

            fieldInfo.SetValue(target, component);
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public virtual void Dispose() {
            _injectedTypes.Clear();
        }

        /// <summary>
        /// Injection対象クラスの各フィールド設定を実行
        /// </summary>
        /// <param name="fieldInfos">フィールド情報</param>
        /// <param name="setFieldInvoker">フィールド設定委譲処理</param>
        protected virtual void ForEachFields(IEnumerable<FieldInfo> fieldInfos, Action<FieldInfo> setFieldInvoker) {
            if(fieldInfos.Count() == 0) {
                return;
            }

            if(QM.Container.CanParallelInjection()) {
                // QuillContainerがConcurrentDictionaryを使っていることを前提に並列実行
                fieldInfos.AsParallel().ForAll(setFieldInvoker);
            } else {
                foreach(var field in fieldInfos) {
                    setFieldInvoker(field);
                }
            }
        }

        /// <summary>
        /// Injection対象フィールドの取得
        /// </summary>
        /// <param name="target">Injection対象オブジェクト</param>
        /// <returns>Injection対象フィールド</returns>
        protected virtual IEnumerable<FieldInfo> GetFields(object target) {
            var componentType = target.GetType();
            var fieldInfos = target.GetType().GetAllFields(QM.InjectionFilter.GetTargetFieldBindinFlags());
            return fieldInfos.Where(fieldInfo => QM.InjectionFilter.IsTargetField(componentType, fieldInfo));
        }
    }
}
