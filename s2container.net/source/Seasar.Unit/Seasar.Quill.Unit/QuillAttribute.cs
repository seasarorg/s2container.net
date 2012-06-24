#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

#if NET_4_0
using Seasar.Unit.Core;
#else
#region NET2.0
using System;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;
using Seasar.Extension.Unit;
using Seasar.Quill.Util;
#endregion
#endif

namespace Seasar.Quill.Unit
{
    /// <summary>
    /// Quillを使用するテスト属性
    /// </summary>
#if NET_4_0
    public class QuillAttribute : S2MbUnitAttributeBase
    {
        public QuillAttribute() : base()
        {
        }

        public QuillAttribute(Seasar.Extension.Unit.Tx txTreatment)
            : base(txTreatment)
        {
        }

        protected override S2TestCaseRunnerBase CreateRunner(Seasar.Extension.Unit.Tx txTreatment)
        {
            return new QuillTestCaseRunner(txTreatment);
        }
    }
#else
#region NET2.0
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class QuillAttribute : DecoratorPatternAttribute
    {
        private readonly Tx _tx;

        public QuillAttribute()
        {
            _tx = Tx.NotSupported;
        }

        public QuillAttribute(Tx tx)
        {
            _tx = tx;
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new QuillTestCaseRunInvoker(invoker, _tx);
        }
    }
#endregion
#endif
}
