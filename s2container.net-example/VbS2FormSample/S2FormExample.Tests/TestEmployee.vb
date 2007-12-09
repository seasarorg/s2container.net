''
'' Copyright 2005-2007 the Seasar Foundation and the Others.
''
'' Licensed under the Apache License, Version 2.0 (the "License");
'' you may not use this file except in compliance with the License.
'' You may obtain a copy of the License at
''
''     http://www.apache.org/licenses/LICENSE-2.0
''
'' Unless required by applicable law or agreed to in writing, software
'' distributed under the License is distributed on an "AS IS" BASIS,
'' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
'' either express or implied. See the License for the specific language
'' governing permissions and limitations under the License.
''
Imports System.IO
Imports Seasar.S2FormExample.Logics.Page
Imports Seasar.S2FormExample.Logics
Imports Seasar.S2FormExample.Logics.Dto
Imports Seasar.S2FormExample.Logics.Dao
Imports log4net.Config
Imports log4net
Imports MbUnit.Framework
Imports System.Reflection
Imports Seasar.Extension.Unit
Imports log4net.Util
Imports Seasar.S2FormExample.Logics.Service

<TestFixture()> _
Public Class TestEmployee
    Inherits S2TestCase

    ''' <summary>
    ''' Logic設定ファイル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PATH As String = "ExampleLogics.dicon"

    ''' <summary>
    ''' テストのセットアップ
    ''' </summary>
    ''' <remarks></remarks>
    <SetUp()> _
    Public Sub Setup()
        Dim info As FileInfo = _
                New FileInfo(Format(SystemInfo.AssemblyShortName(Assembly.GetExecutingAssembly()), _
                                      "{0}.dll.config"))
        XmlConfigurator.Configure(LogManager.GetRepository(), info)
    End Sub

    ''' <summary>
    ''' 検索系のテスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2()> _
    Public Sub TestSelectOfDao()
        Include(PATH)

        Dim dao As IEmployeeDao = CType(GetComponent(GetType(IEmployeeDao)), IEmployeeDao)
        Assert.IsNotNull(dao, "NotNull")

        ' 一覧で取得する
        Dim list As IList(Of EmployeeDto) = dao.GetAll
        Assert.AreEqual(5, list.Count, "Count")
        Dim i As Integer = 0
        For Each dto As EmployeeDto In list
            If i = 2 Then
                Assert.AreEqual(3, dto.Id.Value, "Id")
                Assert.AreEqual("佐藤愛子", dto.Name, "Name")
                Assert.AreEqual("010003", dto.Code, "Code")
                Dim targetDate As New DateTime(2001, 4, 1, 0, 0, 0)
                Assert.AreEqual(2, dto.Gender, "Gender")
                Assert.AreEqual(targetDate, dto.EntryDay.Value, "Entry")
                Assert.AreEqual(1, dto.DeptNo.Value, "DeptNo")
                Assert.AreEqual(1, dto.Department.Id.Value, "Dept.No")
                Assert.AreEqual("営業部", dto.Department.Name, "Dept.Name")
                Assert.AreEqual("0001", dto.Department.Code, "Dept.Code")

            End If
            i += 1
        Next

        list = dao.FindByGender(1)
        Assert.AreEqual(4, list.Count, "Count2")

        ' 個別に取得する
        Dim data As EmployeeDto = dao.GetData(3)
        Assert.AreEqual(3, data.Id.Value, "Id2")
        Assert.AreEqual("佐藤愛子", data.Name, "Name2")
        Assert.AreEqual("010003", data.Code, "Code2")
        Assert.AreEqual(2, data.Gender, "Gender2")
        Dim targetDate2 As New DateTime(2001, 4, 1, 0, 0, 0)
        Assert.AreEqual(targetDate2, data.EntryDay.Value, "Entry2")
        Assert.AreEqual(1, data.DeptNo.Value, "DeptN2o")
        Assert.AreEqual(1, data.Department.Id.Value, "Dept.No2")
        Assert.AreEqual("営業部", data.Department.Name, "Dept.Name2")
        Assert.AreEqual("0001", data.Department.Code, "Dept.Code2")

        Assert.AreEqual(3, dao.GetId("010003"), "GetId")

    End Sub

    ''' <summary>
    ''' 更新系のテスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2(Tx.Rollback)> _
    Public Sub TestInsertOfDao()
        Include(PATH)

        Dim dao As IEmployeeDao = CType(GetComponent(GetType(IEmployeeDao)), IEmployeeDao)
        Assert.IsNotNull(dao, "NotNull")

        ' 挿入のテスト
        Dim data As New EmployeeDto()
        data.Code = "060006"
        data.Name = "後藤六郎"
        data.EntryDay = New DateTime(2001, 4, 1, 0, 0, 0)
        data.Gender = 1
        data.DeptNo = 1

        Assert.AreEqual(1, dao.InsertData(data), "Insert")

        ' 更新のテスト
        Dim id As Integer = dao.GetId("060006")
        data.Id = id
        data.Code = "060006"
        data.Name = "五島六郎"
        data.EntryDay = New DateTime(2001, 4, 1, 0, 0, 0)
        data.Gender = 1
        data.DeptNo = 2

        Assert.AreEqual(1, dao.UpdateData(data), "Update")

        data = dao.GetData(id)
        Assert.AreEqual(id, data.Id.Value, "Id")
        Assert.AreEqual("五島六郎", data.Name, "Name")
        Assert.AreEqual("060006", data.Code, "Code")
        Assert.AreEqual(1, data.Gender, "Gender2")
        Dim targetDate As New DateTime(2001, 4, 1, 0, 0, 0)
        Assert.AreEqual(targetDate, data.EntryDay.Value, "Entry")
        Assert.AreEqual(2, data.DeptNo.Value, "DeptNo")
        Assert.AreEqual(2, data.Department.Id.Value, "Dept.No2")
        Assert.AreEqual("技術部", data.Department.Name, "Dept.Name2")
        Assert.AreEqual("0002", data.Department.Code, "Dept.Code2")

        ' 削除のテスト
        data = New EmployeeDto()
        data.Id = id

        Assert.AreEqual(1, dao.DeleteData(data), "Delete")

        Dim list As IList(Of EmployeeDto) = dao.GetAll()
        Assert.AreEqual(5, list.Count, "Count")

    End Sub

    ''' <summary>
    ''' CSV用テスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2()> _
    Public Sub TestDaoOfCSV()

        Include(PATH)

        ' 取得のテスト
        Dim dao As IEmployeeCSVDao = CType(GetComponent(GetType(IEmployeeCSVDao)), IEmployeeCSVDao)
        Assert.IsNotNull(dao, "NotNull")

        Dim list As IList(Of EmployeeCsvDto) = dao.GetAll()
        Assert.AreEqual(5, list.Count, "Count")

        Dim i As Integer = 0
        For Each dto As EmployeeCsvDto In list
            If i = 2 Then
                Assert.AreEqual("佐藤愛子", dto.Name, "Name")
                Assert.AreEqual("010003", dto.Code, "Code")
                Assert.AreEqual(2, dto.Gender, "Gender")
                Assert.AreEqual("女性", dto.GenderName, "GenderName")
                Dim targetDate As New DateTime(2001, 4, 1, 0, 0, 0)
                Assert.AreEqual(targetDate, dto.EntryDay.Value, "Entry")
                Assert.AreEqual("営業部", dto.DeptName, "Dept.Name")
                Assert.AreEqual("0001", dto.DeptCode, "Dept.Code")
            End If
            i += 1
        Next

        ' 出力のテスト
        Dim filepath As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\csvtest.csv"
        Dim daoOfCsv As IOutputCSVDao = CType(GetComponent(GetType(IOutputCSVDao)), IOutputCSVDao)
        Assert.AreEqual(5, daoOfCsv.OutputEmployeeList(filepath, list))

    End Sub

    ''' <summary>
    ''' 社員リストサービステスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2()> _
    Public Sub TestListService()
        Include(PATH)

        Dim service As IEmployeeListService = CType(GetComponent(GetType(IEmployeeListService)), IEmployeeListService)
        Assert.IsNotNull(service, "NotNull")

        Dim page As EmployeeListPage = service.GetAll()
        Assert.AreEqual(5, page.List.Count, "Count")
        Dim i As Integer = 0
        For Each dto As EmployeeDto In page.List
            If i = 2 Then
                Assert.AreEqual(3, dto.Id.Value, "Id")
                Assert.AreEqual("佐藤愛子", dto.Name, "Name")
                Assert.AreEqual("010003", dto.Code, "Code")
                Assert.AreEqual(2, dto.Gender, "Gender")
                Dim targetDate As Date = New Date(2001, 4, 1, 0, 0, 0)
                Assert.AreEqual(targetDate, dto.EntryDay.Value, "Entry")
                Assert.AreEqual(1, dto.DeptNo.Value, "DeptNo")
                Assert.AreEqual(1, dto.Department.Id.Value, "Dept.No")
                Assert.AreEqual("営業部", dto.Department.Name, "Dept.Name")
                Assert.AreEqual("0001", dto.Department.Code, "Dept.Code")
                Assert.AreEqual("営業部", dto.DeptName, "DeptName")
            End If
            i += 1
        Next
    End Sub

    ''' <summary>
    ''' 社員登録サービスのテスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2(Tx.Rollback)> _
    Public Sub TestEditService()
        Include(PATH)

        Dim service As IEmployeeEditService = CType(GetComponent(GetType(IEmployeeEditService)), IEmployeeEditService)
        Assert.IsNotNull(service, "NotNull")

        Dim data As EmployeeEditPage = service.GetData(3)
        Assert.AreEqual(3, data.Id.Value, "Id")
        Assert.AreEqual("佐藤愛子", data.Name, "Name")
        Assert.AreEqual("010003", data.Code, "Code")
        Assert.AreEqual(2, data.Gender, "Gender")
        Dim targetDate As Date = New Date(2001, 4, 1, 0, 0, 0)
        Assert.AreEqual(targetDate, data.Entry, "Entry")
        Assert.AreEqual(1, data.Depart, "DeptNo")

        '' 挿入のテスト
        data = New EmployeeEditPage()
        data.Code = "060006"
        data.Name = "後藤六郎"
        data.Entry = New DateTime(2006, 4, 1, 0, 0, 0)
        data.Gender = 1
        data.Depart = 1

        Assert.AreEqual(1, service.ExecUpdate(data), "Insert")

        '' 更新のテスト
        data = New EmployeeEditPage()
        data.Id = 2
        data.Code = "999999"
        data.Name = "鈴木二郎"
        data.Entry = New DateTime(1999, 5, 1, 0, 0, 0)
        data.Gender = 1
        data.Depart = 2

        Assert.AreEqual(1, service.ExecUpdate(data), "Update")

        data = service.GetData(2)
        Assert.AreEqual(2, data.Id.Value, "Id")
        Assert.AreEqual("鈴木二郎", data.Name, "Name")
        Assert.AreEqual("999999", data.Code, "Code")
        Assert.AreEqual(1, data.Gender, "Gender")
        targetDate = New Date(1999, 5, 1, 0, 0, 0)
        Assert.AreEqual(targetDate, data.Entry, "Entry")
        Assert.AreEqual(2, data.Depart, "DeptNo")

    End Sub
End Class