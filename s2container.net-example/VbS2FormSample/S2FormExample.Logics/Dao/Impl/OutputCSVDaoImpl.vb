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
Imports System.IO
Imports Seasar.S2FormExample.Logics.Dto
Imports System.Text

Namespace Dao.Impl
    ''' <summary>
    ''' CSV出力用DAO実装クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OutputCSVDaoImpl
        Implements IOutputCSVDao

        ''' <summary>
        ''' 社員データを出力する
        ''' </summary>
        ''' <param name="path">出力先パス</param>
        ''' <param name="list">社員データ</param>
        ''' <returns>出力件数</returns>
        ''' <remarks></remarks>
        Public Function OutputEmployeeList(ByVal path As String, ByVal list As IList(Of EmployeeCsvDto)) As Integer _
            Implements IOutputCSVDao.OutputEmployeeList

            If String.IsNullOrEmpty(path) = True Then
                Throw New ArgumentNullException("path")
            End If
            If list Is Nothing Then
                Throw New ArgumentNullException("list")
            End If

            Dim ret As Integer = 0
            Using fs As FileStream = File.Open(path, FileMode.Create)
                Dim writer As New StreamWriter(fs, Encoding.GetEncoding(932))
                Dim builder As New StringBuilder
                Try
                    For Each dto As EmployeeCsvDto In list
                        builder = New StringBuilder
                        builder.Append(Format(dto.Code, """{0}"","))
                        builder.Append(Format(dto.Name, """{0}"","))
                        builder.Append(Format("{0},", dto.Gender))
                        builder.Append(Format(dto.GenderName, """{0}"","))
                        If (dto.EntryDay.HasValue) Then
                            builder.Append(String.Format("""{0:yyyy/M/d}"",", dto.EntryDay.Value))
                        Else
                            builder.Append(""""",")
                        End If
                        builder.Append(Format(dto.DeptCode, """{0}"","))
                        builder.Append(Format(dto.DeptName, """{0}"""))

                        writer.WriteLine(builder.ToString())
                        ret += 1
                    Next
                    writer.Close()
                Catch ex As Exception
                    writer.Close()
                    Throw ex
                End Try
            End Using

            Return ret
        End Function
    End Class
End Namespace