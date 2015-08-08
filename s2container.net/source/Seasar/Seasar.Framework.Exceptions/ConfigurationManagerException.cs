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
using System.Runtime.Serialization;

namespace Seasar.Framework.Exceptions
{
    /// <summary>
    /// アプリケーション構成ファイルのアクセスでエラーを返したときにスローされる例外
    /// </summary>
    [Serializable]
    public class ConfigurationManagerException : SRuntimeException
    {
        public ConfigurationManagerException(string section, string key)
            : base("ESSR0005", new object[] { section, key })
        {
            Section = section;
            Key = key;
        }

        public ConfigurationManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Section = info.GetString("_section");
            Key = info.GetString("_key");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_section", Section, typeof(string));
            info.AddValue("_key", Key, typeof(string));
            base.GetObjectData(info, context);
        }

        public string Section { get; }

        public string Key { get; }
    }
}
