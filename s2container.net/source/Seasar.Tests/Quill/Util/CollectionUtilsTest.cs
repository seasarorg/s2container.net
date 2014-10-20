using NUnit.Framework;
using Seasar.Quill.Util;
using System.Collections.Generic;
using System.Linq;

namespace Seasar.Tests.Quill.Util
{
    [TestFixture]
	public class CollectionUtilsTest : QuillTestBase
    {
        [Test]
        public void TestAddAllAndForEach()
        {
            // Arrange
            var setA = new HashSet<int>();
            setA.Add(1);
            setA.Add(2);
            setA.Add(3);
            var setB = new HashSet<int>();
            setB.Add(2);
            setB.Add(4);
            setB.Add(6);

            var expectedValues = new int[] { 1, 2, 3, 4, 6 };

            // Act
            setA.AddAll(setB);

            // Assert
            Assert.AreEqual(expectedValues.Length, setA.Count());
            expectedValues.ForEach(expected => Assert.IsTrue(setA.Contains(expected)));
        }
    }
}
