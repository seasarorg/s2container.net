using System;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Unit.Core;

namespace Seasar.Quill.Unit
{
    public class QuillTestCase : S2TestCaseBase
    {
        public QuillContainer QContainer
        {
            get { return QuillInjector.GetInstance().Container; }
        }

        public QuillInjector Injector
        {
            get { return QuillInjector.GetInstance(); }
        }

        public object GetQuillComponent(Type componentClass)
        {
            var component = QContainer.GetComponent(componentClass);
            return component.GetComponentObject(componentClass);
        }

        public object GetQuillComponent(Type interfaceType, Type implType)
        {
            var component = QContainer.GetComponent(interfaceType, implType);
            return component.GetComponentObject(component.ReceiptType);
        }

        public virtual void Inject(object target)
        {
            Injector.Inject(target);
        }
    }
}
