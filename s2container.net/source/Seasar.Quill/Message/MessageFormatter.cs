using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Seasar.Quill.Message
{
    /// <summary>
    /// メッセージコードと引数をプロパティに登録されている
    /// パターンに適用し、メッセージを組み立てます。
    /// メッセージコードは、8桁で構成され最初の1桁がメッセージの種別で、
    /// E:エラー、W:ワーニング、I:インフォメーションで構成されます。
    /// 次の3桁がシステム名でQuillの場合は、QLLになります。
    /// 最後の4桁は連番です。
    /// メッセージ定義ファイルは、システム名 + Messages.resourcesになります。
    /// QLLMessages.ja-JP.resourcesなどを用意することで他言語に対応できます。
    /// </summary>
    public static class MessageFormatter
    {
        private const string MESSAGES = "Messages";
        private static readonly object[] EMPTY_ARRAY = new object[0];
        private static readonly IDictionary<string, ResourceManager> _resourceManagers = new Dictionary<string, ResourceManager>();

        public static string GetMessage(string messageCode, object[] args)
        {
            return GetMessage(messageCode, args, (string)null);
        }

        public static string GetMessage(string messageCode, object[] args, string nameSpace)
        {
            return GetMessage(messageCode, args, Assembly.GetExecutingAssembly(), nameSpace);
        }

        public static string GetMessage(string messageCode, object[] args, Assembly assembly)
        {
            return GetMessage(messageCode, args, assembly, null);
        }

        public static string GetMessage(string messageCode, object[] args, Assembly assembly, string nameSpace)
        {
            if (messageCode == null)
            {
                messageCode = string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[").Append(messageCode).Append("]").Append(GetSimpleMessage(messageCode, args, assembly, nameSpace));
            return builder.ToString();
        }

        public static string GetSimpleMessage(string messageCode, object[] arguments)
        {
            return GetSimpleMessage(messageCode, arguments, (string)null);
        }

        public static string GetSimpleMessage(string messageCode, object[] arguments, string nameSpace)
        {
            return GetSimpleMessage(messageCode, arguments, Assembly.GetExecutingAssembly(), nameSpace);
        }

        public static string GetSimpleMessage(string messageCode, object[] arguments, Assembly assembly)
        {
            return GetSimpleMessage(messageCode, arguments, assembly, null);
        }

        public static string GetSimpleMessage(string messageCode, object[] arguments, Assembly assembly, string nameSpace)
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
                // 例外が発生した場合はパターンなしと同じと見なす
            }
            return GetNoPatternMessage(arguments);
        }

        private static string GetPattern(string nameSpace, string messageCode, Assembly assembly)
        {
            ResourceManager resourceManager = GetMessages(nameSpace, GetSystemName(messageCode), assembly);
            if (resourceManager != null)
            {
                return resourceManager.GetString(messageCode);
            }
            return null;
        }

        private static string GetSystemName(string messageCode)
        {
            return messageCode.Substring(1, Math.Min(3, messageCode.Length));
        }

        private static ResourceManager GetMessages(string nameSpace, string systemName, Assembly assembly)
        {
            string key = systemName + assembly.FullName;
            if (_resourceManagers.ContainsKey(key))
            {
                return (ResourceManager)_resourceManagers[key];
            }
            else
            {
                StringBuilder buf = new StringBuilder();
                if (nameSpace != null)
                {
                    buf.Append(nameSpace);
                    buf.Append(".");
                }
                buf.Append(systemName);
                buf.Append(MESSAGES);
                ResourceManager rm = new ResourceManager(buf.ToString(), assembly);
                _resourceManagers[key] = rm;
                return rm;
            }
        }

        private static string GetNoPatternMessage(object[] args)
        {
            if (args == null || args.Length == 0) return string.Empty;
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
