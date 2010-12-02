using System;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Unit.Core;

namespace Seasar.Extension.Unit
{
    public class S2TestCase : S2TestCaseBase
    {
        private ICommandFactory _commandFactory;
        private IS2Container _container;
        public IS2Container Container
        {
            get { return _container; }
            set { _container = value; }
        }

        protected override ICommandFactory CommandFactory
        {
            get
            {
                if (_commandFactory == null && Container.HasComponentDef(typeof(ICommandFactory)))
                {
                    _commandFactory = Container.GetComponent(typeof(ICommandFactory)) as ICommandFactory;
                }
                if (_commandFactory == null)
                {
                    _commandFactory = BasicCommandFactory.INSTANCE;
                }
                return _commandFactory;
            }
        }

        public object GetComponent(string componentName)
        {
            return _container.GetComponent(componentName);
        }

        public object GetComponent(Type componentClass)
        {
            return _container.GetComponent(componentClass);
        }

        public IComponentDef GetComponentDef(string componentName)
        {
            return _container.GetComponentDef(componentName);
        }

        public IComponentDef GetComponentDef(Type componentClass)
        {
            return _container.GetComponentDef(componentClass);
        }

        public void Register(Type componentClass)
        {
            _container.Register(componentClass);
        }

        public void Register(Type componentClass, string componentName)
        {
            _container.Register(componentClass, componentName);
        }

        public void Register(object component)
        {
            _container.Register(component);
        }

        public void Register(object component, string componentName)
        {
            _container.Register(component, componentName);
        }

        public void Register(IComponentDef componentDef)
        {
            _container.Register(componentDef);
        }

        public void Include(string path)
        {
            S2ContainerFactory.Include(Container, S2TestUtils.ConvertPath(GetType(), path));
        }
    }
}
