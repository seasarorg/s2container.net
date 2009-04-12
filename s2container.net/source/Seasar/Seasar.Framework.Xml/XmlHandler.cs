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
using Seasar.Framework.Xml;

namespace Seasar.Framework.Xml
{
    public sealed class XmlHandler
    {
        private readonly TagHandlerRule _tagHandlerRule;
        private readonly TagHandlerContext _context = new TagHandlerContext();

        public XmlHandler(TagHandlerRule tagHandlerRule)
        {
            _tagHandlerRule = tagHandlerRule;
        }

        public TagHandlerContext TagHandlerContext
        {
            get { return _context; }
        }

        public void StartElement(string qName, IAttributes attributes)
        {
            AppendBody();
            _context.StartElement(qName);
            Start(attributes);
        }

        public void Characters(string text)
        {
            _context.Characters = text;
            AppendBody();
        }

        public void EndElement(string qName)
        {
            AppendBody();
            End();
            _context.EndElement();
        }

        public object Result
        {
            get { return _context.Result; }
        }

        private TagHandler GetTagHandlerByPath()
        {
            return _tagHandlerRule[_context.Path];
        }

        private TagHandler GetTagHandlerByQName()
        {
            return _tagHandlerRule[_context.QName];
        }

        private void Start(IAttributes attributes)
        {
            TagHandler th = GetTagHandlerByPath();
            Start(th, attributes);
            th = GetTagHandlerByQName();
            Start(th, attributes);
        }

        private void Start(TagHandler handler, IAttributes attributes)
        {
            if (handler != null)
            {
                try
                {
                    handler.Start(_context, attributes);
                }
                catch (Exception ex)
                {
                    ReportDetailPath(ex);
                    throw;
                }
            }
        }

        private void AppendBody()
        {
            string characters = _context.Characters;
            if (characters.Length > 0)
            {
                TagHandler th = GetTagHandlerByPath();
                AppendBody(th, characters);
                th = GetTagHandlerByQName();
                AppendBody(th, characters);
                _context.ClearCharacters();
            }
        }

        private void AppendBody(TagHandler handler, string characters)
        {
            if (handler != null)
            {
                try
                {
                    handler.AppendBody(_context, characters);
                }
                catch (Exception ex)
                {
                    ReportDetailPath(ex);
                    throw;
                }
            }
        }

        private void End()
        {
            string body = _context.Body;
            TagHandler th = GetTagHandlerByPath();
            End(th, body);
            th = GetTagHandlerByQName();
            End(th, body);
        }

        private void End(TagHandler handler, string body)
        {
            if (handler != null)
            {
                try
                {
                    handler.End(_context, body);
                }
                catch (Exception ex)
                {
                    ReportDetailPath(ex);
                    throw;
                }
            }
        }

        private void ReportDetailPath(Exception cause)
        {
            Console.WriteLine("Exception occured at " + _context.DetailPath);
            Console.WriteLine(cause.Message);
            Console.WriteLine(cause.StackTrace);
        }
    }
}
