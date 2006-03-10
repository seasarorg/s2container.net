using System;

namespace Seasar.Tests.Framework.Unit
{
	public class Hoge : MarshalByRefObject
	{
		private string aaa;

		public String GetAaa() 
		{
			return aaa;
		}

		public void SetAaa(String aaa) 
		{
			this.aaa = aaa;
		}
	
		public string Greeting2()
		{
			return "Hello2";
		}

		public string Greeting()
		{
			return "Hello";
		}

		public string GetGreeting()
		{
			return "GetHello";
		}
		public string GetGreetingEx()
		{
			return "GetHelloEx";
		}
	}
}