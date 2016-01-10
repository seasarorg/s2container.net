using System;
using System.Data;
using System.Reflection;
using NUnit.Framework;
using Quill.Tests;

namespace Quill.Scope.Impl.Tests {
    [TestFixture()]
    public class AbstractDbConnectDecoratorTests : QuillTestBase {
        [Test()]
        public void Decorate_NoConnectionDecoratorAction_Test() {
            // Arrange
            TestDbConnectionDecorator target = new TestDbConnectionDecorator();

            // Act/Assert
            TestUtils.ExecuteExcectedException<ArgumentException>(() => {
                target.Decorate((con) => {
                    Assert.IsNull(con);
                    throw new ArgumentException();
                });
            });

            // 後処理
            FieldInfo ornerInvokerField = typeof(AbstractDbConnectDecorator).GetField("_ornerInvoker", BindingFlags.NonPublic | BindingFlags.Static);
            ornerInvokerField.SetValue(null, null);
        }

        [Test()]
        public void Decorate_NoConnectionDecoratorFunc_Test() {
            // Arrange
            TestDbConnectionDecorator target = new TestDbConnectionDecorator();

            // Act
            var actual = target.Decorate((con) => {
                Assert.IsNull(con);
                return true;
            });

            // Assert
            Assert.IsTrue(actual);

            // 後処理
            FieldInfo ornerInvokerField = typeof(AbstractDbConnectDecorator).GetField("_ornerInvoker", BindingFlags.NonPublic | BindingFlags.Static);
            ornerInvokerField.SetValue(null, null);
        }

        [Test()]
        public void Decorate_Action_Test() {
            // Arrange
            TestDbConnectionDecorator target = new TestDbConnectionDecorator();
            FieldInfo field = typeof(AbstractDbConnectDecorator).GetField(
                "_connectionDecorator", BindingFlags.NonPublic | BindingFlags.Instance);
            ConnectionDecoratorForTest decorator = new ConnectionDecoratorForTest();
            field.SetValue(target, decorator);

            // Act
            target.Decorate(con => { }); // dummy

            // Assert
            Assert.IsTrue(target.IsCalledStartScope);
            Assert.IsTrue(target.IsCalledEndScope);
            Assert.IsFalse(target.IsCalledHandleException);
            Assert.IsTrue(target.IsCalledHandleFinally);

            // 後処理
            FieldInfo ornerInvokerField = typeof(AbstractDbConnectDecorator).GetField("_ornerInvoker", BindingFlags.NonPublic | BindingFlags.Static);
            ornerInvokerField.SetValue(null, null);
        }

        [Test()]
        public void Decorate_ActionException_Test() {
            // Arrange
            TestDbConnectionDecorator target = new TestDbConnectionDecorator();
            FieldInfo field = typeof(AbstractDbConnectDecorator).GetField(
                "_connectionDecorator", BindingFlags.NonPublic | BindingFlags.Instance);
            ConnectionDecoratorForTest decorator = new ConnectionDecoratorForTest();
            field.SetValue(target, decorator);

            // Act
            target.Decorate(con => { throw new ArgumentException(); }); // dummy

            // Assert
            Assert.IsTrue(target.IsCalledStartScope);
            Assert.IsFalse(target.IsCalledEndScope);
            Assert.IsTrue(target.IsCalledHandleException);
            Assert.IsTrue(target.IsCalledHandleFinally);

            // 後処理
            FieldInfo ornerInvokerField = typeof(AbstractDbConnectDecorator).GetField("_ornerInvoker", BindingFlags.NonPublic | BindingFlags.Static);
            ornerInvokerField.SetValue(null, null);
        }

