#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.IO;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;

namespace Seasar.Framework.Unit
{
    public class S2FrameworkTestCaseBase
    {
        private IS2Container _container;

        public IS2Container Container
        {
            get { return _container; }
            set { _container = value; }
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
            S2ContainerFactory.Include(Container, ConvertPath(path));
        }

        protected string ConvertPath(string path)
        {
            if (ResourceUtil.GetResourceNoException(path, GetType().Assembly) != null)
            {
                return path;
            }
            if (path.IndexOf('/') > 0)
            {
                return path;
            }
            if (path.IndexOf(Path.DirectorySeparatorChar) > 0)
            {
                return path;
            }
            string prefix = GetType().FullName.Replace('.', '/');
            int pos = (prefix.LastIndexOf("/") + 1);
            prefix = prefix.Substring(0, pos);
            return prefix + path;
        }
    }
}
