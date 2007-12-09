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
Imports Seasar.S2FormExample.Logics.Dto
Imports Seasar.S2FormExample.Logics.Dao
Imports log4net.Config
Imports log4net
Imports MbUnit.Framework
Imports System.Reflection
Imports Seasar.Extension.Unit
Imports log4net.Util
Imports Seasar.S2FormExample.Logics.Service

''' <summary>
''' 部門用テストケースクラス
''' </summary>
''' <remarks></remarks>
<TestFixture()> _
Public Class TestDepartment
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

        Dim dao As IDepartmentDao = CType(GetComponent(GetType(IDepartmentDao)), IDepartmentDao)
        Assert.IsNotNull(dao, "NotNull")

        ' 一覧のテスト
        Dim list As IList(Of DepartmentDto) = dao.GetAll()
        Assert.AreEqual(3, list.Count, "Count")
        Dim i As Integer = 0
        For Each dto As DepartmentDto In list
            If i = 1 Then
                Assert.AreEqual(2, dto.Id.Value, "ID")
                Assert.AreEqual("0002", dto.Code, "Code")
                Assert.AreEqual("技術部", dto.Name, "Name")
                Assert.AreEqual(2, dto.ShowOrder, "Order")
            End If

            i += 1
        Next

        ' 個別取得のテスト
        Dim data As DepartmentDto = dao.GetData(2)
        Assert.AreEqual(2, data.Id.Value, "ID")
        Assert.AreEqual("0002", data.Code, "Code")
        Assert.AreEqual("技術部", data.Name, "Name")
        Assert.AreEqual(2, data.ShowOrder, "Order")

        Assert.AreEqual(2, dao.GetId("0002"), "GetId")

    End Sub

    ''' <summary>
    ''' 更新系のテスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2(Tx.Rollback)> _
    Public Sub TestInsertOfDao()
        Include(PATH)

        Dim dao As IDepartmentDao = CType(GetComponent(GetType(IDepartmentDao)), IDepartmentDao)
        Assert.IsNotNull(dao, "NotNull")

        ' 挿入のテスト
        Dim data As New DepartmentDto
        data.Code = "0102"
        data.Name = "管理部"
        data.ShowOrder = 4

        Assert.AreEqual(1, dao.InsertData(data), "Insert")

        ' 更新のテスト
        Dim id As Integer = dao.GetId("0102")
        data = New DepartmentDto()
        data.Code = "0102"
        data.Id = id
        data.Name = "事業管理部"
        data.ShowOrder = 4

        Assert.AreEqual(1, dao.UpdateData(data), "Update")

        data = dao.GetData(id)
        Assert.AreEqual(id, data.Id.Value, "ID")
        Assert.AreEqual("0102", data.Code, "Code")
        Assert.AreEqual("事業管理部", data.Name, "Name")
        Assert.AreEqual(4, data.ShowOrder, "Order")

        ' 削除のテスト
        data = New DepartmentDto()
        data.Id = id
        Assert.AreEqual(1, dao.DeleteData(data), "Delete")

        Dim list As IList(Of DepartmentDto) = dao.GetAll()
        Assert.AreEqual(3, list.Count, "Count")

    End Sub

    ''' <summary>
    ''' 部門リストサービステスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2()> _
    Public Sub TestListService()
        Include(PATH)

        Dim service As IDepartmentListService = CType(GetComponent(GetType(IDepartmentListService)), IDepartmentListService)
        Dim page As DepartmentListPage = service.GetAll

        Assert.AreEqual(3, page.List.Count, "Count")

    End Sub

    ''' <summary>
    ''' 部門登録用サービステスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), S2(Tx.Rollback)> _
    Public Sub TestEditService()
        Include(PATH)

        Dim service As IDepartmentEditService = CType(GetComponent(GetType(IDepartmentEditService)), IDepartmentEditService)
        Assert.IsNotNull(service, "NotNull")

        Dim data As DepartmentEditPage = service.GetData(2)
        Assert.AreEqual(2, data.Id.Value, "ID")
        Assert.AreEqual("0002", data.Code, "Code")
        Assert.AreEqual("技術部", data.Name, "Name")
        Assert.AreEqual("2", data.Order, "Order")

        ' 挿入のテスト
        data = New DepartmentEditPage()
        data.Code = "0102"
        data.Name = "管理部"
        data.Order = "4"

        Assert.AreEqual(1, service.ExecUpdate(data), "Insert")

        ' 更新のテスト
        data = New DepartmentEditPage()
        data.Id = 2
        data.Code = "0020"
        data.Name = "技術事業部"
        data.Order = "5"

        Assert.AreEqual(1, service.ExecUpdate(data), "Update")

        data = service.GetData(2)
        Assert.AreEqual(2, data.Id.Value, "ID")
        Assert.AreEqual("0020", data.Code, "Code")
        Assert.AreEqual("技術事業部", data.Name, "Name")
        Assert.AreEqual("5", data.Order, "Order")

    End Sub
End Class