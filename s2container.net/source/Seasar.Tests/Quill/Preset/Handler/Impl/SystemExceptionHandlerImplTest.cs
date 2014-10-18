using NUnit.Framework;
using Seasar.Quill.Exception;
using Seasar.Quill.Preset.Handler.Impl;
using System;

namespace Seasar.Tests.Quill.Preset.Handler.Impl
{
    [TestFixture]
    public class SystemExceptionHandlerImplTest
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
