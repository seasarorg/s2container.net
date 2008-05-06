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
Imports System.Text.RegularExpressions
Imports Seasar.S2FormExample.Logics.Dto
Imports log4net
Imports System.Reflection
Imports Seasar.S2FormExample.Logics.Page
Imports Seasar.Windows.Attr
Imports Seasar.S2FormExample.Logics.Service

''' <summary>
''' 社員登録画面
''' </summary>
''' <remarks></remarks>
<ControlModifier("Txt", "")> _
    <Control("cmbGender", "SelectedValue", "Gender")> _
    <Control("dtpEntry", "Value", "Entry")> _
    <Control("cmbDepart", "SelectedValue", "Depart")> _
Public Class FrmEmployeeEdit
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
    ''' 社員ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _id As Nullable(Of Integer)

    ''' <summary>
    ''' 画面登録用サービス
    ''' </summary>
    ''' <remarks></remarks>
    Protected service As IEmployeeEditService

    ''' <summary>
    ''' 社員ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Id() As Nullable(Of Integer)
        Get
            Return _id
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _id = value
        End Set
    End Property


    ''' <summary>
    ''' フォームをロードしたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FrmEmployeeEdit_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        logger.InfoFormat("{0}がロードされました", Name)

        Try
            _InitializeControls()

            If _id.HasValue = True Then
                Dim data As EmployeeEditPage = service.GetData(_id.Value)
                If data Is Nothing = False Then
                    Me.DataSource = data
                    BtnDelete.Enabled = True
                Else
                    Me.DataSource = Nothing
                    Throw New ApplicationException("社員データが見つかりませんでした")
                End If
            Else
                Me.DataSource = New EmployeeEditPage
            End If
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 登録ボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnUpdate.Click
        Try
            If MessageBox.Show("本当に登録しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
               = DialogResult.No Then
                Return
            End If

            If _SetInputData() = False Then
                Return
            End If

            Dim data As EmployeeEditPage = CType(Me.DataSource, EmployeeEditPage)
            data.Id = _id
            If service.ExecUpdate(data) > 0 Then
                _InitializeControls()
                MessageBox.Show("登録しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Throw New ApplicationException("登録に失敗しました")
            End If
        Catch ex As Exception
            logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message)
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 削除ボタンを押したときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnDelete.Click
        Try
            If MessageBox.Show("本当に削除しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
               = DialogResult.No Then
                Return
            End If

            If _id.HasValue = True Then
                If service.ExecDelete(_id.Value) > 0 Then
                    MessageBox.Show("削除しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Close()
                Else
                    Throw New ApplicationException("削除に失敗しました")
                End If
            Else
                MessageBox.Show("削除対象を選んでください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
    ''' コントロールを初期化する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _InitializeControls()
        TxtCode.Text = ""
        TxtName.Text = ""
        DtpEntry.Value = DateTime.Today

        _InitializeGenderBox()
        _InializeDepartmentBox()

        BtnDelete.Enabled = False
    End Sub

    ''' <summary>
    ''' 性別コンボボックスを初期化する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _InitializeGenderBox()
        Dim list As IList(Of GenderDto) = service.GetGenderAll

        cmbGender.DataSource = list
        cmbGender.ValueMember = "Id"
        cmbGender.DisplayMember = "Name"
        cmbGender.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' 部門コンボボックスを初期化する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _InializeDepartmentBox()
        Dim list As IList(Of DepartmentDto) = service.GetDepartmentAll

        CmbDepart.DataSource = list
        CmbDepart.ValueMember = "Id"
        CmbDepart.DisplayMember = "Name"
        CmbDepart.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' 入力データをチェックする
    ''' </summary>
    ''' <remarks></remarks>
    Private Function _SetInputData() As Boolean
        Dim ret As Boolean = True

        '' コントロールからDataSourceでバインドしたオブジェクトへ反映。ControlAttributeでも可能。
        Validate()
        ' 社員コード
        If String.IsNullOrEmpty(TxtCode.Text) = False Then
            If Regex.IsMatch(TxtCode.Text, "^\d{6}") = False Then
                MessageBox.Show("コードに数字以外の文字があります", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ret = False
            End If
        Else
            MessageBox.Show("コードを入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ret = False
        End If

        ' 社員名
        If String.IsNullOrEmpty(TxtName.Text) = True Then
            MessageBox.Show("名前を入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ret = False
        End If

        Return ret
    End Function
End Class
