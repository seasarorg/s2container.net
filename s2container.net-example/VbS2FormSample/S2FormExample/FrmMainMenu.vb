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

''' <summary>
''' メインメニュー画面
''' </summary>
''' <remarks></remarks>
Public Class FrmMainMenu

    ''' <summary>
    ''' 画面ディスパッチャー
    ''' </summary>
    ''' <remarks></remarks>
    Protected dispatcher As IFormDispatcher

    ''' <summary>
    ''' フォームを閉じる
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        Close()
    End Sub

    ''' <summary>
    ''' 社員一覧を表示する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnEmployee_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnEmployee.Click
        Try
            dispatcher.ShowDataList()
        Catch ex As Exception
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 部門一覧を表示する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDepartment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnDepartment.Click
        Try
            dispatcher.ShowMasterList()
        Catch ex As Exception
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' フォームを閉じる
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FrmMainMenu_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) _
        Handles MyBase.FormClosing

        If MessageBox.Show("本当に終了しますか?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
           = DialogResult.No Then
            e.Cancel = True
        End If

    End Sub
End Class