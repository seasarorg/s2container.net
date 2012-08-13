<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDepartmentEdit
    Inherits Seasar.Windows.S2Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDepartmentEdit))
        Me.label1 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.quillControl1 = New Seasar.Quill.QuillControl
        Me.TxtOrder = New System.Windows.Forms.TextBox
        Me.TxtName = New System.Windows.Forms.TextBox
        Me.TxtCode = New System.Windows.Forms.TextBox
        Me.BtnClose = New System.Windows.Forms.Button
        Me.BtnDelete = New System.Windows.Forms.Button
        Me.BtnUpdate = New System.Windows.Forms.Button
        Me.label5 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        CType(Me.quillControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.label1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.label1.Location = New System.Drawing.Point(12, 35)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(283, 23)
        Me.label1.TabIndex = 1
        Me.label1.Text = "ïîñÂ"
        Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'label2
        '
        Me.label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.label2.Location = New System.Drawing.Point(12, 26)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(283, 41)
        Me.label2.TabIndex = 2
        '
        'quillControl1
        '
        Me.quillControl1.BackgroundImage = CType(resources.GetObject("quillControl1.BackgroundImage"), System.Drawing.Image)
        Me.quillControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.quillControl1.Location = New System.Drawing.Point(269, 187)
        Me.quillControl1.Name = "quillControl1"
        Me.quillControl1.Size = New System.Drawing.Size(21, 23)
        Me.quillControl1.TabIndex = 21
        Me.quillControl1.Visible = False
        '
        'TxtOrder
        '
        Me.TxtOrder.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TxtOrder.Location = New System.Drawing.Point(118, 178)
        Me.TxtOrder.MaxLength = 4
        Me.TxtOrder.Name = "TxtOrder"
        Me.TxtOrder.Size = New System.Drawing.Size(78, 22)
        Me.TxtOrder.TabIndex = 17
        Me.TxtOrder.Text = "9999"
        '
        'TxtName
        '
        Me.TxtName.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TxtName.Location = New System.Drawing.Point(118, 129)
        Me.TxtName.MaxLength = 50
        Me.TxtName.Name = "TxtName"
        Me.TxtName.Size = New System.Drawing.Size(145, 22)
        Me.TxtName.TabIndex = 15
        Me.TxtName.Text = "NNNNNNNNNNNNNN"
        '
        'TxtCode
        '
        Me.TxtCode.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TxtCode.Location = New System.Drawing.Point(118, 86)
        Me.TxtCode.MaxLength = 4
        Me.TxtCode.Name = "TxtCode"
        Me.TxtCode.Size = New System.Drawing.Size(78, 22)
        Me.TxtCode.TabIndex = 13
        Me.TxtCode.Text = "9999"
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(205, 228)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(90, 46)
        Me.BtnClose.TabIndex = 20
        Me.BtnClose.Text = "ï¬Ç∂ÇÈ(&C)"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnDelete.Location = New System.Drawing.Point(109, 228)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(90, 46)
        Me.BtnDelete.TabIndex = 19
        Me.BtnDelete.Text = "çÌèú(&D)"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnUpdate
        '
        Me.BtnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnUpdate.Location = New System.Drawing.Point(14, 228)
        Me.BtnUpdate.Name = "BtnUpdate"
        Me.BtnUpdate.Size = New System.Drawing.Size(90, 46)
        Me.BtnUpdate.TabIndex = 18
        Me.BtnUpdate.Text = "ìoò^(&R)"
        Me.BtnUpdate.UseVisualStyleBackColor = True
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.label5.Location = New System.Drawing.Point(19, 178)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(67, 15)
        Me.label5.TabIndex = 16
        Me.label5.Text = "ï\é¶èáî‘"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.label4.Location = New System.Drawing.Point(19, 132)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(52, 15)
        Me.label4.TabIndex = 14
        Me.label4.Text = "ïîñÂñº"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.label3.Location = New System.Drawing.Point(19, 89)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(40, 15)
        Me.label3.TabIndex = 12
        Me.label3.Text = "ÉRÅ[Éh"
        '
        'FrmDepartmentEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.ClientSize = New System.Drawing.Size(302, 286)
        Me.Controls.Add(Me.quillControl1)
        Me.Controls.Add(Me.TxtOrder)
        Me.Controls.Add(Me.TxtName)
        Me.Controls.Add(Me.TxtCode)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnUpdate)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.label2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmDepartmentEdit"
        Me.Text = "ïîñÂ"
        CType(Me.quillControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents quillControl1 As Seasar.Quill.QuillControl
    Private WithEvents TxtOrder As System.Windows.Forms.TextBox
    Private WithEvents TxtName As System.Windows.Forms.TextBox
    Private WithEvents TxtCode As System.Windows.Forms.TextBox
    Private WithEvents BtnClose As System.Windows.Forms.Button
    Private WithEvents BtnDelete As System.Windows.Forms.Button
    Private WithEvents BtnUpdate As System.Windows.Forms.Button
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label

End Class
