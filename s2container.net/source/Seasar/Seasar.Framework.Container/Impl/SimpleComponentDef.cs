#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Container.Impl
{
    /// <summary>
    /// コンポーネントのインスタンスを直接返す場合に使用されます。
    /// </summary>
    public class SimpleComponentDef : IComponentDef
    {
        private readonly object _component;
        private readonly Type _componentType;
        private readonly string _componentName;
        private IS2Container _container;
        private readonly IDictionary _proxies = new Hashtable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SimpleComponentDef()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="componentType">コンポーネントのType</param>
        public SimpleComponentDef(Type componentType)
            : this(null, componentType, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="component">コンポーネント</param>
        public SimpleComponentDef(object component)
            : this(component, component.GetType())
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="component">コンポーネント</param>
        /// <param name="componentType">コンポーネントのType</param>
        public SimpleComponentDef(object component, Type componentType)
            : this(component, componentType, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="component">コンポーネント</param>
        /// <param name="componentName">コンポーネントの名前</param>
        public SimpleComponentDef(object component, string componentName)
            : this(component, component.GetType(), componentName)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="component">コンポーネント</param>
        /// <param name="componentType">コンポーネントのType</param>
        /// <param name="componentName">コンポーネントの名前</param>
        public SimpleComponentDef(object component, Type componentType, string componentName)
        {
            _component = component;
            _componentType = componentType;
            _componentName = componentName;
        }

        #region ComponentDef メンバ

        public virtual object GetComponent()
        {
            return _component;
        }

        public virtual object GetComponent(Type receiveType)
        {
            return _component;
        }

        public void InjectDependency(object outerComponent)
        {
            throw new NotSupportedException("InjectDependency");
        }

        public IS2Container Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
            }
        }

        public Type ComponentType
        {
            get
            {
                return _componentType;
            }
        }

        public string ComponentName
        {
            get
            {
                return _componentName;
            }
        }

        public string AutoBindingMode
        {
            get
            {
                throw new NotSupportedException("AutoBindingMode");
            }
            set
            {
                throw new NotSupportedException("AutoBindingMode");
            }
        }

        public string InstanceMode
        {
            get
            {
                throw new NotSupportedException("InstanceMode");
            }
            set
            {
                throw new NotSupportedException("InstanceMode");
            }
        }

        public string Expression
        {
            get
            {
                throw new NotSupportedException("Expression");
            }
            set
            {
                throw new NotSupportedException("Expression");
            }
        }

        public void Init()
        {
        }

        public void Destroy()
        {
        }

        public object GetProxy(Type proxyType)
        {
            return _proxies[proxyType];
        }

        public void AddProxy(Type proxyType, object proxy)
        {
            _proxies[proxyType] = proxy;
        }

        #endregion

        #region IArgDefAware メンバ

        public void AddArgDef(IArgDef argDef)
        {
            throw new NotSupportedException("AddArgDef");
        }

        public int ArgDefSize
        {
            get
            {
                throw new NotSupportedException("ArgDefSize");
            }
        }

        public IArgDef GetArgDef(int index)
        {
            throw new NotSupportedException("GetArgDef");
        }

        #endregion

        #region IPropertyDefAware メンバ

        public void AddPropertyDef(IPropertyDef propertyDef)
        {
            throw new NotSupportedException("AddPropertyDef");
        }

        public int PropertyDefSize
        {
            get
            {
                throw new NotSupportedException("PropertyDefSize");
            }
        }

        public IPropertyDef GetPropertyDef(int index)
        {
            throw new NotSupportedException("GetPropertyDef");
        }

        public IPropertyDef GetPropertyDef(string propertyName)
        {
            throw new NotSupportedException("GetPropertyDef");
        }

        public bool HasPropertyDef(string propertyName)
        {
            throw new NotSupportedException("HasPropertyDef");
        }

        #endregion

        #region IInitMethodDefAware メンバ

        public void AddInitMethodDef(IInitMethodDef methodDef)
        {
            throw new NotSupportedException("AddInitMethodDef");
        }

        public int InitMethodDefSize
        {
            get
            {
                throw new NotSupportedException("InitMethodDefSize");
            }
        }

        public IInitMethodDef GetInitMethodDef(int index)
        {
            throw new NotSupportedException("GetInitMethodDef");
        }

        #endregion

        #region IDestroyMethodDefAware メンバ

        public void AddDestroyMethodDef(IDestroyMethodDef methodDef)
        {
            throw new NotSupportedException("AddDestroyMethodDef");
        }

        public int DestroyMethodDefSize
        {
            get
            {
                throw new NotSupportedException("DestroyMethodDefSize");
            }
        }

        public IDestroyMethodDef GetDestroyMethodDef(int index)
        {

            throw new NotSupportedException("GetDestroyMethodDef");
        }

        public IDestroyMethodDef[] GetDestroyMethodDefs()
        {
            throw new NotSupportedException("GetDestroyMethodDefs");
        }

        #endregion

        #region IAspectDefAware メンバ

        public void AddAspeceDef(IAspectDef aspectDef)
        {
            throw new NotSupportedException("AddAspectDef");
        }

        public int AspectDefSize
        {
            get
            {
                throw new NotSupportedException("AspectDefSize");
            }
        }

        public IAspectDef GetAspectDef(int index)
        {
            throw new NotSupportedException("GetAspectDef");
        }

        #endregion

        #region IMetaDefAware メンバ

        public void AddMetaDef(IMetaDef metaDef)
        {
            throw new NotSupportedException("AddMetaDef");
        }

        public int MetaDefSize
        {
            get
            {
                throw new NotSupportedException("MetaDefSize");
            }
        }

        public IMetaDef GetMetaDef(int index)
        {
            throw new NotSupportedException("GetMetaDef");
        }

        public IMetaDef GetMetaDef(string name)
        {
            throw new NotSupportedException("GetMethodDef");
        }

        public IMetaDef[] GetMetaDefs(string name)
        {
            throw new NotSupportedException("GetMetaDefs");
        }

        #endregion
    }
}
