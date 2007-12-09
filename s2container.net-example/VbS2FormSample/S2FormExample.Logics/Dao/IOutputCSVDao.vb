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
Imports Seasar.Quill.Attrs
Imports Seasar.S2FormExample.Logics.Dao.Impl
Imports Seasar.S2FormExample.Logics.Dto

Namespace Dao
    ''' <summary>
    ''' CSV出力用DAOインターフェイス
    ''' </summary>
    ''' <remarks></remarks>
    <Implementation(GetType(OutputCSVDaoImpl))> _
    Public Interface IOutputCSVDao
        ''' <summary>
        ''' 社員データを出力する
        ''' </summary>
        ''' <param name="path">出力先パス</param>
        ''' <param name="list">社員データ</param>
        ''' <returns>出力件数</returns>
        ''' <remarks></remarks>
        Function OutputEmployeeList(ByVal path As String, ByVal list As IList(Of EmployeeCsvDto)) As Integer
    End Interface
End Namespace