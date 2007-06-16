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

namespace Seasar.Dao.Examples.AutoSelect
{
    /// <summary>
    /// Select文自動生成のサンプルを実行します。
    /// </summary>
    public class MySqlAutoSelectClient
    {
        private const string PATH = "Seasar.Dao.Examples/AutoSelect/MySqlAutoSelect.dicon";

        public void Main()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IEmployeeDao employeeDao = (IEmployeeDao) container.GetComponent(typeof(IEmployeeDao));

            // 全ての従業員を取得
            IList employeeList = employeeDao.GetAllList();

            IEnumerator employees = employeeList.GetEnumerator();
            Console.WriteLine("/** 全ての従業員のリスト **/");
            while (employees.MoveNext())
            {
                Console.WriteLine(((Employee) employees.Current).ToString());
            }
        }
    }
}
