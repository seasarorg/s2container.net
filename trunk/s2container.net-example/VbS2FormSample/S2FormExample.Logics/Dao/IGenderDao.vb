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
    ''' «•Ê—pDAO
    ''' </summary>
    ''' <remarks></remarks>
    <Implementation()> _
        <Aspect("DaoInterceptor")> _
        <Bean(GetType(GenderDto))> _
    Public Interface IGenderDao
        ''' <summary>
        ''' «•Ê‚ğˆê——‚Åæ“¾‚·‚é
        ''' </summary>
        ''' <returns>«•Êˆê——</returns>
        ''' <remarks></remarks>
        Function GetAll() As IList(Of GenderDto)
    End Interface
End Namespace