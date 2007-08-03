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
Imports Seasar.Dao.Attrs

Namespace Dto
    ''' <summary>
    ''' 部門用DTO
    ''' </summary>
    ''' <remarks></remarks>
    <Table("T_DEPT")> _
    Public Class DepartmentDto
        Private _code As String
        Private _id As Nullable(Of Integer)
        Private _name As String
        Private _showOrder As Integer

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ''
        End Sub

        ''' <summary>
        ''' 部門コード
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
        ''' 部門ID
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
        ''' 部門名
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
        ''' 表示順番
        ''' </summary>
        ''' <remarks></remarks>
        <Column("n_show_order")> _
        Public Property ShowOrder() As Integer
            Get
                Return _showOrder
            End Get
            Set(ByVal value As Integer)
                _showOrder = value
            End Set
        End Property
    End Class
End Namespace