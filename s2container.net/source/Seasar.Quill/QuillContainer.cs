using Seasar.Quill.Exception;
using Seasar.Quill.Parts.Container.ImplTypeFactory;
using Seasar.Quill.Parts.Container.ImplTypeFactory.Impl;
using Seasar.Quill.Parts.Container.InstanceFactory;
using Seasar.Quill.Parts.Container.InstanceFactory.Impl;
using Seasar.Quill.Parts.Handler;
using Seasar.Quill.Parts.Handler.Impl;
using Seasar.Quill.Util;
using System;

namespace Seasar.Quill
{
    /// <summary>
    /// Quillコンテナ
    /// </summary>
    public class QuillContainer : IDisposable
    {
        // ====================================================================================================
        #region Consts

        /// <summary>
        /// メッセージ：コンポーネント取得開始
        /// </summary>
        private const string MSG_START_GET_COMPONENT = "QuillContainer#GetComponent start.";

        /// <summary>
        /// メッセージ：コンポーネント取得終了
        /// </summary>
        private const string MSG_END_GET_COMPONENT = "QuillContainer#GetComponent end.";

        /// <summary>
        /// メッセージ：実装クラスが見つからない
        /// </summary>
        private const string MSG_IMPL_NOT_FOUND = "Implementation type is not found. ";

        /// <summary>
        /// メッセージ：コンポーネントが見つからない
        /// </summary>
        private const string MSG_COMPONENT_NOT_FOUND = "Component is not found.";

        /// <summary>
        /// メッセージ：実装型取得コールバックが見つからない
        /// </summary>
        private const string MSG_NOT_FOUND_CALLBACK_GET_IMPL_TYPE = "Callback(GetImplType) is not found.";

        /// <summary>
        /// メッセージ：インスタンス取得コールバックが見つからない
        /// </summary>
        private const string MSG_NOT_FOUND_CALLBACK_GET_INSTANCE = "Callback(GetInstance) is not found.";

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
        #region Factory, Handler, Callback

        /// <summary>
        /// ログ出力コールバック
        /// </summary>
        public virtual Action<string> Log { protected get; set; }
        
        /// <summary>
        /// QuillApplicationExceptionハンドラ
        /// </summary>
        public virtual IQuillApplicationExceptionHandler QuillApplicationExceptionHandler { protected get; set; }

        /// <summary>
        /// System.Exceptionハンドラ
        /// </summary>
        public virtual ISystemExceptionHandler SystemExceptionHandler { protected get; set; }

        /// <summary>
        /// 実装型取得ファクトリ
        /// </summary>
        private IImplTypeFactory _implTypeFactory;

        /// <summary>
        /// 実装型取得ファクトリ
        /// </summary>
        public virtual IImplTypeFactory ImplTypeFactory
        {
            get { return _implTypeFactory; }
            set
            {
                if (_implTypeFactory != null)
                {
                    _implTypeFactory.Dispose();
                }
                _implTypeFactory = value;
            }
        }

        /// <summary>
        /// インスタンス取得ファクトリ
        /// </summary>
        private IInstanceFactory _instanceFactory;

        /// <summary>
        /// インスタンス取得ファクトリ
        /// </summary>
        public virtual IInstanceFactory InstanceFactory
        {
            get { return _instanceFactory; }
            set
            {
                if (_instanceFactory != null)
                {
                    _instanceFactory.Dispose();
                }
                _instanceFactory = value;
            }
        }

        #endregion

        // ====================================================================================================
        #region Delegate

        /// <summary>
        /// デリゲート：実装型取得コールバック
        /// </summary>
        /// <param name="receiptType">受け取り側の型</param>
        /// <returns>実装型</returns>
        public delegate Type CallbackGetImplType(Type receiptType);

        /// <summary>
        /// デリゲート：インスタンス取得コールバック
        /// </summary>
        /// <param name="targetType">取得対象インスタンスの型</param>
        /// <returns>インスタンス</returns>
        public delegate object CallbackGetInstance(Type targetType);

        #endregion

        // ====================================================================================================
        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public QuillContainer()
        {
            Log = (message => { /* デフォルトでは出力処理なし */ });
            ImplTypeFactory = new ImplementationAttributeImplTypeFactory();
            InstanceFactory = new SingletonInstanceFactory();
            QuillApplicationExceptionHandler = new QuillApplicationExceptionHandlerImpl();
            SystemExceptionHandler = new SystemExceptionHandlerImpl();
        }

        #endregion

