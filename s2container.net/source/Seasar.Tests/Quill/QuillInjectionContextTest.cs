using NUnit.Framework;
using Seasar.Quill;

namespace Seasar.Tests.Quill
{
    [TestFixture]
    public class QuillInjectionContextTest : QuillTestBase
    {
        [Test]
        public void TestBeginToEndInjection()
        {
            // Arrange
            var actor = new QuillInjectionContext();

            // Act / Assert
            AssertBeginToEndAndResetInjection(actor);
        }

        [Test]
        public void TestBeginToEndInjection_ThreadSafe()
        {
            // Arrange
            var actor = new QuillInjectionContext(isThreadsafe: true);

            // Act / Assert
            AssertBeginToEndAndResetInjection(actor);
        }

        private void AssertBeginToEndAndResetInjection(QuillInjectionContext context)
        {
            var testType1 = typeof(string);
            var testType2 = typeof(int);

            Assert.AreEqual(0, context.InjectionDepth);
            Assert.IsFalse(context.IsInInjection());
            Assert.IsFalse(context.IsAlreadyInjected(testType1));
            Assert.IsFalse(context.IsAlreadyInjected(testType2));

            context.BeginInjection(testType1);
            Assert.AreEqual(1, context.InjectionDepth);
            Assert.IsTrue(context.IsInInjection());
            Assert.IsTrue(context.IsAlreadyInjected(testType1));
            Assert.IsFalse(context.IsAlreadyInjected(testType2));

            context.BeginInjection(testType2);
            Assert.AreEqual(2, context.InjectionDepth);
            Assert.IsTrue(context.IsInInjection());
            Assert.IsTrue(context.IsAlreadyInjected(testType1));
            Assert.IsTrue(context.IsAlreadyInjected(testType2));

            context.EndInjection();
            Assert.AreEqual(1, context.InjectionDepth);
            Assert.IsTrue(context.IsInInjection());
            Assert.IsTrue(context.IsAlreadyInjected(testType1));
            Assert.IsTrue(context.IsAlreadyInjected(testType2));

            context.BeginInjection(testType2);
            Assert.AreEqual(2, context.InjectionDepth);
            Assert.IsTrue(context.IsInInjection());
            Assert.IsTrue(context.IsAlreadyInjected(testType1));
            Assert.IsTrue(context.IsAlreadyInjected(testType2));

            context.ResetInjection();
            Assert.AreEqual(0, context.InjectionDepth);
            Assert.IsFalse(context.IsInInjection());
            Assert.IsFalse(context.IsAlreadyInjected(testType1));
            Assert.IsFalse(context.IsAlreadyInjected(testType2));
        }
    }
}
