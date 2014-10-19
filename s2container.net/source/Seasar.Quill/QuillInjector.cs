using Seasar.Quill.Custom;
using Seasar.Quill.Exception;
using Seasar.Quill.Preset.FieldSelect;
using Seasar.Quill.Preset.ForEach;
using Seasar.Quill.Preset.Handler;
using Seasar.Quill.Preset.Injection;
using Seasar.Quill.Preset.Scope;
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

        private const string MSG_START_INJECTION = "QuillInjector#Inject start.";
        private const string MSG_END_INJECTION = "QuillInjector#Inject end.";

        private const string MSG_FIELD_NOT_FOUND = "Injection target is not found in the target.";
        private const string MSG_INJECTOR_NOT_FOUND = "Injection invoker is not found.";
        private const string MSG_FIELD_FOR_EACH_NOT_FOUND = "FieldForEach method is not found.";
        private const string MSG_QUILL_APP_EX_HANDLER_NOT_FOUND = "Quill application exception handler is not found.";
        private const string MSG_SYS_EX_HANDLER_NOT_FOUND = "System exception handler is not found.";

        #endregion

        // ====================================================================================================
        #region Callback, Handler

        public virtual Action<string> Log { protected get; set; }
        public virtual IFieldSelector FieldSelector { protected get; set; }
        public virtual IFieldInjector FieldInjector { protected get; set; }
        public virtual IFieldForEach FieldForEach { protected get; set; }
        public virtual IQuillApplicationExceptionHandler QuillApplicationExceptionHandler { protected get; set; }
        public virtual ISystemExceptionHandler SystemExceptionHandler { protected get; set; }

        #endregion

        // ====================================================================================================
        #region delegate
        public delegate IEnumerable<FieldInfo> CallbackSelectField(object target, QuillInjectionContext context);

        /// <summary>
        /// フィールドへのインジェクションデリゲート
        /// </summary>
        /// <param name="target">設定するオブジェクト</param>
        /// <param name="fieldInfo">設定するフィールド</param>
        /// <param name="context">インジェクション状態</param>
        public delegate void CallbackInjectField(object target, FieldInfo fieldInfo, QuillInjectionContext context);
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

        public virtual INJECTED_COMPONENT GetInjectedComponent<INJECTED_COMPONENT>(
            QuillContainer container = null,
            QuillInjectionContext context = null,
            CallbackSelectField callbackSelectField = null,
            CallbackInjectField callbackInjectField = null,
            CallbackFieldForEach callbackFieldForEach = null,
            HandleQuillApplicationException handleQuillApplicationException = null,
            HandleSystemException handleSystemException = null)
        {
            var actualContainer = (container == null ? PreparedSingletonFactory.GetInstance<QuillContainer>() : container);
            var component = actualContainer.GetComponent<INJECTED_COMPONENT>();
            Inject(component, context, callbackSelectField, callbackInjectField, callbackFieldForEach, 
                handleQuillApplicationException, handleSystemException);
            return component;
        }

        // ====================================================================================================
        #region Support method

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

        protected virtual IEnumerable<FieldInfo> SelectField(object target, QuillInjectionContext context, CallbackSelectField callback)
        {
            return callback(target, context);
        }

        protected virtual void InjectFields(object target, QuillInjectionContext context, IEnumerable<FieldInfo> fieldInfos,
            CallbackFieldForEach callbackFieldForEach, CallbackInjectField callbackInjectField)
        {
            callbackFieldForEach(target, context, fieldInfos, callbackInjectField);
        }

        protected virtual void InjectRecursive(FieldInfo fieldInfo, QuillInjectionContext context, 
            CallbackSelectField callbackSelectField,
            CallbackInjectField callbackInjectField,
            CallbackFieldForEach callbackFieldForEach,
            HandleQuillApplicationException handleQuillApplicationException,
            HandleSystemException handleSystemException)
        {
            if (!context.IsAlreadyInjected(fieldInfo.GetType()))
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
