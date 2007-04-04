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

using System;
using System.Reflection;
using System.Resources;
using System.Collections.Generic;
using System.Text;
using Seasar.Framework.Container;

namespace Seasar.Framework.Message
{
    /// <summary>
    /// メッセージコードと引数をプロパティに登録されている
    /// パターンに適用し、メッセージを組み立てます。
    /// メッセージコードは、8桁で構成され最初の1桁がメッセージの種別で、
    /// E:エラー、W:ワーニング、I:インフォメーションで構成されます。
    /// 次の3桁がシステム名でSeasarの場合は、SSRになります。
    /// 最後の4桁は連番です。
    /// メッセージ定義ファイルは、システム名 + Messages.resourcesになります。
    /// SSRMessages.ja-JP.resourcesなどを用意することで他言語に対応できます。
    /// </summary>
    public sealed class MessageFormatter
    {
        private const string MESSAGES = "Messages";
        private static readonly object[] EMPTY_ARRAY = new object[0];
        private static Dictionary<string, ResourceManager> resourceManagers = 
            new Dictionary<string, ResourceManager>();

        private MessageFormatter()
        {
        }

        public static string GetMessage(string messageCode, object[] args)
        {
            return GetMessage(messageCode, args, Assembly.GetExecutingAssembly(), 
                ContainerConstants.PROPERTIES_NAMESPACE);
        }

        public static string GetMessage(
            string messageCode, object[] args, Assembly assembly, string nameSpace)
        {
            if (messageCode == null)
            {
                messageCode = "";
            }

            string simpleMessage = 
                GetSimpleMessage(messageCode, args, assembly, nameSpace);

            return GetFormattedMessage(messageCode, simpleMessage);
        }

        public static string GetFormattedMessage(
            string messageCode, string simpleMessage)
        {
            return "[" + messageCode + "]" + simpleMessage;
        }

        public static string GetSimpleMessage(string messageCode, object[] arguments)
        {
            return GetSimpleMessage(messageCode, arguments,
                Assembly.GetExecutingAssembly(), ContainerConstants.PROPERTIES_NAMESPACE);
        }

        public static string GetSimpleMessage(
            string messageCode, object[] arguments, Assembly assembly, string nameSpace)
        {
            try
            {
                string pattern = GetPattern(nameSpace, messageCode, assembly);

                if (pattern != null)
                {
                    if (arguments == null)
                    {
                        arguments = EMPTY_ARRAY;
                    }

                    return string.Format(pattern, arguments);
                }
            }
            catch
            {
            }

            return GetNoPatternMessage(arguments);
        }

        private static string GetPattern(
            string nameSpace, string messageCode, Assembly assembly)
        {
            string systemName = GetSystemName(messageCode);

            ResourceManager resourceManager = 
                GetMessages(nameSpace, systemName, assembly);

            if (resourceManager == null)
            {
                return null;
            }

            int length = messageCode.Length;

            if (length > 8)
            {
                string key = messageCode[0] + messageCode.Substring(length - 4);

                string pattern = resourceManager.GetString(key);

                if (pattern != null)
                {
                    return pattern;
                }
            }
            
            return resourceManager.GetString(messageCode);
        }

        private static string GetSystemName(string messageCode)
        {
            return messageCode.Substring(1, Math.Max(1, messageCode.Length - 4));
        }

        private static ResourceManager GetMessages(
            string nameSpace, string systemName, Assembly assembly)
        {
            string key = systemName + assembly.FullName;

            if (resourceManagers.ContainsKey(key))
            {
                return resourceManagers[key];
            }
            else
            {
                ResourceManager rm = new ResourceManager(
                    nameSpace + "." + systemName + MESSAGES, assembly);

                resourceManagers[key] = rm;

                return rm;
            }
        }

        private static string GetNoPatternMessage(object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return "";
            }

            StringBuilder buffer = new StringBuilder();

            foreach (object arg in args)
            {
                buffer.Append(arg + ", ");
            }

            buffer.Length = buffer.Length - 2;

            return buffer.ToString();
        }
    }
}
