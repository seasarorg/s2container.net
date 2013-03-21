#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;
using Seasar.Framework.Xml;
using System;

namespace Seasar.Framework.Container.Factory
{
#if NET_1_1
    public class SingletonS2ContainerFactory
#else
    public static class SingletonS2ContainerFactory
#endif
    {
        private static string _configPath = "app.dicon";
        private static IS2Container _container;

        static SingletonS2ContainerFactory()
        {
            S2Section config = S2SectionHandler.GetS2Section();
            if (config != null)
            {
                string s = config.ConfigPath;
                if (!StringUtil.IsEmpty(s))
                {
                    _configPath = s;
                }
            }
        }

        public static string ConfigPath
        {
            set { _configPath = value; }
            get { return _configPath; }
        }

        [Obsolete("[S2Container] is obsolete function. Please use [QuillContainer]")]
        public static void Init()
        {
            _container = S2ContainerFactory.Create(_configPath);
            _container.Init();
        }

        public static void Destroy()
        {
            if (_container != null)
            {
                _container.Destroy();
                _container = null;
            }
        }

        public static IS2Container Container
        {
            get
            {
                if (_container == null)
                {
                    throw new EmptyRuntimeException("S2Container");
                }
                return _container;
            }
            set
            {
                _container = value;
            }
        }

        public static bool HasContainer
        {
            get { return _container != null; }
        }
    }
}
