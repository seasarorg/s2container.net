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
Namespace Page
    ''' <summary>
    ''' 社員編集Pageクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EmployeeEditPage
        Private _id As Nullable(Of Integer)
        Private _code As String
        Private _name As String
        Private _gender As Integer
        Private _entry As Nullable(Of DateTime)
        Private _depart As Integer

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _entry = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
        End Sub

        ''' <summary>
        ''' 社員ID
        ''' </summary>
        ''' <remarks></remarks>
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
        Public Property Gender() As Integer
            Get
                Return _gender
            End Get
            Set(ByVal value As Integer)
                _gender = value
            End Set
        End Property

        ''' <summary>
        ''' 入社日
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Entry() As Nullable(Of Date)
            Get
                Return _entry
            End Get
            Set(ByVal value As Nullable(Of Date))
                _entry = value
            End Set
        End Property

        ''' <summary>
        ''' 部門ID
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Depart() As Integer
            Get
                Return _depart
            End Get
            Set(ByVal value As Integer)
                _depart = value
            End Set
        End Property
    End Class
End Namespace