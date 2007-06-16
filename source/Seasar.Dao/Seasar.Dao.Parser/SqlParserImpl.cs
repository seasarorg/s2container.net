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
using System.Collections;
using Seasar.Dao.Node;
using Seasar.Framework.Util;

namespace Seasar.Dao.Parser
{
    public class SqlParserImpl : ISqlParser
    {
        private readonly ISqlTokenizer _tokenizer;
        private readonly Stack _nodeStack = new Stack();

        public SqlParserImpl(string sql)
        {
            sql = sql.Trim();
            if (sql.EndsWith(";"))
            {
                sql = sql.Substring(0, sql.Length - 1);
            }
            _tokenizer = new SqlTokenizerImpl(sql);
        }

        public INode Parse()
        {
            Push(new ContainerNode());
            while (TokenType.EOF != _tokenizer.Next())
            {
                ParseToken();
            }
            return Pop();
        }

        protected void ParseToken()
        {
            switch (_tokenizer.TokenType)
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
            }
        }

        protected void ParseSql()
        {
            string sql = _tokenizer.Token;
            if (IsElseMode())
            {
                sql = sql.Replace("--", string.Empty);
            }
            INode node = Peek();

            if ((node is IfNode || node is ElseNode) && node.ChildSize == 0)
            {
                ISqlTokenizer st = new SqlTokenizerImpl(sql);
                st.SkipWhitespace();
                string token = st.SkipToken();
                st.SkipWhitespace();
                if ("AND".Equals(token.ToUpper()) || "OR".Equals(token.ToUpper()))
                {
                    node.AddChild(new PrefixSqlNode(st.Before, st.After));
                }
                else
                {
                    node.AddChild(new SqlNode(sql));
                }
            }
            else
            {
                node.AddChild(new SqlNode(sql));
            }
        }

        protected void ParseComment()
        {
            string comment = _tokenizer.Token;
            if (IsTargetComment(comment))
            {
                if (IsIfComment(comment))
                {
                    ParseIf();
                }
                else if (IsBeginComment(comment))
                {
                    ParseBegin();
                }
                else if (IsEndComment(comment))
                {
                    return;
                }
                else
                {
                    ParseCommentBindVariable();
                }
            }
        }

        protected void ParseIf()
        {
            string condition = _tokenizer.Token.Substring(2).Trim();
            if (StringUtil.IsEmpty(condition))
            {
                throw new IfConditionNotFoundRuntimeException();
            }
            IfNode ifNode = new IfNode(condition);
            Peek().AddChild(ifNode);
            Push(ifNode);
            ParseEnd();
        }

        protected void ParseBegin()
        {
            BeginNode beginNode = new BeginNode();
            Peek().AddChild(beginNode);
            Push(beginNode);
            ParseEnd();
        }

        protected void ParseEnd()
        {
            while (TokenType.EOF != _tokenizer.Next())
            {
                if (_tokenizer.TokenType == TokenType.COMMENT
                    && IsEndComment(_tokenizer.Token))
                {
                    Pop();
                    return;
                }
                ParseToken();
            }
            throw new EndCommentNotFoundRuntimeException();
        }

        protected void ParseElse()
        {
            INode parent = Peek();
            if (!(parent is IfNode))
            {
                return;
            }
            IfNode ifNode = (IfNode) Pop();
            ElseNode elseNode = new ElseNode();
            ifNode.ElseNode = elseNode;
            Push(elseNode);
            _tokenizer.SkipWhitespace();
        }

        protected void ParseCommentBindVariable()
        {
            string expr = _tokenizer.Token;
            string s = _tokenizer.SkipToken();
            if (s.StartsWith("(") && s.EndsWith(")"))
            {
                Peek().AddChild(new ParenBindVariableNode(expr));
            }
            else if (expr.StartsWith("$"))
            {
                Peek().AddChild(new EmbeddedValueNode(expr.Substring(1)));
            }
            else
            {
                Peek().AddChild(new BindVariableNode(expr));
            }
        }

        protected void ParseBindVariable()
        {
            string expr = _tokenizer.Token;
            Peek().AddChild(new BindVariableNode(expr));
        }

        protected INode Pop()
        {
            return (INode) _nodeStack.Pop();
        }

        protected INode Peek()
        {
            return (INode) _nodeStack.Peek();
        }

        protected void Push(INode node)
        {
            _nodeStack.Push(node);
        }

        protected bool IsElseMode()
        {
            for (int i = 0; i < _nodeStack.Count; ++i)
            {
                if (_nodeStack.ToArray()[i] is ElseNode)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsTargetComment(string comment)
        {
            return comment != null && comment.Length > 0
                && IsCSharpIdentifierStart(comment.ToCharArray()[0]);
        }

        private static bool IsCSharpIdentifierStart(Char c)
        {
            return Char.IsLetterOrDigit(c) || c == '_' || c == '\\' || c == '$' || c == '@';
        }

        private static bool IsIfComment(string comment)
        {
            return comment.StartsWith("IF");
        }

        private static bool IsBeginComment(string content)
        {
            return content != null && "BEGIN".Equals(content);
        }

        private static bool IsEndComment(string content)
        {
            return content != null && "END".Equals(content);
        }
    }
}
