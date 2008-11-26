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

using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples.AutoInsert
{
    [Bean(typeof(Employee))]
    public interface IEmployeeDao
    {
        /// <summary>
        /// �]�ƈ���ǉ����܂��B
        /// </summary>
        /// <param name="emp">�]�ƈ�</param>
        /// <returns>�ǉ����ꂽ��</returns>
        int InsertEmployee(Employee emp);

        /// <summary>
        /// �]�ƈ��ԍ�����]�ƈ����擾���܂��B
        /// </summary>
        /// <param name="empno">�]�ƈ��ԍ�</param>
        /// <returns>�]�ƈ�</returns>
        [Query("empno=/*empno*/")]
        Employee GetEmployeeByEmpno(int empno);
    }
}