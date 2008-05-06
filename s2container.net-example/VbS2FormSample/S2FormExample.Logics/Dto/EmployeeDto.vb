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
    ''' 社員用DTO
    ''' </summary>
    ''' <remarks></remarks>
    <Table("T_EMP")> _
    Public Class EmployeeDto
        Private _code As String
        Private _department As DepartmentDto
        Private _deptNo As Nullable(Of Integer)
        Private _entryDay As Nullable(Of DateTime)
        Private _genderId As Integer
        Private _id As Nullable(Of Integer)
        Private _name As String

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _id = Nothing
            _code = ""
            _name = ""
        End Sub

        ''' <summary>
        ''' 社員ID
        ''' </summary>
        ''' <remarks></remarks>
        <Column("n_id")> _
        Public Property Id() As Nullable(Of Integer)
            Get
                Return _id
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _id = value
            End Set
        End Property

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
        ''' 部門ID
        ''' </summary>
        ''' <remarks></remarks>
        <Column("n_dept_id")> _
        Public Property DeptNo() As Nullable(Of Integer)
            Get
                Return _deptNo
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _deptNo = value
            End Set
        End Property

        ''' <summary>
        ''' 部門
        ''' </summary>
        ''' <remarks></remarks>
        <Relno(0), Relkeys("n_dept_id:n_id")> _
        Public Property Department() As DepartmentDto
            Get
                Return _department
            End Get
            Set(ByVal value As DepartmentDto)
                _department = value
            End Set
        End Property

        ''' <summary>
        ''' 部門名
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public ReadOnly Property DeptName() As String
            Get
                Return _department.Name
            End Get
        End Property
    End Class
End Namespace