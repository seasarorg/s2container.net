#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Reflection;
using Seasar.Framework.Util;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Assembler
{
	/// <summary>
	/// AbstractConstructorAssembler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public abstract class AbstractConstructorAssembler : AbstractAssembler,
		IConstructorAssembler
	{
		public AbstractConstructorAssembler(IComponentDef componentDef)
			: base(componentDef)
		{
		}

		protected object AssembleDefault()
		{
			Type type = this.ComponentDef.ComponentType;
			ConstructorInfo constructor = ClassUtil.GetConstructorInfo(type,null);
			object obj = ConstructorUtil.NewInstance(constructor,null);
            if (this.ComponentDef.AspectDefSize > 0)
            {
                AopProxyUtil.WeaveAspect(ref obj, this.ComponentDef);
            }
			return obj;
		}

		#region ConstructorAssembler ÉÅÉìÉo

		public virtual object Assemble()
		{
			
			return null;
		}

		#endregion
	}
}
