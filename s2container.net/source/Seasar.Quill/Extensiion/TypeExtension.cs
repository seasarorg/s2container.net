using Seasar.Quill.Attr;
using System;
using System.Linq;
using System.Reflection;
using Seasar.Quill.Exception;
using System.Collections.Generic;

namespace Seasar.Quill.Extensiion
{
    /// <summary>
    /// MemberInfoクラス拡張定義
    /// </summary>
    public static class TypeExtension
    {
        ///// <summary>
        ///// 適用順にソートされたAspect属性の取得
        ///// </summary>
        ///// <typeparam name="ATTR"></typeparam>
        ///// <param name="t"></param>
        ///// <returns>ソートされたAspect属性オブジェクト（見つからない場合はnull）</returns>
        //public static ATTR[] GetAspectAttributes<ATTR>(this Type target) where ATTR : AspectAttribute
        //{
        //    var aspectAttributes = new HashSet<ATTR>();
        //    AddAspectAttributes(aspectAttributes, target);
        //    foreach (var member in target.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        //    {
        //        AddAspectAttributes(aspectAttributes, member);
        //    }
        //    return aspectAttributes.ToArray();
        //}

        /// <summary>
        /// 型、または構成メンバーに指定属性が設定されているか判定
        /// </summary>
        /// <typeparam name="ATTR"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool HasAttribute<ATTR>(this Type target) where ATTR : Attribute
        {
            if (target.HasAttribute<ATTR>())
            {
                return true;
            }

            var result = target.GetMembers().Where(m => m.HasAttribute<ATTR>());
            if (result != null && result.Count() > 0)
            {
                return true;
            }
            return false;
        }

        

        ///// <summary>
        ///// 適用順にソートされたAspect属性の取得
        ///// </summary>
        ///// <typeparam name="ATTR"></typeparam>
        ///// <param name="t"></param>
        ///// <returns>ソートされたAspect属性オブジェクト（見つからない場合はnull）</returns>
        //public static IOrderedEnumerable<ATTR> GetAspectAttributes<ATTR>(this MemberInfo member) where ATTR : AspectAttribute
        //{
        //    var attrs = member.GetCustomAttributes(typeof(ATTR), false);
        //    if (attrs == null || attrs.Length == 0)
        //    {
        //        return null;
        //    }

        //    return ((ATTR[])attrs).OrderBy((source) => source.Ordinal);
        //}
    }
}
