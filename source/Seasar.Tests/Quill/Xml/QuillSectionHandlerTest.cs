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

using MbUnit.Framework;
using Seasar.Quill.Xml;
using System.Diagnostics;
using System.Threading;

namespace Seasar.Tests.Quill.Xml
{
    [TestFixture]
    public class QuillSectionHandlerTest
    {
        /// <summary>
        /// アプリケーション構成ファイルから設定を読み込めているか確認
        /// </summary>
        [Test]
        public void TestGetQuillSection()
        {
            QuillSection actual = QuillSectionHandler.GetQuillSection();

            Assert.IsNotNull(actual);

            Assert.AreEqual("TypicalDaoSetting", actual.DaoSetting, "daoSetting");
            Assert.AreEqual("TypicalTransactionSetting", actual.TransactionSetting, "TransactionSetting");

            //  アセンブリ設定
            string[] expectedAssemblys = new string[] { "Seasar.Tests", "Seasar.Dxo" };
            for (int i = 0; i < expectedAssemblys.Length; i++)
            {
                Assert.AreEqual(expectedAssemblys[i], actual.Assemblys[i],
                    string.Format("Assembly_[{0}]", actual.Assemblys[i]));
            }

            //  データソース設定
            Assert.GreaterThan(actual.DataSources.Count, 0, "DataSourceSections_Count");
            object item = actual.DataSources[0];
            Assert.IsNotNull(item, "DataSourceSection is nothing");
            Assert.IsTrue(item is DataSourceSection, "Type is [DataSourceSection]");

            DataSourceSection dsSection = (DataSourceSection) item;
            Assert.AreEqual("Hoge1", dsSection.DataSourceName);
            Assert.AreEqual("SqlServer", dsSection.ProviderName);
            Assert.IsTrue(dsSection.ConnectionString.StartsWith("\"Server="));
            Assert.AreEqual("Seasar.Extension.Tx.Impl.TxDataSource", dsSection.DataSourceClassName);
        }

        /// <summary>
        /// conime.exeが起動していないか確認
        /// </summary>
        [Test]
        public void TestIsNotCallConime()
        {
            const string KILL_TARGET_PROECESS = "conime";

            Process[] beforeProecesses = Process.GetProcessesByName(KILL_TARGET_PROECESS);
            //  conime.exeが起動していた場合は予め終了させておく
            while(beforeProecesses.Length > 0)
            {
                beforeProecesses[0].Kill();
                Thread.Sleep(1000);
                beforeProecesses = Process.GetProcessesByName(KILL_TARGET_PROECESS);
            }

            QuillSection actual = QuillSectionHandler.GetQuillSection();
            Assert.IsNotNull(actual);

            Process[] afterProecesses = Process.GetProcessesByName(KILL_TARGET_PROECESS);
            Assert.AreEqual(0, afterProecesses.Length, "conime.exeが起動していないか");
        }
    }
}
