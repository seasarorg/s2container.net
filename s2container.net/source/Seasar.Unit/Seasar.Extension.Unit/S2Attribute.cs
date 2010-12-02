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

using Seasar.Unit.Core;

namespace Seasar.Extension.Unit
{
    /// <summary>
    /// S2Container用テスト属性
    /// </summary>
    public class S2Attribute : S2MbUnitAttributeBase
    {
        public S2Attribute()
            : base()
        {
        }

        public S2Attribute(Seasar.Extension.Unit.Tx txTreatment)
            : base(txTreatment)
        {
        }

        protected override S2TestCaseRunnerBase CreateRunner(Seasar.Extension.Unit.Tx txTreatment)
        {
            return new S2TestCaseRunner(txTreatment);
        }
    }
}
