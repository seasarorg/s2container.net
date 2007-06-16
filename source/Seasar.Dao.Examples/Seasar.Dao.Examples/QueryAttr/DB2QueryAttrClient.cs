#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Dao.Examples.QueryAttr
{
    /// <summary>
    /// Query‘®«‚ÌƒTƒ“ƒvƒ‹‚ğÀs‚µ‚Ü‚·B
    /// </summary>
    public class DB2QueryAttrClient
    {
        private const string PATH = "Seasar.Dao.Examples/QueryAttr/DB2QueryAttr.dicon";

        public void Main()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IEmployeeDao employeeDao = (IEmployeeDao) container.GetComponent(typeof(IEmployeeDao));

            // ]‹Æˆõ–¼‚Ì¸‡‚É•À‚Ñ‘Ö‚¦‚½‘S‚Ä‚Ì]‹Æˆõ‚ğæ“¾
            IList employeeList = employeeDao.GetAllListEnameAsc();

            IEnumerator employees = employeeList.GetEnumerator();
            Console.WriteLine("/** ]‹Æˆõ–¼‚Ì¸‡‚É•À‚Ñ‘Ö‚¦‚½‘S‚Ä‚Ì]‹Æˆõ‚ÌƒŠƒXƒg **/");
            while (employees.MoveNext())
            {
                Console.WriteLine(((Employee) employees.Current).ToString());
            }
            Console.WriteLine(string.Empty);

            // ]‹Æˆõ–¼‚©‚ç]‹Æˆõ‚ğæ“¾
            Employee emp = employeeDao.GetEmployeeByEname("ALLEN");
            Console.WriteLine("/** ]‹Æˆõ–¼‚©‚ç]‹Æˆõ‚ğæ“¾ **/");
            Console.WriteLine(emp.ToString());
        }
    }
}
