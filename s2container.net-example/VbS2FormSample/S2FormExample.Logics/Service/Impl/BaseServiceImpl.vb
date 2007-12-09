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
Imports Seasar.S2FormExample.Logics.Dao

Namespace Service.Impl
    ''' <summary>
    ''' 基底サービス用実装クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BaseServiceImpl
        Implements IBaseService

        ''' <summary>
        ''' 部門DAO
        ''' </summary>
        ''' <remarks></remarks>
        Protected daoOfDept As IDepartmentDao

        ''' <summary>
        ''' 性別DAO
        ''' </summary>
        ''' <remarks></remarks>
        Protected daoOfGender As IGenderDao

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '
        End Sub

        ''' <summary>
        ''' 部門DAO
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DaoOfDeptProperty() As IDepartmentDao
            Get
                Return daoOfDept
            End Get
            Set(ByVal value As IDepartmentDao)
                daoOfDept = value
            End Set
        End Property

        ''' <summary>
        ''' 性別DAO
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DaoOfGenderProperty() As IGenderDao
            Get
                Return daoOfGender
            End Get
            Set(ByVal value As IGenderDao)
                daoOfGender = value
            End Set
        End Property

        ''' <summary>
        ''' 部門を一覧で取得する
        ''' </summary>
        ''' <returns>部門一覧</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetDepartmentAll() As IList(Of DepartmentDto) _
            Implements IBaseService.GetDepartmentAll
            Return daoOfDept.GetAll
        End Function

        ''' <summary>
        ''' 性別を一覧で取得する
        ''' </summary>
        ''' <returns>性別一覧</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetGenderAll() As IList(Of GenderDto) Implements IBaseService.GetGenderAll
            Return daoOfGender.GetAll
        End Function
    End Class
End Namespace