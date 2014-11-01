using Seasar.Quill.Exception;
using Seasar.Quill.Parts.Handler;
using Seasar.Quill.Parts.Handler.Impl;
using Seasar.Quill.Parts.Injector.FieldForEach;
using Seasar.Quill.Parts.Injector.FieldForEach.Impl;
using Seasar.Quill.Parts.Injector.FieldInjector;
using Seasar.Quill.Parts.Injector.FieldInjector.Impl;
using Seasar.Quill.Parts.Injector.FieldSelector;
using Seasar.Quill.Parts.Injector.FieldSelector.Impl;
using Seasar.Quill.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill
{
    /// <summary>
    /// インジェクション実行クラス
    /// </summary>
    public class QuillInjector
    {
        // ====================================================================================================
        #region Consts

        /// <summary>
        /// メッセージ：インジェクション開始
        /// </summary>
        private const string MSG_START_INJECTION = "QuillInjector#Inject start.";

        /// <summary>
        /// メッセージ：インジェクション終了
        /// </summary>
        private const string MSG_END_INJECTION = "QuillInjector#Inject end.";

        /// <summary>
        /// メッセージ：インジェクション対象のフィールドが見つからない
        /// </summary>
        private const string MSG_FIELD_NOT_FOUND = "Injection target is not found in the target.";

        /// <summary>
        /// メッセージ：インジェクション処理コールバックが見つからない
        /// </summary>
        private const string MSG_INJECTOR_NOT_FOUND = "Injection invoker is not found.";

        /// <summary>
        /// メッセージ：抽出フィールドループ処理コールバックが見つからない
        /// </summary>
        private const string MSG_FIELD_FOR_EACH_NOT_FOUND = "FieldForEach method is not found.";

        /// <summary>
        /// メッセージ：QuillApplicationExceptionハンドラが見つからない
        /// </summary>
        private const string MSG_QUILL_APP_EX_HANDLER_NOT_FOUND = "Quill application exception handler is not found.";

        /// <summary>
        /// メッセージ：System.Exceptionハンドラが見つからない
        /// </summary>
        private const string MSG_SYS_EX_HANDLER_NOT_FOUND = "System exception handler is not found.";

        #endregion

        // ====================================================================================================
        #region Callback, Handler

        /// <summary>
        /// ログ出力コールバック
        /// </summary>
        public virtual Action<string> Log { protected get; set; }

        /// <summary>
        /// フィールド抽出
        /// </summary>
        public virtual IFieldSelector FieldSelector { protected get; set; }

        /// <summary>
        /// フィールドへのインジェクション
        /// </summary>
        public virtual IFieldInjector FieldInjector { protected get; set; }

        /// <summary>
        /// 抽出フィールドへのループ処理
        /// </summary>
        public virtual IFieldForEach FieldForEach { protected get; set; }

        /// <summary>
        /// QuillApplicationExceptionハンドラ
        /// </summary>
        public virtual IQuillApplicationExceptionHandler QuillApplicationExceptionHandler { protected get; set; }

        /// <summary>
        /// System.Exceptionハンドラ
        /// </summary>
        public virtual ISystemExceptionHandler SystemExceptionHandler { protected get; set; }

        #endregion

        // ====================================================================================================
        #region delegate
        /// <summary>
        /// デリゲート：インジェクション対象フィールド取得コールバック
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <returns>抽出したフィールド</returns>
        public delegate IEnumerable<FieldInfo> CallbackSelectField(object target, QuillInjectionContext context);
 
        /// <summary>
        /// デリゲート：フィールドへのインジェクションコールバック
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="fieldInfo">設定するフィールド</param>
        /// <param name="context">インジェクション状態</param>
        public delegate void CallbackInjectField(object target, FieldInfo fieldInfo, QuillInjectionContext context);

        /// <summary>
        /// デリゲート：抽出フィールドへのループ処理コールバック
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="fields">抽出したフィールド</param>
        /// <param name="callbackInjectField">フィールドへのインジェクションコールバック</param>
        public delegate void CallbackFieldForEach(object target, QuillInjectionContext context, IEnumerable<FieldInfo> fields, CallbackInjectField callbackInjectField);

        #endregion

        // ====================================================================================================
        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public QuillInjector()
        {
            // 既定の設定
            Log = (message => { /* ログ出力処理はデフォルトでは何もしない */ });
            FieldSelector = new ImplementationFieldSelector();
            FieldInjector = new FieldInjectorImpl();
            FieldForEach = new FieldForEachSerial();
            QuillApplicationExceptionHandler = new QuillApplicationExceptionHandlerImpl();
            SystemExceptionHandler = new SystemExceptionHandlerImpl();
        }

        #endregion

        // ====================================================================================================

        /// <summary>
        /// インジェクション実行
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="callbackSelectField">フィールド抽出 Func&lt;object,QuillInjectionContext,IEnumerable&lt;FieldInfo&gt;&gt;</param>
        /// <param name="callbackInjectField">インジェクション実行 Action&lt;object, FieldInfo, QuillInjectionContext&gt;</param>
        /// <param name="callbackFieldForEach">抽出したフィールドへのループ処理 Action&lt;object, QuillInjectionContext, IEnumerable&lt;FieldInfo&gt;, Action&lt;object, FieldInfo, QuillInjectionContext&gt;&gt;</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ Func&lt;QuillApplicationException, object&gt;</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ Func&lt;System.Exception, object&gt;</param>
        public virtual void Inject(object target, 
            QuillInjectionContext context = null, 
            CallbackSelectField callbackSelectField = null,
            CallbackInjectField callbackInjectField = null,
            CallbackFieldForEach callbackFieldForEach = null,
            HandleQuillApplicationException handleQuillApplicationException = null,
            HandleSystemException handleSystemException = null)
        {
            if (target == null) { throw new ArgumentNullException("target"); }
            InvokeInject(target,
                context == null ? GetDefaultContext() : context,
                LogicUtils.GetLogic(callbackSelectField, FieldSelector, f => f.Select, MSG_FIELD_NOT_FOUND),
                LogicUtils.GetLogic(callbackInjectField, FieldInjector, f => f.InjectField, MSG_INJECTOR_NOT_FOUND),
                LogicUtils.GetLogic(callbackFieldForEach, FieldForEach, f => f.ForEach, MSG_FIELD_FOR_EACH_NOT_FOUND),
                LogicUtils.GetLogic(handleQuillApplicationException, QuillApplicationExceptionHandler, handler => handler.Handle, MSG_QUILL_APP_EX_HANDLER_NOT_FOUND),
                LogicUtils.GetLogic(handleSystemException, SystemExceptionHandler, handler => handler.Handle, MSG_SYS_EX_HANDLER_NOT_FOUND));
        }

        /// <summary>
        /// インジェクション済コンポーネントの取得
        /// </summary>
        /// <typeparam name="INJECTED_COMPONENT">コンポーネントの型</typeparam>
        /// <param name="container">Quillコンテナ</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="callbackSelectField">フィールド抽出 Func&lt;object,QuillInjectionContext,IEnumerable&lt;FieldInfo&gt;&gt;</param>
        /// <param name="callbackInjectField">インジェクション実行 Action&lt;object, FieldInfo, QuillInjectionContext&gt;</param>
        /// <param name="callbackFieldForEach">抽出したフィールドへのループ処理 Action&lt;object, QuillInjectionContext, IEnumerable&lt;FieldInfo&gt;, Action&lt;object, FieldInfo, QuillInjectionContext&gt;&gt;</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ Func&lt;QuillApplicationException, object&gt;</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ Func&lt;System.Exception, object&gt;</param>
        /// <returns>インジェクション済のコンポーネント</returns>
        public virtual INJECTED_COMPONENT GetInjectedComponent<INJECTED_COMPONENT>(
            QuillContainer container = null,
            QuillInjectionContext context = null,
            CallbackSelectField callbackSelectField = null,
            CallbackInjectField callbackInjectField = null,
            CallbackFieldForEach callbackFieldForEach = null,
            HandleQuillApplicationException handleQuillApplicationException = null,
            HandleSystemException handleSystemException = null)
        {
            var actualContainer = (container == null ? SingletonInstances.GetInstance<QuillContainer>() : container);
            var component = actualContainer.GetComponent<INJECTED_COMPONENT>();
            Inject(component, context, callbackSelectField, callbackInjectField, callbackFieldForEach, 
                handleQuillApplicationException, handleSystemException);
            return component;
        }

        // ====================================================================================================
        #region Support method

        /// <summary>
        /// インジェクション実行
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="callbackSelectField">フィールド抽出 Func&lt;object,QuillInjectionContext,IEnumerable&lt;FieldInfo&gt;&gt;</param>
        /// <param name="callbackInjectField">インジェクション実行 Action&lt;object, FieldInfo, QuillInjectionContext&gt;</param>
        /// <param name="callbackFieldForEach">抽出したフィールドへのループ処理 Action&lt;object, QuillInjectionContext, IEnumerable&lt;FieldInfo&gt;, Action&lt;object, FieldInfo, QuillInjectionContext&gt;&gt;</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ Func&lt;QuillApplicationException, object&gt;</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ Func&lt;System.Exception, object&gt;</param>
        protected virtual void InvokeInject(object target, QuillInjectionContext context,
            CallbackSelectField callbackSelectField,
            CallbackInjectField callbackInjectField,
            CallbackFieldForEach callbackFieldForEach,
            HandleQuillApplicationException handleQuillApplicationException,
            HandleSystemException handleSystemException)
        {
            Log(MSG_START_INJECTION);
            context.BeginInjection(target.GetType());
            try
            {
                // インジェクション実行
                var fieldInfos = SelectField(target, context, callbackSelectField);
                InjectFields(target, context, fieldInfos, callbackFieldForEach, callbackInjectField);

                // 再帰的なインジェクション実行は並列処理を許可せず、必ず直列で実行
                // （スレッドが無数に作られてしまう可能性があるため）
                foreach (var fieldInfo in fieldInfos)
                {
                    InjectRecursive(fieldInfo, context, callbackSelectField, callbackInjectField, callbackFieldForEach,
                        handleQuillApplicationException, handleSystemException);
                }
            }
            catch (QuillApplicationException qe)
            {
                handleQuillApplicationException(qe);
            }
            catch (System.Exception ex)
            {
                handleSystemException(ex);
            }
            finally
            {
                context.EndInjection();
                Log(MSG_END_INJECTION);
            }
        }

        /// <summary>
        /// インジェクション対象フィールド抽出
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="callback">インジェクション対象フィールド抽出コールバック</param>
        /// <returns>抽出したフィールド</returns>
        protected virtual IEnumerable<FieldInfo> SelectField(object target, QuillInjectionContext context, CallbackSelectField callback)
        {
            return callback(target, context);
        }

        /// <summary>
        /// 抽出した各フィールドへのインジェクション
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="fieldInfos">抽出したフィールド</param>
        /// <param name="callbackFieldForEach">抽出した各フィールドへのループ処理コールバック</param>
        /// <param name="callbackInjectField">フィールドへのインジェクション処理コールバック</param>
        protected virtual void InjectFields(object target, QuillInjectionContext context, IEnumerable<FieldInfo> fieldInfos,
            CallbackFieldForEach callbackFieldForEach, CallbackInjectField callbackInjectField)
        {
            callbackFieldForEach(target, context, fieldInfos, callbackInjectField);
        }

        /// <summary>
        /// 再帰的なインジェクション実行
        /// </summary>
        /// <param name="fieldInfo">フィールド情報</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="callbackSelectField">インジェクション対象フィールド抽出コールバック</param>
        /// <param name="callbackInjectField">フィールドへのインジェクションコールバック</param>
        /// <param name="callbackFieldForEach">抽出したフィールドへのループ処理コールバック</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ</param>
        protected virtual void InjectRecursive(FieldInfo fieldInfo, QuillInjectionContext context, 
            CallbackSelectField callbackSelectField,
            CallbackInjectField callbackInjectField,
            CallbackFieldForEach callbackFieldForEach,
            HandleQuillApplicationException handleQuillApplicationException,
            HandleSystemException handleSystemException)
        {
            var fieldType = fieldInfo.GetType();
            if (!context.IsAlreadyInjected(fieldType))
            {
                InvokeInject(context.Container.GetComponent(fieldInfo.FieldType), context,
                    callbackSelectField, callbackInjectField, callbackFieldForEach,
                    handleQuillApplicationException, handleSystemException);
            }
        }

        /// <summary>
        /// 既定のインジェクション状態オブジェクトを取得
        /// </summary>
        /// <returns>インジェクション状態</returns>
        protected virtual QuillInjectionContext GetDefaultContext()
        {
            return new QuillInjectionContext();
        }
        #endregion
    }
}
