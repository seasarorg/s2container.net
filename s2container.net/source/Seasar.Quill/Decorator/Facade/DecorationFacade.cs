using Seasar.Quill.Util;
using System;

namespace Seasar.Quill.Decorator.Facade
{
    /// <summary>
    /// スコープ修飾起点クラス
    /// </summary>
    /// <typeparam name="DECORATOR"></typeparam>
    public static class DecorationFacade<DECORATOR> where DECORATOR : IScopeDecorator
    {
        /// <summary>
        /// ScopeDecorator取得コールバックの設定
        /// </summary>
        private static Func<Type, IScopeDecorator> _callbackGetScopeDecorator = GetDecotatorDefault;

        /// <summary>
        /// ScopeDecorator取得コールバックの設定
        /// </summary>
        /// <param name="callback"></param>
        public static void SetCallbackGetScopeDecorator(Func<Type, IScopeDecorator> callback)
        {
            _callbackGetScopeDecorator = callback;
        }

        #region Func
        /// <summary>
        /// スコープ修飾
        /// </summary>
        /// <typeparam name="RETURN_TYPE">修飾メソッド戻り値型</typeparam>
        /// <param name="f">修飾メソッド</param>
        /// <returns>修飾メソッド戻り値</returns>
        public static RETURN_TYPE Decorate<RETURN_TYPE>(Func<RETURN_TYPE> f)
        {
            return DoDecorate(typeof(DECORATOR), () => f());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <param name="f"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public static RETURN_TYPE Decorate<RETURN_TYPE, ARG1_TYPE>(Func<ARG1_TYPE, RETURN_TYPE> f, ARG1_TYPE arg1)
        {
            return DoDecorate(typeof(DECORATOR), () => f(arg1), arg1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <param name="f"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public static RETURN_TYPE Decorate<RETURN_TYPE, ARG1_TYPE, ARG2_TYPE>(
            Func<ARG1_TYPE, ARG2_TYPE, RETURN_TYPE> f,
            ARG1_TYPE arg1, ARG2_TYPE arg2)
        {
            return DoDecorate(typeof(DECORATOR), () => f(arg1, arg2), arg1, arg2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <typeparam name="ARG3_TYPE"></typeparam>
        /// <param name="f"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public static RETURN_TYPE Decorate<RETURN_TYPE, ARG1_TYPE, ARG2_TYPE, ARG3_TYPE>(
           Func<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, RETURN_TYPE> f,
           ARG1_TYPE arg1, ARG2_TYPE arg2, ARG3_TYPE arg3)
        {
            return DoDecorate(typeof(DECORATOR), () => f(arg1, arg2, arg3), arg1, arg2, arg3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <typeparam name="ARG3_TYPE"></typeparam>
        /// <typeparam name="ARG4_TYPE"></typeparam>
        /// <param name="f"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <returns></returns>
        public static RETURN_TYPE Decorate<RETURN_TYPE, ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE>(
           Func<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE, RETURN_TYPE> f,
           ARG1_TYPE arg1, ARG2_TYPE arg2, ARG3_TYPE arg3, ARG4_TYPE arg4)
        {
            return DoDecorate(typeof(DECORATOR), () => f(arg1, arg2, arg3, arg4), arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <typeparam name="ARG3_TYPE"></typeparam>
        /// <typeparam name="ARG4_TYPE"></typeparam>
        /// <typeparam name="ARG5_TYPE"></typeparam>
        /// <param name="f"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <returns></returns>
        public static RETURN_TYPE Decorate<RETURN_TYPE, ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE, ARG5_TYPE>(
           Func<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE, ARG5_TYPE, RETURN_TYPE> f,
           ARG1_TYPE arg1, ARG2_TYPE arg2, ARG3_TYPE arg3, ARG4_TYPE arg4, ARG5_TYPE arg5)
        {
            return DoDecorate(typeof(DECORATOR), () => f(arg1, arg2, arg3, arg4, arg5), arg1, arg2, arg3, arg4, arg5);
        }
        #endregion

        #region Action

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        public static void Decorate(Action a)
        {
            DoDecorate(typeof(DECORATOR), () => { a(); return default(object); });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <param name="a"></param>
        /// <param name="arg1"></param>
        public static void Decorate<ARG1_TYPE>(Action<ARG1_TYPE> a, ARG1_TYPE arg1)
        {
            DoDecorate(typeof(DECORATOR), () => { a(arg1); return default(object); }, arg1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <param name="a"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Decorate<ARG1_TYPE, ARG2_TYPE>(Action<ARG1_TYPE, ARG2_TYPE> a, ARG1_TYPE arg1, ARG2_TYPE arg2)
        {
            DoDecorate(typeof(DECORATOR), () => { a(arg1, arg2); return default(object); }, arg1, arg2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <typeparam name="ARG3_TYPE"></typeparam>
        /// <param name="a"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public static void Decorate<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE>(Action<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE> a,
            ARG1_TYPE arg1, ARG2_TYPE arg2, ARG3_TYPE arg3)
        {
            DoDecorate(typeof(DECORATOR), () => { a(arg1, arg2, arg3); return default(object); },
                arg1, arg2, arg3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <typeparam name="ARG3_TYPE"></typeparam>
        /// <typeparam name="ARG4_TYPE"></typeparam>
        /// <param name="a"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        public static void Decorate<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE>(
            Action<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE> a,
            ARG1_TYPE arg1, ARG2_TYPE arg2, ARG3_TYPE arg3, ARG4_TYPE arg4)
        {
            DoDecorate(typeof(DECORATOR), () => { a(arg1, arg2, arg3, arg4); return default(object); },
                arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ARG1_TYPE"></typeparam>
        /// <typeparam name="ARG2_TYPE"></typeparam>
        /// <typeparam name="ARG3_TYPE"></typeparam>
        /// <typeparam name="ARG4_TYPE"></typeparam>
        /// <typeparam name="ARG5_TYPE"></typeparam>
        /// <param name="a"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        public static void Decorate<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE, ARG5_TYPE>(
            Action<ARG1_TYPE, ARG2_TYPE, ARG3_TYPE, ARG4_TYPE, ARG5_TYPE> a,
            ARG1_TYPE arg1, ARG2_TYPE arg2, ARG3_TYPE arg3, ARG4_TYPE arg4, ARG5_TYPE arg5)
        {
            DoDecorate(typeof(DECORATOR), () => { a(arg1, arg2, arg3, arg4, arg5); return default(object); },
                arg1, arg2, arg3, arg4, arg5);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="decoratorType"></param>
        /// <param name="f"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static RETURN_TYPE DoDecorate<RETURN_TYPE>(Type decoratorType, Func<RETURN_TYPE> f, params object[] parameters)
        {
            var decorator = _callbackGetScopeDecorator(decoratorType);
            return decorator.Execute(f, parameters);
        }

        /// <summary>
        /// ScopeDecorator取得コールバックの設定既定処理
        /// </summary>
        /// <param name="decoratorType"></param>
        /// <returns></returns>
        private static IScopeDecorator GetDecotatorDefault(Type decoratorType)
        {
            return (IScopeDecorator)SingletonInstances.GetInstance<QuillContainer>().GetComponent(decoratorType);
        }
    }
}
