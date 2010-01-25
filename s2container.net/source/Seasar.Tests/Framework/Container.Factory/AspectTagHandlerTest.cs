#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Tests.Framework.Container.Factory
{
    [TestFixture]
    public class AspectTagHandlerTest
    {
        private const string PATH
            = "Seasar/Tests/Framework/Container/Factory/AspectTagHandlerTest.dicon";

        [SetUp]
        public void SetUp()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        [Test]
        public void TestAspect()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);

            IList list = (IList) container.GetComponent(typeof(IList));
            int count = list.Count;

            IFoo foo = (IFoo) container.GetComponent(typeof(IFoo));
            int time = foo.Time;
            foo.ToString();
            int hashCode = foo.GetHashCode();
        }

        public interface IFoo
        {
            int Time { get; }
            int GetHashCode();
            string ToString();
        }

        public class FooImpl : IFoo
        {
            private readonly int _time = 3;

            #region IFoo ÉÅÉìÉo

            public int Time
            {
                get { return _time; }
            }

            public override string ToString()
            {
                return _time.ToString();
            }

            #endregion
        }
    }
}
