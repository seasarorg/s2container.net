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
using System.Text;
using System.Text.RegularExpressions;

namespace Seasar.Dao.Node
{
    public class ExpressionUtil
    {
        private IToken _current;
        private int _start;
        private string _text;
        private readonly Regex _reOps;
        private readonly Regex _reLit;
        private readonly Regex _reSym;

        public ExpressionUtil()
        {
            _reOps = new Regex(@"^\s*(&&|\|\||<=|>=|==|!=|[=+\-*/^()!<>])", RegexOptions.Compiled);
            _reSym = new Regex(@"^\s*(\-?\b*[^=+\-*/^()!<>\s]*)", RegexOptions.Compiled);
            _reLit = new Regex(@"^\s*([-+]?[0-9]+(\.[0-9]+)?)", RegexOptions.Compiled);
        }

        public string parseExpression(string expression)
        {
            _current = null;
            _text = expression;
            StringBuilder sb = new StringBuilder(255);
            while (!EOF())
            {
                IToken token = NextToken();
                sb.Append(token.Value + " ");
            }
            return sb.ToString().TrimEnd(' ');
        }

        protected bool EOF()
        {
            if (_current is Eof)
            {
                return true;
            }
            return false;
        }

        protected IToken NextToken()
        {
            Match match;
            match = _reLit.Match(_text);
            if (match.Length != 0)
            {
                SetNumberLiteralToken(match);
            }
            else
            {
                match = _reOps.Match(_text);
                if (match.Length != 0)
                {
                    SetOperatorToken(match);
                }
                else
                {
                    match = _reSym.Match(_text);
                    if (match.Length != 0)
                    {
                        SetSymbolToken(match);
                    }
                    else
                    {
                        _current = new Eof();
                    }
                }
            }
            return _current;
        }

        private void SetNumberLiteralToken(Match match)
        {
            IToken token;
            _start += match.Length;
            _text = _text.Substring(match.Length);
            token = new NumberLiteral();
            token.Value = match.Groups[1].Value;
            _current = token;
        }

        private void SetSymbolToken(Match match)
        {
            IToken token;
            _start += match.Length;
            _text = _text.Substring(match.Length);
            token = new Symbol();
            token.Value = match.Groups[1].Value;
            _current = token;
        }

        private void SetOperatorToken(Match match)
        {
            IToken token;
            _start += match.Length;
            _text = _text.Substring(match.Length);
            token = new Operator();
            token.Value = match.Groups[1].Value;
            _current = token;
        }
    }

    #region Token

    public interface IToken
    {
        object Value { get; set;}
    }

    public class Eof : IToken
    {
        public object Value
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }
    }

    public class Symbol : IToken
    {
        private string _value;
        private readonly string[] _escapes = { "null", "true", "false" };

        public object Value
        {
            get { return GetArgValue(); }
            set { _value = (string) value; }
        }

        private string GetArgValue()
        {
            if (_value.StartsWith("'") && _value.EndsWith("'"))
                return _value;

            foreach (string escape in _escapes)
            {
                if (_value.ToLower() == escape)
                    return _value.ToLower();
            }

            return "self.GetArg('" + _value + "')";
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    public class NumberLiteral : IToken
    {
        private float _value;

        public object Value
        {
            get { return _value; }
            set { _value = (float) Double.Parse(value.ToString()); }
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }

    public class Operator : IToken
    {
        private string _value;

        public object Value
        {
            get { return _value; }
            set { _value = (string) value; }
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }

    #endregion
}
