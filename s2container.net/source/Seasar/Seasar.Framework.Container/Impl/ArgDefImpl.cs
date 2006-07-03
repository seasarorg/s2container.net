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
using Seasar.Framework.Container.Util;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Impl
{
	/// <summary>
	/// 引数を定義します。
	/// </summary>
	public class ArgDefImpl : IArgDef
	{
		private Object value_;
		private IS2Container container_;
		private string expression_;
		private Type argType_;
		private IComponentDef childComponentDef_;
		private MetaDefSupport metaDefSupport_ = new MetaDefSupport();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ArgDefImpl()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">値</param>
		public ArgDefImpl(object value)
		{
			this.value_ = value;
		}

		#region ArgDef メンバ

		public object Value
		{
			get
			{
				if(expression_ != null)
				{
					if(container_.HasComponentDef(expression_))
					{
						return container_.GetComponent(expression_);
					}
					else if (IsCharacterString(expression_))
					{
						return JScriptUtil.Evaluate(expression_, container_);
					}
					else if (expression_.IndexOf(".") > 0)
					{
						int lastIndex = expression_.LastIndexOf(".");
						string enumTypeName =
							expression_.Substring(0, lastIndex);
						Type enumType = ClassUtil.ForName(enumTypeName,
							AppDomain.CurrentDomain.GetAssemblies());
						if (enumType != null && enumType.IsEnum)
						{
							return Enum.Parse(enumType, expression_.Substring(lastIndex + 1));
						}

						Type classType = ClassUtil.ForName(expression_,
							AppDomain.CurrentDomain.GetAssemblies());
						if (classType != null && classType.IsClass)
						{
							return classType;
						}

						return JScriptUtil.Evaluate(expression_, container_);
					}
					else
					{
						return JScriptUtil.Evaluate(expression_, container_);
					}
				}
				if(childComponentDef_ != null) 
				{
					return childComponentDef_.GetComponent(argType_);
				}
				return value_;
			}
			set
			{
				value_ = value;
			}
		}

		public IS2Container Container
		{
			get { return container_; }
			set
			{
				container_ = value;
				if(childComponentDef_ != null) 
				{
					childComponentDef_.Container = value;
				}
				metaDefSupport_.Container = value;
			}
		}

		public string Expression
		{
			get { return expression_; }
			set	{ expression_ = value; }
		}

		public IComponentDef ChildComponentDef
		{
			set
			{
				if(container_ != null)
				{
					value.Container = container_;
				}
				childComponentDef_ = value;
			}
		}

		public Type ArgType
		{
			get { return argType_; }
			set { argType_ = value; }
		}

		#endregion

		#region IMetaDefAware メンバ

		public void AddMetaDef(IMetaDef metaDef)
		{
			metaDefSupport_.AddMetaDef(metaDef);
		}

		public int MetaDefSize
		{
			get { return metaDefSupport_.MetaDefSize; }
		}

		public IMetaDef GetMetaDef(int index)
		{
			return metaDefSupport_.GetMetaDef(index);
		}

		public IMetaDef GetMetaDef(string name)
		{
			return metaDefSupport_.GetMetaDef(name);
		}

		public IMetaDef[] GetMetaDefs(string name)
		{
			return metaDefSupport_.GetMetaDefs(name);
		}

		#endregion

		private bool IsCharacterString(string str)
		{
			if (str == null) return false;
			if (str.StartsWith("\"") && str.EndsWith("\"") && str.Length >= 2) return true;
			return false;
		}
	}
}
