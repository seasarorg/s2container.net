using Seasar.Quill.Attr;
using Seasar.Quill.Container;
using Seasar.Quill.Container.Impl;
using System;
using System.Reflection;

namespace Seasar.Quill.Injection.Impl
{
    public class QuillInjector : AbstractQuillInjector
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container"></param>
        internal QuillInjector(IQuillContainer container) : base(container)
        { }

        protected override BindingFlags GetFieldFilter()
        {
            return (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        protected override bool IsRecursive()
        {
            return true;
        }
    }
}
