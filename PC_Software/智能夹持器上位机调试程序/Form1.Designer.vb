<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle49 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle50 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle51 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle52 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle53 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle54 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle55 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle56 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle57 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle58 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle59 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle60 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Lable1 = New System.Windows.Forms.Label
        Me.ComboBox_Serial_Number = New System.Windows.Forms.ComboBox
        Me.Button_Connect_or_Disconnect = New System.Windows.Forms.Button
        Me.PictureBox_Serial_Port_State = New System.Windows.Forms.PictureBox
        Me.Label_Serial_Port_State = New System.Windows.Forms.Label
        Me.TabControl_Mood = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.NumericUpDown_Force1 = New System.Windows.Forms.NumericUpDown
        Me.Label4 = New System.Windows.Forms.Label
        Me.Button_Export_to_File = New System.Windows.Forms.Button
        Me.TextBox_Gripper_State = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button_Release = New System.Windows.Forms.Button
        Me.Button_Hold = New System.Windows.Forms.Button
        Me.Button_Import_From_File = New System.Windows.Forms.Button
        Me.Button_Set_Para_Mood1 = New System.Windows.Forms.Button
        Me.Button_Refresh_Para_Mood1 = New System.Windows.Forms.Button
        Me.DataGridView_Gripper_Parameters = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.NumericUpDown_P3 = New System.Windows.Forms.NumericUpDown
        Me.Button_F1_Hold = New System.Windows.Forms.Button
        Me.Button_F1_Release = New System.Windows.Forms.Button
        Me.Button_F0_Hold = New System.Windows.Forms.Button
        Me.Button_F0_Release = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Button_P2P3_Set = New System.Windows.Forms.Button
        Me.NumericUpDown_P2 = New System.Windows.Forms.NumericUpDown
        Me.Button2 = New System.Windows.Forms.Button
        Me.ComboBox_Para_select = New System.Windows.Forms.ComboBox
        Me.Button_Set_Force2 = New System.Windows.Forms.Button
        Me.NumericUpDown_Force2 = New System.Windows.Forms.NumericUpDown
        Me.TextBox_Real_Force = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.DataGridView_PARA2 = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label1 = New System.Windows.Forms.Label
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button_Change_Mood = New System.Windows.Forms.Button
        Me.SerialPort_Gripper = New System.IO.Ports.SerialPort(Me.components)
        Me.OpenFileDialog_PARA = New System.Windows.Forms.OpenFileDialog
        Me.SaveFileDialog_PARA = New System.Windows.Forms.SaveFileDialog
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        CType(Me.PictureBox_Serial_Port_State, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl_Mood.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.NumericUpDown_Force1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView_Gripper_Parameters, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.NumericUpDown_P3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_P2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_Force2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView_PARA2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Lable1
        '
        Me.Lable1.AutoSize = True
        Me.Lable1.Location = New System.Drawing.Point(14, 19)
        Me.Lable1.Name = "Lable1"
        Me.Lable1.Size = New System.Drawing.Size(41, 12)
        Me.Lable1.TabIndex = 0
        Me.Lable1.Text = "串口号"
        '
        'ComboBox_Serial_Number
        '
        Me.ComboBox_Serial_Number.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Serial_Number.FormattingEnabled = True
        Me.ComboBox_Serial_Number.Location = New System.Drawing.Point(61, 15)
        Me.ComboBox_Serial_Number.Name = "ComboBox_Serial_Number"
        Me.ComboBox_Serial_Number.Size = New System.Drawing.Size(74, 20)
        Me.ComboBox_Serial_Number.TabIndex = 1
        '
        'Button_Connect_or_Disconnect
        '
        Me.Button_Connect_or_Disconnect.Location = New System.Drawing.Point(148, 11)
        Me.Button_Connect_or_Disconnect.Name = "Button_Connect_or_Disconnect"
        Me.Button_Connect_or_Disconnect.Size = New System.Drawing.Size(51, 28)
        Me.Button_Connect_or_Disconnect.TabIndex = 2
        Me.Button_Connect_or_Disconnect.Text = "连接"
        Me.Button_Connect_or_Disconnect.UseVisualStyleBackColor = True
        '
        'PictureBox_Serial_Port_State
        '
        Me.PictureBox_Serial_Port_State.BackColor = System.Drawing.Color.Red
        Me.PictureBox_Serial_Port_State.Location = New System.Drawing.Point(213, 18)
        Me.PictureBox_Serial_Port_State.Name = "PictureBox_Serial_Port_State"
        Me.PictureBox_Serial_Port_State.Size = New System.Drawing.Size(15, 15)
        Me.PictureBox_Serial_Port_State.TabIndex = 3
        Me.PictureBox_Serial_Port_State.TabStop = False
        '
        'Label_Serial_Port_State
        '
        Me.Label_Serial_Port_State.AutoSize = True
        Me.Label_Serial_Port_State.ForeColor = System.Drawing.Color.Black
        Me.Label_Serial_Port_State.Location = New System.Drawing.Point(234, 19)
        Me.Label_Serial_Port_State.Name = "Label_Serial_Port_State"
        Me.Label_Serial_Port_State.Size = New System.Drawing.Size(41, 12)
        Me.Label_Serial_Port_State.TabIndex = 4
        Me.Label_Serial_Port_State.Text = "未连接"
        '
        'TabControl_Mood
        '
        Me.TabControl_Mood.Controls.Add(Me.TabPage1)
        Me.TabControl_Mood.Controls.Add(Me.TabPage2)
        Me.TabControl_Mood.Enabled = False
        Me.TabControl_Mood.Location = New System.Drawing.Point(12, 48)
        Me.TabControl_Mood.Name = "TabControl_Mood"
        Me.TabControl_Mood.SelectedIndex = 0
        Me.TabControl_Mood.Size = New System.Drawing.Size(357, 296)
        Me.TabControl_Mood.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.NumericUpDown_Force1)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Button_Export_to_File)
        Me.TabPage1.Controls.Add(Me.TextBox_Gripper_State)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Button_Release)
        Me.TabPage1.Controls.Add(Me.Button_Hold)
        Me.TabPage1.Controls.Add(Me.Button_Import_From_File)
        Me.TabPage1.Controls.Add(Me.Button_Set_Para_Mood1)
        Me.TabPage1.Controls.Add(Me.Button_Refresh_Para_Mood1)
        Me.TabPage1.Controls.Add(Me.DataGridView_Gripper_Parameters)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(349, 270)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "一般模式"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'NumericUpDown_Force1
        '
        Me.NumericUpDown_Force1.Location = New System.Drawing.Point(261, 19)
        Me.NumericUpDown_Force1.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.NumericUpDown_Force1.Name = "NumericUpDown_Force1"
        Me.NumericUpDown_Force1.ReadOnly = True
        Me.NumericUpDown_Force1.Size = New System.Drawing.Size(47, 21)
        Me.NumericUpDown_Force1.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(182, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 12)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "夹紧力阈值"
        '
        'Button_Export_to_File
        '
        Me.Button_Export_to_File.Location = New System.Drawing.Point(259, 160)
        Me.Button_Export_to_File.Name = "Button_Export_to_File"
        Me.Button_Export_to_File.Size = New System.Drawing.Size(77, 34)
        Me.Button_Export_to_File.TabIndex = 8
        Me.Button_Export_to_File.Text = "保存至文件"
        Me.Button_Export_to_File.UseVisualStyleBackColor = True
        '
        'TextBox_Gripper_State
        '
        Me.TextBox_Gripper_State.Location = New System.Drawing.Point(97, 18)
        Me.TextBox_Gripper_State.Name = "TextBox_Gripper_State"
        Me.TextBox_Gripper_State.ReadOnly = True
        Me.TextBox_Gripper_State.Size = New System.Drawing.Size(65, 21)
        Me.TextBox_Gripper_State.TabIndex = 7
        Me.TextBox_Gripper_State.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 12)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "夹持器状态"
        '
        'Button_Release
        '
        Me.Button_Release.Location = New System.Drawing.Point(186, 208)
        Me.Button_Release.Name = "Button_Release"
        Me.Button_Release.Size = New System.Drawing.Size(56, 49)
        Me.Button_Release.TabIndex = 5
        Me.Button_Release.Text = "松开"
        Me.Button_Release.UseVisualStyleBackColor = True
        '
        'Button_Hold
        '
        Me.Button_Hold.Location = New System.Drawing.Point(105, 208)
        Me.Button_Hold.Name = "Button_Hold"
        Me.Button_Hold.Size = New System.Drawing.Size(57, 49)
        Me.Button_Hold.TabIndex = 4
        Me.Button_Hold.Text = "夹紧"
        Me.Button_Hold.UseVisualStyleBackColor = True
        '
        'Button_Import_From_File
        '
        Me.Button_Import_From_File.Location = New System.Drawing.Point(177, 160)
        Me.Button_Import_From_File.Name = "Button_Import_From_File"
        Me.Button_Import_From_File.Size = New System.Drawing.Size(77, 34)
        Me.Button_Import_From_File.TabIndex = 3
        Me.Button_Import_From_File.Text = "从文件读入"
        Me.Button_Import_From_File.UseVisualStyleBackColor = True
        '
        'Button_Set_Para_Mood1
        '
        Me.Button_Set_Para_Mood1.Location = New System.Drawing.Point(94, 160)
        Me.Button_Set_Para_Mood1.Name = "Button_Set_Para_Mood1"
        Me.Button_Set_Para_Mood1.Size = New System.Drawing.Size(77, 34)
        Me.Button_Set_Para_Mood1.TabIndex = 2
        Me.Button_Set_Para_Mood1.Text = "下传参数"
        Me.Button_Set_Para_Mood1.UseVisualStyleBackColor = True
        '
        'Button_Refresh_Para_Mood1
        '
        Me.Button_Refresh_Para_Mood1.Location = New System.Drawing.Point(13, 160)
        Me.Button_Refresh_Para_Mood1.Name = "Button_Refresh_Para_Mood1"
        Me.Button_Refresh_Para_Mood1.Size = New System.Drawing.Size(77, 34)
        Me.Button_Refresh_Para_Mood1.TabIndex = 1
        Me.Button_Refresh_Para_Mood1.Text = "更新参数"
        Me.Button_Refresh_Para_Mood1.UseVisualStyleBackColor = True
        '
        'DataGridView_Gripper_Parameters
        '
        Me.DataGridView_Gripper_Parameters.AllowUserToAddRows = False
        Me.DataGridView_Gripper_Parameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_Gripper_Parameters.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7})
        Me.DataGridView_Gripper_Parameters.Location = New System.Drawing.Point(13, 48)
        Me.DataGridView_Gripper_Parameters.Name = "DataGridView_Gripper_Parameters"
        Me.DataGridView_Gripper_Parameters.ReadOnly = True
        Me.DataGridView_Gripper_Parameters.RowTemplate.Height = 23
        Me.DataGridView_Gripper_Parameters.Size = New System.Drawing.Size(313, 98)
        Me.DataGridView_Gripper_Parameters.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = ""
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 60
        '
        'Column2
        '
        DataGridViewCellStyle49.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle49
        Me.Column2.HeaderText = "夹紧1-P2"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 35
        '
        'Column3
        '
        DataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle50
        Me.Column3.HeaderText = "夹紧1-P3"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 35
        '
        'Column4
        '
        DataGridViewCellStyle51.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle51
        Me.Column4.HeaderText = "夹紧2-P2"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 35
        '
        'Column5
        '
        DataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle52
        Me.Column5.HeaderText = "夹紧2-P3"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 35
        '
        'Column6
        '
        DataGridViewCellStyle53.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column6.DefaultCellStyle = DataGridViewCellStyle53
        Me.Column6.HeaderText = "松开-P2"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Width = 35
        '
        'Column7
        '
        DataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column7.DefaultCellStyle = DataGridViewCellStyle54
        Me.Column7.HeaderText = "松开-P3"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Width = 35
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label11)
        Me.TabPage2.Controls.Add(Me.Label10)
        Me.TabPage2.Controls.Add(Me.Label9)
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.NumericUpDown_P3)
        Me.TabPage2.Controls.Add(Me.Button_F1_Hold)
        Me.TabPage2.Controls.Add(Me.Button_F1_Release)
        Me.TabPage2.Controls.Add(Me.Button_F0_Hold)
        Me.TabPage2.Controls.Add(Me.Button_F0_Release)
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.Button_P2P3_Set)
        Me.TabPage2.Controls.Add(Me.NumericUpDown_P2)
        Me.TabPage2.Controls.Add(Me.Button2)
        Me.TabPage2.Controls.Add(Me.ComboBox_Para_select)
        Me.TabPage2.Controls.Add(Me.Button_Set_Force2)
        Me.TabPage2.Controls.Add(Me.NumericUpDown_Force2)
        Me.TabPage2.Controls.Add(Me.TextBox_Real_Force)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.DataGridView_PARA2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(349, 270)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "调试模式"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 175)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(101, 12)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "将当前参数应用于"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 147)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(41, 12)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "P2设置"
        '
        'NumericUpDown_P3
        '
        Me.NumericUpDown_P3.Location = New System.Drawing.Point(191, 143)
        Me.NumericUpDown_P3.Maximum = New Decimal(New Integer() {3, 0, 0, 0})
        Me.NumericUpDown_P3.Name = "NumericUpDown_P3"
        Me.NumericUpDown_P3.Size = New System.Drawing.Size(55, 21)
        Me.NumericUpDown_P3.TabIndex = 17
        '
        'Button_F1_Hold
        '
        Me.Button_F1_Hold.Location = New System.Drawing.Point(220, 238)
        Me.Button_F1_Hold.Name = "Button_F1_Hold"
        Me.Button_F1_Hold.Size = New System.Drawing.Size(69, 25)
        Me.Button_F1_Hold.TabIndex = 16
        Me.Button_F1_Hold.Text = "夹紧方向"
        Me.Button_F1_Hold.UseVisualStyleBackColor = True
        '
        'Button_F1_Release
        '
        Me.Button_F1_Release.Location = New System.Drawing.Point(45, 238)
        Me.Button_F1_Release.Name = "Button_F1_Release"
        Me.Button_F1_Release.Size = New System.Drawing.Size(74, 25)
        Me.Button_F1_Release.TabIndex = 15
        Me.Button_F1_Release.Text = "松开方向"
        Me.Button_F1_Release.UseVisualStyleBackColor = True
        '
        'Button_F0_Hold
        '
        Me.Button_F0_Hold.Location = New System.Drawing.Point(220, 201)
        Me.Button_F0_Hold.Name = "Button_F0_Hold"
        Me.Button_F0_Hold.Size = New System.Drawing.Size(69, 25)
        Me.Button_F0_Hold.TabIndex = 14
        Me.Button_F0_Hold.Text = "夹紧方向"
        Me.Button_F0_Hold.UseVisualStyleBackColor = True
        '
        'Button_F0_Release
        '
        Me.Button_F0_Release.Location = New System.Drawing.Point(45, 201)
        Me.Button_F0_Release.Name = "Button_F0_Release"
        Me.Button_F0_Release.Size = New System.Drawing.Size(74, 25)
        Me.Button_F0_Release.TabIndex = 13
        Me.Button_F0_Release.Text = "松开方向"
        Me.Button_F0_Release.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(142, 244)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 12)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "手指1"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(142, 208)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 12)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "手指0"
        '
        'Button_P2P3_Set
        '
        Me.Button_P2P3_Set.Location = New System.Drawing.Point(270, 142)
        Me.Button_P2P3_Set.Name = "Button_P2P3_Set"
        Me.Button_P2P3_Set.Size = New System.Drawing.Size(57, 23)
        Me.Button_P2P3_Set.TabIndex = 10
        Me.Button_P2P3_Set.Text = "下传"
        Me.Button_P2P3_Set.UseVisualStyleBackColor = True
        '
        'NumericUpDown_P2
        '
        Me.NumericUpDown_P2.Location = New System.Drawing.Point(59, 143)
        Me.NumericUpDown_P2.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.NumericUpDown_P2.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown_P2.Name = "NumericUpDown_P2"
        Me.NumericUpDown_P2.Size = New System.Drawing.Size(55, 21)
        Me.NumericUpDown_P2.TabIndex = 9
        Me.NumericUpDown_P2.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(270, 170)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(57, 23)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "确认"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ComboBox_Para_select
        '
        Me.ComboBox_Para_select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Para_select.FormattingEnabled = True
        Me.ComboBox_Para_select.Items.AddRange(New Object() {"手指0·夹紧1阶段", "手指0·夹紧2阶段", "手指0·松开阶段", "手指1·夹紧1阶段", "手指1·夹紧2阶段", "手指1·松开阶段"})
        Me.ComboBox_Para_select.Location = New System.Drawing.Point(132, 171)
        Me.ComboBox_Para_select.Name = "ComboBox_Para_select"
        Me.ComboBox_Para_select.Size = New System.Drawing.Size(102, 20)
        Me.ComboBox_Para_select.TabIndex = 6
        '
        'Button_Set_Force2
        '
        Me.Button_Set_Force2.Enabled = False
        Me.Button_Set_Force2.Location = New System.Drawing.Point(270, 113)
        Me.Button_Set_Force2.Name = "Button_Set_Force2"
        Me.Button_Set_Force2.Size = New System.Drawing.Size(57, 23)
        Me.Button_Set_Force2.TabIndex = 5
        Me.Button_Set_Force2.Text = "下传"
        Me.Button_Set_Force2.UseVisualStyleBackColor = True
        '
        'NumericUpDown_Force2
        '
        Me.NumericUpDown_Force2.Location = New System.Drawing.Point(195, 114)
        Me.NumericUpDown_Force2.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.NumericUpDown_Force2.Name = "NumericUpDown_Force2"
        Me.NumericUpDown_Force2.ReadOnly = True
        Me.NumericUpDown_Force2.Size = New System.Drawing.Size(64, 21)
        Me.NumericUpDown_Force2.TabIndex = 4
        '
        'TextBox_Real_Force
        '
        Me.TextBox_Real_Force.Location = New System.Drawing.Point(102, 114)
        Me.TextBox_Real_Force.Name = "TextBox_Real_Force"
        Me.TextBox_Real_Force.ReadOnly = True
        Me.TextBox_Real_Force.Size = New System.Drawing.Size(52, 21)
        Me.TextBox_Real_Force.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(160, 118)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 12)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "阈值"
        '
        'DataGridView_PARA2
        '
        Me.DataGridView_PARA2.AllowUserToAddRows = False
        Me.DataGridView_PARA2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_PARA2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn7})
        Me.DataGridView_PARA2.Location = New System.Drawing.Point(14, 6)
        Me.DataGridView_PARA2.Name = "DataGridView_PARA2"
        Me.DataGridView_PARA2.ReadOnly = True
        Me.DataGridView_PARA2.RowTemplate.Height = 23
        Me.DataGridView_PARA2.Size = New System.Drawing.Size(313, 98)
        Me.DataGridView_PARA2.TabIndex = 1
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = ""
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 60
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle55.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle55
        Me.DataGridViewTextBoxColumn2.HeaderText = "夹紧1-P2"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 35
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle56.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle56
        Me.DataGridViewTextBoxColumn3.HeaderText = "夹紧1-P3"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 35
        '
        'DataGridViewTextBoxColumn4
        '
        DataGridViewCellStyle57.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle57
        Me.DataGridViewTextBoxColumn4.HeaderText = "夹紧2-P2"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 35
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle58.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle58
        Me.DataGridViewTextBoxColumn5.HeaderText = "夹紧2-P3"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 35
        '
        'DataGridViewTextBoxColumn6
        '
        DataGridViewCellStyle59.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle59
        Me.DataGridViewTextBoxColumn6.HeaderText = "松开-P2"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 35
        '
        'DataGridViewTextBoxColumn7
        '
        DataGridViewCellStyle60.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle60
        Me.DataGridViewTextBoxColumn7.HeaderText = "松开-P3"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Width = 35
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 362)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(317, 12)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "北京航空航天大学 宇航学院图像处理中心 2013年10月17日"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel1.Location = New System.Drawing.Point(195, 382)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(119, 12)
        Me.LinkLabel1.TabIndex = 7
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "yifei_feng@yeah.net"
        Me.LinkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(51, 382)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(137, 12)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "软件反馈报错联系邮箱："
        '
        'Button_Change_Mood
        '
        Me.Button_Change_Mood.Enabled = False
        Me.Button_Change_Mood.Location = New System.Drawing.Point(283, 10)
        Me.Button_Change_Mood.Name = "Button_Change_Mood"
        Me.Button_Change_Mood.Size = New System.Drawing.Size(72, 28)
        Me.Button_Change_Mood.TabIndex = 9
        Me.Button_Change_Mood.Text = "切换模式"
        Me.Button_Change_Mood.UseVisualStyleBackColor = True
        '
        'SerialPort_Gripper
        '
        Me.SerialPort_Gripper.BaudRate = 19200
        '
        'OpenFileDialog_PARA
        '
        Me.OpenFileDialog_PARA.FileName = "OpenFileDialog1"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 118)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(77, 12)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "夹紧力实时值"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(138, 146)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(41, 12)
        Me.Label11.TabIndex = 21
        Me.Label11.Text = "P3设置"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(382, 402)
        Me.Controls.Add(Me.Button_Change_Mood)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TabControl_Mood)
        Me.Controls.Add(Me.Label_Serial_Port_State)
        Me.Controls.Add(Me.PictureBox_Serial_Port_State)
        Me.Controls.Add(Me.Button_Connect_or_Disconnect)
        Me.Controls.Add(Me.ComboBox_Serial_Number)
        Me.Controls.Add(Me.Lable1)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "智能夹持器上位机软件"
        CType(Me.PictureBox_Serial_Port_State, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl_Mood.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.NumericUpDown_Force1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView_Gripper_Parameters, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.NumericUpDown_P3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_P2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_Force2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView_PARA2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Lable1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Serial_Number As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Connect_or_Disconnect As System.Windows.Forms.Button
    Friend WithEvents PictureBox_Serial_Port_State As System.Windows.Forms.PictureBox
    Friend WithEvents Label_Serial_Port_State As System.Windows.Forms.Label
    Friend WithEvents TabControl_Mood As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Button_Release As System.Windows.Forms.Button
    Friend WithEvents Button_Hold As System.Windows.Forms.Button
    Friend WithEvents Button_Import_From_File As System.Windows.Forms.Button
    Friend WithEvents Button_Set_Para_Mood1 As System.Windows.Forms.Button
    Friend WithEvents Button_Refresh_Para_Mood1 As System.Windows.Forms.Button
    Friend WithEvents TextBox_Gripper_State As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button_Export_to_File As System.Windows.Forms.Button
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button_Change_Mood As System.Windows.Forms.Button
    Friend WithEvents SerialPort_Gripper As System.IO.Ports.SerialPort
    Friend WithEvents NumericUpDown_Force1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog_PARA As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog_PARA As System.Windows.Forms.SaveFileDialog
    Friend WithEvents DataGridView_PARA2 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button_F1_Hold As System.Windows.Forms.Button
    Friend WithEvents Button_F1_Release As System.Windows.Forms.Button
    Friend WithEvents Button_F0_Hold As System.Windows.Forms.Button
    Friend WithEvents Button_F0_Release As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Button_P2P3_Set As System.Windows.Forms.Button
    Friend WithEvents NumericUpDown_P2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ComboBox_Para_select As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Set_Force2 As System.Windows.Forms.Button
    Friend WithEvents NumericUpDown_Force2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents TextBox_Real_Force As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown_P3 As System.Windows.Forms.NumericUpDown
    Public WithEvents DataGridView_Gripper_Parameters As System.Windows.Forms.DataGridView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label

End Class
