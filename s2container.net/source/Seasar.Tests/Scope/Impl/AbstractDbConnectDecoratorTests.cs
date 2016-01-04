using NUnit.Framework;
using Quill.Scope.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quill.Tests;
using System.Data;

namespace Quill.Scope.Impl.Tests {
    [TestFixture()]
    public class AbstractDbConnectDecoratorTests : QuillTestBase {
        [Test()]
        public void Decorate_NoConnectionDecoratorAction_Test() {
            // Arrange
            var target = new TestDbConnectionDecorator();

            // Act/Assert
            TestUtils.ExecuteExcectedException<ArgumentException>(() => {
                target.Decorate((con) => {
                    Assert.IsNull(con);
                    throw new ArgumentException();
                });
            });
        }

        [Test()]
        public void Decorate_NoConnectionDecoratorFunc_Test() {
            // Arrange
            var target = new TestDbConnectionDecorator();

            // Act
            var actual = target.Decorate((con) => {
                Assert.IsNull(con);
                return true;
            });

            // Assert
            Assert.IsTrue(actual);
        }

        [Test()]
        public void DecorateTest1() {
            //Assert.Fail();
        }

        #region helper

        /// <summary>
        /// テスト用DB接続修飾クラス
        /// </summary>
        private class TestDbConnectionDecorator : AbstractDbConnectDecorator {
            protected override void ExecuteAction(IDbConnection connection, Action<IDbConnection> action) {
                throw new NotImplementedException();
            }

            protected override RETURN_TYPE ExecuteFunc<RETURN_TYPE>(IDbConnection connection, Func<IDbConnection, RETURN_TYPE> func) {
                throw new NotImplementedException();
            }

            protected override void HandleException(IDbConnection connection, System.Exception ex, object invoker) {
                throw new NotImplementedException();
            }

            protected override void HandleFinally(IDbConnection connection, object invoker) {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}