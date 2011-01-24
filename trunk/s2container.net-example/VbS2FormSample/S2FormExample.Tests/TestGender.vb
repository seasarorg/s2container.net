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
''
Imports System.IO
Imports System.Reflection
Imports Seasar.S2FormExample.Logics.Dto
Imports log4net.Config
Imports log4net
Imports Seasar.S2FormExample.Logics.Dao
Imports MbUnit.Framework
Imports Seasar.Quill.Unit
Imports Seasar.Extension.Unit
Imports log4net.Util

''' <summary>
'''  性別用テストケースクラス
''' </summary>
''' <remarks></remarks>
<TestFixture()> _
Public Class TestGender
    Inherits QuillTestCase

    Protected dao As IGenderDao

    ''' <summary>
    ''' テストのセットアップ
    ''' </summary>
    ''' <remarks></remarks>
    <SetUp()> _
    Public Sub Setup()
        Dim info As FileInfo = _
                New FileInfo(Format(SystemInfo.AssemblyShortName(Assembly.GetExecutingAssembly()), _
                                      "{0}.dll.config"))
        XmlConfigurator.Configure(LogManager.GetRepository(), info)
    End Sub

    ''' <summary>
    ''' DAOのテスト
    ''' </summary>
    ''' <remarks></remarks>
    <Test(), Quill(Tx.Rollback)> _
    Public Sub TestDao()

        Dim list As IList(Of GenderDto) = dao.GetAll()
        Assert.AreEqual(2, list.Count, "Count")
    End Sub
End Class