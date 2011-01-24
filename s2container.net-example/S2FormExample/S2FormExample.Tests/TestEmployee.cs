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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Quill.Unit;
using Seasar.S2FormExample.Logics.Dao;
using Seasar.S2FormExample.Logics.Dto;
using Seasar.S2FormExample.Logics.Page;
using Seasar.S2FormExample.Logics.Service;

namespace Seasar.S2FormExample.Tests
{
    /// <summary>
    /// 社員用テストケースクラス
    /// </summary>
    [TestFixture]
    public class TestEmployee : QuillTestCase
    {
        protected IEmployeeDao daoOfEmp;
        protected IOutputCSVDao daoOfOutput;
        protected IEmployeeCSVDao daoOfCsv;
        protected IEmployeeEditService editService;
        protected IEmployeeListService listService;

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
        [Test, Quill(Tx.Rollback)]
        public void TestSelectOfDao()
        {
            // 一覧で取得する            
            IList<EmployeeDto> list = daoOfEmp.GetAll();
            Assert.AreEqual(5, list.Count, "Count");
            int i = 0;
            foreach (EmployeeDto dto in list)
            {
                if (i == 2)
                {
                    Assert.AreEqual(3, dto.Id.Value, "Id");
                    Assert.AreEqual("佐藤愛子", dto.Name, "Name");
                    Assert.AreEqual("010003", dto.Code, "Code");
                    Assert.AreEqual(2, dto.Gender, "Gender");
                    Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                                    dto.EntryDay.Value, "Entry");
                    Assert.AreEqual(1, dto.DeptNo.Value, "DeptNo");
                    Assert.AreEqual(1, dto.Department.Id.Value, "Dept.No");
                    Assert.AreEqual("営業部", dto.Department.Name, "Dept.Name");
                    Assert.AreEqual("0001", dto.Department.Code, "Dept.Code");
                }
                i++;
            }

            list = daoOfEmp.FindByGender(1);
            Assert.AreEqual(4, list.Count, "Count2");

            // 個別に取得する

            EmployeeDto data = daoOfEmp.GetData(3);

            Assert.AreEqual(3, data.Id.Value, "Id2");
            Assert.AreEqual("佐藤愛子", data.Name, "Name2");
            Assert.AreEqual("010003", data.Code, "Code2");
            Assert.AreEqual(2, data.Gender, "Gender2");
            Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                            data.EntryDay.Value, "Entry2");
            Assert.AreEqual(1, data.DeptNo.Value, "DeptN2o");
            Assert.AreEqual(1, data.Department.Id.Value, "Dept.No2");
            Assert.AreEqual("営業部", data.Department.Name, "Dept.Name2");
            Assert.AreEqual("0001", data.Department.Code, "Dept.Code2");

