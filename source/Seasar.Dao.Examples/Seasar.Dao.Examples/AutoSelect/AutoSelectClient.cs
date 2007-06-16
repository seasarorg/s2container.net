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
using System.Data.SqlTypes;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Nullables;

namespace Seasar.Dao.Examples.AutoSelect
{
    /// <summary>
    /// Select文自動生成のサンプルを実行します。
    /// </summary>
    public class AutoSelectClient
    {
        private const string PATH = "Seasar.Dao.Examples/AutoSelect/AutoSelect.dicon";

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

            IDepartmentDao1 deptDao = (IDepartmentDao1) container.GetComponent(typeof(IDepartmentDao1));

            // 全ての部署を取得
            IList deptList = deptDao.GetAllList();

            IEnumerator depts = deptList.GetEnumerator();
            Console.WriteLine("/** 全ての部署のリスト(System.Data.SqlTypesを使用) **/");
            while (depts.MoveNext())
            {
                Console.WriteLine(((Department1) depts.Current).ToString());
            }

            IDepartmentDao2 deptDao2 = (IDepartmentDao2) container.GetComponent(typeof(IDepartmentDao2));

            // 全ての部署を取得
            IList deptList2 = deptDao2.GetAllList();

            IEnumerator depts2 = deptList2.GetEnumerator();
            Console.WriteLine("/** 全ての部署のリスト(NullableTypeを使用) **/");
            while (depts2.MoveNext())
            {
                Console.WriteLine(((Department2) depts2.Current).ToString());
            }

            // Nullである項目を取得(INullable)
            SqlInt16 active = deptDao2.GetActiveByDeptno(20);
            Console.WriteLine("/** deptnoが20と40のActiveを取得(INullableを使用) **/");
            Console.WriteLine("deptno=20");
            Console.WriteLine(active.IsNull ? "null" : active.Value.ToString());
            active = deptDao2.GetActiveByDeptno(40);
            Console.WriteLine("deptno=40");
            Console.WriteLine(active.IsNull ? "null" : active.Value.ToString());

            // Nullである項目を取得(INullable)
            NullableInt16 active2 = deptDao2.GetActiveByDeptno2(20);
            Console.WriteLine("/** deptnoが20と40のActiveを取得(INullableTypeを使用) **/");
            Console.WriteLine("deptno=20");
            Console.WriteLine(active2.HasValue ? active2.Value.ToString() : "null");
            active2 = deptDao2.GetActiveByDeptno2(40);
            Console.WriteLine("deptno=40");
            Console.WriteLine(active2.HasValue ? active2.Value.ToString() : "null");

            // NullをStringで受け取る
            string dname = deptDao2.GetDnameByDeptno(50);
            Console.WriteLine("/** deptnoが50のdnameを取得(NULLで受け取る) **/");
            Console.WriteLine("deptno=50");
            Console.WriteLine(dname == null ? "null" : dname);

            // NullをStringで受け取る
            dname = deptDao2.GetDnameByDeptno(60);
            Console.WriteLine("/** deptnoが60のdnameを取得(存在しないデータを受け取る) **/");
            Console.WriteLine("deptno=60");
            Console.WriteLine(dname == null ? "null" : dname);
        }
    }
}