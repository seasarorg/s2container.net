#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using MbUnit.Framework;
using Seasar.Quill.Xml;

namespace Seasar.Tests.Quill.Xml
{
    [TestFixture]
    public class QuillSectionHandlerTest
    {
        [Test]
        public void TestGetQuillSection()
        {
            //  ## Arrange / Act ##
            QuillSection section = QuillSectionHandler.GetQuillSection();

            //  ## Assert ##
            Assert.IsNotNull(section, "1");
            Assert.AreEqual("TypicalDaoSetting", section.DaoSetting, "2");
            Assert.AreEqual("TypicalTransactionSetting", section.TransactionSetting, "3");

            IList dataSources = section.DataSources;
            Assert.IsTrue(dataSources.Count > 0);
            foreach (object item in dataSources)
            {
                Assert.IsTrue(item is DataSourceSection);
                DataSourceSection dsSection = (DataSourceSection)item;
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.ConnectionString));
                Console.WriteLine("connectionString={0}", dsSection.ConnectionString);
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.DataSourceClassName));
                Console.WriteLine("dataSourceNameClass={0}", dsSection.DataSourceClassName);
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.DataSourceName));
                Console.WriteLine("dataSourceName={0}", dsSection.DataSourceName);
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.ProviderName));
                Console.WriteLine("providerName={0}", dsSection.ProviderName);
            }
        }
    }
}
