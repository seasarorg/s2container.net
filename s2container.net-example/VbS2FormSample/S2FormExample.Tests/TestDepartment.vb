''
'' Copyright 2005-2008 the Seasar Foundation and the Others.
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
Imports System.Reflection
Imports Seasar.S2FormExample.Logics.Page
Imports Seasar.S2FormExample.Logics.Dto
Imports log4net.Config
Imports log4net
Imports Seasar.S2FormExample.Logics.Dao
Imports MbUnit.Framework
Imports Seasar.Quill.Unit
Imports Seasar.S2FormExample.Logics.Service
Imports Seasar.Extension.Unit
Imports log4net.Util

''' <summary>
''' 部門用テストケースクラス
''' </summary>
''' <remarks></remarks>
<TestFixture()> _
Public Class TestDepartment
    Inherits QuillTestCase

    Protected daoOfDepartment As IDepartmentDao
    Protected editService As IDepartmentEditService
    Protected listService As IDepartmentListService

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
    <Test(), Quill(Tx.Rollback)> _
    Public Sub TestSelectOfDao()

        ' 一覧のテスト
        Dim list As IList(Of DepartmentDto) = daoOfDepartment.GetAll()
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
        Dim data As DepartmentDto = daoOfDepartment.GetData(2)
        Assert.AreEqual(2, data.Id.Value, "ID")
        Assert.AreEqual("0002", data.Code, "Code")
        Assert.AreEqual("技術部", data.Name, "Name")
        Assert.AreEqual(2, data.ShowOrder, "Order")

        Assert.AreEqual(2, daoOfDepartment.GetId("0002"), "GetId")

    End Sub

    ''' <summary>
    ''' 更新系のテスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), Quill(Tx.Rollback)> _
    Public Sub TestInsertOfDao()
        ' 挿入のテスト
        Dim data As New DepartmentDto
        data.Code = "0102"
        data.Name = "管理部"
        data.ShowOrder = 4

        Assert.AreEqual(1, daoOfDepartment.InsertData(data), "Insert")

        ' 更新のテスト
        Dim id As Integer = daoOfDepartment.GetId("0102")
        data = New DepartmentDto()
        data.Code = "0102"
        data.Id = id
        data.Name = "事業管理部"
        data.ShowOrder = 4

        Assert.AreEqual(1, daoOfDepartment.UpdateData(data), "Update")

        data = daoOfDepartment.GetData(id)
        Assert.AreEqual(id, data.Id.Value, "ID")
        Assert.AreEqual("0102", data.Code, "Code")
        Assert.AreEqual("事業管理部", data.Name, "Name")
        Assert.AreEqual(4, data.ShowOrder, "Order")

        ' 削除のテスト
        data = New DepartmentDto()
        data.Id = id
        Assert.AreEqual(1, daoOfDepartment.DeleteData(data), "Delete")

        Dim list As IList(Of DepartmentDto) = daoOfDepartment.GetAll()
        Assert.AreEqual(3, list.Count, "Count")

    End Sub

    ''' <summary>
    ''' 部門リストサービステスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), Quill(Tx.Rollback)> _
    Public Sub TestListService()
        Dim page As DepartmentListPage = listService.GetAll

        Assert.AreEqual(3, page.List.Count, "Count")
    End Sub

    ''' <summary>
    ''' 部門登録用サービステスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), Quill(Tx.Rollback)> _
    Public Sub TestEditService()

        Dim data As DepartmentEditPage = editService.GetData(2)
        Assert.AreEqual(2, data.Id.Value, "ID")
        Assert.AreEqual("0002", data.Code, "Code")
        Assert.AreEqual("技術部", data.Name, "Name")
        Assert.AreEqual("2", data.Order, "Order")

        ' 挿入のテスト
        data = New DepartmentEditPage()
        data.Code = "0102"
        data.Name = "管理部"
        data.Order = "4"

        Assert.AreEqual(1, editService.ExecUpdate(data), "Insert")

        ' 更新のテスト
        data = New DepartmentEditPage()
        data.Id = 2
        data.Code = "0020"
        data.Name = "技術事業部"
        data.Order = "5"

        Assert.AreEqual(1, editService.ExecUpdate(data), "Update")

        data = editService.GetData(2)
        Assert.AreEqual(2, data.Id.Value, "ID")
        Assert.AreEqual("0020", data.Code, "Code")
        Assert.AreEqual("技術事業部", data.Name, "Name")
        Assert.AreEqual("5", data.Order, "Order")

    End Sub
End Class