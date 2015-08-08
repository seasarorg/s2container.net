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
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.SessionState;
using Seasar.Framework.Container.Util;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Impl
{
    public class S2ContainerImpl : IS2Container
    {
        private readonly Hashtable _componentDefTable = new Hashtable();
        private readonly IList _componentDefList = new ArrayList();
        private string _namespace;
        private readonly IList _children = new ArrayList();
        private readonly Hashtable _descendants = CollectionsUtil.CreateCaseInsensitiveHashtable();
        private readonly MetaDefSupport _metaDefSupport;
        private bool _inited = false;

        public S2ContainerImpl()
        {
            Root = this;
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
            var cd = GetComponentDef(componentKey);
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
            var cd = GetComponentDef(componentName);
            return cd.GetComponent(componentType);
        }

        public void InjectDependency(object outerComponent)
        {
            InjectDependency(outerComponent, outerComponent.GetExType());
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
                _Register0(componentDef);
                _componentDefList.Add(componentDef);
            }
        }

        private void _Register0(IComponentDef componentDef)
        {
            if (componentDef.Container == null)
            {
                componentDef.Container = this;
            }
            _RegisterByType(componentDef);
            _RegisterByName(componentDef);
        }

        private void _RegisterByType(IComponentDef componentDef)
        {
            var types = _GetAssignableTypes(componentDef.ComponentType);
            foreach (var type in types)
            {
                _RegisterTable(type, componentDef);
            }
        }

        private void _RegisterByName(IComponentDef componentDef)
        {
            var componentName = componentDef.ComponentName;
            if (componentName != null)
            {
                _RegisterTable(componentName, componentDef);
            }
        }

        private void _RegisterTable(object key, IComponentDef componentDef)
        {
            if (_componentDefTable.ContainsKey(key))
            {
                _ProcessTooManyRegistration(key, componentDef);
            }
            else
            {
                _componentDefTable[key] = componentDef;
            }
        }

        public int ComponentDefSize => _componentDefList.Count;

        public IComponentDef GetComponentDef(int index)
        {
            lock (this)
            {
                return (IComponentDef) _componentDefList[index];
            }
        }

        public IComponentDef GetComponentDef(object key)
        {
            var cd = _GetComponentDef0(key);
            if (cd == null)
            {
                throw new ComponentNotFoundRuntimeException(key);
            }
            return cd;
        }

        private IComponentDef _GetComponentDef0(object key)
        {
            lock (this)
            {
                var cd = (IComponentDef) _componentDefTable[key];
                if (cd != null)
                {
                    return cd;
                }
                if (key is string)
                {
                    var name = (string) key;
                    var index = name.IndexOf(ContainerConstants.NS_SEP);
                    if (index > 0)
                    {
                        var ns = name.Substring(0, index);
                        if (HasComponentDef(ns))
                        {
                            var child = (IS2Container) GetComponent(ns);
                            name = name.Substring(index + 1);
                            if (child.HasComponentDef(name))
                            {
                                return child.GetComponentDef(name);
                            }
                        }
                    }
                }
                for (var i = 0; i < ChildSize; ++i)
                {
                    var child = GetChild(i);
                    if (child.HasComponentDef(key)) return child.GetComponentDef(key);
                }
                return null;
            }
        }

        public bool HasComponentDef(object componentKey) => _GetComponentDef0(componentKey) != null;

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
                var descendant = (IS2Container) _descendants[path];
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
                var ns = child.Namespace;
                if (ns != null)
                {
                    _RegisterTable(ns, new S2ContainerComponentDef(child, ns));
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
            for (var i = 0; i < ChildSize; ++i)
            {
                GetChild(i).Init();
            }
            for (var i = 0; i < ComponentDefSize; ++i)
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

        public string Path { get; set; }

        public IS2Container Root { get; set; }

        public void Destroy()
        {
            if (!_inited)
            {
                return;
            }
            for (var i = ComponentDefSize - 1; 0 <= i; --i)
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
            for (var i = ChildSize - 1; 0 <= i; --i)
            {
                GetChild(i).Destroy();
            }
            _inited = false;
        }

        public HttpApplication HttpApplication => HttpContext?.ApplicationInstance;

        public HttpResponse Response => HttpContext?.Response;

        public HttpRequest Request => HttpContext?.Request;

        public HttpSessionState Session => HttpContext?.Session;

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

        public int MetaDefSize => _metaDefSupport.MetaDefSize;

        public IMetaDef GetMetaDef(int index) => _metaDefSupport.GetMetaDef(index);

        public IMetaDef GetMetaDef(string name) => _metaDefSupport.GetMetaDef(name);

        public IMetaDef[] GetMetaDefs(string name) => _metaDefSupport.GetMetaDefs(name);

        #endregion

        private static Type[] _GetAssignableTypes(Type componentType)
        {
            var types = new ArrayList();
            for (var type = componentType; type != typeof(object)
                && type != null; type = type.BaseType)
            {
                _AddAssignableTypes(types, type);
            }
            return (Type[]) types.ToArray(typeof(Type));
        }

        private static void _AddAssignableTypes(IList types, Type type)
        {
            if (types.Contains(type))
            {
                return;
            }
            types.Add(type);
            var interfaces = type.GetInterfaces();
            foreach (var interfaceTemp in interfaces)
            {
                _AddAssignableTypes(types, interfaceTemp);
            }
        }

        private void _ProcessTooManyRegistration(object key, IComponentDef componentDef)
        {
            var cd = (IComponentDef) _componentDefTable[key];
            if (cd is TooManyRegistrationComponentDef)
            {
                ((TooManyRegistrationComponentDef) cd).AddComponentType(componentDef.ComponentType);
            }
            else
            {
                var tmrcf = new TooManyRegistrationComponentDef(key);
                tmrcf.AddComponentType(cd.ComponentType);
                tmrcf.AddComponentType(componentDef.ComponentType);
                _componentDefTable[key] = tmrcf;
            }
        }
    }
}
