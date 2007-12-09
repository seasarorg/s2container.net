<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMainMenu
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMainMenu))
        Me.QuillControl1 = New Seasar.Quill.QuillControl
        Me.BtnEmployee = New System.Windows.Forms.Button
        Me.BtnDepartment = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.BtnClose = New System.Windows.Forms.Button
        CType(Me.QuillControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'QuillControl1
        '
        Me.QuillControl1.BackgroundImage = CType(resources.GetObject("QuillControl1.BackgroundImage"), System.Drawing.Image)
        Me.QuillControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.QuillControl1.Location = New System.Drawing.Point(203, -9)
        Me.QuillControl1.Name = "QuillControl1"
        Me.QuillControl1.Size = New System.Drawing.Size(21, 23)
        Me.QuillControl1.TabIndex = 0
        Me.QuillControl1.Visible = False
        '
        'BtnEmployee
        '
        Me.BtnEmployee.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnEmployee.Location = New System.Drawing.Point(12, 20)
        Me.BtnEmployee.Name = "BtnEmployee"
        Me.BtnEmployee.Size = New System.Drawing.Size(192, 70)
        Me.BtnEmployee.TabIndex = 1
        Me.BtnEmployee.Text = "社員マスタ編集(&E)"
        Me.BtnEmployee.UseVisualStyleBackColor = True
        '
        'BtnDepartment
        '
        Me.BtnDepartment.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnDepartment.Location = New System.Drawing.Point(203, 20)
        Me.BtnDepartment.Name = "BtnDepartment"
        Me.BtnDepartment.Size = New System.Drawing.Size(192, 70)
        Me.BtnDepartment.TabIndex = 2
        Me.BtnDepartment.Text = "部門マスタ編集(&D)"
        Me.BtnDepartment.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(12, 90)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(192, 70)
        Me.Button1.TabIndex = 3
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(203, 90)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(192, 70)
        Me.Button2.TabIndex = 4
        Me.Button2.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(11, 160)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(383, 70)
        Me.BtnClose.TabIndex = 5
        Me.BtnClose.Text = "閉じる(&C)"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'FrmMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 244)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.BtnDepartment)
        Me.Controls.Add(Me.BtnEmployee)
        Me.Controls.Add(Me.QuillControl1)
        Me.MaximizeBox = False
        Me.Name = "FrmMainMenu"
        Me.Text = "メインメニュー"
        CType(Me.QuillControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents QuillControl1 As Seasar.Quill.QuillControl
    Friend WithEvents BtnEmployee As System.Windows.Forms.Button
    Friend WithEvents BtnDepartment As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
End Class
