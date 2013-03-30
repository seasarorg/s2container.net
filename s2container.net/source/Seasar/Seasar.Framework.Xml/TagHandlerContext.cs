#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Text;
using System.Collections;

namespace Seasar.Framework.Xml
{
    public sealed class TagHandlerContext
    {
        private StringBuilder _body = null;
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

        public object Result
        {
            get { return _result; }
        }

        public object Pop()
        {
            return _stack.Pop();
        }

        public object Peek()
        {
            return _stack.Peek();
        }

        public object Peek(int n)
        {
            IEnumerator enu = _stack.GetEnumerator();
            int index = _stack.Count - n - 1;
            int i = 0;
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
            IEnumerator enu = _stack.GetEnumerator();
            while (enu.MoveNext())
            {
                object o = enu.Current;
                if (type.IsInstanceOfType(o))
                {
                    return o;
                }
            }
            return null;
        }

        public object GetParameter(string name)
        {
            return _parameters[name];
        }

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
            int pathCount = IncrementPathCount();
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

        public string Body
        {
            get { return _body.ToString().Trim(); }
        }

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
            RemoveLastPath(_path);
            RemoveLastPath(_detailPath);
            _qName = (string) _qNameStack.Pop();
        }

        private static void RemoveLastPath(StringBuilder path)
        {
            int last = path.ToString().LastIndexOf("/");
            path.Remove(last, path.Length - last);
        }

        public string Path
        {
            get { return _path.ToString(); }
        }

        public string DetailPath
        {
            get { return _detailPath.ToString(); }
        }

        public string QName
        {
            get { return _qName; }
        }

        private int IncrementPathCount()
        {
            string path = Path;
            int pathCount = 0;

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
