using NUnit.Framework;
using Seasar.Quill;
using Seasar.Quill.Preset.Injection;
using Seasar.Quill.Preset.Scope;

namespace Seasar.Tests.Quill.Preset.Injection
{
    [TestFixture]
    public class FieldInjectorImplTest : QuillTestBase
    {
        [Test]
        public void TestInjectField()
        {
            // Arrange
            var entity = new TestEntity();
            entity.Hoge = null;
            var fieldInfo = typeof(TestEntity).GetField("Hoge");
            var actor = new FieldInjectorImpl();
            var context = new QuillInjectionContext();

            // Act
            Assert.IsNull(entity.Hoge);
            actor.InjectField(entity, fieldInfo, context);

            // Assert
            Assert.IsNotNull(entity.Hoge);
            Assert.AreEqual(typeof(FieldEntity), entity.Hoge.GetType());
        }

        private class TestEntity
        {
            public FieldEntity Hoge = null;
        }

        private class FieldEntity
        { }
    }
}
