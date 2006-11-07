#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
	/// <summary>
	/// S2ContainerImpl の概要の説明です。
	/// </summary>
	public class S2ContainerImpl : IS2Container
	{
		private Hashtable componentDefTable_ = new Hashtable();
		private IList componentDefList_ = new ArrayList();
		private string namespace_;
		private string path_;
		private IList children_ = new ArrayList();
		private Hashtable descendants_ = CollectionsUtil.CreateCaseInsensitiveHashtable();
		private IS2Container root_;
		private MetaDefSupport metaDefSupport_;
		private bool inited_ = false;

		public S2ContainerImpl()
		{
			root_ = this;
			IComponentDef componentDef = new SimpleComponentDef(this,ContainerConstants.INSTANCE_PROTOTYPE);
			componentDefTable_.Add(ContainerConstants.CONTAINER_NAME,componentDef);
			componentDefTable_.Add(typeof(IS2Container),componentDef);
			
			IComponentDef requestCd = new RequestComponentDef(this);
			componentDefTable_.Add(ContainerConstants.REQUEST_NAME, requestCd);
			componentDefTable_.Add(typeof(HttpRequest), requestCd);

			IComponentDef sessionCd = new SessionComponentDef(this);
			componentDefTable_.Add(ContainerConstants.SESSION_NAME, sessionCd);
			componentDefTable_.Add(typeof(HttpSessionState), sessionCd);

			IComponentDef responseCd = new ResponseComponentDef(this);
			componentDefTable_.Add(ContainerConstants.RESPONSE_NAME, responseCd);
			componentDefTable_.Add(typeof(HttpResponse), responseCd);

			IComponentDef applicationCd = new HttpApplicationComponentDef(this);
			componentDefTable_.Add(ContainerConstants.HTTP_APPLICATION_NAME, applicationCd);
			componentDefTable_.Add(typeof(HttpApplication), applicationCd);

			metaDefSupport_ = new MetaDefSupport(this);
		}

		#region S2Container メンバ

		public object GetComponent(object componentKey)
		{
			IComponentDef cd = this.GetComponentDef(componentKey);
			if(componentKey is Type)
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
            IComponentDef cd = this.GetComponentDef(componentName);
            return cd.GetComponent(componentType);
        }

		public void InjectDependency(object outerComponent)
		{
			
			this.InjectDependency(outerComponent,outerComponent.GetType());
		}

		public void InjectDependency(object outerComponent, Type componentType)
		{
			
			this.GetComponentDef(componentType).InjectDependency(outerComponent);
		}

		public void InjectDependency(object outerComponent, string componentName)
		{
			
			this.GetComponentDef(componentName).InjectDependency(outerComponent);
		}

		public void Register(object component)
		{
			
			this.Register(new SimpleComponentDef(component));
		}

		public void Register(object component, string componentName)
		{
			
			this.Register(new SimpleComponentDef(component,componentName));

		}

		public void Register(Type componentType)
		{
			
			this.Register(new ComponentDefImpl(componentType));
		}

		public void Register(Type componentType, string componentName)
		{
			
			this.Register(new ComponentDefImpl(componentType,componentName));
		}

		public void Register(IComponentDef componentDef)
		{
			
			lock(this)
			{
				this.Register0(componentDef);
				componentDefList_.Add(componentDef);
			}
		}

		private void Register0(IComponentDef componentDef)
		{
			if(componentDef.Container == null)
			{
				componentDef.Container = this;
			}
			this.RegisterByType(componentDef);
			this.RegisterByName(componentDef);
		}

		private void RegisterByType(IComponentDef componentDef)
		{
			Type[] types = GetAssignableTypes(componentDef.ComponentType);
			foreach(Type type in types)
			{
				this.RegisterTable(type,componentDef);
			}
		}

		private void RegisterByName(IComponentDef componentDef)
		{
			string componentName = componentDef.ComponentName;
			if(componentName != null) 
				this.RegisterTable(componentName,componentDef);
		}

		private void RegisterTable(object key,IComponentDef componentDef)
		{
			if(componentDefTable_.ContainsKey(key))
			{
				this.ProcessTooManyRegistration(key,componentDef);
			}
			else
			{
				componentDefTable_[key] = componentDef;
			}
		}

		public int ComponentDefSize
		{
			get
			{
				
				return componentDefList_.Count;
			}
		}

		public IComponentDef GetComponentDef(int index)
		{
			
			lock(this)
			{
				return (IComponentDef) componentDefList_[index];
			}
		}

		public IComponentDef GetComponentDef(object key)
		{
			
			IComponentDef cd = this.GetComponentDef0(key);
			if(cd == null) throw new ComponentNotFoundRuntimeException(key);
			return cd;
		}

		private IComponentDef GetComponentDef0(object key)
		{
			lock(this)
			{
				IComponentDef cd = (IComponentDef) componentDefTable_[key];
				if(cd != null) return cd;
				if(key is string)
				{
					string name = (string) key;
					int index = name.IndexOf(ContainerConstants.NS_SEP);
					if(index > 0)
					{
						string ns = name.Substring(0,index);
						if(this.HasComponentDef(ns))
						{
							IS2Container child = (IS2Container) this.GetComponent(ns);
							name = name.Substring(index + 1);
							if(child.HasComponentDef(name))
							{
								return child.GetComponentDef(name);
							}
						}
					}
				}
				for(int i = 0; i < this.ChildSize; ++i)
				{
					IS2Container child = this.GetChild(i);
					if(child.HasComponentDef(key)) return child.GetComponentDef(key);
				}
				return null;
			}
		}

		public bool HasComponentDef(object componentKey)
		{
			
			return this.GetComponentDef0(componentKey) != null;
		}

		public bool HasDescendant(string path)
		{
			
			lock(this)
			{
				return descendants_.ContainsKey(path);
			}
		}

		public IS2Container GetDescendant(string path)
		{
			
			lock(this)
			{
				IS2Container descendant = (IS2Container) descendants_[path];
				if(descendant != null)
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
			
			lock(this)
			{
				descendants_[descendant.Path] = descendant;
			}
		}

		public void Include(IS2Container child)
		{
			
			lock(this)
			{
				child.Root = this.Root;
				children_.Add(child);
				string ns = child.Namespace;
				if(ns!=null)
					this.RegisterTable(ns,new S2ContainerComponentDef(child,ns));
			}
		}

		public int ChildSize
		{
			get
			{
				
				lock(this)
				{
					return children_.Count;
				}
			}
		}

		public IS2Container GetChild(int index)
		{
			
			lock(this)
			{
				return (IS2Container) children_[index];
			}
		}

		public void Init()
		{
			
			if(inited_) return;
			for(int i = 0;i < this.ChildSize; ++i)
			{
				this.GetChild(i).Init();
			}
			for(int i = 0; i < this.ComponentDefSize; ++i)
			{
				this.GetComponentDef(i).Init();
			}
			inited_ = true;
		}

		public string Namespace
		{
			get
			{
				
				return namespace_;
			}
			set
			{
				
				lock(this)
				{
					if(namespace_ != null && componentDefTable_.Contains(namespace_))
						componentDefTable_.Remove(namespace_);
					namespace_ = value;
					componentDefTable_[namespace_] =
						new SimpleComponentDef(this,namespace_);
				}
			}
		}

		public string Path
		{
			get
			{
				
				return path_;
			}
			set
			{
				
				path_ = value;
			}
		}

		public IS2Container Root
		{
			get
			{
				
				return root_;
			}
			set
			{
				
				root_ = value;
			}
		}

		public void Destroy()
		{
			if (!inited_) return;
			for(int i = this.ComponentDefSize - 1; 0 <= i; --i)
			{
				try
				{
					this.GetComponentDef(i).Destroy();
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
			}
			for(int i = this.ChildSize - 1; 0 <= i; --i)
			{
				this.GetChild(i).Destroy();
			}
			inited_ = false;
		}

		public HttpApplication HttpApplication
		{
			get
			{
				if(this.HttpContext == null)
				{
					return null;
				}
				else
				{
					return this.HttpContext.ApplicationInstance;
				}
			}
		}

		public HttpResponse Response
		{
			get
			{
				if(this.HttpContext == null)
				{
					return null;
				}
				else
				{
					return this.HttpContext.Response;
				}
			}
		}

		public HttpRequest Request
		{
			get
			{
				if(this.HttpContext == null)
				{
					return null;
				}
				else
				{
					return this.HttpContext.Request;
				}
			}
		}

		public HttpSessionState Session
		{
			get
			{
				if(this.HttpContext == null)
				{
					return null;
				}
				else
				{
					return this.HttpContext.Session;
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

		#region IMetaDefAware メンバ

		public void AddMetaDef(IMetaDef metaDef)
		{
			
			metaDefSupport_.AddMetaDef(metaDef);
		}

		public int MetaDefSize
		{
			get
			{
				
				return metaDefSupport_.MetaDefSize;
			}
		}

		public IMetaDef GetMetaDef(int index)
		{
			
			return metaDefSupport_.GetMetaDef(index);
		}

		public IMetaDef GetMetaDef(string name)
		{
			
			return metaDefSupport_.GetMetaDef(name);
		}

		public IMetaDef[] GetMetaDefs(string name)
		{
			
			return metaDefSupport_.GetMetaDefs(name);
		}

		#endregion

		private static Type[] GetAssignableTypes(Type componentType)
		{
			ArrayList types = new ArrayList();
			for(Type type = componentType; type != typeof(object)
				&& type != null; type = type.BaseType)
			{
				AddAssignableTypes(types,type);
			}
			return (Type[]) types.ToArray(typeof(Type));
		}

		private static void AddAssignableTypes(IList types,Type type)
		{
			if(types.Contains(type)) return;
			types.Add(type);
			Type[] interfaces = type.GetInterfaces();
			foreach(Type interfaceTemp in interfaces)
			{
				AddAssignableTypes(types,interfaceTemp);
			}
		}

		private void ProcessTooManyRegistration(object key,IComponentDef componentDef)
		{
			IComponentDef cd = (IComponentDef) componentDefTable_[key];
			if(cd is TooManyRegistrationComponentDef)
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
				componentDefTable_[key] = tmrcf;
			}
		}

	}
}
