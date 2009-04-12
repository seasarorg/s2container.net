#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

using MbUnit.Framework;
using Seasar.Framework.Container.Factory;

namespace Seasar.Tests.Framework.Container.Factory
{
    [TestFixture]
    public class SingletonS2ContainerFactoryTest
    {
        [Test]
        public void TestConfigPathUseS2Section()
        {
            SingletonS2ContainerFactory.Init();
            Assert.AreEqual("Seasar.Tests.test.dicon", SingletonS2ContainerFactory.ConfigPath);
            SingletonS2ContainerFactory.Destroy();
        }

        [Test]
        public void TestConfigPathUseConfigPath()
        {
            string configPath = SingletonS2ContainerFactory.ConfigPath;
            try
            {
                SingletonS2ContainerFactory.ConfigPath = "Seasar.Tests.Ado.dicon";
                SingletonS2ContainerFactory.Init();
                Assert.AreEqual("Seasar.Tests.Ado.dicon", SingletonS2ContainerFactory.ConfigPath);
                SingletonS2ContainerFactory.Destroy();
            }
            finally
            {
                SingletonS2ContainerFactory.ConfigPath = configPath;
            }
        }
    }
}
