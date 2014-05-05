using System;

namespace Seasar.Quill.Attr
{
    /// <summary>
    /// Aspectを指定する属性クラス
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AspectAttribute : Attribute
    {
        private readonly Type _interceptorType;

        public virtual Type InterceptorType
        {
            get { return _interceptorType; }
        }

        private readonly int _ordinal;

        /// <summary>
        /// Interceptorの適用順
        /// </summary>
        public virtual int Ordinal
        {
            get { return _ordinal; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interceptorType">適用するInterceptor</param>
        /// <param name="ordinal">適用順</param>
        public AspectAttribute(Type interceptorType, int ordinal)
        {
            _interceptorType = interceptorType;
            _ordinal = ordinal;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interceptorType">適用するInterceptor</param>
        public AspectAttribute(Type interceptorType) : this(interceptorType, 0)
        {  }

    }  
}
