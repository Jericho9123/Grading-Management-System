Imports System.Text
Imports Guna.UI2.WinForms
Imports MySqlConnector

Public Class Form3
    Dim loginPage As New Form2
    Private ReadOnly connectionString As String = "Server=localhost;Port=3308;Database=casestudy;Uid=root;Pwd=redmercy44;"


    Private Sub overviewBtn_Click(sender As Object, e As EventArgs) Handles overviewBtn.Click
        ActivateButton(overviewBtn, overviewPanel)
    End Sub

    Private Sub gradeBtn_Click(sender As Object, e As EventArgs) Handles gradeBtn.Click
        ActivateButton(gradeBtn, gradingPanel)
    End Sub



    ' Utility method to show the active panel and highlight the active button
    Private Sub ActivateButton(activeButton As Guna.UI2.WinForms.Guna2Button, activePanel As Panel)
        ' Array of buttons (Guna2Button)
        Dim buttons As Guna.UI2.WinForms.Guna2Button() = {
            overviewBtn, gradeBtn
        }

        ' Reset all buttons' background colors
        For Each btn As Guna.UI2.WinForms.Guna2Button In buttons
            btn.FillColor = Color.FromArgb(233, 234, 236)
        Next

        ' Highlight the active button
        activeButton.FillColor = Color.FromArgb(94, 148, 255)

        ' Show the active panel and hide others
        ShowPanel(activePanel)
    End Sub

    ' Utility method to show a specific panel and hide others
    Private Sub ShowPanel(visiblePanel As Panel)
        Dim panels As Panel() = {gradingPanel, overviewPanel}
        For Each panel As Panel In panels
            panel.Visible = (panel Is visiblePanel)
        Next
    End Sub




    Private Sub signOutBtn_Click(sender As Object, e As EventArgs) Handles signOutBtn.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to sign out?", "Sign Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Me.Dispose()
            loginPage.Show()
        End If

    End Sub



    Private Sub loadStudentId()
        Dim query As String = "SELECT student_id FROM students"

        ' Create a new connection to the database
        Using connection As New MySqlConnection(connectionString)
            Dim command As New MySqlCommand(query, connection)

            Try
                connection.Open()
                Dim reader As MySqlDataReader = command.ExecuteReader()

                ' Clear the ComboBox before adding new items
                studentIdBox.Items.Clear()

                ' Load the student IDs into the ComboBox
                While reader.Read()
                    studentIdBox.Items.Add(reader("student_id").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub loadSubject()
        Dim query As String = "SELECT subjects.subject_title " &
                        "FROM classes " &
                        "JOIN subjects ON classes.subject_code = subjects.subject_code " &
                        "WHERE classes.professor_id = @professorId"

        ' Create a new connection to the database
        Using connection As New MySqlConnection(connectionString)
            Dim command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@professorId", Integer.Parse(idLabel.Text))

            Try
                connection.Open()
                Dim reader As MySqlDataReader = command.ExecuteReader()

                ' Clear the ComboBox before adding new items
                subjectBox.Items.Clear()

                ' Load the subject names into the ComboBox
                While reader.Read()
                    subjectBox.Items.Add(reader("subject_title").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub subjectGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gradeGrid.CellContentClick

    End Sub

    Private Sub subjectBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles subjectBox.SelectedIndexChanged

    End Sub


    Private Function FetchAverageCurriculumYear(facultyId As String) As Integer
        Dim query As String = "
        SELECT 
            AVG(s.curriculum_year) AS average_curriculum_year
        FROM 
            classes c
        JOIN 
            subjects s ON c.subject_code = s.subject_code
        WHERE 
            c.professor_id = @facultyId;"

        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    ' Add parameter for faculty ID
                    command.Parameters.AddWithValue("@facultyId", facultyId)

                    Dim result = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        ' Convert the result to an Integer
                        Return Convert.ToInt32(Math.Round(Convert.ToDouble(result)))
                    Else
                        ' Return -1 or another appropriate value if no data is found
                        Return -1
                    End If
                End Using
            End Using
        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return -1 ' Return -1 in case of an error
        End Try
    End Function



    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadSubject()
        loadStudentId()
        loadClasses()
        yearLabel.Text = FetchAverageCurriculumYear(idLabel.Text).ToString()

    End Sub


    Public Function ValidateMidtermFields() As Boolean
        ' List of field names and their corresponding TextBox controls
        Dim midtermFields As Dictionary(Of String, Guna.UI2.WinForms.Guna2TextBox) = New Dictionary(Of String, Guna.UI2.WinForms.Guna2TextBox) From {
        {"Midterm Attendance", midtermAttendance},
        {"Midterm Classwork", midtermClasswork},
        {"Midterm Quiz 1", midtermQuiz1},
        {"Midterm Quiz 2", midtermQuiz2},
        {"Midterm Project", midtermProject},
        {"Midterm Exam", midtermExam},
        {"Midterm Attendance Total", midtermAttendanceTotal},
        {"Midterm Classwork Total", midtermClassworkTotal},
        {"Midterm Quiz 1 Total", midtermQuiz1Total},
        {"Midterm Quiz 2 Total", midtermQuiz2Total},
        {"Midterm Project Total", midtermProjectTotal},
        {"Midterm Exam Total", midtermExamTotal}
    }

        ' Validate each field
        For Each field In midtermFields
            Dim fieldName As String = field.Key
            Dim textBox As Guna.UI2.WinForms.Guna2TextBox = field.Value

            ' Check if empty
            If String.IsNullOrWhiteSpace(textBox.Text) Then
                MessageBox.Show($"{fieldName} cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                textBox.Focus()
                Return False
            End If

            ' Check if numeric
            If Not IsNumeric(textBox.Text) Then
                MessageBox.Show($"{fieldName} must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                textBox.Focus()
                Return False
            End If

            ' Check if non-negative
            If Convert.ToDouble(textBox.Text) < 0 Then
                MessageBox.Show($"{fieldName} cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                textBox.Focus()
                Return False
            End If
        Next

        ' All fields are valid
        Return True
    End Function
    Public Function ValidateFinalFields() As Boolean
        ' List of field names and their corresponding TextBox controls
        Dim finalFields As Dictionary(Of String, Guna.UI2.WinForms.Guna2TextBox) = New Dictionary(Of String, Guna.UI2.WinForms.Guna2TextBox) From {
        {"Final Attendance", finalAttendance},
        {"Final Classwork", finalClasswork},
        {"Final Quiz 1", finalQuiz1},
        {"Final Quiz 2", finalQuiz2},
        {"Final Project", finalProject},
        {"Final Exam", finalExam},
        {"Final Attendance Total", finalAttendanceTotal},
        {"Final Classwork Total", finalClassworkTotal},
        {"Final Quiz 1 Total", finalQuiz1Total},
        {"Final Quiz 2 Total", finalQuiz2Total},
        {"Final Project Total", finalProjectTotal},
        {"Final Exam Total", finalExamTotal}
    }

        ' Validate each field
        For Each field In finalFields
            Dim fieldName As String = field.Key
            Dim textBox As Guna.UI2.WinForms.Guna2TextBox = field.Value

            ' Check if empty
            If String.IsNullOrWhiteSpace(textBox.Text) Then
                MessageBox.Show($"{fieldName} cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                textBox.Focus()
                Return False
            End If

            ' Check if numeric
            If Not IsNumeric(textBox.Text) Then
                MessageBox.Show($"{fieldName} must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                textBox.Focus()
                Return False
            End If

            ' Check if non-negative
            If Convert.ToDouble(textBox.Text) < 0 Then
                MessageBox.Show($"{fieldName} cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                textBox.Focus()
                Return False
            End If
        Next

        ' All fields are valid
        Return True
    End Function

    Private Sub calculateFinal_Click(sender As Object, e As EventArgs) Handles calculateFinal.Click
        If Not ValidateFinalFields() Then
            ' If validation fails, stop further execution
            MsgBox("Validation failed. Please correct the errors and try again.", MsgBoxStyle.Critical, "Validation Error")
            Return
        End If

        Dim connection As New MySqlConnection(connectionString)
        connection.Open()

        Dim command As New MySqlCommand("SELECT component_name, weight FROM weightdetails WHERE grading_id = 2", connection)
        Dim reader As MySqlDataReader = command.ExecuteReader()

        Dim totalWeightedScore As Decimal = 0

        While reader.Read()
            Dim componentName As String = reader("component_name").ToString()
            Dim weight As Decimal = Convert.ToDecimal(reader("weight"))

            Debug.WriteLine("Component: " & componentName & ", Weight: " & weight.ToString("F2"))

            Dim weightedScore As Decimal = 0
            Select Case componentName
                Case "Attendance"
                    Dim attendance As Decimal
                    Dim attendanceTotal As Decimal
                    If Decimal.TryParse(finalAttendance.Text, attendance) AndAlso Decimal.TryParse(finalAttendanceTotal.Text, attendanceTotal) AndAlso attendanceTotal > 0 Then
                        weightedScore = (attendance / attendanceTotal) * weight
                        Debug.WriteLine("Attendance Value: " & attendance.ToString("F2"))
                        Debug.WriteLine("Attendance Total: " & attendanceTotal.ToString("F2"))
                        Debug.WriteLine("Attendance Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Attendance and Attendance Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Classwork"
                    Dim classwork As Decimal
                    Dim classworkTotal As Decimal
                    If Decimal.TryParse(finalClasswork.Text, classwork) AndAlso Decimal.TryParse(finalClassworkTotal.Text, classworkTotal) AndAlso classworkTotal > 0 Then
                        weightedScore = (classwork / classworkTotal) * weight
                        Debug.WriteLine("Classwork Value: " & classwork.ToString("F2"))
                        Debug.WriteLine("Classwork Total: " & classworkTotal.ToString("F2"))
                        Debug.WriteLine("Classwork Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Classwork and Classwork Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Quiz1"
                    Dim quiz1 As Decimal
                    Dim quiz1Total As Decimal
                    If Decimal.TryParse(finalQuiz1.Text, quiz1) AndAlso Decimal.TryParse(finalQuiz1Total.Text, quiz1Total) AndAlso quiz1Total > 0 Then
                        weightedScore = (quiz1 / quiz1Total) * weight
                        Debug.WriteLine("Quiz1 Value: " & quiz1.ToString("F2"))
                        Debug.WriteLine("Quiz1 Total: " & quiz1Total.ToString("F2"))
                        Debug.WriteLine("Quiz1 Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Quiz1 and Quiz1 Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Quiz2"
                    Dim quiz2 As Decimal
                    Dim quiz2Total As Decimal
                    If Decimal.TryParse(finalQuiz2.Text, quiz2) AndAlso Decimal.TryParse(finalQuiz2Total.Text, quiz2Total) AndAlso quiz2Total > 0 Then
                        weightedScore = (quiz2 / quiz2Total) * weight
                        Debug.WriteLine("Quiz2 Value: " & quiz2.ToString("F2"))
                        Debug.WriteLine("Quiz2 Total: " & quiz2Total.ToString("F2"))
                        Debug.WriteLine("Quiz2 Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Quiz2 and Quiz2 Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Project"
                    Dim project As Decimal
                    Dim projectTotal As Decimal
                    If Decimal.TryParse(finalProject.Text, project) AndAlso Decimal.TryParse(finalProjectTotal.Text, projectTotal) AndAlso projectTotal > 0 Then
                        weightedScore = (project / projectTotal) * weight
                        Debug.WriteLine("Project Value: " & project.ToString("F2"))
                        Debug.WriteLine("Project Total: " & projectTotal.ToString("F2"))
                        Debug.WriteLine("Project Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Project and Project Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Exam"
                    Dim exam As Decimal
                    Dim examTotal As Decimal
                    If Decimal.TryParse(finalExam.Text, exam) AndAlso Decimal.TryParse(finalExamTotal.Text, examTotal) AndAlso examTotal > 0 Then
                        weightedScore = (exam / examTotal) * weight
                        Debug.WriteLine("Exam Value: " & exam.ToString("F2"))
                        Debug.WriteLine("Exam Total: " & examTotal.ToString("F2"))
                        Debug.WriteLine("Exam Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Exam and Exam Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If
            End Select

            totalWeightedScore += weightedScore
            Debug.WriteLine("Total Weighted Score after " & componentName & ": " & totalWeightedScore.ToString("F2"))
        End While

        reader.Close()
        connection.Close()

        Debug.WriteLine("Final Total Weighted Score: " & totalWeightedScore.ToString("F2"))
        finalGrade.Text = totalWeightedScore.ToString("F2")



    End Sub

    Private Sub calculateMidterm_Click(sender As Object, e As EventArgs) Handles calculateMidterm.Click
        If Not ValidateMidtermFields() Then
            MsgBox("Validation failed. Please correct the errors and try again.", MsgBoxStyle.Critical, "Validation Error")
            Return
        End If

        Dim connection As New MySqlConnection(connectionString)
        connection.Open()

        Dim command As New MySqlCommand("SELECT component_name, weight FROM weightdetails WHERE grading_id = 2", connection)
        Dim reader As MySqlDataReader = command.ExecuteReader()

        Dim totalWeightedScore As Decimal = 0

        While reader.Read()
            Dim componentName As String = reader("component_name").ToString()
            Dim weight As Decimal = Convert.ToDecimal(reader("weight"))

            Debug.WriteLine("Component: " & componentName & ", Weight: " & weight.ToString("F2"))

            Dim weightedScore As Decimal = 0
            Select Case componentName
                Case "Attendance"
                    Dim attendance As Decimal
                    Dim attendanceTotal As Decimal
                    If Decimal.TryParse(midtermAttendance.Text, attendance) AndAlso Decimal.TryParse(midtermAttendanceTotal.Text, attendanceTotal) AndAlso attendanceTotal > 0 Then
                        weightedScore = (attendance / attendanceTotal) * weight
                        Debug.WriteLine("Attendance Value: " & attendance.ToString("F2"))
                        Debug.WriteLine("Attendance Total: " & attendanceTotal.ToString("F2"))
                        Debug.WriteLine("Attendance Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Attendance and Attendance Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Classwork"
                    Dim classwork As Decimal
                    Dim classworkTotal As Decimal
                    If Decimal.TryParse(midtermClasswork.Text, classwork) AndAlso Decimal.TryParse(midtermClassworkTotal.Text, classworkTotal) AndAlso classworkTotal > 0 Then
                        weightedScore = (classwork / classworkTotal) * weight
                        Debug.WriteLine("Classwork Value: " & classwork.ToString("F2"))
                        Debug.WriteLine("Classwork Total: " & classworkTotal.ToString("F2"))
                        Debug.WriteLine("Classwork Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Classwork and Classwork Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Quiz1"
                    Dim quiz1 As Decimal
                    Dim quiz1Total As Decimal
                    If Decimal.TryParse(midtermQuiz1.Text, quiz1) AndAlso Decimal.TryParse(midtermQuiz1Total.Text, quiz1Total) AndAlso quiz1Total > 0 Then
                        weightedScore = (quiz1 / quiz1Total) * weight
                        Debug.WriteLine("Quiz1 Value: " & quiz1.ToString("F2"))
                        Debug.WriteLine("Quiz1 Total: " & quiz1Total.ToString("F2"))
                        Debug.WriteLine("Quiz1 Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Quiz1 and Quiz1 Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Quiz2"
                    Dim quiz2 As Decimal
                    Dim quiz2Total As Decimal
                    If Decimal.TryParse(midtermQuiz2.Text, quiz2) AndAlso Decimal.TryParse(midtermQuiz2Total.Text, quiz2Total) AndAlso quiz2Total > 0 Then
                        weightedScore = (quiz2 / quiz2Total) * weight
                        Debug.WriteLine("Quiz2 Value: " & quiz2.ToString("F2"))
                        Debug.WriteLine("Quiz2 Total: " & quiz2Total.ToString("F2"))
                        Debug.WriteLine("Quiz2 Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Quiz2 and Quiz2 Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Project"
                    Dim project As Decimal
                    Dim projectTotal As Decimal
                    If Decimal.TryParse(midtermProject.Text, project) AndAlso Decimal.TryParse(midtermProjectTotal.Text, projectTotal) AndAlso projectTotal > 0 Then
                        weightedScore = (project / projectTotal) * weight
                        Debug.WriteLine("Project Value: " & project.ToString("F2"))
                        Debug.WriteLine("Project Total: " & projectTotal.ToString("F2"))
                        Debug.WriteLine("Project Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Project and Project Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If

                Case "Exam"
                    Dim exam As Decimal
                    Dim examTotal As Decimal
                    If Decimal.TryParse(midtermExam.Text, exam) AndAlso Decimal.TryParse(midtermExamTotal.Text, examTotal) AndAlso examTotal > 0 Then
                        weightedScore = (exam / examTotal) * weight
                        Debug.WriteLine("Exam Value: " & exam.ToString("F2"))
                        Debug.WriteLine("Exam Total: " & examTotal.ToString("F2"))
                        Debug.WriteLine("Exam Weighted Score: " & weightedScore.ToString("F2"))
                    Else
                        MsgBox("Please enter valid numbers for Exam and Exam Total.", MsgBoxStyle.Critical, "Input Error")
                        reader.Close()
                        connection.Close()
                        Return
                    End If
            End Select

            totalWeightedScore += weightedScore

        End While

        reader.Close()
        connection.Close()

        midtermGrade.Text = totalWeightedScore.ToString("F2")



    End Sub

    Private Sub saveBtn_Click(sender As Object, e As EventArgs) Handles saveBtn.Click
        If subjectBox.SelectedIndex = -1 OrElse studentIdBox.SelectedIndex = -1 Then
            MsgBox("Please select a subject and a student ID. Both fields are required.", MsgBoxStyle.Critical, "Input Error")
            Return
        End If

        If midtermGrade.Text = "0" And finalGrade.Text = "0" Then
            Return
        End If
        ' Retrieve the selected values
        Dim selectedStudentId As String = studentIdBox.SelectedItem.ToString()
        Dim selectedSubjectTitle As String = subjectBox.SelectedItem.ToString()

        ' Create a connection to the database
        Dim connection As New MySqlConnection(connectionString)
        Dim selectedClassId As Integer?

        Try
            connection.Open()

            ' Query to fetch the class_id based on the subject title
            Dim query As String = "SELECT class_id FROM classes WHERE subject_code = (SELECT subject_code FROM subjects WHERE subject_title = @SubjectTitle) AND professor_id = @ProfessorId"
            Dim command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@SubjectTitle", selectedSubjectTitle)
            command.Parameters.AddWithValue("@ProfessorId", idLabel.Text)
            ' Execute the query and get the result
            Dim result As Object = command.ExecuteScalar()

            If result IsNot Nothing Then
                selectedClassId = Convert.ToInt32(result)
            Else
                MsgBox("No class found for the selected subject title.", MsgBoxStyle.Information, "Lookup Result")
                Return
            End If

            ' Retrieve midterm grade values from the textboxes
            Dim attendance As Integer = If(String.IsNullOrEmpty(midtermAttendance.Text), 0, Convert.ToInt32(midtermAttendance.Text))
            Dim classwork As Integer = If(String.IsNullOrEmpty(midtermClasswork.Text), 0, Convert.ToInt32(midtermClasswork.Text))
            Dim quiz1 As Integer = If(String.IsNullOrEmpty(midtermQuiz1.Text), 0, Convert.ToInt32(midtermQuiz1.Text))
            Dim quiz2 As Integer = If(String.IsNullOrEmpty(midtermQuiz2.Text), 0, Convert.ToInt32(midtermQuiz2.Text))
            Dim project As Integer = If(String.IsNullOrEmpty(midtermProject.Text), 0, Convert.ToInt32(midtermProject.Text))
            Dim exam As Integer = If(String.IsNullOrEmpty(midtermExam.Text), 0, Convert.ToInt32(midtermExam.Text))
            Dim grade As Double
            If Double.TryParse(midtermGrade.Text, grade) Then
                ' Successfully parsed midtermGrade as a double
            Else
                ' Handle parsing failure, set grade to a default or display an error
                grade = 0
            End If

            ' Determine the remarks based on the grade
            Dim midtermRemarks As String = If(grade >= 75, "Passed", "Failed")

            ' Insert midterm data
            Dim insertMidtermQuery As String = "INSERT INTO grades (student_id, class_id, term_name, attendance, cw, quiz1, quiz2, project, exam, grade, remarks) " &
                                           "VALUES (@StudentId, @ClassId, @TermName, @Attendance, @Classwork, @Quiz1, @Quiz2, @Project, @Exam, @Grade, @Remarks)"
            Dim command2 As New MySqlCommand(insertMidtermQuery, connection)
            command2.Parameters.AddWithValue("@StudentId", selectedStudentId)
            command2.Parameters.AddWithValue("@ClassId", selectedClassId)
            command2.Parameters.AddWithValue("@TermName", "midterm")
            command2.Parameters.AddWithValue("@Attendance", attendance)
            command2.Parameters.AddWithValue("@Classwork", classwork)
            command2.Parameters.AddWithValue("@Quiz1", quiz1)
            command2.Parameters.AddWithValue("@Quiz2", quiz2)
            command2.Parameters.AddWithValue("@Project", project)
            command2.Parameters.AddWithValue("@Exam", exam)
            command2.Parameters.AddWithValue("@Grade", grade)
            command2.Parameters.AddWithValue("@Remarks", midtermRemarks)

            command2.ExecuteNonQuery()
            MsgBox("Midterm record has been saved successfully.", MsgBoxStyle.Information, "Save Complete")

            ' Retrieve final grade values from the textboxes
            Dim attendance2 As Integer = If(String.IsNullOrEmpty(finalAttendance.Text), 0, Convert.ToInt32(finalAttendance.Text))
            Dim classwork2 As Integer = If(String.IsNullOrEmpty(finalClasswork.Text), 0, Convert.ToInt32(finalClasswork.Text))
            Dim quiz12 As Integer = If(String.IsNullOrEmpty(finalQuiz1.Text), 0, Convert.ToInt32(finalQuiz1.Text))
            Dim quiz22 As Integer = If(String.IsNullOrEmpty(finalQuiz2.Text), 0, Convert.ToInt32(finalQuiz2.Text))
            Dim project2 As Integer = If(String.IsNullOrEmpty(finalProject.Text), 0, Convert.ToInt32(finalProject.Text))
            Dim exam2 As Integer = If(String.IsNullOrEmpty(finalExam.Text), 0, Convert.ToInt32(finalExam.Text))
            Dim grade2 As Double
            If Double.TryParse(finalGrade.Text, grade2) Then
                ' Successfully parsed finalGrade as a double
            Else
                ' Handle parsing failure, set grade2 to a default or display an error
                grade2 = 0
            End If

            ' Determine the remarks for the final grade
            Dim finalRemarks As String = If(grade2 >= 75, "Passed", "Failed")

            ' Insert final data
            Dim insertFinalQuery As String = "INSERT INTO grades (student_id, class_id, term_name, attendance, cw, quiz1, quiz2, project, exam, grade, remarks) " &
                                            "VALUES (@StudentId, @ClassId, @TermName, @Attendance, @Classwork, @Quiz1, @Quiz2, @Project, @Exam, @Grade, @Remarks)"
            Dim command3 As New MySqlCommand(insertFinalQuery, connection)
            command3.Parameters.AddWithValue("@StudentId", selectedStudentId)
            command3.Parameters.AddWithValue("@ClassId", selectedClassId)
            command3.Parameters.AddWithValue("@TermName", "final")
            command3.Parameters.AddWithValue("@Attendance", attendance2)
            command3.Parameters.AddWithValue("@Classwork", classwork2)
            command3.Parameters.AddWithValue("@Quiz1", quiz12)
            command3.Parameters.AddWithValue("@Quiz2", quiz22)
            command3.Parameters.AddWithValue("@Project", project2)
            command3.Parameters.AddWithValue("@Exam", exam2)
            command3.Parameters.AddWithValue("@Grade", grade2)
            command3.Parameters.AddWithValue("@Remarks", finalRemarks)

            command3.ExecuteNonQuery()
            MsgBox("Final record has been saved successfully.", MsgBoxStyle.Information, "Save Complete")

        Catch ex As MySqlException
            MsgBox("An error occurred while saving the records: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            connection.Close()
        End Try
    End Sub



    Private Sub loadClasses()
        Dim query As String = "SELECT classes.class_id, subjects.subject_title " &
                          "FROM classes " &
                          "INNER JOIN subjects ON classes.subject_code = subjects.subject_code " &
                          "WHERE classes.professor_id = @professorId"

        ' Create a new connection to the database
        Using connection As New MySqlConnection(connectionString)
            Dim command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@professorId", Integer.Parse(idLabel.Text))

            Try
                connection.Open()
                Dim reader As MySqlDataReader = command.ExecuteReader()

                ' Clear the ComboBox before adding new items
                classBox.Items.Clear()
                Dim dataTable As New DataTable()
                dataTable.Load(reader)

                ' Bind the ComboBox
                classBox.DataSource = dataTable
                classBox.DisplayMember = "subject_title" ' Corrected column name
                classBox.ValueMember = "class_id"       ' The underlying value
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub


    Private Sub classBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles classBox.SelectedIndexChanged
        ' Ensure a valid selection is made
        If classBox.SelectedValue IsNot Nothing AndAlso IsNumeric(classBox.SelectedValue) Then
            Dim selectedClassId As Integer = Integer.Parse(classBox.SelectedValue.ToString())
            LoadGradesByClassId(selectedClassId)
        End If
    End Sub



    Private Sub LoadGradesByClassId(classId As Integer)
        Dim query As String = "SELECT * FROM student_grades WHERE class_id = @classId"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@classId", classId)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        gradeGrid.Rows.Clear()
                        While reader.Read()
                            gradeGrid.Rows.Add(reader("student_id").ToString(),
                                           reader("student_name").ToString(),
                                           reader("term_name").ToString(),
                                           reader("attendance").ToString(),
                                           reader("cw").ToString(),
                                           reader("quiz1").ToString(),
                                           reader("quiz2").ToString(),
                                           reader("project").ToString(),
                                           reader("exam").ToString(),
                                           reader("grade").ToString(),
                                           reader("remarks").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub overviewPanel_Paint(sender As Object, e As PaintEventArgs) Handles overviewPanel.Paint

    End Sub

    Private Sub generateReportBtn_Click(sender As Object, e As EventArgs) Handles generateReportBtn.Click
        ' Extract data from labels
        Dim facultyId As String = idLabel.Text
        Dim department As String = departmentLabel.Text
        Dim name As String = nameLabel.Text
        Dim academicYear As String = yearLabel.Text

        ' Create the report content
        Dim reportContent As New StringBuilder()
        reportContent.AppendLine("Report")
        reportContent.AppendLine("====================================")
        reportContent.AppendLine($"Faculty ID: {facultyId}")
        reportContent.AppendLine($"Name: {name}")
        reportContent.AppendLine($"Department: {department}")
        reportContent.AppendLine($"Academic Year: {academicYear}")
        reportContent.AppendLine("====================================")
        reportContent.AppendLine("Additional Report Content Goes Here")
        reportContent.AppendLine("====================================")

        ' Display the report in a message box
        MessageBox.Show(reportContent.ToString(), "Generated Report", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Optional: Save the report to a file
        Dim filePath As String = "C:\Users\PLPASIG\Downloads\Report.txt" ' Modify the path as needed
        Try
            System.IO.File.WriteAllText(filePath, reportContent.ToString())
            MessageBox.Show("Report saved successfully to " & filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error saving the report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub gradingPanel_Paint(sender As Object, e As PaintEventArgs) Handles gradingPanel.Paint

    End Sub
End Class