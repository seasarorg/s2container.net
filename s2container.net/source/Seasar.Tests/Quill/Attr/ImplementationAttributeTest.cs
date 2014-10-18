using NUnit.Framework;
using Seasar.Quill.Attr;

namespace Seasar.Tests.Quill.Attr
{
    /// <summary>
    /// Implementation属性テストクラス
    /// </summary>
    [TestFixture]
    public class ImplementationAttributeTest
    {
        /// <summary>
        /// 実装型取得テスト
        /// </summary>
        [Test]
        public void TestImplType()
        {
            var actual = new TestImplementedClass();
            var attrs = actual.GetType().GetCustomAttributes(typeof(ImplementationAttribute), true);
            Assert.IsNotNull(attrs);
            Assert.Greater(attrs.Length, 0);
            ImplementationAttribute attr = (ImplementationAttribute)attrs[0];

            Assert.AreEqual(typeof(string), attr.ImplType);
        }

        /// <summary>
        /// 実装型取得テスト（実装型の設定はなし）
        /// </summary>
        [Test]
        public void TestImplType_Null()
        {
            var actual = new TestImplementationNull();
            var attrs = actual.GetType().GetCustomAttributes(typeof(ImplementationAttribute), true);
            Assert.IsNotNull(attrs);
            Assert.Greater(attrs.Length, 0);
            ImplementationAttribute attr = (ImplementationAttribute)attrs[0];

            Assert.IsNull(attr.ImplType);
        }

        /// <summary>
        /// Implementation属性付属判定テスト
        /// </summary>
        [Test]
        public void TestIsImplementationAttrAttached()
        {
            Assert.IsTrue(typeof(TestImplementedClass).IsImplementationAttrAttached());
            Assert.IsTrue(typeof(TestImplementationNull).IsImplementationAttrAttached());
            Assert.IsFalse(typeof(NoImplAttr).IsImplementationAttrAttached());
        }

        /// <summary>
        /// 実装型取得テスト
        /// </summary>
        [Test]
        public void TestGetImplType()
        {
            var target = new TestImplementedClass();
            var actual = target.GetType().GetImplType();

            Assert.AreEqual(typeof(string), actual);
        }

        /// <summary>
        /// 実装型取得テスト（実装型の設定はなし）
        /// </summary>
        [Test]
        public void TestGetImplType_Null()
        {
            var target = new TestImplementationNull();
            var actual = target.GetType().GetImplType();

            Assert.IsNull(actual);
        }
    }

    #region Test Support
    /// <summary>
    /// Implementation属性ありクラス
    /// </summary>
    [Implementation(ImplType=typeof(string))]
    public class TestImplementedClass
    {
    }

    /// <summary>
    /// Implementation属性あり（実装型設定なし）クラス
    /// </summary>
    [Implementation]
    public class TestImplementationNull
    {
    }

    /// <summary>
    /// Implementation属性なしクラス
    /// </summary>
    public class NoImplAttr
    {
    }
    #endregion
}
