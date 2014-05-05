using Seasar.Quill.Attr;
using Seasar.Quill.Container;
using Seasar.Quill.Container.Impl;
using System;
using System.Reflection;

namespace Seasar.Quill.Injection.Impl
{
    public class QuillInjector : AbstractQuillInjector
    {
        #region static
        private static IQuillInjector _injector = null;

        public static void Init()
        {
            var builder = new DefaultQuillContainerBuilder();
            _injector = new QuillInjector(builder.Build());
        }

        public static void Init(IQuillContainerBuilder builder)
        {
            _injector = new QuillInjector(builder.Build());
        }

        public static void Init(IQuillContainer container)
        {
            _injector = new QuillInjector(container);
        }

        public static IQuillInjector GetInstance()
        {
            return _injector;
        }
        #endregion

        private QuillInjector(IQuillContainer container) : base(container)
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
