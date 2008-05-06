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
Imports Seasar.S2FormExample.Logics.Service.Impl
Imports Seasar.Quill.Attrs
Imports Seasar.S2FormExample.Logics.Page

Namespace Service
    ''' <summary>
    ''' 社員リストサービス用インターフェイス
    ''' </summary>
    ''' <remarks></remarks>
    <Implementation(GetType(EmployeeListServiceImpl))> _
    Public Interface IEmployeeListService
        ''' <summary>
        ''' 社員一覧を取得する
        ''' </summary>
        ''' <returns>社員一覧</returns>
        ''' <remarks></remarks>
        Function GetAll() As EmployeeListPage

        ''' <summary>
        ''' 社員一覧を検索する
        ''' </summary>
        ''' <param name="condition">検索条件</param>
        ''' <returns>社員一覧</returns>
        ''' <remarks></remarks>
        Function Find(ByVal condition As EmployeeListPage) As EmployeeListPage

        ''' <summary>
        ''' CSVで出力する
        ''' </summary>
        ''' <param name="path">出力先パス</param>
        ''' <returns>出力件数</returns>
        ''' <remarks></remarks>
        Function OutputCSV(ByVal path As String) As Integer
    End Interface
End Namespace