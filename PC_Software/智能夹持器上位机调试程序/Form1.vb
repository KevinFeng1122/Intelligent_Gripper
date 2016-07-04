Imports System.IO
Imports System.Text
Public Class Form1
    Dim Gripper_State As Integer '夹持器工作模式：1为一般模式，2为调试模式
    Dim Finger0_Released As Boolean = True '手指0是否到达端部
    Dim Finger1_Released As Boolean = True '手指1是否到达端部
    Dim SerialPort_Buffer(20) As Byte

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Control.CheckForIllegalCrossThreadCalls = False 'VB.NET 解决线程间操作无效错误
    End Sub

    Private Sub ComboBox_Serial_Number_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Serial_Number.DropDown
        ComboBox_Serial_Number.Items.Clear()
        ComboBox_Serial_Number.Items.AddRange(SerialPort_Gripper.GetPortNames())
    End Sub

    Private Sub Button_Connect_or_Disconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Connect_or_Disconnect.Click
        With SerialPort_Gripper
            If .IsOpen Then
                .Close()
                Button_Connect_or_Disconnect.Text = "连接"
                PictureBox_Serial_Port_State.BackColor = Color.Red
                Label_Serial_Port_State.Text = "未连接"
                TabControl_Mood.Enabled = False
                Button_Change_Mood.Enabled = False
            Else
                If ComboBox_Serial_Number.SelectedIndex = -1 Then '表示组合框中没有选择任何一项
                    MsgBox("请先选择串口号！")
                    Exit Sub
                End If
                .PortName = ComboBox_Serial_Number.Text
                .Open()
                Button_Connect_or_Disconnect.Text = "断开"
                PictureBox_Serial_Port_State.BackColor = Color.Green
                Label_Serial_Port_State.Text = "已连接"
                TabControl_Mood.Enabled = True
                Button_Change_Mood.Enabled = True
                Select_Gripper_Mood()
            End If
        End With
    End Sub

    Private Sub Select_Gripper_Mood()
        MsgBox("请确认已经复位夹持器", MsgBoxStyle.Exclamation, "复位确认")

        If MsgBox("请为夹持器选择工作模式：""是""进入一般模式，""否""进入调试模式", MsgBoxStyle.YesNo, "夹持器工作模式选择") = MsgBoxResult.Yes Then
            Gripper_State = 1 '一般模式
            TabControl_Mood.SelectedIndex = 0
            SerialPort_Gripper.Write("xx0100") '使夹持器进入一般工作模式
            Delay() '等待单片机处理指令
            SerialPort_Gripper.Write("xx1000") '读取EEPROM中存储的数据
            Delay()
            TextBox_Gripper_State.Text = "已经松开"
            Button_Release.Enabled = False
        Else
            Gripper_State = 2 '调试模式
            TabControl_Mood.SelectedIndex = 1 '显示第二个选项卡
            SerialPort_Gripper.Write("xx0200") '使夹持器进入调试工作模式
        End If
    End Sub

    Private Sub NumericUpDown_Force1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown_Force1.DoubleClick
        NumericUpDown_Force1.ReadOnly = False
    End Sub

    Private Sub DataGridView_Gripper_Parameters_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView_Gripper_Parameters.DoubleClick
        DataGridView_Gripper_Parameters.ReadOnly = False
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If SerialPort_Gripper.IsOpen Then
            SerialPort_Gripper.Close()
        End If
    End Sub

    Private Sub TabControl_Mood_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl_Mood.SelectedIndexChanged
        TabControl_Mood.SelectedIndex = Gripper_State - 1 '未切换模式时，选项卡显示页不得改变
    End Sub

    Private Sub Button_Refresh_Para_Mood1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Refresh_Para_Mood1.Click
        SerialPort_Gripper.Write("xx1000") '读取EEPROM中存储的数据
        DataGridView_Gripper_Parameters.ReadOnly = True
        NumericUpDown_Force1.ReadOnly = True
    End Sub

    Private Sub Button_Set_Para_Mood1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Set_Para_Mood1.Click
        '获取参数
        Dim Finger0_PARA_array(5) As Integer
        Dim Finger1_PARA_array(5) As Integer
        Dim force As Integer
        Dim Cell_Value As String = ""
        Dim i As Integer
        i = 0
        For i = 0 To 5
            Cell_Value = DataGridView_Gripper_Parameters.Rows(0).Cells(i + 1).Value
            Finger0_PARA_array(i) = Val(Cell_Value)
        Next
        i = 0
        For i = 0 To 5
            Cell_Value = DataGridView_Gripper_Parameters.Rows(1).Cells(i + 1).Value
            Finger1_PARA_array(i) = Val(Cell_Value)
        Next
        force = NumericUpDown_Force1.Value

        '检测参数正确性
        If PARA_verify(Finger0_PARA_array, Finger1_PARA_array, force) = False Then
            Exit Sub
        End If

        '下传参数
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("1")

        '手指0，Ratio 1 夹紧1
        Command_array(3) = Asc("3")
        Command_array(4) = Finger0_PARA_array(0)
        Command_array(5) = Finger0_PARA_array(1)
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()

        '手指0，Ratio 2 夹紧2
        Command_array(3) = Asc("4")
        Command_array(4) = Finger0_PARA_array(2)
        Command_array(5) = Finger0_PARA_array(3)
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()

        '手指0，Ratio 3 松开
        Command_array(3) = Asc("5")
        Command_array(4) = Finger0_PARA_array(4)
        Command_array(5) = Finger0_PARA_array(5)
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()

        '手指1，Ratio 1 夹紧1
        Command_array(3) = Asc("6")
        Command_array(4) = Finger1_PARA_array(0)
        Command_array(5) = Finger1_PARA_array(1)
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()

        '手指1，Ratio 2 夹紧2
        Command_array(3) = Asc("7")
        Command_array(4) = Finger1_PARA_array(2)
        Command_array(5) = Finger1_PARA_array(3)
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()

        '手指1，Ratio 3 松开
        Command_array(3) = Asc("8")
        Command_array(4) = Finger1_PARA_array(4)
        Command_array(5) = Finger1_PARA_array(5)
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()

        '夹紧力阈值
        Command_array(3) = Asc("9")
        Command_array(5) = Asc("0")
        Command_array(4) = force
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()

        '其它
        DataGridView_Gripper_Parameters.ReadOnly = True
        NumericUpDown_Force1.ReadOnly = True

        MsgBox("完成！")
    End Sub

    Private Function PARA_verify(ByRef f0_data_array() As Integer, ByRef f1_data_array() As Integer, ByVal force As Integer) As Boolean
        Dim i As Integer = 0
        Dim PARA2, PARA3 As Integer
        'PARA2:01~255,PARA3:0~3
        For i = 0 To 3 Step 2
            PARA2 = f0_data_array(i)
            If PARA2 < 1 Or PARA2 > 255 Then
                MsgBox("请检查第1行第" & (i + 1).ToString & "列数据是否有误！", MsgBoxStyle.Critical)
                Return False
            End If
            PARA3 = f0_data_array(i + 1)
            If PARA3 < 0 Or PARA3 > 3 Then
                MsgBox("请检查第1行第" & (i + 2).ToString & "列数据是否有误！", MsgBoxStyle.Critical)
                Return False
            End If
        Next
        Dim k As Integer = 0
        For k = 0 To 3 Step 2
            PARA2 = f1_data_array(k)
            If PARA2 < 1 Or PARA2 > 255 Then
                MsgBox("请检查第2行第" & (k + 1).ToString & "列数据是否有误！", MsgBoxStyle.Critical)
                Return False
            End If
            PARA3 = f1_data_array(k + 1)
            If PARA3 < 0 Or PARA3 > 3 Then
                MsgBox("请检查第2行第" & (k + 2).ToString & "列数据是否有误！", MsgBoxStyle.Critical)
                Return False
            End If
        Next
        If force < 1 Or force > 255 Then
            MsgBox("请检查第夹紧力阈值是否有误！", MsgBoxStyle.Critical)
            Return False
        End If
        Return True
    End Function

    Private Sub Button_Hold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Hold.Click
        SerialPort_Gripper.Write("xx1200") '一般模式•夹紧
        Finger0_Released = False
        Finger1_Released = False
        TextBox_Gripper_State.Text = "正在夹紧"
    End Sub

    Private Sub Button_Release_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Release.Click
        SerialPort_Gripper.Write("xx1100") '一般模式•松开
    End Sub

    Private Sub Button_Export_to_File_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Export_to_File.Click
        '↓确定数据文件保存位置
        Dim myFilePath As String = ""
        With SaveFileDialog_PARA
            .Filter = "文本文件(*.txt)|*.txt"
            .AddExtension = True
            .DefaultExt = "txt"
            .OverwritePrompt = True
            .Title = "参数保存位置"
        End With
        If SaveFileDialog_PARA.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        myFilePath = SaveFileDialog_PARA.FileName

        '将参数写入文件↓
        Dim myFileWriter As New StreamWriter(myFilePath, False, System.Text.Encoding.Default)
        Dim k As Integer
        Dim Cell_Text As String = ""
        '保存手指0的参数
        k = 0
        For k = 0 To 5
            Cell_Text = DataGridView_Gripper_Parameters.Rows(0).Cells(k + 1).Value
            myFileWriter.WriteLine(Cell_Text)
        Next
        '保存手指1的参数
        k = 0
        For k = 0 To 5
            Cell_Text = DataGridView_Gripper_Parameters.Rows(1).Cells(k + 1).Value
            myFileWriter.WriteLine(Cell_Text)
        Next
        '保存夹紧力阈值
        Dim Force As Integer
        Force = NumericUpDown_Force1.Value
        myFileWriter.WriteLine(Force.ToString)
        myFileWriter.Close()

        DataGridView_Gripper_Parameters.ReadOnly = True
        NumericUpDown_Force1.ReadOnly = True
    End Sub

    Private Sub Button_Import_From_File_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Import_From_File.Click
        '获得目标文件名
        Dim myFileName As String
        With OpenFileDialog_PARA
            .FileName = ""
            .Title = "选择参数文件"
            .Filter = "文本文件(*.txt)|*.txt"
        End With
        If OpenFileDialog_PARA.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        myFileName = OpenFileDialog_PARA.FileName

        '清除表格中的原有内容
        DataGridView_Gripper_Parameters.Rows.Clear()

        Dim myReader As New System.IO.StreamReader(myFileName, System.Text.Encoding.Default)
        Dim myReceiver As String
        Dim Finger0_PARA_array(5) As String
        Dim Finger1_PARA_array(5) As String
        Dim Force As String = ""
        Dim i As Integer
        Try
            '读取手指0的参数
            i = 0
            For i = 0 To 5
                myReceiver = myReader.ReadLine()
                Finger0_PARA_array(i) = myReceiver
            Next
            '读取手指1的参数
            i = 0
            For i = 0 To 5
                myReceiver = myReader.ReadLine()
                Finger1_PARA_array(i) = myReceiver
            Next
            '读取夹紧力阈值
            myReceiver = myReader.ReadLine()
            Force = myReceiver
        Catch ex As Exception
            MessageBox.Show(ex.Message, "读取文件发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        '填写参数
        DataGridView_Gripper_Parameters.Rows.Add("手指0", Finger0_PARA_array(0), Finger0_PARA_array(1), Finger0_PARA_array(2), Finger0_PARA_array(3), Finger0_PARA_array(4), Finger0_PARA_array(5))
        DataGridView_Gripper_Parameters.Rows.Add("手指1", Finger1_PARA_array(0), Finger1_PARA_array(1), Finger1_PARA_array(2), Finger1_PARA_array(3), Finger1_PARA_array(4), Finger1_PARA_array(5))
        NumericUpDown_Force1.Value = Val(Force)
    End Sub

    Private Sub Delay()
        Dim i, k As Integer
        For i = 0 To 1000
            For k = 0 To 100000
            Next
        Next
    End Sub

    Private Sub Button_Change_Mood_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Change_Mood.Click
        Select_Gripper_Mood()
    End Sub

    Private Sub SerialPort_Gripper_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort_Gripper.DataReceived
        Try
            'Dim SerialPort_Buffer(20) As Byte
            For i = 0 To 20
                SerialPort_Buffer(i) = 0
            Next
            SerialPort_Gripper.Read(SerialPort_Buffer, 0, 20)
            AccessControl()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub AccessControl()
        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf AccessControl))
        Else
            ' Code wasn't working in the threading sub

            Dim Categoray_Buffer(4) As Byte
            For i = 0 To 3
                Categoray_Buffer(i) = SerialPort_Buffer(i)
            Next

            '不要用Dim MsgReceived As String = SerialPort_Gripper.ReadExisting，因为按照字符读取，有些数据会读成问号
            Dim encoding As System.Text.Encoding = System.Text.Encoding.Default
            Dim MsgCategory As String = encoding.GetString(Categoray_Buffer)
            MsgCategory = Microsoft.VisualBasic.Left(MsgCategory, 4) '必须有此句！否则下面识别不出来
            Select Case MsgCategory
                Case "zz00" '上电后手指复位完成
                    TextBox_Gripper_State.Text = "已经松开"
                    Button_Release.Enabled = False
                Case "zz30" '手指0复位
                    Finger0_Released = True
                    If Finger0_Released And Finger1_Released Then
                        TextBox_Gripper_State.Text = "已经松开"
                        Button_Release.Enabled = False
                        Button_Hold.Enabled = True
                    End If
                    If Gripper_State = 2 Then
                        MsgBox("手指0到达端部")
                    End If
                Case "zz31" '手指1复位
                    Finger1_Released = True
                    If Finger0_Released And Finger1_Released Then
                        TextBox_Gripper_State.Text = "已经松开"
                        Button_Release.Enabled = False
                        Button_Hold.Enabled = True
                    End If
                    If Gripper_State = 2 Then
                        MsgBox("手指1到达端部")
                    End If
                Case "zz32" '空夹
                    TextBox_Gripper_State.Text = "空夹！"
                    MsgBox("请注意夹持器处于空夹状态！", MsgBoxStyle.Exclamation)
                    Button_Hold.Enabled = False
                    Button_Release.Enabled = True
                Case "zz33" '返回EEPROM中现存Ratio值+现存夹紧力阈值数值
                    Dim PARA_data(13) As Byte
                    For i = 0 To 12
                        PARA_data(i) = SerialPort_Buffer(i + 4)
                    Next

                    If Gripper_State = 1 Then '一般模式
                        Fill_DataGridView(DataGridView_Gripper_Parameters, PARA_data)
                    Else '调试模式
                        Fill_DataGridView(DataGridView_PARA2, PARA_data)
                    End If

                Case "zz10" '已夹紧
                    TextBox_Gripper_State.Text = "已经夹紧"
                    Button_Hold.Enabled = False
                    Button_Release.Enabled = True
                Case "zz21" '夹紧力实时数值
                    TextBox_Real_Force.Text = SerialPort_Buffer(4).ToString
                    'Case "zz41" '报告夹持器上方触碰
                    '    MsgBox("夹持器上方触碰！请在排除阻碍后单击""确认""按钮。", MsgBoxStyle.Exclamation, "夹持器上方触碰！")
                    '    SerialPort_Gripper.Write("xx3100")
                    'Case "zz42" '报告夹持器下方触碰
                    '    MsgBox("夹持器下方触碰！请在排除阻碍后单击""确认""按钮。", MsgBoxStyle.Exclamation, "夹持器下方触碰！")
                    '    SerialPort_Gripper.Write("xx3200")
                    'Case "zz43" '报告夹持器前方触碰
                    '    MsgBox("夹持器前方触碰！请在排除阻碍后单击""确认""按钮。", MsgBoxStyle.Exclamation, "夹持器前方触碰！")
                    '    SerialPort_Gripper.Write("xx3300")
                    'Case "zz44" '返回夹持器与外部碰撞报警（中断）允许变量的值
            End Select
        End If
    End Sub

    Private Sub Fill_DataGridView(ByRef dgv As DataGridView, ByRef data() As Byte) '
        Try
            With dgv
                .Rows.Clear()
                .Rows.Add("手指0", "", "", "", "", "", "")
                .Rows.Add("手指1", "", "", "", "", "", "")
                For i = 0 To 1
                    For k = 0 To 5
                        .Rows(i).Cells(k + 1).Value = data(i * 6 + k).ToString
                    Next
                Next
            End With
            '下位机→(十六进制数)→.tostring→文本控件
            NumericUpDown_Force1.Value = data(12)
            NumericUpDown_Force2.Value = data(12)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    
    Private Sub NumericUpDown_Force2_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown_Force2.DoubleClick
        NumericUpDown_Force2.ReadOnly = False
        Button_Set_Force2.Enabled = True
    End Sub

    Private Sub Button_Set_Force2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Set_Force2.Click
        '下传参数
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        '夹紧力阈值
        Command_array(3) = Asc("4")
        Command_array(5) = Asc("0")
        Command_array(4) = NumericUpDown_Force2.Value
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
        '
        NumericUpDown_Force2.ReadOnly = True
        Button_Set_Force2.Enabled = False
    End Sub

    Private Sub Button_P2P3_Set_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_P2P3_Set.Click
        If NumericUpDown_P2.Value > 255 Or NumericUpDown_P2.Value < 1 Then
            MsgBox("请检查P2值的范围！须在1-255之间。", MsgBoxStyle.Critical, "P2值错误")
            Exit Sub
        End If
        If NumericUpDown_P3.Value > 3 Or NumericUpDown_P3.Value < 0 Then
            MsgBox("请检查P3值的范围！须在0-2之间。", MsgBoxStyle.Critical, "P3值错误")
            Exit Sub
        End If
        '下传参数
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        '夹紧力阈值
        Command_array(3) = Asc("2")
        Command_array(4) = NumericUpDown_P2.Value
        Command_array(5) = NumericUpDown_P3.Value
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("3")
        Select Case ComboBox_Para_select.SelectedIndex
            Case -1
                MsgBox("请先选择设定参数", MsgBoxStyle.Critical, )
                Exit Sub
            Case 0 '手指0夹紧1
                '下传参数
                Command_array(4) = Asc("0")
                Command_array(5) = Asc("1")
                SerialPort_Gripper.Write(Command_array, 0, 6)
                Delay()
            Case 1 '手指0夹紧1
                Command_array(4) = Asc("0")
                Command_array(5) = Asc("2")
                SerialPort_Gripper.Write(Command_array, 0, 6)
                Delay()
            Case 2 '手指0松开
                Command_array(4) = Asc("0")
                Command_array(5) = Asc("3")
                SerialPort_Gripper.Write(Command_array, 0, 6)
                Delay()
            Case 3
                Command_array(4) = Asc("1")
                Command_array(5) = Asc("1")
                SerialPort_Gripper.Write(Command_array, 0, 6)
                Delay()
            Case 4
                Command_array(4) = Asc("1")
                Command_array(5) = Asc("2")
                SerialPort_Gripper.Write(Command_array, 0, 6)
                Delay()
            Case 5
                Command_array(4) = Asc("1")
                Command_array(5) = Asc("3")
                SerialPort_Gripper.Write(Command_array, 0, 6)
                Delay()
        End Select
    End Sub

    Private Sub Button_F0_Release_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F0_Release.MouseDown
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("0")
        Command_array(5) = Asc("1")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button_F0_Release_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F0_Release.MouseUp
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("0")
        Command_array(5) = Asc("0")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button_F0_Hold_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F0_Hold.MouseDown
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("0")
        Command_array(5) = Asc("2")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button_F0_Hold_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F0_Hold.MouseUp
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("0")
        Command_array(5) = Asc("0")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button_F1_Release_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F1_Release.MouseDown
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("1")
        Command_array(5) = Asc("1")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button_F1_Release_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F1_Release.MouseUp
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("1")
        Command_array(5) = Asc("0")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button_F1_Hold_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F1_Hold.MouseDown
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("1")
        Command_array(5) = Asc("2")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub Button_F1_Hold_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_F1_Hold.MouseUp
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("1")
        Command_array(4) = Asc("1")
        Command_array(5) = Asc("0")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub

    Private Sub DataGridView_PARA2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView_PARA2.Click
        Dim Command_array(5) As Byte
        Command_array(0) = Asc("x")
        Command_array(1) = Asc("x")
        Command_array(2) = Asc("2")
        Command_array(3) = Asc("5")
        Command_array(4) = Asc("0")
        Command_array(5) = Asc("0")
        SerialPort_Gripper.Write(Command_array, 0, 6)
        Delay()
    End Sub
End Class
