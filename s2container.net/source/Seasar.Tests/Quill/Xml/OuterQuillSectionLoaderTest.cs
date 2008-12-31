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

using System.Reflection;
using MbUnit.Framework;
using Seasar.Quill.Xml;

namespace Seasar.Tests.Quill.Xml
{
    [TestFixture]
    public class OuterQuillSectionLoaderTest
    {
        /// <summary>
        /// 指定したパスに設定ファイルがない場合のテスト
        /// </summary>
        [Test]
        public void TestLoadFromOuterConfig_FileNotFound()
        {
            {
                QuillSection section = OuterQuillSectionLoader.LoadFromOuterConfig(null);
                Assert.IsNull(section, "1");
            }
            {
                QuillSection section = OuterQuillSectionLoader.LoadFromOuterConfig("NotFound");
                Assert.IsNull(section, "2");
            }
        }

        /// <summary>
        /// 外部設定ファイルの読み込み（正常）
        /// </summary>
        [Test]
        public void TestLoadFromOuterConfig_Normal()
        {
            string location = Assembly.GetExecutingAssembly().CodeBase;
            string configPath = location.Replace("Seasar.Tests.DLL", "Quill/Xml/Test.Quill.dll.config").Replace("file:///", "");
            QuillSection actual = OuterQuillSectionLoader.LoadFromOuterConfig(configPath);

            Assert.IsNotNull(actual, "1_" + configPath);
            Assert.AreEqual("HogeDaoSetting", actual.DaoSetting, "daoSetting");
            Assert.AreEqual("HogeTxSetting", actual.TransactionSetting, "TransactionSetting");
            
            //  アセンブリ設定
            string[] expectedAssemblys = new string[] { "Hoge", "Huga", "Hogo" };
            for(int i = 0; i < expectedAssemblys.Length; i++)
            {
                Assert.AreEqual(expectedAssemblys[i], actual.Assemblys[i], 
                    string.Format("Assembly_[{0}]", actual.Assemblys[i]));
            }

            //  データソース設定
            Assert.AreEqual(3, actual.DataSources.Count, "DataSourceSections_Count");
            for(int i = 0; i < 0; i++)
            {
                object item = (DataSourceSection)actual.DataSources[i];
                Assert.IsNotNull(item, "DataSourceSection is nothing");
                Assert.IsTrue(item is DataSourceSection, "Type is [DataSourceSection]");

                DataSourceSection dsSection = (DataSourceSection) item;

                Assert.AreEqual("cs" + i.ToString(), dsSection.ConnectionString, "cs");
                Assert.AreEqual("ds" + i.ToString(), dsSection.DataSourceClassName, "ds");
                Assert.AreEqual("ds_name" + i.ToString(), dsSection.DataSourceName, "ds_name");
                Assert.AreEqual("pv" + i.ToString(), dsSection.ProviderName, "pv");
            }
        }
    }
}
