#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dxo 
{
    /*
     * Created by: H.Fujii
     * Created: 2007/11/21
     */
    [TestFixture]
    public class S2DxoTest : S2TestCase
    {
        /// <summary>
        /// Logic Log
        /// </summary>
        private const string PATH = "Seasar.Tests.Dxo.dxo.dicon";

        [SetUp]
        public void SetUp()
        {
            FileInfo info = new FileInfo(
                string.Format("{0}.dll.config", SystemInfo.AssemblyShortName(
                                                    Assembly.GetExecutingAssembly())));

            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        [Test, S2]
        public void TestSimple()
        {
            Include(PATH);

            IEmployeeDxo dxo = (IEmployeeDxo) GetComponent(typeof (IEmployeeDxo));


            Employee employee = new Employee();
            employee.EName = "Mike";
            Department dept = new Department();
            dept.Id = 2;
            dept.DName = "Sales";
            employee.Department = dept;

            // プロパティに値がコピーされる場合
            EmployeePage page = new EmployeePage();
            page.Name = "Myers";
            dxo.ConvertToEmpPage(employee, page);
            Assert.AreEqual("Mike", page.EName, "EName2");
            Assert.AreEqual("Sales", page.DName, "DName2");
            Assert.AreEqual("Myers", page.Name, "Name");
            Assert.AreEqual(2, page.Id, "Id");

            // 属性による型変換
            page = new EmployeePage();
            page.Name = "Myers";
            dxo.ConvertToSpecialEmpPage(employee, page);
            Assert.AreEqual("Sales", page.EName, "EName");
            Assert.AreEqual("Mike", page.DName, "DName");
            Assert.AreEqual("Myers", page.Name, "Name");
            Assert.AreEqual(2, page.Id, "Id");


            // 新しいオブジェクトが生成される場合
            page = dxo.ConvertToEmployeePage(employee);
            Assert.AreEqual("Mike", page.EName, "EName3");
            Assert.AreEqual("Sales", page.DName, "DName3");
            if (!String.IsNullOrEmpty(page.Name))
                Assert.Fail("Name2");
            Assert.AreEqual(2, page.Id, "Id");

            // 逆変換
            page = new EmployeePage();
            page.DName = "Tech";
            page.EName = "John";
            page.Name = "Smith";
            page.Id = 3;
            employee = new Employee();
            employee.Department = new Department();
            dxo.ConvertPageToEmp(page, employee);
            Assert.AreEqual("John", employee.EName, "EName Ex");
            Assert.AreEqual("Tech", employee.Department.DName, "DName Ex");
            Assert.AreEqual(3, employee.Department.Id, "Id Ex");
        }

        /// <summary>
        /// 型変換テスト
        /// </summary>
        [Test, S2]
        public void TestBeanToBean()
        {
            Include(PATH);
            IBeanToBeanDxo dxo = (IBeanToBeanDxo) GetComponent(typeof (IBeanToBeanDxo));

            BeanA source = new BeanA();
            source.FlagToBool = true;
            source.ShortToBool = 1;
            source.IntToBool = 0;
            source.LongToBool = 1;
            source.StringToBool = "yes";
            source.FlagToLong = true;
            source.ShortToLong = 123;
            source.IntToLong = 9876;
            source.LongToLong = 7654321;
            source.StringToLong = "1234567";
            char[] testchar = {'t', 'e', 's', 't'};
            source.CharToString = testchar;
            DateTime date = new DateTime(2007, 7, 2, 1, 2, 3);
            source.DateToString = date;
            char[] testchar2 = {'t', 'e', 's', 't', '2'};
            source.CharToChar = testchar2;
            source.StringToChar = "test3";
            source.StringToDateTime = "20070630";
            source.LongToDateTime = DateTime.Now.Ticks;

            TargetBean target = dxo.ConvertBeanToTarget(source);
            Assert.IsTrue(target.FlagToBool, "FlagToBool");
            Assert.IsTrue(target.ShortToBool, "ShortToBool");
            Assert.IsFalse(target.IntToBool, "IntToBool");
            Assert.IsTrue(target.LongToBool, "LongToBool");
            Assert.IsTrue(target.StringToBool, "StringToBool");
            Assert.AreEqual(1, target.FlagToLong, "FlagToLong");
            Assert.AreEqual(123, target.ShortToLong, "ShortToLong");
            Assert.AreEqual(9876, target.IntToLong, "IntToLong");
            Assert.AreEqual(7654321, target.LongToLong, "LongToLong");
            Assert.AreEqual(1234567, target.StringToLong, "StringToLong");
            Assert.AreEqual("test", target.CharToString, "CharToString");
            Assert.AreEqual("20070702", target.DateToString, "DateToString");
            char[] test2 = target.CharToChar;
            Assert.AreEqual('t', test2[0], "CharToChar1");
            Assert.AreEqual('e', test2[1], "CharToChar2");
            Assert.AreEqual('s', test2[2], "CharToChar3");
            Assert.AreEqual('t', test2[3], "CharToChar4");
            Assert.AreEqual('2', test2[4], "CharToChar5");
            char[] test3 = target.StringToChar;
            Assert.AreEqual('t', test3[0], "CharToChar6");
            Assert.AreEqual('e', test3[1], "CharToChar7");
            Assert.AreEqual('s', test3[2], "CharToChar8");
            Assert.AreEqual('t', test3[3], "CharToChar9");
            Assert.AreEqual('3', test3[4], "CharToChar10");
            DateTime destTime = target.StringToDateTime;
            Assert.AreEqual(2007, destTime.Year, "Year");
            Assert.AreEqual(6, destTime.Month, "Month");
            Assert.AreEqual(30, destTime.Day, "Day");
            destTime = target.LongToDateTime;
            Console.Out.WriteLine("destTime:" + destTime);
        }

        [Test, S2]
        public void TestArray()
        {
            Include(PATH);
            ICollectionDxo dxo = (ICollectionDxo) GetComponent(typeof (ICollectionDxo));

            // 配列To配列
            Department dept = new Department();
            dept.DName = "Sales";
            Employee[] emp = new Employee[2];
            emp[0] = new Employee();
            emp[0].EName = "Mike";
            emp[0].Department = dept;
            emp[1] = new Employee();
            emp[1].EName = "Scott";
            emp[1].Department = dept;

            EmployeePage[] target = new EmployeePage[2];

            dxo.ConvertFromArrayToArray(emp, target);
            int i = 0;
            foreach (EmployeePage employee in target)
            {
                if (i == 0) Assert.AreEqual("Mike", employee.EName, "EName");
                if (i == 1) Assert.AreEqual("Scott", employee.EName, "EName");
                i++;
            }

            emp[0].EName = "Mike Smith";
            emp[1].EName = "Scott Tiger";
            target = dxo.ConvertToArray(emp);
            i = 0;
            foreach (EmployeePage employee in target)
            {
                if (i == 0) Assert.AreEqual("Mike Smith", employee.EName, "EName");
                if (i == 1) Assert.AreEqual("Scott Tiger", employee.EName, "EName");
                i++;
            }
        }

        [Test, S2]
        public void TestList()
        {
            Include(PATH);
            ICollectionDxo dxo = (ICollectionDxo)GetComponent(typeof(ICollectionDxo));

            IList<Employee> srcList = new List<Employee>();
            Department dept = new Department();
            dept.DName = "Sales";
            Employee emp = new Employee();
            emp.EName = "Mike";
            emp.Department = dept;
            srcList.Add(emp);
            emp = new Employee();
            emp.EName = "Scott";
            emp.Department = dept;
            srcList.Add(emp);

            IList<EmployeePage> destList = new List<EmployeePage>();

            int i = 0;
            dxo.ConvertListToList(srcList, destList);
            foreach (EmployeePage page in destList)
            {
                if (i == 0) Assert.AreEqual("Mike", page.EName, "EName1-" + i);
                if (i == 1) Assert.AreEqual("Scott", page.EName, "ENam1-" + i);
                i++;
            }

            srcList.Clear();
            emp = new Employee();
            emp.EName = "Mike Smith";
            emp.Department = dept;
            srcList.Add(emp);
            destList = dxo.ConvertToList(srcList);
            foreach (EmployeePage page in destList)
            {
                if (i == 0) Assert.AreEqual("Mike Smith", page.EName, "EName2-" + i);
                if (i == 1) Assert.AreEqual("Scott", page.EName, "ENam2-" + i);
                i++;
            }
        }

        [Test, S2]
        public void TestList2()
        {
            Include(PATH);
            ICollectionDxo dxo = (ICollectionDxo)GetComponent(typeof(ICollectionDxo));

            Department dept = new Department();
            dept.DName = "Sales";
            Employee emp = new Employee();
            emp.EName = "Mike Smith";
            emp.Department = dept;
            IList<EmployeePage> list = new List<EmployeePage>();
            EmployeePage targetPage = new EmployeePage();
            targetPage.EName = "Scott";
            targetPage.DName = "Sales";
            targetPage.Id = 1;
            list.Add(targetPage);
            dxo.ConvertPonoToList(emp, list);
            Assert.AreEqual(2, list.Count, "Count");
            Assert.AreEqual("Mike Smith", list[1].EName, "EName");

            List<EmployeePage> list2 = dxo.ConvertToIList(emp);
            Assert.AreEqual(1, list2.Count, "Count2");
        }

        [Test, S2]
        public void TestDictionary()
        {
            Include(PATH);

            ICollectionDxo dxo = (ICollectionDxo)GetComponent(typeof(ICollectionDxo));

            IDictionary dict = new Hashtable();
            Employee emp = new Employee();
            emp.EName = "Mike Smith";
            emp.Department = new Department();
            emp.Department.Id = 1;
            emp.Department.DName = "Sales";

            dxo.ConvertToDictinary(emp, dict);
            Assert.AreEqual("Mike Smith", dict["EName"], "EName");
            Department dept = (Department)dict["Department"];
            Assert.AreEqual(1, dept.Id, "Id");
            Assert.AreEqual("Sales", dept.DName, "DName");

            Hashtable hash = dxo.ConvertToHashtable(emp);
            Assert.AreEqual("Mike Smith", hash["EName"], "EName");
            Department dept2 = (Department)hash["Department"];
            Assert.AreEqual(1, dept2.Id, "Id");
            Assert.AreEqual("Sales", dept2.DName, "DName");

        }

        [Test, S2]
        public void TestDateTime()
        {
            Include(PATH);

            IDateTimeToStringBean dxo = (IDateTimeToStringBean)GetComponent(typeof(IDateTimeToStringBean));

            DateTimeBean bean = new DateTimeBean();
            bean.DateTimeToString = new DateTime(2009, 4, 10, 0, 0, 0);
            Console.Out.WriteLine("bean.DateToString = {0}", bean.DateTimeToString);

            StringBean target = dxo.ConvertBeanToTargetWithoutDatePattern(bean);
            Assert.AreEqual("2009/04/10 0:00:00", target.DateTimeToString, "test1");
            Console.Out.WriteLine("targetDateTimeToString = {0}", target.DateTimeToString);

            target = dxo.ConvertBeanToTarget1(bean);
            Assert.AreEqual("20090410", target.DateTimeToString, "test2");
            Console.Out.WriteLine("targetDateTimeToString = {0}", target.DateTimeToString);

            target = dxo.ConvertBeanToTargetWithoutDatePattern(bean);
            Assert.AreEqual("2009/04/10 0:00:00", target.DateTimeToString, "test3");
            Console.Out.WriteLine("targetDateTimeToString = {0}", target.DateTimeToString);

            target = dxo.ConvertBeanToTarget2(bean);
            Assert.AreEqual("2009-04-10", target.DateTimeToString, "test4");
            Console.Out.WriteLine("targetDateTimeToString = {0}", target.DateTimeToString);
        }
    }
}