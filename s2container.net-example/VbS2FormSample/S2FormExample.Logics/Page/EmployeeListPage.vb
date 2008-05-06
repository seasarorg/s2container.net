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
Imports Seasar.S2FormExample.Logics.Dto

Namespace Page
    ''' <summary>
    ''' 社員一覧Pageクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EmployeeListPage
        Private _genderId As String
        Private _genderName As String
        Private _list As IList(Of EmployeeDto)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _list = New List(Of EmployeeDto)
        End Sub

        ''' <summary>
        ''' 性別ID
        ''' </summary>
        ''' <remarks></remarks>
        Public Property GenderId() As String
            Get
                Return _genderId
            End Get
            Set(ByVal value As String)
                _genderId = value
            End Set
        End Property

        ''' <summary>
        ''' 性別名
        ''' </summary>
        ''' <remarks></remarks>
        Public Property GenderName() As String
            Get
                Return _genderName
            End Get
            Set(ByVal value As String)
                _genderName = value
            End Set
        End Property

        ''' <summary>
        ''' 社員リスト
        ''' </summary>
        ''' <remarks></remarks>
        Public Property List() As IList(Of EmployeeDto)
            Get
                Return _list
            End Get
            Set(ByVal value As IList(Of EmployeeDto))
                _list = value
            End Set
        End Property
    End Class
End Namespace