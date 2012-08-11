#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using Seasar.Framework.Container.Factory;

namespace Seasar.Examples.Reference.Injection
{
    public class HelloConstructorInjection : IHello
    {
        private readonly string _message;

        public HelloConstructorInjection(string message)
        {
            _message = message;
        }

        public void ShowMessage()
        {
            Console.WriteLine(_message);
        }

    }

    public class HelloConstructorInjectionClient
    {
        private const string PATH = "Seasar.Examples/Reference/Injection/HelloConstructorInjection.dicon";

        public void Main()
        {
            // 型を指定してコンポーネントを取得する場合
            IS2Container container = S2ContainerFactory.Create(PATH);
            IHello hello = (IHello) container.GetComponent(typeof(IHello));
            hello.ShowMessage();

            // 名前を指定してコンポーネントを取得する場合
            IHello hello2 = (IHello) container.GetComponent("hello");
            hello2.ShowMessage();
        }
    }
}
