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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Examples.Reference.Tx
{
    /// <summary>
    /// MS DTCトランザクション
    /// </summary>
    public class TxClient
    {
        private const string PATH = "Seasar.Examples/Reference/Tx/TxClient.dicon";

        public void Main()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            container.Init();

            IEmployeeDao dao = (IEmployeeDao) container.GetComponent(typeof(IEmployeeDao));
            try
            {
                dao.Insert();
            }
            catch (ApplicationException) { }
        }
    }
}
