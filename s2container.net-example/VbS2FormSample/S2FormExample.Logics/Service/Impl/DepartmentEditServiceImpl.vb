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
Imports Seasar.S2FormExample.Logics.Dto
Imports Seasar.S2FormExample.Logics.Page
Imports Seasar.Quill.Attrs
Imports Seasar.S2FormExample.Logics.Dao

Namespace Service.Impl
    ''' <summary>
    ''' 部門登録サービス用実装クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DepartmentEditServiceImpl
        Inherits BaseServiceImpl
        Implements IDepartmentEditService

        ''' <summary>
        ''' 部門用DAO
        ''' </summary>
        ''' <remarks></remarks>
        Protected dao As IDepartmentDao

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 部門用DAO
        ''' </summary>
        ''' <remarks>S2Unitでテストするために追加(Injection用)</remarks>
        Public Property DaoProperty() As IDepartmentDao
            Get
                Return dao
            End Get
            Set(ByVal value As IDepartmentDao)
                dao = value
            End Set
        End Property

        ''' <summary>
        ''' 部門を削除する
        ''' </summary>
        ''' <param name="id">部門ID</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        <Aspect("LocalRequiredTx")> _
        Public Function ExecDelete(ByVal id As Integer) As Integer Implements IDepartmentEditService.ExecDelete
            Dim data As New DepartmentDto
            data.Id = id

            Return dao.DeleteData(data)
        End Function

        ''' <summary>
        ''' 部門を登録する
        ''' </summary>
        ''' <param name="dto">登録部門編集Page</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        <Aspect("LocalRequiredTx")> _
        Public Function ExecUpdate(ByVal dto As DepartmentEditPage) As Integer _
            Implements IDepartmentEditService.ExecUpdate

            If dto Is Nothing Then
                Throw New ArgumentNullException
            End If

            Dim data As New DepartmentDto
            data.Code = dto.Code
            data.Id = dto.Id
            data.Name = dto.Name
            data.ShowOrder = Convert.ToInt32(dto.Order)

            If data.Id.HasValue = True Then
                Dim departmentDto As DepartmentDto = dao.GetData(dto.Id.Value)
                If departmentDto Is Nothing = False Then
                    Return dao.UpdateData(data)
                Else
                    Return dao.InsertData(data)
                End If
            Else
                Return dao.InsertData(data)
            End If

        End Function

        ''' <summary>
        ''' 部門編集Pageを取得する
        ''' </summary>
        ''' <param name="id">部門ID</param>
        ''' <returns>部門編集Page</returns>
        ''' <remarks></remarks>
        Public Function GetData(ByVal id As Integer) As DepartmentEditPage Implements IDepartmentEditService.GetData
            Dim page As New DepartmentEditPage

            Dim dto As DepartmentDto = dao.GetData(id)
            If dto Is Nothing = False Then
                page.Code = dto.Code
                page.Id = dto.Id
                page.Name = dto.Name
                page.Order = Convert.ToString(dto.ShowOrder)
            Else
                page = Nothing
            End If

            Return page
        End Function
    End Class
End Namespace