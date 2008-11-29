<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDepartmentList
    Inherits Seasar.Windows.S2Form



    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDepartmentList))
        Me.QuillControl1 = New Seasar.Quill.QuillControl
        Me.GridList = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.BtnNew = New System.Windows.Forms.Button
        Me.BtnClose = New System.Windows.Forms.Button
        Me.ColumnCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnShowOrder = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnId = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.QuillControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'QuillControl1
        '
        Me.QuillControl1.BackgroundImage = CType(resources.GetObject("QuillControl1.BackgroundImage"), System.Drawing.Image)
        Me.QuillControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.QuillControl1.Location = New System.Drawing.Point(12, 248)
        Me.QuillControl1.Name = "QuillControl1"
        Me.QuillControl1.Size = New System.Drawing.Size(21, 23)
        Me.QuillControl1.TabIndex = 0
        Me.QuillControl1.Visible = False
        '
        'GridList
        '
        Me.GridList.AllowUserToAddRows = False
        Me.GridList.AllowUserToDeleteRows = False
        Me.GridList.AllowUserToResizeColumns = False
        Me.GridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnCode, Me.ColumnName, Me.ColumnShowOrder, Me.ColumnId})
        Me.GridList.Location = New System.Drawing.Point(30, 40)
        Me.GridList.Name = "GridList"
        Me.GridList.ReadOnly = True
        Me.GridList.RowTemplate.Height = 21
        Me.GridList.Size = New System.Drawing.Size(418, 176)
        Me.GridList.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(34, 232)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(201, 41)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "部門一覧"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnNew
        '
        Me.BtnNew.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnNew.Location = New System.Drawing.Point(276, 233)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(83, 40)
        Me.BtnNew.TabIndex = 3
        Me.BtnNew.Text = "新規(&N)"
        Me.BtnNew.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(365, 233)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(83, 40)
        Me.BtnClose.TabIndex = 4
        Me.BtnClose.Text = "閉じる(&C)"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'ColumnCode
        '
        Me.ColumnCode.DataPropertyName = "Code"
        Me.ColumnCode.HeaderText = "コード"
        Me.ColumnCode.Name = "ColumnCode"
        Me.ColumnCode.ReadOnly = True
        Me.ColumnCode.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        '
        'ColumnName
        '
        Me.ColumnName.DataPropertyName = "Name"
        Me.ColumnName.HeaderText = "名前"
        Me.ColumnName.Name = "ColumnName"
        Me.ColumnName.ReadOnly = True
        Me.ColumnName.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ColumnName.Width = 200
        '
        'ColumnShowOrder
        '
        Me.ColumnShowOrder.DataPropertyName = "ShowOrder"
        Me.ColumnShowOrder.HeaderText = "表示順番"
        Me.ColumnShowOrder.Name = "ColumnShowOrder"
        Me.ColumnShowOrder.ReadOnly = True
        Me.ColumnShowOrder.Visible = False
        '
        'ColumnId
        '
        Me.ColumnId.DataPropertyName = "Id"
        Me.ColumnId.HeaderText = "ID"
        Me.ColumnId.Name = "ColumnId"
        Me.ColumnId.ReadOnly = True
        Me.ColumnId.Visible = False
        '
        'FrmDepartmentList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(474, 283)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnNew)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GridList)
        Me.Controls.Add(Me.QuillControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmDepartmentList"
        Me.Text = "部門一覧"
        CType(Me.QuillControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents QuillControl1 As Seasar.Quill.QuillControl
    Friend WithEvents GridList As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnNew As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents ColumnCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnShowOrder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnId As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
