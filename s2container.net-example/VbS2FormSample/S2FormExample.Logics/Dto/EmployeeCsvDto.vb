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
Imports Seasar.Dao.Attrs

Namespace Dto
    ''' <summary>
    ''' CSV用社員DTO
    ''' </summary>
    ''' <remarks></remarks>
    <Table("T_EMP")> _
    Public Class EmployeeCsvDto
        Private _code As String
        Private _name As String
        Private _genderId As Integer
        Private _genderName As String
        Private _entryDay As Nullable(Of DateTime)
        Private _deptCode As String
        Private _deptName As String

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _code = ""
            _name = ""
            _entryDay = Nothing
            _deptCode = ""
            _deptName = ""
        End Sub

        ''' <summary>
        ''' 社員コード
        ''' </summary>
        ''' <remarks></remarks>
        <Column("s_code")> _
        Public Property Code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        ''' <summary>
        ''' 社員名
        ''' </summary>
        ''' <remarks></remarks>
        <Column("s_name")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        ''' <summary>
        ''' 性別ID
        ''' </summary>
        ''' <remarks></remarks>
        <Column("n_gender")> _
        Public Property Gender() As Integer
            Get
                Return _genderId
            End Get
            Set(ByVal value As Integer)
                _genderId = value
            End Set
        End Property

        ''' <summary>
        ''' 性別名
        ''' </summary>
        ''' <remarks></remarks>
        <Column("s_gender_name")> _
        Public Property GenderName() As String
            Get
                Return _genderName
            End Get
            Set(ByVal value As String)
                _genderName = value
            End Set
        End Property

        ''' <summary>
        ''' 入社日
        ''' </summary>
        ''' <remarks></remarks>
        <Column("d_entry")> _
        Public Property EntryDay() As Nullable(Of Date)
            Get
                Return _entryDay
            End Get
            Set(ByVal value As Nullable(Of Date))
                _entryDay = value
            End Set
        End Property

        ''' <summary>
        ''' 部門コード
        ''' </summary>
        ''' <remarks></remarks>
        <Column("s_dept_code")> _
        Public Property DeptCode() As String
            Get
                Return _deptCode
            End Get
            Set(ByVal value As String)
                _deptCode = value
            End Set
        End Property

        ''' <summary>
        ''' 部門名
        ''' </summary>
        ''' <remarks></remarks>
        <Column("s_dept_name")> _
        Public Property DeptName() As String
            Get
                Return _deptName
            End Get
            Set(ByVal value As String)
                _deptName = value
            End Set
        End Property
    End Class
End Namespace