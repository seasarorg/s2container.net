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
using System.IO;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Seasar.Quill.Exception;
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
            Assert.IsNotNull(section, "セクション情報取得");
            Assert.AreEqual("TypicalDaoSetting", section.DaoSetting, "標準S2Dao設定取得");
            Assert.AreEqual("TypicalTransactionSetting", section.TransactionSetting, "標準トランザクション設定取得");

            IList dataSources = section.DataSources;
            Assert.IsTrue(dataSources.Count > 0, "データソースの設定が取得されている");
            foreach (object item in dataSources)
            {
                Assert.IsTrue(item is DataSourceSection, "dataSourceセクションとして取得されている");
                DataSourceSection dsSection = (DataSourceSection)item;
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.ConnectionString), "接続文字列が取得されている");
                Console.WriteLine("connectionString={0}", dsSection.ConnectionString);
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.DataSourceClassName), "データソースクラス名が取得されている");
                Console.WriteLine("dataSourceNameClass={0}", dsSection.DataSourceClassName);
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.DataSourceName), "データソス名が設定されている");
                Console.WriteLine("dataSourceName={0}", dsSection.DataSourceName);
                Assert.IsFalse(string.IsNullOrEmpty(dsSection.ProviderName), "プロバイダクラス名が取得されている");
                Console.WriteLine("providerName={0}", dsSection.ProviderName);
            }

            IList assemblys = section.Assemblys;
            Assert.IsTrue(assemblys.Count > 0, "アセンブリ情報が取得されている");
            foreach (object item in assemblys)
            {
                Assert.IsFalse(string.IsNullOrEmpty(item as string), "アセンブリ名が取得されている");
            }
        }

        [Test]
        public void TestGetQuillSection_FromAppConfig()
        {
            
        }

        /// <summary>
        /// Quill設定が見つからない場合のテスト
        /// </summary>
        [Test]
        public void TestGetQuillSection_SectionNotFound()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Assembly.GetExecutingAssembly().CodeBase);
            builder.Replace("file:///", "");
            builder.Replace("Seasar.Tests.DLL", "Seasar.Quill.dll.config");
            string configPath = builder.ToString();

            Assert.IsTrue(File.Exists(configPath), "config exists_" + configPath);

            //  一時的に設定ファイルの場所をかえる
            string dummyPath = configPath + "ex";
            File.Move(configPath, dummyPath);
            try
            {
                //  既定の場所には設定ファイルがないことを確認
                Assert.IsFalse(File.Exists(configPath), "config not found_" + configPath);
                try
                {
                    QuillSection section = QuillSectionHandler.GetQuillSection();
                    Assert.Fail("設定がないので例外となっているはず:" + (section == null ? "null" : section.ToString()));
                }
                catch(QuillConfigNotFoundException)
                {}
            }
            finally
            {
                //  移した設定ファイルを元に戻す
                File.Move(dummyPath, configPath);
            }
        }
    }
}
