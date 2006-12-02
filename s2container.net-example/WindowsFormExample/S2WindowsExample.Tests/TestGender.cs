/*
 * Created by: 
 * Created: 2006年9月23日
 */

#if NET_1_1
// NET 1.1
using System.Collections;
#else
// NET 2.0
using System.Collections.Generic;
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;
#endif
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.WindowsExample.Logics.Dao;

namespace Seasar.WindowsExample.Tests
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

#if NET_1_1
            // NET 1.1
            
            IList list = dao.GetAll();
            Assert.AreEqual(2, list.Count, "Count");
#else
            // NET 2.0
            
            IList<GenderDto> list = dao.GetAll();
            Assert.AreEqual(2, list.Count, "Count");
#endif            
        }
    }
}