Public Class Form1

    'カレントディレクトリ
    Private strCurrentDir As String = System.IO.Directory.GetCurrentDirectory()
    'For TEST
    'Private strCurrentDir As String = "D:\Work0\Waha!WTJ_20160405"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' カレントディレクトリを取得する
        'Dim stCurrentDir As String = System.IO.Directory.GetCurrentDirectory()
        TextBox1.Text = strCurrentDir

        'ListBox1.ItemHeight = 25
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        '1秒間 Sleep
        System.Threading.Thread.Sleep(500)

        Dim ret = MsgBox("　処理を開始します。 処理効率はおよそ 3 [file/sec] です。", MessageBoxButtons.OKCancel)
        Select Case ret
            Case vbOK
                ' continue
            Case vbCancel
                Return
        End Select

        '開始時刻
        Dim timeStampNow As DateTime = System.DateTimeOffset.Now.ToString
        Dim strTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeStampNow)
        ListBox1.Items.Add("> " & strTime & "   Start")

        'カーソル（Wait)
        Me.Cursor = Cursors.WaitCursor

        'カレントディレクトリ以下のファイルをすべて取得する
        'ワイルドカード"*"は、すべてのファイルを意味する
        Dim files As String() = System.IO.Directory.GetFiles(strCurrentDir, "*.wtj", System.IO.SearchOption.AllDirectories)

        'プログレスバー初期設定
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = files.Length

        'ListBox1に結果を表示する
        'ListBox1.Items.AddRange(files)
        Console.Write(files.Length)

        '出力ファイル名称生成（タイムスタンプ付加）
        Dim strTimeStampNow = String.Format("{0:yyyyMMdd_HHmm}", timeStampNow)
        Dim OUTPUT_FILENAME = Me.strCurrentDir & "\wtjStructData_" & strTimeStampNow & ".csv"

        'カレントディレクトリパスの表示
        ListBox1.Items.Add("[CurrentDir] " & strCurrentDir)
        ListBox1.Items.Add("")
        'カレントディレクトリ文字列の長さ
        Dim iLength As Integer = strCurrentDir.Length()

        'ファイル名称リスト 表示
        Dim analyser = New SyntacticAnalysis()
        Dim strFileName = ""
        For i = 0 To files.Length - 1

            'プログレスバー進捗状況
            ProgressBar1.Value = i + 1
            ProgressBar1.Update()
            ProgressBar1.Invalidate()

            '画面更新
            Me.Refresh()
            Application.DoEvents()

            'wtjファイル名取得
            strFileName = Util.Mid(files(i), iLength + 1)
            '1行（wtjファイル名）追加
            ListBox1.Items.Add("[" & i & "] " & strFileName)

            Debug.WriteLine(files(i))

            'WTJデータ構造解析
            '------------------------------------------------
            'If Me.CheckBox1.Checked Then
            '    analyser.Conversion(files(i), OUTPUT_FILENAME, True)
            'Else
            '    analyser.Conversion(files(i), OUTPUT_FILENAME)
            'End If
            '------------------------------------------------
            analyser.Conversion(files(i), OUTPUT_FILENAME, Me.CheckBox1.Checked)

        Next

        ListBox1.Items.Add("> " & files.Length & " ファイルを解析しました。")
        ListBox1.Items.Add("[解析結果] " & OUTPUT_FILENAME)

        '終了時刻
        timeStampNow = System.DateTimeOffset.Now.ToString
        strTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeStampNow)
        ListBox1.Items.Add("> " & strTime & "   End")
        ListBox1.Items.Add(" ")

        'カーソル（Default)
        Me.Cursor = Cursors.Default

        'Form1.ActiveForm.BackColor = Color.LightYellow

    End Sub

End Class

