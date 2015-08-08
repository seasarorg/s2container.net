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
using Seasar.Framework.Util;

namespace Seasar.Dao.Node
{
    public class ParenBindVariableNode : AbstractNode
    {
        private readonly string _bindName;

        public ParenBindVariableNode(string expression)
        {
            _bindName = expression;
            Expression = "self.GetArg('" + expression + "')";
        }

        public string Expression { get; }

        public override void Accept(ICommandContext ctx)
        {
            var var = InvokeExpression(Expression, ctx);
            if (var != null)
            {
                var list = var as IList;
                if (list != null)
                {
                    Array array = new object[list.Count];
                    list.CopyTo(array, 0);
                    _BindArray(ctx, array);
                }
            }
            else if (var == null)
            {
                return;
            }
            else if (var.GetExType().IsArray)
            {
                _BindArray(ctx, var);
            }
            else
            {
                ctx.AddSql(var, var.GetExType(), _bindName);
            }
        }

        private void _BindArray(ICommandContext ctx, object arrayArg)
        {
            var array = arrayArg as object[];
            if (array != null)
            {
                var length = array.Length;
                if (length == 0) return;
                Type type = null;
                for (var i = 0; i < length; ++i)
                {
                    var o = array[i];
                    if (o != null) type = o.GetExType();
                }
                ctx.AddSql("(");
                ctx.AddSql(array[0], type, _bindName + 1);
                for (var i = 1; i < length; ++i)
                {
                    ctx.AppendSql(array[i], type, _bindName + (i + 1));
                }
            }
            ctx.AddSql(")");
        }
    }
}
