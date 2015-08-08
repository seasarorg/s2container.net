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
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using Seasar.Framework.Util;
using Seasar.Framework.Xml.Impl;

namespace Seasar.Framework.Xml
{
    public sealed class XmlHandlerParser
    {
        public XmlHandlerParser(XmlHandler xmlHandler)
        {
            XmlHandler = xmlHandler;
        }

        public XmlHandler XmlHandler { get; }

        public object Parse(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!Path.IsPathRooted(path))
                throw new DirectoryNotFoundException(nameof(path));

            StreamReader reader = null;
            var pathWithoutExt = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            var extension = ResourceUtil.GetExtension(path);

            if (File.Exists(path))
            {
                reader = new StreamReader(path);
            }
            else if (HttpContext.Current != null)
            {
                var path4Http = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);
                if (File.Exists(path4Http))
                {
                    reader = new StreamReader(path4Http);
                }
            }

            if (reader == null)
            {
                reader = ResourceUtil.GetResourceAsStreamReaderNoException(pathWithoutExt, extension);
            }

            if (reader == null)
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblys)
                {
                    reader = ResourceUtil.GetResourceAsStreamReaderNoException(pathWithoutExt, extension, assembly);
                    if (reader != null)
                    {
                        break;
                    }
                }
            }

            if (reader == null)
            {
                throw new ResourceNotFoundRuntimeException(path);
            }

            return Parse(reader);
        }

        public object Parse(StreamReader input)
        {
#if NET_1_1
            XmlValidatingReader reader = new XmlValidatingReader(
                new XmlTextReader(input));
            reader.ValidationType = ValidationType.DTD;
            reader.XmlResolver = new S2XmlResolver();
            reader.ValidationEventHandler += 
                new ValidationEventHandler(ValidationHandler);
#else

            var settings = new XmlReaderSettings
            {
                XmlResolver = new S2XmlResolver(),
                ValidationType = ValidationType.DTD
            };
            settings.ValidationEventHandler += ValidationHandler;
            settings.DtdProcessing = DtdProcessing.Parse;
            var reader = XmlReader.Create(input, settings);
#endif

            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            var elementName = reader.Name;
                            var isEmptyElement = reader.IsEmptyElement;
                            var attributes = new AttributesImpl();
                            if (reader.MoveToFirstAttribute())
                            {
                                do
                                {
                                    attributes.AddAttribute(reader.Name, reader.Value);
                                } while (reader.MoveToNextAttribute());
                            }
                            XmlHandler.StartElement(elementName, attributes);
                            if (isEmptyElement) XmlHandler.EndElement(reader.Name);
                            break;
                        case XmlNodeType.Text:
                            XmlHandler.Characters(reader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            XmlHandler.EndElement(reader.Name);
                            break;
                    }
                }
            }
            finally
            {
                reader.Close();
                input.Close();
            }

            return XmlHandler.Result;
        }

        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Console.Error.WriteLine("***Validation error");
            Console.Error.WriteLine($"\tSeverity:{args.Severity}");
            Console.Error.WriteLine($"\tMessage  :{args.Message}");
        }
    }
}
