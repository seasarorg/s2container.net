using NUnit.Framework;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Tests.Quill.Util
{
    [TestFixture]
    public class LogicUtilsTest : QuillTestBase
    {
        [Test]
        [ExpectedException(typeof(QuillApplicationException))]
        public void TestGetLogic_NotFound()
        {
            LogicUtils.GetLogic<object, object>(null, null, null);
        }

        [Test]
        public void TestGetLogic_Callback()
        {
            // Arrange
            const int TEST_INDEX = 3;
            const string EXPECTED = "aiueo";
            const string NOT_EXPECTED = "kakikukeko";

            // Act
            var actual = LogicUtils.GetLogic(EXPECTED, NOT_EXPECTED, s => s.Substring(TEST_INDEX));

            // Assert
            Assert.AreEqual(EXPECTED, actual);
        }

        [Test]
        public void TestGetLogic_DefailtInterface()
        {
            // Arrange
            const int TEST_INDEX = 3;
            const string EXPECTED = "kakikukeko";

            // Act
            var actual = LogicUtils.GetLogic(null, EXPECTED, s => s.Substring(TEST_INDEX));

            // Assert
            Assert.AreEqual(EXPECTED.Substring(TEST_INDEX), actual);
        }
    }
}
