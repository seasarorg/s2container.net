using NUnit.Framework;
using Seasar.Quill;
using Seasar.Quill.Attr;
using Seasar.Quill.Exception;
using Seasar.Quill.Parts.Handler;
using Seasar.Quill.Parts.Injector.FieldForEach;
using Seasar.Quill.Parts.Injector.FieldInjector;
using Seasar.Quill.Parts.Injector.FieldSelector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seasar.Tests.Quill
{
    [TestFixture]
    public class QuillInjectorTest : QuillTestBase
    {
        [Test]
        public void TestInject()
        {
            // Arrange
            var actual = new Hoge();
            var injector = new QuillInjector();
            Assert.IsNull(actual.Property);
            
            // Act
            injector.Inject(actual);
        
            // Assert
            Assert.IsNotNull(actual.Property);
            Assert.AreEqual(Prop.EXPECTED, actual.GetResult());
        }

        [Test]
        public void TestInjectedComponent()
        {
            // Arrange
            var injector = new QuillInjector();

            // Act
            var actual = injector.GetInjectedComponent<Hoge>();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Property);
            Assert.AreEqual(Prop.EXPECTED, actual.GetResult());
        }

        [Test]
        public void TestInject_Recursive()
        {
            // Arrange
            var injector = new QuillInjector();

            // Act
            var actual = injector.GetInjectedComponent<HogeEx>();

            // Assert
            Assert.AreEqual(Prop.EXPECTED + Prop.EXPECTED, actual.GetResult());
            Assert.IsNull(actual.Hoge); // Implementation属性がないフィールドはインジェクションされない
        }

        #region SelectField
        [Test]
        public void TestInject_CustomSelectField()
        {
            // Arrange
            var injector = new QuillInjector();
            var hoge = new Hoge();
            var actual = new FieldSelectForTest();
            Assert.IsFalse(actual.IsCalled);

            // Act
            injector.Inject(hoge, callbackSelectField: actual.Select);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_ExchangeSelectField()
        {
            // Arrange
            var injector = new QuillInjector();
            var hoge = new Hoge();
            var actual = new FieldSelectForTest();
            Assert.IsFalse(actual.IsCalled);

            // Act
            injector.FieldSelector = actual;
            injector.Inject(hoge);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_CustomAndExchangeSelectField()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual1 = new FieldSelectForTest();
            Assert.IsFalse(actual1.IsCalled);
            var actual2 = new FieldSelectForTest();
            Assert.IsFalse(actual2.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.FieldSelector = actual1;
            injector.Inject(hoge, callbackSelectField: actual2.Select);

            // Assert
            Assert.IsFalse(actual1.IsCalled);
            Assert.IsTrue(actual2.IsCalled);
        }

        #endregion

        #region ForEach

        [Test]
        public void TestInject_CustomForEach()
        {
            // Arrange
            var injector = new QuillInjector();
            var builder = new StringBuilder();
            var context = new QuillInjectionContext();
            const string EXPECTED = "10";　　　　　　　　// Hoge, Propのフィールド数

            // Act
            var actual = injector.GetInjectedComponent<Hoge>(context:context, callbackFieldForEach: (o, c, f, invoker) => builder.Append(f.Count()));
            
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.Property);   // callbackInjectFieldを呼んでいないのでインジェクションは行われていない
            Assert.AreEqual(EXPECTED, builder.ToString());   
        }

        [Test]
        public void TestInject_ExchangeForEach()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual = new ForEachForTest();
            Assert.IsFalse(actual.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.FieldForEach = actual;
            injector.Inject(hoge);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        /// <summary>
        /// プロパティとコールバックの両方を設定した場合はコールバックが優先されるテスト
        /// </summary>
        /// <remarks></remarks>
        [Test]
        public void TestInject_CustomAndExchangeForEach()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual1 = new ForEachForTest();
            Assert.IsFalse(actual1.IsCalled);
            var actual2 = new ForEachForTest();
            Assert.IsFalse(actual2.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.FieldForEach = actual1;
            injector.Inject(hoge, callbackFieldForEach: actual2.ForEach);

            // Assert
            Assert.IsFalse(actual1.IsCalled);
            Assert.IsTrue(actual2.IsCalled);
        }

        #endregion

        #region InjectField

        [Test]
        public void TestInject_CustomInjectField()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual = new FieldInjectImplForTest();
            Assert.IsFalse(actual.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.Inject(hoge, callbackInjectField: actual.InjectField);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_ExchangeInjectField()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual = new FieldInjectImplForTest();
            Assert.IsFalse(actual.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.FieldInjector = actual;
            injector.Inject(hoge);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_CustomAndExchangeInjectField()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual1 = new FieldInjectImplForTest();
            Assert.IsFalse(actual1.IsCalled);
            var actual2 = new FieldInjectImplForTest();
            Assert.IsFalse(actual2.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.FieldInjector = actual1;
            injector.Inject(hoge, callbackInjectField: actual2.InjectField);

            // Assert
            Assert.IsFalse(actual1.IsCalled);
            Assert.IsTrue(actual2.IsCalled);
        }

        #endregion

        #region QuillApplicationExceptionHandler

        [Test]
        public void TestInject_CustomQAExHandler()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual = new QAppHandlerForTest();
            Assert.IsFalse(actual.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.Inject(hoge,callbackFieldForEach:new ExceptionRaiser<QuillApplicationException>().ForEach, 
                handleQuillApplicationException: actual.Handle);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_ExchangeQAExHandler()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual = new QAppHandlerForTest();
            Assert.IsFalse(actual.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.QuillApplicationExceptionHandler = actual;
            injector.Inject(hoge, callbackFieldForEach: new ExceptionRaiser<QuillApplicationException>().ForEach);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_CustomAndExchangeQAExHandler()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual1 = new QAppHandlerForTest();
            Assert.IsFalse(actual1.IsCalled);
            var actual2 = new QAppHandlerForTest();
            Assert.IsFalse(actual2.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.QuillApplicationExceptionHandler = actual1;
            injector.Inject(hoge, callbackFieldForEach: new ExceptionRaiser<QuillApplicationException>().ForEach,
                handleQuillApplicationException:actual2.Handle);

            // Assert
            Assert.IsFalse(actual1.IsCalled);
            Assert.IsTrue(actual2.IsCalled);
        }

        #endregion

        #region SystemExceptionHandler

        [Test]
        public void TestInject_CustomSysExHandler()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual = new SExHandlerForTest();
            Assert.IsFalse(actual.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.Inject(hoge, callbackFieldForEach: new ExceptionRaiser<System.Exception>().ForEach,
                handleSystemException: actual.Handle);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_ExchangeSysExHandler()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual = new SExHandlerForTest();
            Assert.IsFalse(actual.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.SystemExceptionHandler = actual;
            injector.Inject(hoge, callbackFieldForEach: new ExceptionRaiser<System.Exception>().ForEach);

            // Assert
            Assert.IsTrue(actual.IsCalled);
        }

        [Test]
        public void TestInject_CustomAndExchangeSysExHandler()
        {
            // Arrange
            var injector = new QuillInjector();
            var actual1 = new SExHandlerForTest();
            Assert.IsFalse(actual1.IsCalled);
            var actual2 = new SExHandlerForTest();
            Assert.IsFalse(actual2.IsCalled);
            var hoge = new Hoge();

            // Act
            injector.SystemExceptionHandler = actual1;
            injector.Inject(hoge, callbackFieldForEach: new ExceptionRaiser<System.Exception>().ForEach,
                handleSystemException: actual2.Handle);

            // Assert
            Assert.IsFalse(actual1.IsCalled);
            Assert.IsTrue(actual2.IsCalled);
        }

        #endregion

        /// <summary>
        /// コメントテスト <see cref="HogeB"/>
        /// </summary>
        private class HogeA
        {

        }

        /// <summary>
        /// コメントテスト
        /// </summary>
        private class HogeB
        {

        }

        private IEnumerable<FieldInfo> SelectField(object target, QuillInjectionContext context)
        {
            return target.GetType().GetFields(context.Condition).Where(fi => fi.Name == "_selectedProp");
        }

        private class Hoge
        {
            private Prop _prop = null;

            public Prop Property { get { return _prop; } }

            public string GetResult()
            {
                return _prop.GetValue();
            }
        }

        private class HogeEx
        {
            private Prop _prop = null;

            private PropParent _parent = null;

            private Prop _selectedProp = null;

            public Prop Selected { get { return _selectedProp; } }

            private Hoge _hoge = null;

            public Hoge Hoge { get { return _hoge; } }

            public string GetResult()
            {
                return _prop.GetValue() + _parent.GetValue();
            }
        }

        [Implementation]
        private class Prop
        {
            public const string EXPECTED = "aiueo";

            public string GetValue()
            {
                return EXPECTED;
            }
        }

        [Implementation]
        private class PropParent
        {
            private Prop _child = null;

            public string GetValue()
            {
                return _child.GetValue();
            }
        }

        private class ExceptionRaiser<EX> : IFieldForEach where EX : System.Exception, new()
        {
            public void ForEach(object target, QuillInjectionContext context, IEnumerable<FieldInfo> fields, QuillInjector.CallbackInjectField injectField)
            {
                throw new EX();
            }
        }


        private class SExHandlerForTest : ISystemExceptionHandler
        {
            public bool IsCalled { get; set; }
            public object Handle(Exception ex)
            {
                IsCalled = true;
                return IsCalled;
            }
        }


        private class QAppHandlerForTest : IQuillApplicationExceptionHandler
        {
            public bool IsCalled { get; set; }
            public object Handle(Seasar.Quill.Exception.QuillApplicationException ex)
            {
                IsCalled = true;
                return IsCalled;
            }
        }


        private class FieldSelectForTest : IFieldSelector
        {
            public bool IsCalled { get; set; }
            public IEnumerable<FieldInfo> Select(object target, QuillInjectionContext context)
            {
                IsCalled = true;
                return new List<FieldInfo>();
            }
        }


        private class ForEachForTest : IFieldForEach
        {
            public bool IsCalled { get; set; }
            public void ForEach(object target, QuillInjectionContext context, IEnumerable<FieldInfo> fields, QuillInjector.CallbackInjectField injectField)
            {
                IsCalled = true;
            }
        }

        private class FieldInjectImplForTest : IFieldInjector
        {
            public bool IsCalled { get; set; }
            public void InjectField(object target, FieldInfo fieldInfo, QuillInjectionContext context)
            {
                IsCalled = true;
            }
        }


    }
}
