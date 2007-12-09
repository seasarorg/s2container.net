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
Imports Seasar.S2FormExample.Logics.Service.Impl
Imports Seasar.Quill.Attrs
Imports Seasar.S2FormExample.Logics.Dto

Namespace Service
    ''' <summary>
    ''' 基底サービスインターフェイス
    ''' </summary>
    ''' <remarks></remarks>
    <Implementation(GetType(BaseServiceImpl))> _
    Public Interface IBaseService
        ''' <summary>
        ''' 部門を一覧で取得する
        ''' </summary>
        ''' <returns>部門一覧</returns>
        ''' <remarks></remarks>
        Function GetDepartmentAll() As IList(Of DepartmentDto)

        ''' <summary>
        ''' 性別を一覧で取得する
        ''' </summary>
        ''' <returns>性別一覧</returns>
        ''' <remarks></remarks>
        Function GetGenderAll() As IList(Of GenderDto)
    End Interface
End Namespace