            Assert.AreEqual(3, daoOfEmp.GetId("010003"), "GetId");
        }

        /// <summary>
        /// 更新系のテスト
        /// </summary>
        [Test, Quill(Tx.Rollback)]
        public void TestInsertOfDao()
        {
            // 挿入のテスト
            EmployeeDto data = new EmployeeDto();
            data.Code = "060006";
            data.Name = "後藤六郎";
            data.EntryDay = new DateTime(2006, 4, 1, 0, 0, 0);
            data.Gender = 1;
            data.DeptNo = 1;

            Assert.AreEqual(1, daoOfEmp.InsertData(data), "Insert");

            // 更新のテスト
            int id = daoOfEmp.GetId("060006");
            data.Id = id;
            data.Code = "060006";
            data.Name = "五島六郎";
            data.EntryDay = new DateTime(2006, 4, 1, 0, 0, 0);
            data.Gender = 1;
            data.DeptNo = 2;

            Assert.AreEqual(1, daoOfEmp.UpdateData(data), "Update");

            data = daoOfEmp.GetData(id);
            Assert.AreEqual(id, data.Id.Value, "Id");
            Assert.AreEqual("五島六郎", data.Name, "Name");
            Assert.AreEqual("060006", data.Code, "Code");
            Assert.AreEqual(1, data.Gender, "Gender2");
            Assert.AreEqual(new DateTime(2006, 4, 1, 0, 0, 0),
                            data.EntryDay.Value, "Entry");
            Assert.AreEqual(2, data.DeptNo.Value, "DeptNo");
            Assert.AreEqual(2, data.Department.Id.Value, "Dept.No2");
            Assert.AreEqual("技術部", data.Department.Name, "Dept.Name2");
            Assert.AreEqual("0002", data.Department.Code, "Dept.Code2");

            // 削除のテスト
            data = new EmployeeDto();
            data.Id = id;

            Assert.AreEqual(1, daoOfEmp.DeleteData(data), "Delete");

            IList<EmployeeDto> list = daoOfEmp.GetAll();
            Assert.AreEqual(5, list.Count, "Count");
        }

        /// <summary>
        /// CSV用テスト
        /// </summary>
        [Test, Quill(Tx.Rollback)]
        public void TestDaoOfCSV()
        {
            // 取得のテスト
            IList<EmployeeCsvDto> list = daoOfCsv.GetAll();
            Assert.AreEqual(5, list.Count, "Count");
            int i = 0;
            foreach (EmployeeCsvDto dto in list)
            {
                if (i == 2)
                {
                    Assert.AreEqual("佐藤愛子", dto.Name, "Name");
                    Assert.AreEqual("010003", dto.Code, "Code");
                    Assert.AreEqual(2, dto.Gender, "Gender");
                    Assert.AreEqual("女性", dto.GenderName, "GenderName");
                    Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                                    dto.EntryDay.Value, "Entry");
                    Assert.AreEqual("営業部", dto.DeptName, "Dept.Name");
                    Assert.AreEqual("0001", dto.DeptCode, "Dept.Code");
                }
                i++;
            }

            // 出力のテスト
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\csvtest.csv";
            Assert.AreEqual(5, daoOfOutput.OutputEmployeeList(path, list));
        }

        /// <summary>
        /// 社員リストサービステスト
        /// </summary>
        [Test, Quill(Tx.Rollback)]
        public void TestListService()
        {
            EmployeeListPage page = listService.GetAll();
            Assert.AreEqual(5, page.List.Count, "Count");
            int i = 0;
            foreach (EmployeeDto dto in page.List)
            {
                if (i == 2)
                {
                    Assert.AreEqual(3, dto.Id.Value, "Id");
                    Assert.AreEqual("佐藤愛子", dto.Name, "Name");
                    Assert.AreEqual("010003", dto.Code, "Code");
                    Assert.AreEqual(2, dto.Gender, "Gender");
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
        [Test, Quill(Tx.Rollback)]
        public void TestEditService()
        {
            EmployeeEditPage data = editService.GetData(3);
            Assert.AreEqual(3, data.Id.Value, "Id");
            Assert.AreEqual("佐藤愛子", data.Name, "Name");
            Assert.AreEqual("010003", data.Code, "Code");
            Assert.AreEqual(2, data.Gender, "Gender");
            Assert.AreEqual(new DateTime(2001, 4, 1, 0, 0, 0),
                            data.Entry, "Entry");
            Assert.AreEqual(1, data.Depart, "DeptNo");

            // 挿入のテスト
            data = new EmployeeEditPage();
            data.Code = "060006";
            data.Name = "後藤六郎";
            data.Entry = new DateTime(2006, 4, 1, 0, 0, 0);
            data.Gender = 1;
            data.Depart = 1;

            Assert.AreEqual(1, editService.ExecUpdate(data), "Insert");

            // 更新のテスト
            data = new EmployeeEditPage();
            data.Id = 2;
            data.Code = "999999";
            data.Name = "鈴木二郎";
            data.Entry = new DateTime(1999, 5, 1, 0, 0, 0);
            data.Gender = 1;
            data.Depart = 2;

            Assert.AreEqual(1, editService.ExecUpdate(data), "Update");

            data = editService.GetData(2);
            Assert.AreEqual(2, data.Id.Value, "Id");
            Assert.AreEqual("鈴木二郎", data.Name, "Name");
            Assert.AreEqual("999999", data.Code, "Code");
            Assert.AreEqual(1, data.Gender, "Gender");
            Assert.AreEqual(new DateTime(1999, 5, 1, 0, 0, 0),
                            data.Entry, "Entry");
            Assert.AreEqual(2, data.Depart, "DeptNo");
        }
    }
}