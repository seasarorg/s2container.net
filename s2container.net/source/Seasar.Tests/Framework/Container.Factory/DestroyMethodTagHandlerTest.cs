#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

using System.Collections;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Tests.Framework.Container.Factory
{
    [TestFixture]
    public class DestroyMethodTagHandlerTest
    {
        private const string PATH
            = "Seasar/Tests/Framework/Container/Factory/DestroyMethodTagHandlerTest.dicon";

        [Test]
        public void TestArg()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            container.Init();
            Hashtable table = (Hashtable) container.GetComponent(typeof(Hashtable));
            container.Destroy();
            Assert.AreEqual(111, table["aaa"]);
        }
    }
}
