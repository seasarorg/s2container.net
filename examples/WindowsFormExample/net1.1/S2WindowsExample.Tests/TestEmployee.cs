#region Copyright

/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Nullables;
using Seasar.Extension.Unit;
using Seasar.WindowsExample.Logics.Dao;
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;

namespace Seasar.WindowsExample.Tests
{
    /// <summary>
    /// 社員用テストケースクラス
    /// </summary>
    [TestFixture]
    public class TestEmployee : S2TestCase
    {
        /// <summary>
        /// Logic設定ファイル
        /// </summary>
        private const string PATH = "ExampleLogics.dicon";

        /// <summary>
        /// テストのセットアップ
        /// </summary>
        [SetUp]
        public void Setup()
        {
            FileInfo info = new FileInfo(
                string.Format("{0}.dll.config", SystemInfo.AssemblyShortName(
                                                    Assembly.GetExecutingAssembly())));
            // アセンブリがdllの場合は".dll.config"

            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        /// <summary>
        /// 検索系のテスト
        /// </summary>
        [Test, S2]
        public void TestSelectOfDao()
        {
            Include(PATH);

            IEmployeeDao dao = (IEmployeeDao) GetComponent(typeof (IEmployeeDao));
            Assert.IsNotNull(dao, "NotNull");

            // 一覧で取得する
            IList list = dao.GetAll();
            Assert.AreEqual(5, list.Count, "Count");
            int i = 0;
            foreach (EmployeeDto dto in list)
            {
                if ( i == 2 )
                {
                    Assert.AreEqual(3, dto.Id.Value, "Id");
                    Assert.AreEqual("佐藤愛子", dto.Name, "Name");
                    Assert.AreEqual("010003", dto.Code, "Code");
                    Assert.AreEqual(1, dto.Gender, "Gender");
                    Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                                    dto.EntryDay.Value, "Entry");
                    Assert.AreEqual(1, dto.DeptNo.Value, "DeptNo");
                    Assert.AreEqual(1, dto.Department.Id.Value, "Dept.No");
                    Assert.AreEqual("営業部", dto.Department.Name, "Dept.Name");
                    Assert.AreEqual("0001", dto.Department.Code, "Dept.Code");
                }
                i++;
            }

            // 個別に取得する

            EmployeeDto data = dao.GetData(3);

            Assert.AreEqual(3, data.Id.Value, "Id2");
            Assert.AreEqual("佐藤愛子", data.Name, "Name2");
            Assert.AreEqual("010003", data.Code, "Code2");
            Assert.AreEqual(1, data.Gender, "Gender2");
            Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                            data.EntryDay.Value, "Entry2");
            Assert.AreEqual(1, data.DeptNo.Value, "DeptN2o");
            Assert.AreEqual(1, data.Department.Id.Value, "Dept.No2");
            Assert.AreEqual("営業部", data.Department.Name, "Dept.Name2");
            Assert.AreEqual("0001", data.Department.Code, "Dept.Code2");

            Assert.AreEqual(3, dao.GetId("010003"), "GetId");
        }

        /// <summary>
        /// 更新系のテスト
        /// </summary>
        [Test, S2(Tx.Rollback)]
        public void TestInsertOfDao()
        {
            Include(PATH);

            IEmployeeDao dao = (IEmployeeDao) GetComponent(typeof (IEmployeeDao));
            Assert.IsNotNull(dao, "NotNull");

            // 挿入のテスト
            EmployeeDto data = new EmployeeDto();
            data.Code = "060006";
            data.Name = "後藤六郎";
            data.EntryDay = new NullableDateTime(new DateTime(2006, 4, 1, 0, 0, 0));
            data.Gender = 0;
            data.DeptNo = 1;

            Assert.AreEqual(1, dao.InsertData(data), "Insert");

            // 更新のテスト
            int id = dao.GetId("060006");
            data.Id = id;
            data.Code = "060006";
            data.Name = "五島六郎";
            data.EntryDay = new NullableDateTime(new DateTime(2006, 4, 1, 0, 0, 0));
            data.Gender = 0;
            data.DeptNo = 2;

            Assert.AreEqual(1, dao.UpdateData(data), "Update");

            data = dao.GetData(id);
            Assert.AreEqual(id, data.Id.Value, "Id");
            Assert.AreEqual("五島六郎", data.Name, "Name");
            Assert.AreEqual("060006", data.Code, "Code");
            Assert.AreEqual(0, data.Gender, "Gender2");
            Assert.AreEqual(new DateTime(2006, 4, 1, 0, 0, 0),
                            data.EntryDay.Value, "Entry");
            Assert.AreEqual(2, data.DeptNo.Value, "DeptNo");
            Assert.AreEqual(2, data.Department.Id.Value, "Dept.No2");
            Assert.AreEqual("技術部", data.Department.Name, "Dept.Name2");
            Assert.AreEqual("0002", data.Department.Code, "Dept.Code2");

            // 削除のテスト
            data = new EmployeeDto();
            data.Id = id;

            Assert.AreEqual(1, dao.DeleteData(data), "Delete");

            IList list = dao.GetAll();
            Assert.AreEqual(5, list.Count, "Count");
        }

