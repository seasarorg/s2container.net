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

namespace Seasar.Examples.Reference.Includes
{
	/// <summary>
	/// HelloClientInvoker の概要の説明です。
	/// </summary>
	public class HelloClientInvoker
	{
		private IHelloClient root;
		private IHelloClient aaa;
		private IHelloClient bbb;

		public HelloClientInvoker()
		{
		}

		public IHelloClient Root
		{
			get { return this.root; }
			set { this.root = value; }
		}

		public IHelloClient Aaa
		{
			get { return this.aaa; }
			set { this.aaa = value; }
		}
		
		public IHelloClient Bbb
		{
			get { return this.bbb; }
			set { this.bbb = value; }
		}

		public void Main()
		{
			Console.WriteLine("rootの実行結果");
			Root.ShowMessage();
			Console.WriteLine(" ------------------------- ");
			Console.WriteLine();

			Console.WriteLine("aaaの実行結果");
			Aaa.ShowMessage();
			Console.WriteLine(" ------------------------- ");
			Console.WriteLine();
			
			Console.WriteLine("bbbの実行結果");
			Bbb.ShowMessage();
			Console.WriteLine(" ------------------------- ");
			Console.WriteLine();
		}
	}
}
