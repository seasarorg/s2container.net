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
''' 部門一覧画面
''' </summary>
''' <remarks></remarks>
<ControlModifier("Txt", "")> _
    <Control("GridList", "DataSource", "List")> _
Public Class FrmDepartmentList
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
    ''' 部門一覧サービス
    ''' </summary>
    ''' <remarks></remarks>
    Protected service As IDepartmentListService

    ''' <summary>
    ''' 新規ボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnNew.Click
        Try
            dispatcher.ShowMasterEdit(Nothing)
            Me.DataSource = service.GetAll
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
    ''' フォームをロードしたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FrmDepartmentList_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            logger.InfoFormat("{0}がロードされました", Name)

            Me.DataSource = service.GetAll()
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' GridViewを初期化する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _InitializeGridView()
        GridList.RowCount = 0
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
            Dim id As Nullable(Of Integer) = (CType(Me.DataSource, DepartmentListPage)).List(index).Id
            dispatcher.ShowMasterEdit(id)

            Me.DataSource = service.GetAll
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
End Class