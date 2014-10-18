using NUnit.Framework;
using Seasar.Quill.Exception;
using Seasar.Quill.Preset.Handler.Impl;

namespace Seasar.Tests.Quill.Preset.Handler.Impl
{
    [TestFixture]
    public class QuillApplicationExceptionHandlerImplTest
    {
        [Test]
        [ExpectedException(ExpectedException=typeof(QuillApplicationException))]
        public void TestHandle()
        {
            // Arrange
            var actor = new QuillApplicationExceptionHandlerImpl();

            // Act
            actor.Handle(new QuillApplicationException());

            // Assert
            Assert.Fail();   // このコードは実行されない
        }
    }
}
