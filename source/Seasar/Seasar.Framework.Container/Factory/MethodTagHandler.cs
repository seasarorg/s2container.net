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
using Seasar.Framework.Util;
using Seasar.Framework.Xml;

namespace Seasar.Framework.Container.Factory
{
	/// <summary>
	/// MethodTagHandler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public abstract class MethodTagHandler : TagHandler
	{
		protected void ProcessExpression(
			IMethodDef methodDef,string expression,string tagName)
		{
			string expr = expression;
			if(expr != null)
			{
				expr = expr.Trim();
				if(!StringUtil.IsEmpty(expr))
				{
					methodDef.Expression = expr;
				}
				else
				{
					expr = null;
				}
			}
			if(methodDef.MethodName == null && expr == null)
			{
				throw new TagAttributeNotDefinedRuntimeException(tagName,"name");
			}
		}
	}
}
