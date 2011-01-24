<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmEmployeeList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmEmployeeList))
        Me.Label3 = New System.Windows.Forms.Label
        Me.LblGenderName = New System.Windows.Forms.Label
        Me.TxtGenderId = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.quillControl1 = New Seasar.Quill.QuillControl
        Me.Label1 = New System.Windows.Forms.Label
        Me.BtnClose = New System.Windows.Forms.Button
        Me.BtnOutput = New System.Windows.Forms.Button
        Me.BtnNew = New System.Windows.Forms.Button
        Me.GridList = New System.Windows.Forms.DataGridView
        Me.ColumnCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnDepart = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnGender = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnEntryDay = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnDeptNo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ColumnDepartment = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dlgSave = New System.Windows.Forms.SaveFileDialog
        CType(Me.quillControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(193, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(135, 12)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "01:男性　02:女性　99:全員"
        '
        'LblGenderName
        '
        Me.LblGenderName.AutoSize = True
        Me.LblGenderName.Location = New System.Drawing.Point(134, 23)
        Me.LblGenderName.Name = "LblGenderName"
        Me.LblGenderName.Size = New System.Drawing.Size(35, 12)
        Me.LblGenderName.TabIndex = 31
        Me.LblGenderName.Text = "label3"
        '
        'TxtGenderId
        '
        Me.TxtGenderId.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.TxtGenderId.Location = New System.Drawing.Point(85, 20)
        Me.TxtGenderId.Name = "TxtGenderId"
        Me.TxtGenderId.Size = New System.Drawing.Size(43, 19)
        Me.TxtGenderId.TabIndex = 30
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(50, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 12)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "性別"
        '
        'quillControl1
        '
        Me.quillControl1.BackgroundImage = CType(resources.GetObject("quillControl1.BackgroundImage"), System.Drawing.Image)
        Me.quillControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.quillControl1.Location = New System.Drawing.Point(14, 241)
        Me.quillControl1.Name = "quillControl1"
        Me.quillControl1.Size = New System.Drawing.Size(21, 23)
        Me.quillControl1.TabIndex = 28
        Me.quillControl1.Visible = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(41, 218)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(161, 46)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "社員一覧"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(408, 219)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(90, 46)
        Me.BtnClose.TabIndex = 26
        Me.BtnClose.Text = "閉じる(&C)"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnOutput
        '
        Me.BtnOutput.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnOutput.Location = New System.Drawing.Point(312, 219)
        Me.BtnOutput.Name = "BtnOutput"
        Me.BtnOutput.Size = New System.Drawing.Size(90, 46)
        Me.BtnOutput.TabIndex = 25
        Me.BtnOutput.Text = "出力(&O)"
        Me.BtnOutput.UseVisualStyleBackColor = True
        '
        'BtnNew
        '
        Me.BtnNew.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.BtnNew.Location = New System.Drawing.Point(216, 219)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(90, 46)
        Me.BtnNew.TabIndex = 24
        Me.BtnNew.Text = "新規(&N)"
        Me.BtnNew.UseVisualStyleBackColor = True
        '
        'GridList
        '
        Me.GridList.AllowUserToAddRows = False
        Me.GridList.AllowUserToDeleteRows = False
        Me.GridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnCode, Me.ColumnName, Me.ColumnDepart, Me.ColumnId, Me.ColumnGender, Me.ColumnEntryDay, Me.ColumnDeptNo, Me.ColumnDepartment})
        Me.GridList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.GridList.Location = New System.Drawing.Point(41, 47)
        Me.GridList.MultiSelect = False
        Me.GridList.Name = "GridList"
        Me.GridList.ReadOnly = True
        Me.GridList.RowTemplate.Height = 21
        Me.GridList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GridList.Size = New System.Drawing.Size(457, 146)
        Me.GridList.TabIndex = 23
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
        Me.ColumnName.FillWeight = 150.0!
        Me.ColumnName.HeaderText = "名前"
        Me.ColumnName.Name = "ColumnName"
        Me.ColumnName.ReadOnly = True
        Me.ColumnName.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ColumnName.Width = 150
        '
        'ColumnDepart
        '
        Me.ColumnDepart.DataPropertyName = "DeptName"
        Me.ColumnDepart.FillWeight = 150.0!
        Me.ColumnDepart.HeaderText = "部門"
        Me.ColumnDepart.Name = "ColumnDepart"
        Me.ColumnDepart.ReadOnly = True
        Me.ColumnDepart.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ColumnDepart.Width = 150
        '
        'ColumnId
        '
        Me.ColumnId.DataPropertyName = "Id"
        Me.ColumnId.HeaderText = "Id"
        Me.ColumnId.Name = "ColumnId"
        Me.ColumnId.ReadOnly = True
        Me.ColumnId.Visible = False
        '
        'ColumnGender
        '
        Me.ColumnGender.DataPropertyName = "Gender"
        Me.ColumnGender.HeaderText = "Gender"
        Me.ColumnGender.Name = "ColumnGender"
        Me.ColumnGender.ReadOnly = True
        Me.ColumnGender.Visible = False
        '
        'ColumnEntryDay
        '
        Me.ColumnEntryDay.DataPropertyName = "EntryDay"
        Me.ColumnEntryDay.HeaderText = "EntryDay"
        Me.ColumnEntryDay.Name = "ColumnEntryDay"
        Me.ColumnEntryDay.ReadOnly = True
        Me.ColumnEntryDay.Visible = False
        '
        'ColumnDeptNo
        '
        Me.ColumnDeptNo.DataPropertyName = "DeptNo"
        Me.ColumnDeptNo.HeaderText = "DeptNo"
        Me.ColumnDeptNo.Name = "ColumnDeptNo"
        Me.ColumnDeptNo.ReadOnly = True
        Me.ColumnDeptNo.Visible = False
        '
        'ColumnDepartment
        '
        Me.ColumnDepartment.DataPropertyName = "Department"
        Me.ColumnDepartment.HeaderText = "ColumnDepartment"
        Me.ColumnDepartment.Name = "ColumnDepartment"
        Me.ColumnDepartment.ReadOnly = True
        Me.ColumnDepartment.Visible = False
        '
        'FrmEmployeeList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.ClientSize = New System.Drawing.Size(512, 285)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LblGenderName)
        Me.Controls.Add(Me.TxtGenderId)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.quillControl1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnOutput)
        Me.Controls.Add(Me.BtnNew)
        Me.Controls.Add(Me.GridList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmEmployeeList"
        Me.Text = "社員一覧"
        CType(Me.quillControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents LblGenderName As System.Windows.Forms.Label
    Private WithEvents TxtGenderId As System.Windows.Forms.TextBox
    Private WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents quillControl1 As Seasar.Quill.QuillControl
    Private WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents BtnClose As System.Windows.Forms.Button
    Private WithEvents BtnOutput As System.Windows.Forms.Button
    Private WithEvents BtnNew As System.Windows.Forms.Button
    Private WithEvents GridList As System.Windows.Forms.DataGridView
    Friend WithEvents dlgSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ColumnCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnDepart As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnGender As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnEntryDay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnDeptNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnDepartment As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
