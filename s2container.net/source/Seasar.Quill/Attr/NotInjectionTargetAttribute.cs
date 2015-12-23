using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quill.Attr {
    /// <summary>
    /// インジェクション除外属性
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class NotInjectionTargetAttribute : Attribute {

    }
}
