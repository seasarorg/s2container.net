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
using System.Text;
using System.Collections;

namespace Seasar.Framework.Xml
{
	/// <summary>
	/// TagHandlerContext ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public sealed class TagHandlerContext
	{
		private StringBuilder body_ = null;
		private StringBuilder characters_ = new StringBuilder();
		private Stack bodyStack_ = new Stack();
		private StringBuilder path_ = new StringBuilder();
		private StringBuilder detailPath_ = new StringBuilder();
		private string qName_ = "";
		private Stack qNameStack_ = new Stack();
		private object result_;
		private Stack stack_ = new Stack();
		private Hashtable pathCounts_ = new Hashtable();
		private Hashtable parameters_ = new Hashtable();

		public TagHandlerContext()
		{
		}

		public void Push(object obj)
		{
			if(stack_.Count == 0) result_ = obj;
			stack_.Push(obj);
		}

		public object Result
		{
			get { return result_; }
		}

		public object Pop()
		{
			return stack_.Pop();
		}

		public object Peek()
		{
			return stack_.Peek();
		}

		public object Peek(int n)
		{
			IEnumerator enu = stack_.GetEnumerator();
			int index = stack_.Count - n - 1;
			int i = 0;
			while(enu.MoveNext())
			{
				if(index == i++) return enu.Current;
			}
			return null;
		}

		public object Peek(Type type)
		{
			IEnumerator enu = stack_.GetEnumerator();
			while(enu.MoveNext())
			{
				object o = enu.Current;
				if(type.IsInstanceOfType(o)) return o;
			}
			return null;
		}

		public object GetParameter(string name)
		{
			return parameters_[name];
		}

		public void AddParameter(string name,object parameter)
		{
			parameters_[name] = parameter;
		}

		public void StartElement(string qName)
		{
			bodyStack_.Push(body_);
			body_ = new StringBuilder();
			characters_ = new StringBuilder();
			qNameStack_.Push(qName_);
			qName_ = qName;
			path_.Append("/");
			path_.Append(qName);
			int pathCount = this.IncrementPathCount();
			detailPath_.Append("/");
			detailPath_.Append(qName);
			detailPath_.Append("[");
			detailPath_.Append(pathCount);
			detailPath_.Append("]");
		}

		public string Characters
		{
			get { return characters_.ToString().Trim(); }
			set
			{
				body_.Append(value);
				characters_.Append(value);
			}
		}

		public string Body
		{
			get { return body_.ToString().Trim(); }
		}

		public bool IsCharactersEol
		{
			get
			{
				if(characters_.Length == 0) return false;
				return characters_[characters_.Length - 1] == '\n';
			}
		}

		public void ClearCharacters()
		{
			characters_ = new StringBuilder();
		}

		public void EndElement()
		{
			body_ = (StringBuilder) bodyStack_.Pop();
			RemoveLastPath(path_);
			RemoveLastPath(detailPath_);
			qName_ = (string) qNameStack_.Pop();
		}

		private static void RemoveLastPath(StringBuilder path)
		{
			int last = path.ToString().LastIndexOf("/");
			path.Remove(last,path.Length - last);
		}

		public string Path
		{
			get { return path_.ToString(); }
		}

		public string DetailPath
		{
			get { return detailPath_.ToString(); }
		}

		public string QName
		{
			get { return qName_; }
		}

		private int IncrementPathCount()
		{
			string path = this.Path;
			int pathCount = 0;
			
            if(pathCounts_[path] != null)
            {
                pathCount = (int) pathCounts_[path];
            }

			pathCount++;
			pathCounts_[path] = pathCount;
			return pathCount;
		}
	}
}
