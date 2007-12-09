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

namespace Seasar.Dao.Node
{
    public class ParenBindVariableNode : AbstractNode
    {
        private readonly string _bindName;
        private readonly string _expression;

        public ParenBindVariableNode(string expression)
        {
            _bindName = expression;
            _expression = "self.GetArg('" + expression + "')";
        }

        public string Expression
        {
            get { return _expression; }
        }

        public override void Accept(ICommandContext ctx)
        {
            object var = InvokeExpression(_expression, ctx);
            if (var != null)
            {
                IList list = var as IList;
                Array array = new object[list.Count];
                list.CopyTo(array, 0);
                BindArray(ctx, array);
            }
            else if (var == null)
            {
                return;
            }
            else if (var.GetType().IsArray)
            {
                BindArray(ctx, var);
            }
            else
            {
                ctx.AddSql(var, var.GetType(), _bindName);
            }
        }

        private void BindArray(ICommandContext ctx, object arrayArg)
        {
            object[] array = arrayArg as object[];
            int length = array.Length;
            if (length == 0) return;
            Type type = null;
            for (int i = 0; i < length; ++i)
            {
                object o = array[i];
                if (o != null) type = o.GetType();
            }
            ctx.AddSql("(");
            ctx.AddSql(array[0], type, _bindName + 1);
            for (int i = 1; i < length; ++i)
            {
                ctx.AppendSql(array[i], type, _bindName + (i + 1));
            }
            ctx.AddSql(")");
        }
    }
}
