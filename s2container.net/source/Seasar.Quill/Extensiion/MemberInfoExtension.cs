using System;
using System.Linq;
using System.Reflection;

namespace Seasar.Quill.Extensiion
{
    /// <summary>
    /// MembetInfo拡張定義
    /// </summary>
    public static class MemberInfoExtension
    {
        /// <summary>
        /// 属性が設定されているか判定
        /// </summary>
        /// <typeparam name="ATTRIBUTE">判定する属性</typeparam>
        /// <param name="memberInfo">判定対象のMemberInfo</param>
        /// <returns>true:設定されている, false:設定されていない</returns>
        public static bool HasAttribute<ATTRIBUTE>(this MemberInfo memberInfo) where ATTRIBUTE : Attribute
        {
            var attrs = memberInfo.GetCustomAttributes<ATTRIBUTE>(false);
            if (attrs != null && attrs.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
