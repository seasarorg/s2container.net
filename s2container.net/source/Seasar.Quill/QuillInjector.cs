using Seasar.Quill.Attr;
using Seasar.Quill.Exception;
using Seasar.Quill.Typical.Creation;
using Seasar.Quill.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Seasar.Quill
{
    /// <summary>
    /// インジェクション実行クラス
    /// </summary>
    public class QuillInjector
    {
        // ====================================================================================================
        #region Callback
        /// <summary>
        /// インジェクション開始コールバック
        /// （引数　：インジェクションを行うオブジェクト）
        /// （　　　　インジェクション状態）
        /// （戻り値：なし）
        /// </summary>
        public Action<object, QuillInjectionContext> OnInjectHandler { get; set; }

        /// <summary>
        /// フィールド抽出処理
        /// （引数　：抽出対象の型）
        /// （　　　　インジェクション状態オブジェクト）
        /// （戻り値：抽出したフィールドリスト）
        /// </summary>
        public Func<Type, QuillInjectionContext, IEnumerable<FieldInfo>> SelectTargetFieldsCallback { get; set; }

        /// <summary>
        /// インジェクション実行
        /// （引数　：インジェクションを行うオブジェクト）
        /// （　　　　インジェクションを行うフィールド）
        /// （　　　　インジェクション状態）
        /// （戻り値：設定したオブジェクト）
        /// </summary>
        public Func<object, FieldInfo, QuillInjectionContext, object> DoInjectCallback { get; set; }

        /// <summary>
        /// フィールドへのインジェクション終了コールバック
        /// （引数　：設定したコンポーネント）
        /// （　　　　インジェクション状態）
        /// （　　　　インジェクション処理コールバック）
        /// （戻り値：なし）
        /// </summary>
        public Action<object, QuillInjectionContext, Action<object, QuillInjectionContext>> OnFieldInjectedHandler { get; set; }

        /// <summary>
        /// インジェクション終了コールバック
        /// （引数　：インジェクションを行うオブジェクト）
        /// （　　　　インジェクション状態）
        /// （戻り値：なし）
        /// </summary>
        public Action<object, QuillInjectionContext> OnInjectedHandler { get; set; }

        /// <summary>
        /// インジェクション失敗時に呼び出されるコールバック
        /// </summary>
        public Action<object, QuillInjectionContext, System.Exception> OnErrorHandler { get; set; }

        #endregion

        // ====================================================================================================
        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public QuillInjector()
        {
            OnInjectHandler = OnInjectDefault;
            SelectTargetFieldsCallback = SelectFieldDefault;
            OnFieldInjectedHandler = OnFieldInjectedDefault;
            DoInjectCallback = DoInjectDefault;
            OnInjectedHandler = OnInjectedDefault;
            OnErrorHandler = OnFailureDefault;
        }

        #endregion

        // ====================================================================================================
        /// <summary>
        /// インジェクション実行（直列）
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態</param>
        public virtual void Inject(object target, QuillInjectionContext context = null)
        {
            Inject(target, context, (targetFields, actualContext) =>
            {
                // インジェクション実行
                var components = new List<object>(targetFields.Count());
                targetFields.ForEach(fieldInfo =>
                {
                    var component = DoInjectCallback(target, fieldInfo, actualContext);
                    // 子フィールドに対して再帰的にインジェクションを実行していく
                    OnFieldInjectedHandler(component, actualContext, Inject);
                });
            });
        }

        /// <summary>
        /// インジェクション実行（並列）※QuillContainerがsingletonであること前提です
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態</param>
        public virtual void InjectAsParallel(object target, QuillInjectionContext context = null)
        {
            Inject(target, context, (targetFields, actualContext) =>
            {
                // 各フィールドへの設定を並列に実行
                var components = new ConcurrentBag<object>();
                targetFields.AsParallel().ForAll(fieldInfo =>
                {
                    var component = DoInjectCallback(target, fieldInfo, actualContext);
                    components.Add(component);
                });

                // インジェクション済の型管理にずれが生じる可能性があるので子フィールドへの再帰処理は並列化しない
                foreach(var component in components)
                {
                    OnFieldInjectedHandler(component, actualContext, InjectAsParallel);
                }
            });
        }

        /// <summary>
        /// インジェクション実行
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態</param>
        /// <param name="injectCallback">インジェクション委譲処理</param>
        public virtual void Inject(object target, QuillInjectionContext context, Action<IEnumerable<FieldInfo>, QuillInjectionContext> injectCallback)
        {
            if (target == null) { throw new ArgumentNullException("target"); }

            var actualContext = (context == null ? GetDefaultContext() : context);
            var targetType = target.GetType();

            // インジェクション開始
            OnInjectHandler(target, actualContext);
            actualContext.BeginInjection(targetType);

            try
            {
                // インジェクション実行
                var targetFields = SelectTargetFieldsCallback(targetType, actualContext);
                injectCallback(targetFields, actualContext);
            }
            catch (System.Exception ex)
            {
                OnErrorHandler(target, actualContext, ex);
            }
            finally
            {
                // インジェクション終了
                actualContext.EndInjection();
                OnInjectedHandler(target, actualContext);
            }
        }

        /// <summary>
        /// インジェクション済コンポーネントの取得（直列処理）
        /// </summary>
        /// <typeparam name="COMPONENT">コンポーネントの型</typeparam>
        /// <param name="context">インジェクション状態</param>
        /// <returns>インジェクション済のコンポーネント</returns>
        public virtual COMPONENT GetInjectedComponent<COMPONENT>(QuillInjectionContext context = null)
        {
            return GetInjectedComponent<COMPONENT>(context, Inject);
        }

        /// <summary>
        /// インジェクション済コンポーネントの取得（並列処理） ※QuillContainerがsingletonであること前提です
        /// </summary>
        /// <typeparam name="COMPONENT">コンポーネントの型</typeparam>
        /// <param name="context">インジェクション状態</param>
        /// <returns>インジェクション済のコンポーネント</returns>
        public virtual COMPONENT GetInjectedComponentAsParallel<COMPONENT>(QuillInjectionContext context = null)
        {
            return GetInjectedComponent<COMPONENT>(context, InjectAsParallel);
        }

        // ====================================================================================================
        #region Helper

        /// <summary>
        /// インジェクション済コンポーネントの取得
        /// </summary>
        /// <typeparam name="COMPONENT">コンポーネントの型</typeparam>
        /// <param name="context">インジェクション状態</param>
        /// <param name="injectionInvoker">インジェクション委譲処理</param>
        /// <returns>インジェクション済のコンポーネント</returns>
        protected virtual COMPONENT GetInjectedComponent<COMPONENT>(QuillInjectionContext context, Action<object, QuillInjectionContext> injectionInvoker)
        {
            var actualContext = (context == null ? GetDefaultContext() : context);
            var component = actualContext.Container.GetComponent<COMPONENT>();
            injectionInvoker(component, actualContext);
            return component;
        }

        /// <summary>
        /// 既定のインジェクション状態オブジェクトを取得
        /// </summary>
        /// <returns>インジェクション状態</returns>
        private QuillInjectionContext GetDefaultContext()
        {
            return new QuillInjectionContext(); 
        }
        #endregion

        // ====================================================================================================
        #region Default callback methods

        /// <summary>
        /// インジェクション開始既定処理
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態</param>
        protected virtual void OnInjectDefault(object target, QuillInjectionContext context) { /* デフォルトでは処理なし */ }

        /// <summary>
        /// インジェクション対象抽出既定処理
        /// </summary>
        /// <param name="targetType">インジェクション対象の型</param>
        /// <param name="context">インジェクション状態</param>
        /// <returns>インジェクション対象のフィールドリスト</returns>
        protected virtual IEnumerable<FieldInfo> SelectFieldDefault(Type targetType, QuillInjectionContext context)
        {
            var targetFields = targetType.GetFields(context.Condition);
            // Implementation属性が設定されている型のみ対象とする
            return targetFields.Where(fieldInfo => fieldInfo.FieldType.IsImplementationAttrAttached());
        }

        /// <summary>
        /// インジェクション既定処理
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual object DoInjectDefault(object target, FieldInfo fieldInfo, QuillInjectionContext context)
        {
            var component = context.Container.GetComponent(fieldInfo.FieldType);
            fieldInfo.SetValue(target, component);
            return component;
        }

        /// <summary>
        /// フィールドへのインジェクション終了既定処理
        /// </summary>
        /// <param name="component">フィールドに設定したコンポーネント</param>
        /// <param name="context">インジェクション状態</param>
        /// <param name="injectionInvoke">インジェクション委譲処理</param>
        protected virtual void OnFieldInjectedDefault(object component, QuillInjectionContext context, Action<object, QuillInjectionContext> injectionInvoke)
        {
            var componentType = component.GetType();
            if (!context.IsAlreadyInjected(componentType))
            {
                // インジェクション済の型でなければ再帰的にインジェクション処理を呼び出す
                injectionInvoke(component, context);
            }
        }

        /// <summary>
        /// インジェクション終了既定処理
        /// </summary>
        /// <param name="target"></param>
        /// <param name="context"></param>
        private void OnInjectedDefault(object target, QuillInjectionContext context) { /* デフォルトでは処理なし */ }

        /// <summary>
        /// インジェクション失敗既定処理
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態</param>
        /// <param name="ex">発生例外</param>
        private void OnFailureDefault(object target, QuillInjectionContext context, System.Exception ex)
        {
            // TODO 例外メッセージ詳細設定
            throw new QuillApplicationException("");
        }
        #endregion
    }
}
