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

using Seasar.Framework.Container;

namespace Seasar.Examples.Reference.Container
{
	public class ContainerClient
	{
		private IS2Container aaa;
		private IS2Container bbb;

		public ContainerClient(IS2Container container) {
			this.aaa = container;
		}
		
		public IS2Container DIContainer
		{
			get { return this.bbb; }
			set { this.bbb = value; }
		}

		public void Main()
		{
			Console.WriteLine(aaa.Root.Path);
			Console.WriteLine(this.DIContainer.Path);
		}
	}
}
