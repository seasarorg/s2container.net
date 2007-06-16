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

namespace Seasar.Dao.Parser
{
    public class SqlTokenizerImpl : ISqlTokenizer
    {
        private readonly string _sql;
        private int _position = 0;
        private string _token;
        private TokenType _tokenType = TokenType.SQL;
        private TokenType _nextTokenType = TokenType.SQL;
        private int _bindVariableNum = 0;

        public SqlTokenizerImpl(string sql)
        {
            _sql = sql;
        }

        #region ISqlTokenizer ƒƒ“ƒo

        public string Token
        {
            get { return _token; }
        }

        public string Before
        {
            get { return _sql.Substring(0, _position); }
        }

        public string After
        {
            get { return _sql.Substring(_position); }
        }

        public int Position
        {
            get { return _position; }
        }

        public TokenType TokenType
        {
            get { return _tokenType; }
        }

        public TokenType NextTokenType
        {
            get { return NextTokenType; }
        }

        public TokenType Next()
        {
            if (_position >= _sql.Length)
            {
                _token = null;
                _tokenType = TokenType.EOF;
                _nextTokenType = TokenType.EOF;
                return _tokenType;
            }
            switch (_nextTokenType)
            {
                case TokenType.SQL:
                    ParseSql();
                    break;
                case TokenType.COMMENT:
                    ParseComment();
                    break;
                case TokenType.ELSE:
                    ParseElse();
                    break;
                case TokenType.BIND_VARIABLE:
                    ParseBindVariable();
                    break;
                default:
                    ParseEof();
                    break;
            }
            return _tokenType;
        }

        public string SkipToken()
        {
            int index = _sql.Length;
            char quote = _position < _sql.Length ? _sql.ToCharArray()[_position] : '\0';
            bool quoting = quote == '\'' || quote == '(';
            if (quote == '(') quote = ')';

            for (int i = quoting ? _position + 1 : _position; i < _sql.Length; ++i)
            {
                char c = _sql.ToCharArray()[i];
                if ((Char.IsWhiteSpace(c) || c == ',' || c == ')' || c == '(')
                    && !quoting)
                {
                    index = i;
                    break;
                }
                else if (c == '/' && i + 1 < _sql.Length
                    && _sql.ToCharArray()[i + 1] == '*')
                {
                    index = i;
                    break;
                }
                else if (c == '-' && i + 1 < _sql.Length
                    && _sql.ToCharArray()[i + 1] == '-')
                {
                    index = i;
                    break;
                }
                else if (quoting && quote == '\'' && c == '\''
                    && (i + 1 >= _sql.Length || _sql.ToCharArray()[i + 1] != '\''))
                {
                    index = i + 1;
                    break;
                }
                else if (quoting && c == quote)
                {
                    index = i + 1;
                    break;
                }
            }
            _token = _sql.Substring(_position, (index - _position));
            _tokenType = TokenType.SQL;
            _nextTokenType = TokenType.SQL;
            _position = index;
            return _token;
        }

        public string SkipWhitespace()
        {
            int index = SkipWhitespace(_position);
            _token = _sql.Substring(_position, (index - _position));
            _position = index;
            return _token;
        }

        #endregion

        protected void ParseSql()
        {
            int commentStartPos = _sql.IndexOf("/*", _position);
            int lineCommentStartPos = _sql.IndexOf("--", _position);
            int bindVariableStartPos = _sql.IndexOf("?", _position);
            int elseCommentStartPos = -1;
            int elseCommentLength = -1;

            if (bindVariableStartPos < 0)
            {
                bindVariableStartPos = _sql.IndexOf("?", _position);
            }
            if (lineCommentStartPos >= 0)
            {
                int skipPos = SkipWhitespace(lineCommentStartPos + 2);
                if (skipPos + 4 < _sql.Length
                    && "ELSE" == _sql.Substring(skipPos, ((skipPos + 4) - skipPos)))
                {
                    elseCommentStartPos = lineCommentStartPos;
                    elseCommentLength = skipPos + 4 - lineCommentStartPos;
                }
            }
            int nextStartPos = GetNextStartPos(commentStartPos,
                elseCommentStartPos, bindVariableStartPos);
            if (nextStartPos < 0)
            {
                _token = _sql.Substring(_position);
                _nextTokenType = TokenType.EOF;
                _position = _sql.Length;
                _tokenType = TokenType.SQL;
            }
            else
            {
                _token = _sql.Substring(_position, nextStartPos - _position);
                _tokenType = TokenType.SQL;
                bool needNext = nextStartPos == _position;
                if (nextStartPos == commentStartPos)
                {
                    _nextTokenType = TokenType.COMMENT;
                    _position = commentStartPos + 2;
                }
                else if (nextStartPos == elseCommentStartPos)
                {
                    _nextTokenType = TokenType.ELSE;
                    _position = elseCommentStartPos + elseCommentLength;
                }
                else if (nextStartPos == bindVariableStartPos)
                {
                    _nextTokenType = TokenType.BIND_VARIABLE;
                    _position = bindVariableStartPos;
                }
                if (needNext) Next();
            }
        }

        protected int GetNextStartPos(int commentStartPos, int elseCommentStartPos,
            int bindVariableStartPos)
        {
            int nextStartPos = -1;
            if (commentStartPos >= 0)
                nextStartPos = commentStartPos;

            if (elseCommentStartPos >= 0
                && (nextStartPos < 0 || elseCommentStartPos < nextStartPos))
                nextStartPos = elseCommentStartPos;

            if (bindVariableStartPos >= 0
                && (nextStartPos < 0 || bindVariableStartPos < nextStartPos))
                nextStartPos = bindVariableStartPos;

            return nextStartPos;
        }

        protected string NextBindVariableName
        {
            get { return "$" + ++_bindVariableNum; }
        }

        protected void ParseComment()
        {
            int commentEndPos = _sql.IndexOf("*/", _position);
            if (commentEndPos < 0)
                throw new TokenNotClosedRuntimeException("*/",
                    _sql.Substring(_position));

            _token = _sql.Substring(_position, (commentEndPos - _position));
            _nextTokenType = TokenType.SQL;
            _position = commentEndPos + 2;
            _tokenType = TokenType.COMMENT;
        }

        protected void ParseBindVariable()
        {
            _token = NextBindVariableName;
            _nextTokenType = TokenType.SQL;
            _position++;
            _tokenType = TokenType.BIND_VARIABLE;
        }

        protected void ParseElse()
        {
            _token = null;
            _nextTokenType = TokenType.SQL;
            _tokenType = TokenType.ELSE;
        }

        protected void ParseEof()
        {
            _token = null;
            _tokenType = TokenType.EOF;
            _nextTokenType = TokenType.EOF;
        }

        private int SkipWhitespace(int position)
        {
            int index = _sql.Length;
            for (int i = position; i < _sql.Length; ++i)
            {
                char c = _sql.ToCharArray()[i];
                if (!Char.IsWhiteSpace(c))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
