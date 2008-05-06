#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.Collections;
using MbUnit.Core.Invokers;
using Seasar.Extension.Unit;

namespace Seasar.Quill.Unit
{
	public class QuillTestCaseRunInvoker : DecoratorRunInvoker
	{
        private readonly QuillTestCaseRunner _runner;
        private readonly Tx _tx;
        private readonly Type _daoSettingType;
        private readonly Type _transactionSettingType;

        public QuillTestCaseRunInvoker(IRunInvoker invoker, Tx tx,
            Type daoSettingType, Type transactionSettingType)
            : base(invoker)
        {
            _tx = tx;
            _daoSettingType = daoSettingType;
            _transactionSettingType = transactionSettingType;
            _runner = new QuillTestCaseRunner();
        }

        public override object Execute(object o, IList args)
        {
            return _runner.Run(Invoker, o, args, _tx, _daoSettingType, _transactionSettingType);
        }
	}
}
