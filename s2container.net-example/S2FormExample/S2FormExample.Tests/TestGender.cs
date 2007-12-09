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

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.S2FormExample.Logics.Dao;
using Seasar.S2FormExample.Logics.Dto;

namespace Seasar.S2FormExample.Tests
{
    /// <summary>
    /// 性別用テストケースクラス
    /// </summary>
    [TestFixture]
    public class TestGender : S2TestCase
    {
        /// <summary>
        /// Logic設定ファイル
        /// </summary>
        private const string PATH = "ExampleLogics.dicon";

        /// <summary>
        /// テストのセットアップ
        /// </summary>
        [SetUp]
        public void Setup()
        {
            FileInfo info = new FileInfo(
                string.Format("{0}.dll.config", SystemInfo.AssemblyShortName(
                                                    Assembly.GetExecutingAssembly())));
            // アセンブリがdllの場合は".dll.config"

            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        /// <summary>
        /// DAOのテスト
        /// </summary>
        [Test, S2]
        public void TestDao()
        {
            Include(PATH);

            IGenderDao dao = (IGenderDao) GetComponent(typeof (IGenderDao));

            IList<GenderDto> list = dao.GetAll();
            Assert.AreEqual(2, list.Count, "Count");
        }
    }
}