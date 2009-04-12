#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.SessionState;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Impl
{
    public class S2ContainerImpl : IS2Container
    {
        private readonly Hashtable _componentDefTable = new Hashtable();
        private readonly IList _componentDefList = new ArrayList();
        private string _namespace;
        private string _path;
        private readonly IList _children = new ArrayList();
        private readonly Hashtable _descendants = CollectionsUtil.CreateCaseInsensitiveHashtable();
        private IS2Container _root;
        private readonly MetaDefSupport _metaDefSupport;
        private bool _inited = false;

        public S2ContainerImpl()
        {
            _root = this;
            IComponentDef componentDef = new SimpleComponentDef(this, ContainerConstants.INSTANCE_PROTOTYPE);
            _componentDefTable.Add(ContainerConstants.CONTAINER_NAME, componentDef);
            _componentDefTable.Add(typeof(IS2Container), componentDef);

            IComponentDef requestCd = new RequestComponentDef(this);
            _componentDefTable.Add(ContainerConstants.REQUEST_NAME, requestCd);
            _componentDefTable.Add(typeof(HttpRequest), requestCd);

            IComponentDef sessionCd = new SessionComponentDef(this);
            _componentDefTable.Add(ContainerConstants.SESSION_NAME, sessionCd);
            _componentDefTable.Add(typeof(HttpSessionState), sessionCd);

            IComponentDef responseCd = new ResponseComponentDef(this);
            _componentDefTable.Add(ContainerConstants.RESPONSE_NAME, responseCd);
            _componentDefTable.Add(typeof(HttpResponse), responseCd);

            IComponentDef applicationCd = new HttpApplicationComponentDef(this);
            _componentDefTable.Add(ContainerConstants.HTTP_APPLICATION_NAME, applicationCd);
            _componentDefTable.Add(typeof(HttpApplication), applicationCd);

            _metaDefSupport = new MetaDefSupport(this);
        }

        #region S2Container ƒƒ“ƒo

        public object GetComponent(object componentKey)
        {
            IComponentDef cd = GetComponentDef(componentKey);
            if (componentKey is Type)
            {
                return cd.GetComponent((Type) componentKey);
            }
            else
            {
                return cd.GetComponent();
            }
        }

        public object GetComponent(Type componentType, string componentName)
        {
            IComponentDef cd = GetComponentDef(componentName);
            return cd.GetComponent(componentType);
        }

        public void InjectDependency(object outerComponent)
        {
            InjectDependency(outerComponent, outerComponent.GetType());
        }

        public void InjectDependency(object outerComponent, Type componentType)
        {
            GetComponentDef(componentType).InjectDependency(outerComponent);
        }

        public void InjectDependency(object outerComponent, string componentName)
        {
            GetComponentDef(componentName).InjectDependency(outerComponent);
        }

        public void Register(object component)
        {
            Register(new SimpleComponentDef(component));
        }

        public void Register(object component, string componentName)
        {
            Register(new SimpleComponentDef(component, componentName));
        }

        public void Register(Type componentType)
        {
            Register(new ComponentDefImpl(componentType));
        }

        public void Register(Type componentType, string componentName)
        {
            Register(new ComponentDefImpl(componentType, componentName));
        }

        public void Register(IComponentDef componentDef)
        {
            lock (this)
            {
                Register0(componentDef);
                _componentDefList.Add(componentDef);
            }
        }

        private void Register0(IComponentDef componentDef)
        {
            if (componentDef.Container == null)
            {
                componentDef.Container = this;
            }
            RegisterByType(componentDef);
            RegisterByName(componentDef);
        }

        private void RegisterByType(IComponentDef componentDef)
        {
            Type[] types = GetAssignableTypes(componentDef.ComponentType);
            foreach (Type type in types)
            {
                RegisterTable(type, componentDef);
            }
        }

        private void RegisterByName(IComponentDef componentDef)
        {
            string componentName = componentDef.ComponentName;
            if (componentName != null)
            {
                RegisterTable(componentName, componentDef);
            }
        }

        private void RegisterTable(object key, IComponentDef componentDef)
        {
            if (_componentDefTable.ContainsKey(key))
            {
                ProcessTooManyRegistration(key, componentDef);
            }
            else
            {
                _componentDefTable[key] = componentDef;
            }
        }

        public int ComponentDefSize
        {
            get { return _componentDefList.Count; }
        }

        public IComponentDef GetComponentDef(int index)
        {
            lock (this)
            {
                return (IComponentDef) _componentDefList[index];
            }
        }

        public IComponentDef GetComponentDef(object key)
        {
            IComponentDef cd = GetComponentDef0(key);
            if (cd == null)
            {
                throw new ComponentNotFoundRuntimeException(key);
            }
            return cd;
        }

        private IComponentDef GetComponentDef0(object key)
        {
            lock (this)
            {
                IComponentDef cd = (IComponentDef) _componentDefTable[key];
                if (cd != null)
                {
                    return cd;
                }
                if (key is string)
                {
                    string name = (string) key;
                    int index = name.IndexOf(ContainerConstants.NS_SEP);
                    if (index > 0)
                    {
                        string ns = name.Substring(0, index);
                        if (HasComponentDef(ns))
                        {
                            IS2Container child = (IS2Container) GetComponent(ns);
                            name = name.Substring(index + 1);
                            if (child.HasComponentDef(name))
                            {
                                return child.GetComponentDef(name);
                            }
                        }
                    }
                }
                for (int i = 0; i < ChildSize; ++i)
                {
                    IS2Container child = GetChild(i);
                    if (child.HasComponentDef(key)) return child.GetComponentDef(key);
                }
                return null;
            }
        }

        public bool HasComponentDef(object componentKey)
        {
            return GetComponentDef0(componentKey) != null;
        }

        public bool HasDescendant(string path)
        {
            lock (this)
            {
                return _descendants.ContainsKey(path);
            }
        }

        public IS2Container GetDescendant(string path)
        {
            lock (this)
            {
                IS2Container descendant = (IS2Container) _descendants[path];
                if (descendant != null)
                {
                    return descendant;
                }
                else
                {
                    throw new ContainerNotRegisteredRuntimeException(path);
                }
            }
        }

        public void RegisterDescendant(IS2Container descendant)
        {
            lock (this)
            {
                _descendants[descendant.Path] = descendant;
            }
        }

        public void Include(IS2Container child)
        {
            lock (this)
            {
                child.Root = Root;
                _children.Add(child);
                string ns = child.Namespace;
                if (ns != null)
                {
                    RegisterTable(ns, new S2ContainerComponentDef(child, ns));
                }
            }
        }

        public int ChildSize
        {
            get
            {
                lock (this)
                {
                    return _children.Count;
                }
            }
        }

        public IS2Container GetChild(int index)
        {
            lock (this)
            {
                return (IS2Container) _children[index];
            }
        }

        public void Init()
        {
            if (_inited)
            {
                return;
            }
            for (int i = 0; i < ChildSize; ++i)
            {
                GetChild(i).Init();
            }
            for (int i = 0; i < ComponentDefSize; ++i)
            {
                GetComponentDef(i).Init();
            }
            _inited = true;
        }

        public string Namespace
        {
            get { return _namespace; }
            set
            {
                lock (this)
                {
                    if (_namespace != null && _componentDefTable.Contains(_namespace))
                    {
                        _componentDefTable.Remove(_namespace);
                    }
                    _namespace = value;
                    _componentDefTable[_namespace] =
                        new SimpleComponentDef(this, _namespace);
                }
            }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public IS2Container Root
        {
            get { return _root; }
            set { _root = value; }
        }

        public void Destroy()
        {
            if (!_inited)
            {
                return;
            }
            for (int i = ComponentDefSize - 1; 0 <= i; --i)
            {
                try
                {
                    GetComponentDef(i).Destroy();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            for (int i = ChildSize - 1; 0 <= i; --i)
            {
                GetChild(i).Destroy();
            }
            _inited = false;
        }

        public HttpApplication HttpApplication
        {
            get
            {
                if (HttpContext == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.ApplicationInstance;
                }
            }
        }

        public HttpResponse Response
        {
            get
            {
                if (HttpContext == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Response;
                }
            }
        }

        public HttpRequest Request
        {
            get
            {
                if (HttpContext == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Request;
                }
            }
        }

        public HttpSessionState Session
        {
            get
            {
                if (HttpContext == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Session;
                }
            }
        }

        public HttpContext HttpContext
        {
            set
            {
                CallContext.SetData(ContainerConstants.HTTP_CONTEXT_NAME, value);
            }
            get
            {
                return (HttpContext) CallContext.GetData(ContainerConstants.HTTP_CONTEXT_NAME);
            }
        }

        #endregion

        #region IMetaDefAware ƒƒ“ƒo

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

        private static Type[] GetAssignableTypes(Type componentType)
        {
            ArrayList types = new ArrayList();
            for (Type type = componentType; type != typeof(object)
                && type != null; type = type.BaseType)
            {
                AddAssignableTypes(types, type);
            }
            return (Type[]) types.ToArray(typeof(Type));
        }

        private static void AddAssignableTypes(IList types, Type type)
        {
            if (types.Contains(type))
            {
                return;
            }
            types.Add(type);
            Type[] interfaces = type.GetInterfaces();
            foreach (Type interfaceTemp in interfaces)
            {
                AddAssignableTypes(types, interfaceTemp);
            }
        }

        private void ProcessTooManyRegistration(object key, IComponentDef componentDef)
        {
            IComponentDef cd = (IComponentDef) _componentDefTable[key];
            if (cd is TooManyRegistrationComponentDef)
            {
                ((TooManyRegistrationComponentDef) cd)
                    .AddComponentType(componentDef.ComponentType);
            }
            else
            {
                TooManyRegistrationComponentDef tmrcf =
                    new TooManyRegistrationComponentDef(key);
                tmrcf.AddComponentType(cd.ComponentType);
                tmrcf.AddComponentType(componentDef.ComponentType);
                _componentDefTable[key] = tmrcf;
            }
        }
    }
}
