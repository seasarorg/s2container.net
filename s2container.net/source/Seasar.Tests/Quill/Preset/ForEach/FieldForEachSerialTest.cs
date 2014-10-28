using NUnit.Framework;
using Seasar.Quill;
using Seasar.Quill.Parts.Injector.FieldForEach.Impl;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seasar.Tests.Quill.Preset.ForEach
{
    [TestFixture]
    public class FieldForEachSerialTest : QuillTestBase
    {
        [Test]
        public void TestForEach()
        {
            // Arrange
            var actor = new FieldForEachSerial();
            var fields = typeof(FieldForEachEntity).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(fi => fi.Name);
            var entity = new FieldForEachEntity();
            var actual = new StringBuilder();
            const string EXPECTED = "hoge1hoge2hoge3";

            // Act
            actor.ForEach(entity, new QuillInjectionContext(), fields, (e, c, i) =>  actual.Append(c.Name) );

            // Assert
            Assert.AreEqual(EXPECTED, actual.ToString());
        }

        private class FieldForEachEntity
        {
            private string hoge1 = "a";
            private string hoge2 = "b";
            private string hoge3 = "c";

            public FieldForEachEntity()
            {
                // 警告を出さないため
                hoge1.Trim();
                hoge2.Trim();
                hoge3.Trim();
            }
        }
    }
}
