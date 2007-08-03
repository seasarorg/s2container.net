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
    ''' 部門用DAO
    ''' </summary>
    ''' <remarks></remarks>
    <Implementation()> _
        <Aspect("DaoInterceptor")> _
        <Bean(GetType(DepartmentDto))> _
    Public Interface IDepartmentDao
        ''' <summary>
        ''' 部門一覧を取得する
        ''' </summary>
        ''' <returns>部門リスト</returns>
        ''' <remarks></remarks>
        <Query("order by n_show_order")> _
        Function GetAll() As IList(Of DepartmentDto)

        ''' <summary>
        ''' 部門データを取得する
        ''' </summary>
        ''' <param name="id">部門ID</param>
        ''' <returns>部門データ</returns>
        ''' <remarks></remarks>
        <Query("n_id = /*id*/1")> _
        Function GetData(ByVal id As Integer) As DepartmentDto

        ''' <summary>
        ''' 部門IDを取得する
        ''' </summary>
        ''' <param name="code">部門コード</param>
        ''' <returns>部門ID</returns>
        ''' <remarks></remarks>
        <Sql("select n_id from t_dept where s_code = /*code*/'0002'")> _
        Function GetId(ByVal code As String) As Integer

        ''' <summary>
        ''' 部門を挿入する
        ''' </summary>
        ''' <param name="dto">挿入するデータ</param>
        ''' <returns>挿入件数</returns>
        ''' <remarks></remarks>
        <NoPersistentProps("Id")> _
        Function InsertData(ByVal dto As DepartmentDto) As Integer

        ''' <summary>
        ''' 部門を更新する
        ''' </summary>
        ''' <param name="dto">更新データ</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Function UpdateData(ByVal dto As DepartmentDto) As Integer

        ''' <summary>
        ''' 部門を削除する
        ''' </summary>
        ''' <param name="dto">削除データ</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Function DeleteData(ByVal dto As DepartmentDto) As Integer

    End Interface
End Namespace