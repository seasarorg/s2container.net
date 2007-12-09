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
Imports Seasar.Dao.Attrs
Imports Seasar.Quill.Attrs

Namespace Dao
    ''' <summary>
    ''' 社員用DAO
    ''' </summary>
    ''' <remarks></remarks>
    <Implementation()> _
    <Aspect("DaoInterceptor")> _
    <Bean(GetType(EmployeeDto))> _
    Public Interface IEmployeeDao
        ''' <summary>
        ''' 社員一覧を取得する
        ''' </summary>
        ''' <returns>社員一覧</returns>
        ''' <remarks></remarks>
        <Query("order by t_emp.n_id")> _
        Function GetAll() As IList(Of EmployeeDto)

        ''' <summary>
        ''' 社員データを取得する
        ''' </summary>
        ''' <param name="id">社員ID</param>
        ''' <returns>社員データ</returns>
        ''' <remarks></remarks>
        <Query("t_emp.n_id = /*id*/1")> _
        Function GetData(ByVal id As Integer) As EmployeeDto

        ''' <summary>
        ''' 社員IDを取得する
        ''' </summary>
        ''' <param name="code">社員コード</param>
        ''' <returns>社員ID</returns>
        ''' <remarks></remarks>
        <Sql("select n_id from t_emp where s_code = /*code*/'000001'")> _
        Function GetId(ByVal code As String) As Integer

        ''' <summary>
        ''' 性別で検索する
        ''' </summary>
        ''' <param name="gender">性別ID</param>
        ''' <returns>社員一覧</returns>
        ''' <remarks></remarks>
        <Query("n_gender = /*gender*/1")> _
        Function FindByGender(ByVal gender As Integer) As IList(Of EmployeeDto)

        ''' <summary>
        ''' 社員データを挿入する
        ''' </summary>
        ''' <param name="data">挿入するデータ</param>
        ''' <returns>挿入件数</returns>
        ''' <remarks></remarks>
        <NoPersistentProps("Id")> _
        Function InsertData(ByVal data As EmployeeDto) As Integer

        ''' <summary>
        ''' 社員データを更新する
        ''' </summary>
        ''' <param name="data">更新するデータ</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function UpdateData(ByVal data As EmployeeDto) As Integer

        ''' <summary>
        ''' 社員データを削除する
        ''' </summary>
        ''' <param name="data">社員データ</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Function DeleteData(ByVal data As EmployeeDto) As Integer

    End Interface
End Namespace