        [Test()]
        public void Decorate_Func_Test() {
            // Arrange
            TestDbConnectionDecorator target = new TestDbConnectionDecorator();
            FieldInfo field = typeof(AbstractDbConnectDecorator).GetField(
                "_connectionDecorator", BindingFlags.NonPublic | BindingFlags.Instance);
            ConnectionDecoratorForTest decorator = new ConnectionDecoratorForTest();
            field.SetValue(target, decorator);

            // Act
            target.Decorate(con => true); // dummy

            // Assert
            Assert.IsTrue(target.IsCalledStartScope);
            Assert.IsTrue(target.IsCalledEndScope);
            Assert.IsFalse(target.IsCalledHandleException);
            Assert.IsTrue(target.IsCalledHandleFinally);

            // 後処理
            FieldInfo ornerInvokerField = typeof(AbstractDbConnectDecorator).GetField("_ornerInvoker", BindingFlags.NonPublic | BindingFlags.Static);
            ornerInvokerField.SetValue(null, null);
        }

        [Test()]
        public void Decorate_FuncException_Test() {
            // Arrange
            TestDbConnectionDecorator target = new TestDbConnectionDecorator();
            FieldInfo field = typeof(AbstractDbConnectDecorator).GetField(
                "_connectionDecorator", BindingFlags.NonPublic | BindingFlags.Instance);
            ConnectionDecoratorForTest decorator = new ConnectionDecoratorForTest();
            field.SetValue(target, decorator);

            // Act
            int result = target.Decorate<int>(con => { throw new ArgumentException(); }); // dummy

            // Assert
            Assert.IsTrue(target.IsCalledStartScope);
            Assert.IsFalse(target.IsCalledEndScope);
            Assert.IsTrue(target.IsCalledHandleException);
            Assert.IsTrue(target.IsCalledHandleFinally);

            // 後処理
            FieldInfo ornerInvokerField = typeof(AbstractDbConnectDecorator).GetField("_ornerInvoker", BindingFlags.NonPublic | BindingFlags.Static);
            ornerInvokerField.SetValue(null, null);
        }

        /// <summary>
        /// スコープの開始終了処理は最も外側のDecorate処理に対してのみ行われることを確認
        /// </summary>
        [Test()]
        public void Decorate_OrnerInvoker_Test() {
            // Arrange
            FieldInfo field = typeof(AbstractDbConnectDecorator).GetField(
                "_connectionDecorator", BindingFlags.NonPublic | BindingFlags.Instance);

            TestDbConnectionDecorator target = new TestDbConnectionDecorator();
            ConnectionDecoratorForTest decorator = new ConnectionDecoratorForTest();
            field.SetValue(target, decorator);

            TestDbConnectionDecorator targetSub = new TestDbConnectionDecorator();
            ConnectionDecoratorForTest decoratorSub = new ConnectionDecoratorForTest();
            field.SetValue(targetSub, decoratorSub);

            // Act
            target.Decorate(con => targetSub.Decorate(conSub => { }));

            // Assert
            Assert.IsTrue(target.IsCalledStartScope);
            Assert.IsTrue(target.IsCalledEndScope);
            Assert.IsFalse(targetSub.IsCalledStartScope);
            Assert.IsFalse(targetSub.IsCalledEndScope);
        }

        #region helper

        /// <summary>
        /// テスト用DB接続修飾クラス
        /// </summary>
        private class TestDbConnectionDecorator : AbstractDbConnectDecorator {
            public bool IsCalledStartScope { get; private set; }
            protected override void StartScope(IDbConnection con) {
                IsCalledStartScope = true;
            }

            public bool IsCalledEndScope { get; private set; }
            protected override void EndScope(IDbConnection con) {
                IsCalledEndScope = true;
            }

            public bool IsCalledHandleException { get; private set; }
            protected override void HandleException(IDbConnection connection, System.Exception ex, object invoker) {
                IsCalledHandleException = true;
            }

            public bool IsCalledHandleFinally { get; private set; }
            protected override void HandleFinally(IDbConnection connection, object invoker) {
                IsCalledHandleFinally = true;
            }

 
        }

        private class ConnectionDecoratorForTest : ConnectionDecorator {
            public override void Decorate(Action<IDbConnection> action) {
                base.Decorate(action);
            }

            public override RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
                return base.Decorate<RETURN_TYPE>(func);
            }

            public override bool IsOpen() {
                return true;
            }
        }

        #endregion
    }
}