        // ====================================================================================================

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <typeparam name="COMPONENT_TYPE">コンポーネントの型</typeparam>
        /// <param name="callbackGetImplType">実装型取得コールバック</param>
        /// <param name="callbackGetInstance">インスタンス取得コールバック</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ</param>
        /// <returns>コンポーネント</returns>
        public virtual COMPONENT_TYPE GetComponent<COMPONENT_TYPE>(
            CallbackGetImplType callbackGetImplType = null, 
            CallbackGetInstance callbackGetInstance = null,
            HandleQuillApplicationException handleQuillApplicationException = null,
            HandleSystemException handleSystemException = null)
        {
            return (COMPONENT_TYPE)GetComponent(typeof(COMPONENT_TYPE), 
                callbackGetImplType, callbackGetInstance, 
                handleQuillApplicationException, handleSystemException);
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="receiptType">受取側の型</param>
        /// <param name="callbackGetImplType">実装型取得コールバック</param>
        /// <param name="callbackGetInstance">インスタンス取得コールバック</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ</param>
        /// <returns>コンポーネント</returns>
        public virtual object GetComponent(Type receiptType,
            CallbackGetImplType callbackGetImplType = null,
            CallbackGetInstance callbackGetInstance = null,
            HandleQuillApplicationException handleQuillApplicationException = null,
            HandleSystemException handleSystemException = null)
        {
            if (receiptType == null) { throw new ArgumentNullException("receiptType"); }
            return InvokeGetComponent(receiptType,
                LogicUtils.GetLogic(callbackGetImplType, ImplTypeFactory, f => f.GetImplType, MSG_NOT_FOUND_CALLBACK_GET_IMPL_TYPE),
                LogicUtils.GetLogic(callbackGetInstance, InstanceFactory, f => f.GetInstance, MSG_NOT_FOUND_CALLBACK_GET_INSTANCE),
                LogicUtils.GetLogic(handleQuillApplicationException, QuillApplicationExceptionHandler, f => f.Handle, MSG_QUILL_APP_EX_HANDLER_NOT_FOUND),
                LogicUtils.GetLogic(handleSystemException, SystemExceptionHandler, f => f.Handle, MSG_SYS_EX_HANDLER_NOT_FOUND));
        }

        /// <summary>
        /// オブジェクトの破棄
        /// </summary>
        public void Dispose()
        {
            if (ImplTypeFactory != null)
            {
                ImplTypeFactory.Dispose();
            }

            if (InstanceFactory != null)
            {
                InstanceFactory.Dispose();
            }
        }

        // ====================================================================================================
        #region Support method

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="receiptType">受取側の型</param>
        /// <param name="callbackGetImplType">実装型取得コールバック</param>
        /// <param name="callbackGetInstance">インスタンス取得コールバック</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ</param>
        /// <returns>コンポーネント</returns>
        protected virtual object InvokeGetComponent(Type receiptType,
            CallbackGetImplType callbackGetImplType,
            CallbackGetInstance callbackGetInstance,
            HandleQuillApplicationException handleQuillApplicationException,
            HandleSystemException handleSystemException)
        {
            Log(MSG_START_GET_COMPONENT);
            try
            {
                // 実装型の取得
                var implType = GetImplType(receiptType, callbackGetImplType);
                if (implType == null)
                {
                    // 実装型が見つからなかった場合は受取側の型を同じとする
                    Log(MSG_IMPL_NOT_FOUND);
                    implType = receiptType;
                }

                // コンポーネントの取得
                var component = GetInstance(implType, callbackGetInstance);
                if (component == null)
                {
                    Log(MSG_COMPONENT_NOT_FOUND);
                }
                return component;
            }
            catch (QuillApplicationException qe)
            {
                return HandleQuillApplicationException(qe, handleQuillApplicationException);
            }
            catch (System.Exception ex)
            {
                return HandleSystemException(ex, handleSystemException);
            }
            finally
            {
                Log(MSG_END_GET_COMPONENT);
            }
        }

        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="receiptType">受け取り側の型</param>
        /// <param name="callbackGetImplType">実装型取得コールバック</param>
        /// <returns>実装型</returns>
        protected virtual Type GetImplType(Type receiptType, CallbackGetImplType callbackGetImplType)
        {
            return callbackGetImplType(receiptType);
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="targetType">取得対象インスタンスの型</param>
        /// <param name="callbackGetInstance">インスタンス取得コールバック</param>
        /// <returns>インスタンス</returns>
        protected virtual object GetInstance(Type targetType, CallbackGetInstance callbackGetInstance)
        {
            return callbackGetInstance(targetType);
        }

        /// <summary>
        /// QuillApplicationException例外処理
        /// </summary>
        /// <param name="ex">発生例外</param>
        /// <param name="handleQuillApplicationException">QuillApplicationExceptionハンドラ</param>
        /// <returns>例外処理結果</returns>
        protected virtual object HandleQuillApplicationException(QuillApplicationException ex, HandleQuillApplicationException handleQuillApplicationException)
        {
            Log(string.Format("{0} : [{1}] is occured. Stack trace is [{2}].", ex.Message, ex.GetType().FullName, ex.StackTrace));
            return handleQuillApplicationException(ex);
        }

        /// <summary>
        /// System.Exception例外処理
        /// </summary>
        /// <param name="ex">発生例外</param>
        /// <param name="handleSystemException">System.Exceptionハンドラ</param>
        /// <returns>例外処理結果</returns>
        protected virtual object HandleSystemException(System.Exception ex, HandleSystemException handleSystemException)
        {
            Log(string.Format("{0} : [{1}] is occured. Stack trace is [{2}].", ex.Message, ex.GetType().FullName, ex.StackTrace));
            return handleSystemException(ex);
        }

        #endregion
    }
}
