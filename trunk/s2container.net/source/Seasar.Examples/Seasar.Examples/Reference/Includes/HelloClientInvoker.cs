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

namespace Seasar.Examples.Reference.Includes
{
    public class HelloClientInvoker
    {
        private IHelloClient _root;
        private IHelloClient _aaa;
        private IHelloClient _bbb;

        public IHelloClient Root
        {
            get { return _root; }
            set { _root = value; }
        }

        public IHelloClient Aaa
        {
            get { return _aaa; }
            set { _aaa = value; }
        }

        public IHelloClient Bbb
        {
            get { return _bbb; }
            set { _bbb = value; }
        }

        public void Main()
        {
            Console.WriteLine("rootÇÃé¿çsåãâ ");
            Root.ShowMessage();
            Console.WriteLine(" ------------------------- ");
            Console.WriteLine();

            Console.WriteLine("aaaÇÃé¿çsåãâ ");
            Aaa.ShowMessage();
            Console.WriteLine(" ------------------------- ");
            Console.WriteLine();

            Console.WriteLine("bbbÇÃé¿çsåãâ ");
            Bbb.ShowMessage();
            Console.WriteLine(" ------------------------- ");
            Console.WriteLine();
        }
    }
}
