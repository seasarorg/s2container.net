#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using Gallio.Framework.Pattern;
using MbUnit.Framework;
using Seasar.Framework.Log;
using Seasar.Framework.Message;

namespace Seasar.Unit.Core
{
    /// <summary>
    /// MbUnit3用テスト属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class S2MbUnitAttributeBase : TestDecoratorAttribute
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly S2TestCaseRunnerBase _runner;
        protected IDictionary<int, Exception> _errors;

        public S2MbUnitAttributeBase()
        {
            _runner = CreateRunner(Seasar.Extension.Unit.Tx.NotSupported);
        }

        public S2MbUnitAttributeBase(Seasar.Extension.Unit.Tx txTreatment)
        {
            _runner = CreateRunner(txTreatment);
        }

        protected override void SetUp(PatternTestInstanceState testInstanceState)
        {
            try
            {
                _runner.SetUp<PatternTestInstanceState>(testInstanceState.FixtureInstance,
                    ExecuteSetup, testInstanceState);
            }
            catch (System.Exception e)
            {
                HandleException(e, testInstanceState.TestMethod);
                throw;
            }
        }

        protected override void Execute(PatternTestInstanceState testInstanceState)
        {
            try
            {
                _runner.Execute<PatternTestInstanceState>(base.Execute, testInstanceState);
            }
            catch (System.Exception e)
            {
                HandleException(e, testInstanceState.TestMethod);
                throw;
            }
        }

        protected override void TearDown(PatternTestInstanceState testInstanceState)
        {
            try
            {
                _runner.TearDown<PatternTestInstanceState>(testInstanceState.FixtureInstance,
                        ExecuteTearDown, testInstanceState);
            }
            catch (System.Exception e)
            {
                HandleException(e, testInstanceState.TestMethod);
                throw;
            }
        }

        protected abstract S2TestCaseRunnerBase CreateRunner(Seasar.Extension.Unit.Tx txTreatment);

        protected virtual void ExecuteSetup(PatternTestInstanceState testInstanceState)
        {
            // SetUp属性が付いたメソッドを呼び出す
            base.SetUp(testInstanceState);
            // 「SetUp」という名称のメソッドを呼び出す
            S2TestUtils.CallAsHavingAttribute<SetUpAttribute>(
                    testInstanceState.FixtureInstance, S2SpecialMethodNameConst.SET_UP);
            // 「SetUpXXX」という名称のメソッドを呼び出す
            S2TestUtils.CallForSpecificMethod(testInstanceState.FixtureInstance,
                    testInstanceState.TestMethod.Name, S2SpecialMethodNameConst.SET_UP);
        }

        protected virtual void ExecuteTearDown(PatternTestInstanceState testInstanceState)
        {
            try
            {
                S2TestUtils.CallForSpecificMethod(testInstanceState.FixtureInstance,
                    testInstanceState.TestMethod.Name, S2SpecialMethodNameConst.TEAR_DOWN);
            }
            finally
            {
                try
                {
                    S2TestUtils.CallAsHavingAttribute<TearDownAttribute>(
                            testInstanceState.FixtureInstance, S2SpecialMethodNameConst.TEAR_DOWN);
                }
                finally
                {
                    base.TearDown(testInstanceState);
                }
            }
        }

        protected virtual void HandleException(Exception e, MethodBase method)
        {
            if (_errors == null)
            {
                _errors = new Dictionary<int, Exception>();
            }

            if (!_errors.ContainsKey(e.GetHashCode()))
            {
                if (IsExpectedException(e, method))
                {
                    return;
                }

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(MessageFormatter.GetSimpleMessage("ESSR0017", new object[] { e }), e);
                }
                else
                {
                    Console.Error.WriteLine(e);
                }

                _errors.Add(e.GetHashCode(), e);
            }
        }

        /// <summary>
        /// 予期している例外か判定する
        /// </summary>
        /// <param name="e"></param>
        /// <param name="method"></param>
        /// <param name="attributeType"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        private bool IsExpectedException(Exception e, MethodBase method)
        {
            if (method == null)
            {
                return false;
            }

            var attrs = method.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false);
            foreach (var attribute in attrs)
            {
                if (S2TestUtils.IsMatchExpectedException(
                    ((ExpectedExceptionAttribute)attribute).ExceptionType, e))
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}
