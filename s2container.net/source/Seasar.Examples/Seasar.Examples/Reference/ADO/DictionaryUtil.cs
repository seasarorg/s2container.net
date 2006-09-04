#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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

namespace Seasar.Examples.Reference.ADO
{
	public sealed class DictionaryUtil
	{
		private DictionaryUtil()
		{
		}

		public static string ToDecorateString(IDictionary dictionary)
		{
			if (dictionary == null || dictionary.Count == 0)
			{
				return string.Empty;
			}

			StringBuilder buf = new StringBuilder();
			foreach (object key in dictionary.Keys)
			{
				buf.AppendFormat("{0}={1}, ", key, (dictionary[key] == null) ? "null" : dictionary[key]);
			}
			buf.Length -= 2;
			return buf.ToString();
		}
	}
}
