#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System;
using System.Collections;
using Seasar.Framework.Container.Util;
using Seasar.Framework.Container.Deployer;
using Seasar.Framework.Beans;

namespace Seasar.Framework.Container.Impl
{
    public class ComponentDefImpl : IComponentDef
    {
        private readonly Type _componentType;
        private readonly string _componentName;
        private IS2Container _container;
        private string _expression;
        private readonly ArgDefSupport _argDefSupport = new ArgDefSupport();
        private readonly PropertyDefSupport _propertyDefSupport = new PropertyDefSupport();
        private readonly InitMethodDefSupport _initMethodDefSupport = new InitMethodDefSupport();
        private readonly DestroyMethodDefSupport _destroyMethodDefSupport = new DestroyMethodDefSupport();
        private readonly AspectDefSupport _aspectDefSupport = new AspectDefSupport();
        private readonly MetaDefSupport _metaDefSupport = new MetaDefSupport();
        private string _instanceMode = ContainerConstants.INSTANCE_SINGLETON;
        private string _autoBindingMode = ContainerConstants.AUTO_BINDING_AUTO;
        private IComponentDeployer _componentDeployer;
        private readonly IDictionary _proxies = new Hashtable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComponentDefImpl()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="componentType">コンポーネントのType</param>
        public ComponentDefImpl(Type componentType)
            : this(componentType, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="componentType">コンポーネントのType</param>
        /// <param name="componentName">コンポーネントの名前</param>
        public ComponentDefImpl(Type componentType, string componentName)
        {
            _componentType = componentType;
            _componentName = componentName;
        }

        #region ComponentDef メンバ

        public object GetComponent()
        {
            return ComponentDeployer.Deploy(ComponentType);
        }

        public object GetComponent(Type receiveType)
        {
            return ComponentDeployer.Deploy(receiveType);
        }

        public void InjectDependency(object outerComponent)
        {
            ComponentDeployer.InjectDependency(outerComponent);
        }

        public IS2Container Container
        {
            get { return _container; }
            set
            {
                _container = value;
                _argDefSupport.Container = value;
                _metaDefSupport.Container = value;
                _metaDefSupport.Container = value;
                _propertyDefSupport.Container = value;
                _initMethodDefSupport.Container = value;
                _destroyMethodDefSupport.Container = value;
                _aspectDefSupport.Container = value;
            }
        }

        public Type ComponentType
        {
            get { return _componentType; }
        }

        public string ComponentName
        {
            get { return _componentName; }
        }

        public string AutoBindingMode
        {
            get { return _autoBindingMode; }
            set
            {
                if (AutoBindingUtil.IsAuto(value) || AutoBindingUtil.IsConstructor(value) || AutoBindingUtil.IsProperty(value) || AutoBindingUtil.IsNone(value))
                {
                    _autoBindingMode = value;
                }
                else
                {
                    throw new ArgumentException(value);
                }
            }
        }

        public string InstanceMode
        {
            get { return _instanceMode; }
            set
            {
                if (InstanceModeUtil.IsSingleton(value) || InstanceModeUtil.IsPrototype(value) || InstanceModeUtil.IsRequest(value) || InstanceModeUtil.IsSession(value) || InstanceModeUtil.IsOuter(value))
                {
                    _instanceMode = value;
                }
                else
                {
                    throw new ArgumentException(value);
                }
            }
        }

        public string Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }

        public void Init()
        {
            ComponentDeployer.Init();
        }

        public object GetProxy(Type proxyType)
        {
            if (proxyType == null)
            {
                return null;
            }
            return _proxies[proxyType];
        }

        public void AddProxy(Type proxyType, object proxy)
        {
            _proxies[proxyType] = proxy;
        }

        public void Destroy()
        {
            ComponentDeployer.Destroy();
        }

        #endregion

        #region IArgDefAware メンバ

        public void AddArgDef(IArgDef argDef)
        {
            _argDefSupport.AddArgDef(argDef);
        }

        public int ArgDefSize
        {
            get { return _argDefSupport.ArgDefSize; }
        }

        public IArgDef GetArgDef(int index)
        {
            return _argDefSupport.GetArgDef(index);
        }

        #endregion

        #region PropertyDefAware メンバ

        public void AddPropertyDef(IPropertyDef propertyDef)
        {
            _propertyDefSupport.AddPropertyDef(propertyDef);
        }

        public int PropertyDefSize
        {
            get { return _propertyDefSupport.PropertyDefSize; }
        }

        public IPropertyDef GetPropertyDef(int index)
        {
            return _propertyDefSupport.GetPropertyDef(index);
        }

        public IPropertyDef GetPropertyDef(string propertyName)
        {
            if (HasPropertyDef(propertyName))
            {
                return _propertyDefSupport.GetPropertyDef(propertyName);
            }
            else
            {
                throw new PropertyNotFoundRuntimeException(_componentType, propertyName);
            }
        }

        public bool HasPropertyDef(string propertyName)
        {
            return _propertyDefSupport.HasPropertyDef(propertyName);
        }

        #endregion

        #region InitMethodDefAware メンバ

        public void AddInitMethodDef(IInitMethodDef methodDef)
        {
            _initMethodDefSupport.AddInitMethodDef(methodDef);
        }

        public int InitMethodDefSize
        {
            get { return _initMethodDefSupport.InitMethodDefSize; }
        }

        public IInitMethodDef GetInitMethodDef(int index)
        {
            return _initMethodDefSupport.GetInitMethodDef(index);
        }

        #endregion

        #region DestroyMethodDefAware メンバ

        public void AddDestroyMethodDef(IDestroyMethodDef methodDef)
        {
            _destroyMethodDefSupport.AddDestroyMethodDef(methodDef);
        }

        public int DestroyMethodDefSize
        {
            get { return _destroyMethodDefSupport.DestroyMethodDefSize; }
        }

        public IDestroyMethodDef GetDestroyMethodDef(int index)
        {
            return _destroyMethodDefSupport.GetDestroyMethodDef(index);
        }

        #endregion

        #region AspectDefAware メンバ

        public void AddAspeceDef(IAspectDef aspectDef)
        {
            _aspectDefSupport.AddAspectDef(aspectDef);
        }

        public int AspectDefSize
        {
            get { return _aspectDefSupport.AspectDefSize; }
        }

        public IAspectDef GetAspectDef(int index)
        {
            return _aspectDefSupport.GetAspectDef(index);
        }

        #endregion

        #region MetaDefAware メンバ

        public void AddMetaDef(IMetaDef metaDef)
        {
            _metaDefSupport.AddMetaDef(metaDef);
        }

        public int MetaDefSize
        {
            get { return _metaDefSupport.MetaDefSize; }
        }

        public IMetaDef GetMetaDef(int index)
        {
            return _metaDefSupport.GetMetaDef(index);
        }

        public IMetaDef GetMetaDef(string name)
        {
            return _metaDefSupport.GetMetaDef(name);
        }

        public IMetaDef[] GetMetaDefs(string name)
        {
            return _metaDefSupport.GetMetaDefs(name);
        }

        #endregion

        /// <summary>
        /// ComponentDeployer
        /// </summary>
        private IComponentDeployer ComponentDeployer
        {
            get
            {
                lock (this)
                {
                    if (_componentDeployer == null)
                    {
                        _componentDeployer = ComponentDeployerFactory.Create(this);
                    }
                    return _componentDeployer;
                }
            }
        }
    }
}
