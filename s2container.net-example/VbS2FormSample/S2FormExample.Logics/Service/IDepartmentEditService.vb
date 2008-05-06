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
    ''' 部門登録サービス用インターフェイス
    ''' </summary>
    ''' <remarks></remarks>
    <Implementation(GetType(DepartmentEditServiceImpl))> _
    Public Interface IDepartmentEditService
        Inherits IBaseService

        ''' <summary>
        ''' 部門データを取得する
        ''' </summary>
        ''' <param name="id">部門ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetData(ByVal id As Integer) As DepartmentEditPage

        ''' <summary>
        ''' 部門を登録する
        ''' </summary>
        ''' <param name="dto">録部門編集Page</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Function ExecUpdate(ByVal dto As DepartmentEditPage) As Integer

        ''' <summary>
        ''' 部門を削除する
        ''' </summary>
        ''' <param name="id">部門ID</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Function ExecDelete(ByVal id As Integer) As Integer
    End Interface
End Namespace