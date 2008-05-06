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
    ''' 性別用DTO
    ''' </summary>
    ''' <remarks></remarks>
    <Table("T_GENDER")> _
    Public Class GenderDto
        Private _id As Integer
        Private _name As String

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ''
        End Sub

        ''' <summary>
        ''' 性別ID
        ''' </summary>
        ''' <remarks></remarks>
        <Column("N_ID")> _
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' 性別名
        ''' </summary>
        ''' <remarks></remarks>
        <Column("S_NAME")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
    End Class
End Namespace