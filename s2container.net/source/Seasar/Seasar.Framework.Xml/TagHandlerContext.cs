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
using System.Collections;
using System.Text;

namespace Seasar.Framework.Xml
{
    public sealed class TagHandlerContext
    {
        private StringBuilder _body;
        private StringBuilder _characters = new StringBuilder();
        private readonly Stack _bodyStack = new Stack();
        private readonly StringBuilder _path = new StringBuilder();
        private readonly StringBuilder _detailPath = new StringBuilder();
        private string _qName = string.Empty;
        private readonly Stack _qNameStack = new Stack();
        private object _result;
        private readonly Stack _stack = new Stack();
        private readonly Hashtable _pathCounts = new Hashtable();
        private readonly Hashtable _parameters = new Hashtable();

        public void Push(object obj)
        {
            if (_stack.Count == 0)
            {
                _result = obj;
            }
            _stack.Push(obj);
        }

        public object Result => _result;

        public object Pop() => _stack.Pop();

        public object Peek() => _stack.Peek();

        public object Peek(int n)
        {
            var enu = _stack.GetEnumerator();
            var index = _stack.Count - n - 1;
            var i = 0;
            while (enu.MoveNext())
            {
                if (index == i++)
                {
                    return enu.Current;
                }
            }
            return null;
        }

        public object Peek(Type type)
        {
            var enu = _stack.GetEnumerator();
            while (enu.MoveNext())
            {
                var o = enu.Current;
                if (type.IsInstanceOfType(o))
                {
                    return o;
                }
            }
            return null;
        }

        public object GetParameter(string name) => _parameters[name];

        public void AddParameter(string name, object parameter)
        {
            _parameters[name] = parameter;
        }

        public void StartElement(string value)
        {
            _bodyStack.Push(_body);
            _body = new StringBuilder();
            _characters = new StringBuilder();
            _qNameStack.Push(_qName);
            _qName = value;
            _path.Append("/");
            _path.Append(value);
            var pathCount = _IncrementPathCount();
            _detailPath.Append("/");
            _detailPath.Append(value);
            _detailPath.Append("[");
            _detailPath.Append(pathCount);
            _detailPath.Append("]");
        }

        public string Characters
        {
            get { return _characters.ToString().Trim(); }
            set
            {
                _body.Append(value);
                _characters.Append(value);
            }
        }

        public string Body => _body.ToString().Trim();

        public bool IsCharactersEol
        {
            get
            {
                if (_characters.Length == 0)
                {
                    return false;
                }
                return _characters[_characters.Length - 1] == '\n';
            }
        }

        public void ClearCharacters()
        {
            _characters = new StringBuilder();
        }

        public void EndElement()
        {
            _body = (StringBuilder) _bodyStack.Pop();
            _RemoveLastPath(_path);
            _RemoveLastPath(_detailPath);
            _qName = (string) _qNameStack.Pop();
        }

        private static void _RemoveLastPath(StringBuilder path)
        {
            var last = path.ToString().LastIndexOf('/');
            path.Remove(last, path.Length - last);
        }

        public string Path => _path.ToString();

        public string DetailPath => _detailPath.ToString();

        public string QName => _qName;

        private int _IncrementPathCount()
        {
            var path = Path;
            var pathCount = 0;

            if (_pathCounts[path] != null)
            {
                pathCount = (int) _pathCounts[path];
            }

            pathCount++;
            _pathCounts[path] = pathCount;
            return pathCount;
        }
    }
}
