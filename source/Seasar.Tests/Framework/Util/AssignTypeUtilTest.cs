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

using System;
using System.Collections;
using System.Collections.Generic;
using MbUnit.Framework;
using Seasar.Framework.Util;
using Seasar.Tests.Dao.Impl;

namespace Seasar.Tests.Framework.Util
{
    [TestFixture]
    public class AssignTypeUtilTest
    {
        [Test]
        public void TestIsSimpleType()
        {
            //  true ----------------------------------------------------
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(string));
                Assert.IsTrue(ret, "string");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(int));
                Assert.IsTrue(ret, "int");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(long));
                Assert.IsTrue(ret, "long");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(char));
                Assert.IsTrue(ret, "char");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(byte[]));
                Assert.IsTrue(ret, "byte[]");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(double));
                Assert.IsTrue(ret, "double");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(float));
                Assert.IsTrue(ret, "float");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(DateTime));
                Assert.IsTrue(ret, "DateTime");
            }
            
            //  false ----------------------------------------------------
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(Employee));
                Assert.IsFalse(ret, "Employee");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(int[]));
                Assert.IsFalse(ret, "int[]");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(Employee[]));
                Assert.IsFalse(ret, "Employee[]");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(ArrayList));
                Assert.IsFalse(ret, "ArrayList");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(IList<string>));
                Assert.IsFalse(ret, "ArrayList");
            }
            {
                bool ret = AssignTypeUtil.IsSimpleType(typeof(IList<Employee>));
                Assert.IsFalse(ret, "IList<Employee>");
            }
        }

        [Test]
        public void TestIsList()
        {
            //  true --------------------------------------------------
            {
                bool ret = AssignTypeUtil.IsList(typeof(IList));
                Assert.IsTrue(ret, "IList");
            }
            {
                bool ret = AssignTypeUtil.IsList(typeof(ArrayList));
                Assert.IsTrue(ret, "ArrayList");
            }

            //  false -------------------------------------------------
            {
                bool ret = AssignTypeUtil.IsList(typeof(string));
                Assert.IsFalse(ret, "string");
            }
            {
                bool ret = AssignTypeUtil.IsList(typeof(Employee));
                Assert.IsFalse(ret, "Employee");
            }
            {
                bool ret = AssignTypeUtil.IsList(typeof(DateTime));
                Assert.IsFalse(ret, "DateTime");
            }
            {
                bool ret = AssignTypeUtil.IsList(typeof(IList<string>));
                Assert.IsFalse(ret, "IList<string>");
            }
            {
                bool ret = AssignTypeUtil.IsList(typeof(string[]));
                Assert.IsFalse(ret, "string[]");
            }
        }

        [Test]
        public void TestIsGeneric()
        {
            //  true ------------------------------------------------------
            {
                bool ret = AssignTypeUtil.IsGenericList(typeof(IList<string>));
                Assert.IsTrue(ret, "IList<string>");
            }
            {
                bool ret = AssignTypeUtil.IsGenericList(typeof(List<string>));
                Assert.IsTrue(ret, "List<string>");
            }

            //  false -----------------------------------------------------
            {
                bool ret = AssignTypeUtil.IsGenericList(typeof(string));
                Assert.IsFalse(ret, "string");
            }
            {
                bool ret = AssignTypeUtil.IsGenericList(typeof(Employee));
                Assert.IsFalse(ret, "Employee");
            }
            {
                bool ret = AssignTypeUtil.IsGenericList(typeof(DateTime));
                Assert.IsFalse(ret, "DateTime");
            }
            {
                bool ret = AssignTypeUtil.IsGenericList(typeof(string[]));
                Assert.IsFalse(ret, "string[]");
            }
            {
                bool ret = AssignTypeUtil.IsGenericList(typeof(IList));
                Assert.IsFalse(ret, "IList");
            }
        }
    }
}
