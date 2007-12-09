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
Imports log4net
Imports System.Reflection
Imports Seasar.S2FormExample.Logics.Page
Imports Seasar.Windows.Attr
Imports Seasar.S2FormExample.Logics.Service

''' <summary>
''' 社員一覧画面
''' </summary>
''' <remarks></remarks>
<ControlModifier("Txt", "")> _
    <Control("gridList", "DataSource", "List")> _
    <Control("lblGenderName", "Text", "GenderName")> _
    <Control("txtGenderId", "Text", "GenderId", DataSourceUpdateMode.OnPropertyChanged)> _
Public Class FrmEmployeeList
    ''' <summary>
    ''' 例外エラーメッセージ書式
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCEPTION_MSG_FORMAT As String = "予期できないエラーが発生しました。詳細を確認してください。（{0}）"

    ''' <summary>
    ''' ログ(log4net)
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    ''' <summary>
    ''' 画面ディスパッチャー
    ''' </summary>
    ''' <remarks></remarks>
    Protected dispatcher As IFormDispatcher

    ''' <summary>
    ''' 社員一覧サービス
    ''' </summary>
    ''' <remarks></remarks>
    Protected service As IEmployeeListService

    ''' <summary>
    ''' 新規ボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnNew.Click
        Try
            dispatcher.ShowDataEdit(Nothing)

            Me.DataSource = service.GetAll
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 出力ボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnOutput_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnOutput.Click
        Try
            MessageBox.Show("保存先を指定してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)

            _InitializeSaveDialog()

            If dlgSave.ShowDialog(Me) = DialogResult.OK Then
                If service.OutputCSV(dlgSave.FileName) > 0 Then
                    MessageBox.Show("出力しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("出力するデータがありませんでした", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 閉じるボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        logger.InfoFormat("{0}を終了", Name)
        Close()
    End Sub

    ''' <summary>
    ''' グリッドをダブルクリックしたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GridList_CellDoubleClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
        Handles GridList.CellDoubleClick
        Try
            Dim index As Integer = GridList.CurrentRow.Index
            Dim id As Nullable(Of Integer) = (CType(Me.DataSource, EmployeeListPage)).List(index).Id
            dispatcher.ShowDataEdit(id)

            Me.DataSource = service.GetAll
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' フォームをロードしたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FrmEmployeeList_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            logger.InfoFormat("{0}がロードされました", Name)

            Me.DataSource = service.GetAll()
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 保存ダイアログを初期化する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _InitializeSaveDialog()
        dlgSave.DefaultExt = "*.csv"
        dlgSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        dlgSave.Title = "CSV出力"
        dlgSave.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル (*.*)|*.*"
        dlgSave.AddExtension = True
        dlgSave.OverwritePrompt = True
        dlgSave.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\社員一覧.csv"
        dlgSave.RestoreDirectory = True
    End Sub

    ''' <summary>
    ''' 性別IDからフォーカスが外れたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TxtGenderId_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles TxtGenderId.Leave
        Try
            Dim page As EmployeeListPage = CType(Me.DataSource, EmployeeListPage)
            If String.IsNullOrEmpty(page.GenderId) = True Then
                MessageBox.Show("性別を入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            Else
                If page.GenderId <> "01" And page.GenderId <> "02" And page.GenderId <> "99" Then
                    MessageBox.Show("性別を正しく入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return
                End If
            End If

            Me.DataSource = service.Find(page)
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
End Class
