using NUnit.Framework;
using Seasar.Quill.Parts.Handler.Impl;
using System;

namespace Seasar.Tests.Quill.Preset.Handler
{
    [TestFixture]
    public class SystemExceptionHandlerImplTest : QuillTestBase
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException))]
        public void TestHandle()
        {
            // Arrange
            var actor = new SystemExceptionHandlerImpl();

            // Act
            actor.Handle(new ApplicationException());

            // Assert
            Assert.Fail();   // このコードは実行されない
        }
    }
}
