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

namespace Seasar.Framework.Container
{
    public class ContainerConstants
    {
        public const string INSTANCE_SINGLETON = "singleton";
        public const string INSTANCE_PROTOTYPE = "prototype";
        public const string INSTANCE_REQUEST = "request";
        public const string INSTANCE_SESSION = "session";
        public const string INSTANCE_OUTER = "outer";
        public const string AUTO_BINDING_AUTO = "auto";
        public const string AUTO_BINDING_CONSTRUCTOR = "constructor";
        public const string AUTO_BINDING_PROPERTY = "property";
        public const string AUTO_BINDING_NONE = "none";
        public const char NS_SEP = '.';
        public const string CONTAINER_NAME = "container";
        public const string REQUEST_NAME = "request";
        public const string RESPONSE_NAME = "response";
        public const string SESSION_NAME = "session";
        public const string HTTP_APPLICATION_NAME = "httpApplication";
        public const string HTTP_CONTEXT_NAME = "httpContext";
        public const string COMPONENT_DEF_NAME = "componentDef";
        public const string SEASAR_CONFIG = "seasar";
        public const string CONFIG_PATH_KEY = "configPath";
        public const string CONFIG_ASSEMBLYS_KEY = "assemblys";
        public const string CONFIG_ASSEMBLY_KEY = "assembly";
    }
}
