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

using MbUnit.Framework;
using Seasar.Framework.Container.AutoRegister;

namespace Seasar.Tests.Framework.Container.AutoRegister
{
    [TestFixture]
    public class ClassPatternTest
    {
        [Test]
        public void TestAppliedForShortClassNameNull()
        {
            ClassPattern cp = new ClassPattern();
            Assert.IsTrue(cp.IsAppliedShortClassName("Hoge"));
        }

        [Test]
        public void TestAppliedForNormalPattern()
        {
            ClassPattern cp = new ClassPattern();
            cp.ShortClassNames = ".*Impl";
            Assert.IsTrue(cp.IsAppliedShortClassName("HogeImpl"), "1");
            Assert.IsFalse(cp.IsAppliedShortClassName("Hoge"), "2");
        }

        [Test]
        public void TestAppliedForMulti()
        {
            ClassPattern cp = new ClassPattern();
            cp.ShortClassNames = "Hoge$, HogeImpl$";
            Assert.IsTrue(cp.IsAppliedShortClassName("HogeImpl"), "1");
            Assert.IsTrue(cp.IsAppliedShortClassName("Hoge"), "2");
            Assert.IsFalse(cp.IsAppliedShortClassName("Hoge2"), "3");
        }

        [Test]
        public void TestAppliedNamespaceName()
        {
            ClassPattern cp = new ClassPattern();
            cp.NamespaceName = "Seasar.Framework";
            Assert.IsTrue(cp.IsAppliedNamespaceName("Seasar.Framework"), "1");
            Assert.IsTrue(cp.IsAppliedNamespaceName("Seasar.Framework.Container"), "2");
            Assert.IsFalse(cp.IsAppliedNamespaceName("Seasar"), "3");
            Assert.IsFalse(cp.IsAppliedNamespaceName("Seasar.Framework2"), "4");
            Assert.IsFalse(cp.IsAppliedNamespaceName(null), "5");

            cp.NamespaceName = null;
            Assert.IsTrue(cp.IsAppliedNamespaceName(null), "6");
            Assert.IsFalse(cp.IsAppliedNamespaceName("Seasar"), "7");
        }
    }
}
