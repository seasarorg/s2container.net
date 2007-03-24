#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

namespace Seasar.Extension.ADO
{
    public class DataProvider
    {
        private string connectionType;
        private string commandType;
        private string parameterType;
        private string dataAdapterType;

        public string ConnectionType
        {
            set { connectionType = value; }
            get { return connectionType; }
        }

        public string CommandType
        {
            set { commandType = value; }
            get { return commandType; }
        }

        public string ParameterType
        {
            set { parameterType = value; }
            get { return parameterType; }
        }

        public string DataAdapterType
        {
            set { dataAdapterType = value; }
            get { return dataAdapterType; }
        }
    }
}
