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
using System.Xml.Serialization;
using Seasar.Framework.Container;

namespace Seasar.Framework.Xml
{
    /// <summary>
    /// S2.NETの構成セクション
    /// </summary>
    [Serializable]
    [XmlRoot(ContainerConstants.SEASAR_CONFIG)]
    public class S2Section
    {
        private string _configPath = null;
        private IList _assemblys = new ArrayList();

        [XmlElement(ContainerConstants.CONFIG_PATH_KEY)]
        public string ConfigPath
        {
            set { _configPath = value; }
            get { return _configPath; }
        }

        [XmlArray(ContainerConstants.CONFIG_ASSEMBLYS_KEY)]
        [XmlArrayItem(ContainerConstants.CONFIG_ASSEMBLY_KEY, typeof(string))]
        public IList Assemblys
        {
            set { _assemblys = value; }
            get { return _assemblys; }
        }
    }
}
