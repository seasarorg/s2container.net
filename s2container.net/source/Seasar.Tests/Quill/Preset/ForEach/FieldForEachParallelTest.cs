using NUnit.Framework;
using Seasar.Quill;
using Seasar.Quill.Parts.Injector.FieldForEach.Impl;
using System.Linq;
using System.Reflection;

namespace Seasar.Tests.Quill.Preset.ForEach
{
    [TestFixture]
    public class FieldForEachParallelTest : QuillTestBase
    {
        [Test]
        public void TestForEach()
        {
            // Arrange
            var actor = new FieldForEachParallel();
            var fields = typeof(FieldForEachEntity).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(fi => fi.Name);
            var entity = new FieldForEachEntity();
            var actual = new System.Collections.Concurrent.ConcurrentBag<string>();
            const int EXPECTED = 10;

            // Act
            actor.ForEach(entity, new QuillInjectionContext(), fields, (e, field, context) => actual.Add(field.Name));

            // Assert
            Assert.AreEqual(EXPECTED, actual.Count());
        }

        private class FieldForEachEntity
        {
            private string hoge01 = "a";
            private string hoge02 = "b";
            private string hoge03 = "c";
            private string hoge04 = "d";
            private string hoge05 = "e";
            private string hoge06 = "f";
            private string hoge07 = "g";
            private string hoge08 = "h";
            private string hoge09 = "i";
            private string hoge10 = "j";

            public FieldForEachEntity()
            {
                // 警告を出さないようにするため
                hoge01.Trim();
                hoge02.Trim();
                hoge03.Trim();
                hoge04.Trim();
                hoge05.Trim();
                hoge06.Trim();
                hoge07.Trim();
                hoge08.Trim();
                hoge09.Trim();
                hoge10.Trim();
            }
        }
    }
}