        /// <summary>
        /// CSV用テスト
        /// </summary>
        [Test, S2]
        public void TestDaoOfCSV()
        {
            Include(PATH);

            // 取得のテスト
            IEmployeeCSVDao dao = (IEmployeeCSVDao) GetComponent(typeof (IEmployeeCSVDao));
            Assert.IsNotNull(dao, "NotNull");

            IList list = dao.GetAll();
            Assert.AreEqual(5, list.Count, "Count");
            int i = 0;
            foreach (EmployeeCsvDto dto in list)
            {
                if ( i == 2 )
                {
                    Assert.AreEqual("佐藤愛子", dto.Name, "Name");
                    Assert.AreEqual("010003", dto.Code, "Code");
                    Assert.AreEqual(1, dto.Gender, "Gender");
                    Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                                    dto.EntryDay.Value, "Entry");
                    Assert.AreEqual("営業部", dto.DeptName, "Dept.Name");
                    Assert.AreEqual("0001", dto.DeptCode, "Dept.Code");
                }
                i++;
            }

            // 出力のテスト
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\csvtest.csv";
            IOutputCSVDao daoOfCsv = (IOutputCSVDao) GetComponent(typeof (IOutputCSVDao));
            Assert.AreEqual(5, daoOfCsv.OutputEmployeeList(path, list));
        }

        /// <summary>
        /// 社員リストサービステスト
        /// </summary>
        [Test, S2]
        public void TestListService()
        {
            Include(PATH);

            IEmployeeListService service = (IEmployeeListService) GetComponent(typeof (IEmployeeListService));
            Assert.IsNotNull(service, "NotNull");

            IList list = service.GetAll();
            Assert.AreEqual(5, list.Count, "Count");
            int i = 0;
            foreach (EmployeeDto dto in list)
            {
                if ( i == 2 )
                {
                    Assert.AreEqual(3, dto.Id.Value, "Id");
                    Assert.AreEqual("佐藤愛子", dto.Name, "Name");
                    Assert.AreEqual("010003", dto.Code, "Code");
                    Assert.AreEqual(1, dto.Gender, "Gender");
                    Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                                    dto.EntryDay.Value, "Entry");
                    Assert.AreEqual(1, dto.DeptNo.Value, "DeptNo");
                    Assert.AreEqual(1, dto.Department.Id.Value, "Dept.No");
                    Assert.AreEqual("営業部", dto.Department.Name, "Dept.Name");
                    Assert.AreEqual("0001", dto.Department.Code, "Dept.Code");
                    Assert.AreEqual("営業部", dto.DeptName, "DeptName");
                }
                i++;
            }
        }

        /// <summary>
        /// 社員登録サービスのテスト
        /// </summary>
        [Test, S2(Tx.Rollback)]
        public void TestEditService()
        {
            Include(PATH);

            IEmployeeEditService service = (IEmployeeEditService) GetComponent(typeof (IEmployeeEditService));
            Assert.IsNotNull(service, "NotNull");

            EmployeeDto data = service.GetData(3);
            Assert.AreEqual(3, data.Id.Value, "Id");
            Assert.AreEqual("佐藤愛子", data.Name, "Name");
            Assert.AreEqual("010003", data.Code, "Code");
            Assert.AreEqual(1, data.Gender, "Gender");
            Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                            data.EntryDay.Value, "Entry");
            Assert.AreEqual(1, data.DeptNo.Value, "DeptNo");
            Assert.AreEqual(1, data.Department.Id.Value, "Dept.No");
            Assert.AreEqual("営業部", data.Department.Name, "Dept.Name");
            Assert.AreEqual("0001", data.Department.Code, "Dept.Code");
            Assert.AreEqual("営業部", data.DeptName, "DeptName");

            // 挿入のテスト
            data = new EmployeeDto();
            data.Code = "060006";
            data.Name = "後藤六郎";
            data.EntryDay = new NullableDateTime(new DateTime(2006, 4, 1, 0, 0, 0));
            data.Gender = 0;
            data.DeptNo = 1;

            Assert.AreEqual(1, service.ExecUpdate(data), "Insert");

            // 更新のテスト
            data = new EmployeeDto();
            data.Id = 2;
            data.Code = "999999";
            data.Name = "鈴木二郎";
            data.EntryDay = new NullableDateTime(new DateTime(1999, 5, 1, 0, 0, 0));
            data.Gender = 0;
            data.DeptNo = 2;

            Assert.AreEqual(1, service.ExecUpdate(data), "Update");

            data = service.GetData(2);
            Assert.AreEqual(2, data.Id.Value, "Id");
            Assert.AreEqual("鈴木二郎", data.Name, "Name");
            Assert.AreEqual("999999", data.Code, "Code");
            Assert.AreEqual(0, data.Gender, "Gender");
            Assert.AreEqual(new DateTime(1999, 5, 1, 0, 0, 0),
                            data.EntryDay.Value, "Entry");
            Assert.AreEqual(2, data.DeptNo.Value, "DeptNo");
            Assert.AreEqual(2, data.Department.Id.Value, "Dept.No");
            Assert.AreEqual("技術部", data.Department.Name, "Dept.Name");
            Assert.AreEqual("0002", data.Department.Code, "Dept.Code");
            Assert.AreEqual("技術部", data.DeptName, "DeptName");
        }
    }
}