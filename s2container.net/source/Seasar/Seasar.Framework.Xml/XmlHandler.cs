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

namespace Seasar.Framework.Xml
{
    public sealed class XmlHandler
    {
        private readonly TagHandlerRule _tagHandlerRule;

        public XmlHandler(TagHandlerRule tagHandlerRule)
        {
            _tagHandlerRule = tagHandlerRule;
        }

        public TagHandlerContext TagHandlerContext { get; } = new TagHandlerContext();

        public void StartElement(string qName, IAttributes attributes)
        {
            _AppendBody();
            TagHandlerContext.StartElement(qName);
            _Start(attributes);
        }

        public void Characters(string text)
        {
            TagHandlerContext.Characters = text;
            _AppendBody();
        }

        public void EndElement(string qName)
        {
            _AppendBody();
            _End();
            TagHandlerContext.EndElement();
        }

        public object Result => TagHandlerContext.Result;

        private TagHandler GetTagHandlerByPath()
        {
            return _tagHandlerRule[TagHandlerContext.Path];
        }

        private TagHandler GetTagHandlerByQName()
        {
            return _tagHandlerRule[TagHandlerContext.QName];
        }

        private void _Start(IAttributes attributes)
        {
            var th = GetTagHandlerByPath();
            _Start(th, attributes);
            th = GetTagHandlerByQName();
            _Start(th, attributes);
        }

        private void _Start(TagHandler handler, IAttributes attributes)
        {
            if (handler != null)
            {
                try
                {
                    handler.Start(TagHandlerContext, attributes);
                }
                catch (Exception ex)
                {
                    _ReportDetailPath(ex);
                    throw;
                }
            }
        }

        private void _AppendBody()
        {
            var characters = TagHandlerContext.Characters;
            if (characters.Length > 0)
            {
                var th = GetTagHandlerByPath();
                _AppendBody(th, characters);
                th = GetTagHandlerByQName();
                _AppendBody(th, characters);
                TagHandlerContext.ClearCharacters();
            }
        }

        private void _AppendBody(TagHandler handler, string characters)
        {
            if (handler != null)
            {
                try
                {
                    handler.AppendBody(TagHandlerContext, characters);
                }
                catch (Exception ex)
                {
                    _ReportDetailPath(ex);
                    throw;
                }
            }
        }

        private void _End()
        {
            var body = TagHandlerContext.Body;
            var th = GetTagHandlerByPath();
            _End(th, body);
            th = GetTagHandlerByQName();
            _End(th, body);
        }

        private void _End(TagHandler handler, string body)
        {
            if (handler != null)
            {
                try
                {
                    handler.End(TagHandlerContext, body);
                }
                catch (Exception ex)
                {
                    _ReportDetailPath(ex);
                    throw;
                }
            }
        }

        private void _ReportDetailPath(Exception cause)
        {
            Console.WriteLine(@"Exception occured at " + TagHandlerContext.DetailPath);
            Console.WriteLine(cause.Message);
            Console.WriteLine(cause.StackTrace);
        }
    }
}
