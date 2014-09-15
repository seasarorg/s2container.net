using Seasar.Quill.Attr;
using System;

namespace Seasar.Quill.Container.Impl.Detector
{
    /// <summary>
    /// Implementation属性から実装型を走査するクラス
    /// </summary>
    public class AttributeImplTypeDetector : IImplTypeDetector
    {
        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="baseType">走査元の型</param>
        /// <returns>実装型</returns>
        public virtual Type GetImplType(Type currentType)
        {
            ImplementationAttribute implAttr = GetImplementationAttribute(currentType);
            if (implAttr != null)
            {
                return implAttr.ImplType;
            }

            // 実装クラスが見つからない場合はnullを返す
            return null;
        }

        /// <summary>
        /// Implemntation属性情報の取得
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        protected virtual ImplementationAttribute GetImplementationAttribute(Type baseType)
        {
            object[] attrs = baseType.GetCustomAttributes(typeof(ImplementationAttribute), false);
            if (attrs == null || attrs.Length == 0)
            {
                return null;
            }
            return (ImplementationAttribute)attrs[0];
        }
    }
}
