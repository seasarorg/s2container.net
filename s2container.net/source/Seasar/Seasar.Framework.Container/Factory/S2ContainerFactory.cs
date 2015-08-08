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
using System.Globalization;
using System.Reflection;
using System.Resources;
using Seasar.Framework.Util;
using Seasar.Framework.Xml;

namespace Seasar.Framework.Container.Factory
{
    public sealed class S2ContainerFactory
    {
        public const string PUBLIC_ID = "-//SEASAR//DTD S2Container//EN";
        public const string DTD_PATH = "components.dtd";
        public const string BUILDER_CONFIG_PATH = "s2containerbuilder";
        private static readonly ResourceSet _builderProps;
        private static readonly Hashtable _builders = new Hashtable();
        private static readonly IS2ContainerBuilder _defaultBuilder = new XmlS2ContainerBuilder();

        static S2ContainerFactory()
        {
            var resourceManager =
                new ResourceManager(BUILDER_CONFIG_PATH,
                Assembly.GetExecutingAssembly());

            _builderProps = resourceManager.GetResourceSet(
                CultureInfo.CurrentCulture, true, false);

            _builders.Add("xml", _defaultBuilder);
            _builders.Add("dicon", _defaultBuilder);
        }

        private S2ContainerFactory()
        {
        }

        [Obsolete("[S2Container] is obsolete function. Please use [QuillContainer]")]
        public static IS2Container Create(string path)
        {
            var ext = _GetExtension(path);
            var config = S2SectionHandler.GetS2Section();
            if (config != null)
            {
                var assemblys = config.Assemblys;
                foreach (string assembly in assemblys)
                {
                    if (!StringUtil.IsEmpty(assembly)) AppDomain.CurrentDomain.Load(assembly);
                }
            }
            var container = _GetBuilder(ext).Build(path);
            return container;
        }

        public static IS2Container Include(IS2Container parent, string path)
        {
            var root = parent.Root;
            IS2Container child;
            lock (root)
            {
                if (root.HasDescendant(path))
                {
                    child = root.GetDescendant(path);
                    parent.Include(child);
                }
                else
                {
                    var ext = _GetExtension(path);
                    var builder = _GetBuilder(ext);
                    child = builder.Include(parent, path);
                    root.RegisterDescendant(child);
                }
            }
            return child;
        }

        private static string _GetExtension(string path)
        {
            var ext = ResourceUtil.GetExtension(path);
            if (ext == null)
            {
                throw new ExtensionNotFoundRuntimeException(path);
            }
            return ext;
        }

        private static IS2ContainerBuilder _GetBuilder(string ext)
        {
            IS2ContainerBuilder builder;
            lock (_builders)
            {
                builder = (IS2ContainerBuilder) _builders[ext];

                if (builder != null)
                {
                    return builder;
                }

                string className = null;

                if (_builderProps != null)
                {
                    className = _builderProps.GetString(ext);
                }

                if (className != null)
                {
                    var asms = AppDomain.CurrentDomain.GetAssemblies();
                    var type = ClassUtil.ForName(className, asms);
                    builder = (IS2ContainerBuilder) ClassUtil.NewInstance(type);
                    _builders[ext] = builder;
                }
                else
                {
                    builder = _defaultBuilder;
                }
            }
            return builder;
        }
    }
}
