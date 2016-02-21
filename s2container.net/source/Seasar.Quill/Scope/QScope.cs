using System;
using QM = Quill.QuillManager;

namespace Quill.Scope {
    /// <summary>
    /// Quillスコープ修飾クラス
    /// </summary>
    /// <typeparam name="DECORATOR_TYPE"></typeparam>
    public static class QScope<DECORATOR_TYPE> where DECORATOR_TYPE : class, IQuillDecorator {
        /// <summary>
        /// 特定の修飾処理を付与して実行
        /// </summary>
        /// <param name="action"></param>
        public static void Execute(Action action) {
            var decorator = QM.Container.GetComponent<DECORATOR_TYPE>(withInjection: true);
            decorator.Decorate(action);
        }

        /// <summary>
        /// 特定の修飾処理を付与して実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static RETURN_TYPE Execute<RETURN_TYPE>(Func<RETURN_TYPE> func) {
            
            var decorator = QM.Container.GetComponent<DECORATOR_TYPE>(withInjection: true);
            return decorator.Decorate(func);
        }
    }

    /// <summary>
    /// Quillスコープ修飾クラス
    /// </summary>
    /// <typeparam name="DECORATOR_TYPE"></typeparam>
    /// <typeparam name="PARAMETER_TYPE"></typeparam>
    public static class QScope<DECORATOR_TYPE, PARAMETER_TYPE> where DECORATOR_TYPE : class, IQuillDecorator<PARAMETER_TYPE> {
        /// <summary>
        /// 特定の修飾処理を付与して実行
        /// </summary>
        /// <param name="action"></param>
        public static void Execute(Action<PARAMETER_TYPE> action) {
            var decorator = QM.Container.GetComponent<DECORATOR_TYPE>(withInjection: true);
            decorator.Decorate(action);
        }

        /// <summary>
        /// 特定の修飾処理を付与して実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static RETURN_TYPE Execute<RETURN_TYPE>(Func<PARAMETER_TYPE, RETURN_TYPE> func) {
            var decorator = QM.Container.GetComponent<DECORATOR_TYPE>(withInjection: true);
            return decorator.Decorate(func);
        }
    }
}
