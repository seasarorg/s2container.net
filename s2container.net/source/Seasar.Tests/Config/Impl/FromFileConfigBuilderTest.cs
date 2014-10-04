using NUnit.Framework;
using Seasar.Quill.Attr;
using Seasar.Quill.Config.Impl;
using Seasar.Quill.Exception;
using Seasar.Tests.Config.Impl.ForFromFileConfigBuilderTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Tests.Config.Impl
{
    /// <summary>
    /// ファイルから設定情報を構築するクラスのテスト
    /// </summary>
    [TestFixture]
    public class FromFileConfigBuilderTest
    {
        /// <summary>
        /// 指定した設定ファイルが見つからない場合のテスト
        /// </summary>
        [Test]
        [ExpectedException(typeof(ConfigFileLoadException))]
        public void TestFileNotFound()
        {
            var notExistsPath = "Hoge.xml";
            var actual = new FromFileConfigBuilder(notExistsPath, output);
        }

        /// <summary>
        /// quillセクションが見つからない場合のテスト
        /// </summary>
        [Test]
        [ExpectedException(typeof(ConfigFileLoadException))]
        public void TestNotExistsQuillSection()
        {
            var path = GetTestFilePath("NoQuillSection.xml");
            var actual = new FromFileConfigBuilder(path, output);
        }

        /// <summary>
        /// quillセクションしかないファイルを読み込んだ場合（デフォルト値が適用される）
        /// </summary>
        [Test]
        public void TestBuild_ApplyDefault()
        {
            // Arrange
            var path = GetTestFilePath("QuillSectionOnly.xml");
            var actual = new FromFileConfigBuilder(path, output);

            // Act
            var result = actual.Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Container);
            Assert.IsNotNull(result.Extension);
            Assert.IsNotNull(result.Injector);
            Assert.IsNotNull(result.LoggerFactory);
        }

        [Test]
        public void TestLoad()
        {
            // Arrange
            var path = GetTestFilePath("QuillSectionTest.xml");
            var actual = new FromFileConfigBuilder(path, output);
            output(typeof(CustomContainer).FullName);

            // Act
            var result = actual.Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(CustomContainer), result.Container.GetType());
            Assert.AreEqual(typeof(CustomInjector), result.Injector.GetType());
            Assert.AreEqual(typeof(CustomLoggerFactory), result.LoggerFactory.GetType());
            Assert.Greater(result.Extension.Count, 0);
        }

        // =========================================================================
        // helper
        // ======

        private string GetTestFilePath(string fileName)
        {
            return string.Format("Config{0}Impl{0}ForFromFileConfigBuilderTest{0}{1}", Path.DirectorySeparatorChar, fileName);
        }

        private void output(string log)
        {
            Console.WriteLine(log);
        }
    }
}
