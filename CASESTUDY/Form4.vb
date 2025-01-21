Imports MySqlConnector

Public Class Form4
    Private ReadOnly connectionString As String = "Server=localhost;Port=3308;Database=casestudy;Uid=root;Pwd=redmercy44;"

    Private Sub overviewBtn_Click(sender As Object, e As EventArgs) Handles overviewBtn.Click
        ActivateButton(overviewBtn, overviewPanel)
    End Sub

    Private Sub subjectBtn_Click(sender As Object, e As EventArgs) Handles subjectBtn.Click
        ActivateButton(subjectBtn, subjectPanel)
        LoadSubjects()
        subjectCount.Text = GetSubjectCount()
    End Sub

    Private Sub sectionBtn_Click(sender As Object, e As EventArgs) Handles sectionBtn.Click
        ActivateButton(sectionBtn, sectionPanel)
        LoadSections()
        sectionCount.Text = GetSectionCount()


    End Sub

    Private Sub studentBtn_Click(sender As Object, e As EventArgs) Handles studentBtn.Click
        ActivateButton(studentBtn, studentPanel)
        LoadStudents()
        LoadSectionBox()
        studentCount.Text = GetStudentCount()
    End Sub

    Private Sub professorBtn_Click(sender As Object, e As EventArgs) Handles professorBtn.Click
        ActivateButton(professorBtn, professorPanel)
        LoadProfessors()
        professorCount.Text = GetProfessorCount()


    End Sub

    Private Sub classesBtn_Click(sender As Object, e As EventArgs) Handles classesBtn.Click
        ActivateButton(classesBtn, classesPanel)
        loadRecords()
        loadBoxes()
        classCount.Text = GetClassCount()

    End Sub

    ' Utility method to show the active panel and highlight the active button
    Private Sub ActivateButton(activeButton As Guna.UI2.WinForms.Guna2Button, activePanel As Panel)
        ' Array of buttons (Guna2Button)
        Dim buttons As Guna.UI2.WinForms.Guna2Button() = {
            overviewBtn, subjectBtn, sectionBtn, studentBtn, professorBtn, classesBtn
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
        Dim panels As Panel() = {subjectPanel, sectionPanel, studentPanel, professorPanel, classesPanel, overviewPanel}
        For Each panel As Panel In panels
            panel.Visible = (panel Is visiblePanel)
        Next
    End Sub

    Private Sub LoadSections()
        Dim query As String = "SELECT * FROM sections"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        sectionGrid.Rows.Clear()
                        While reader.Read()
                            sectionGrid.Rows.Add(reader("section_id").ToString(), reader("section_name").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As MySqlException
            ShowError("Database error: " & ex.Message)
        End Try
    End Sub
    Private Sub ShowError(message As String)
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub sectionAddBtn_Click(sender As Object, e As EventArgs) Handles sectionAddBtn.Click
        Dim sectionName As String = sectionNameField.Text.Trim()
        Dim sectionId As Integer

        ' Validate Section ID
        If Not Integer.TryParse(sectionIdField.Text.Trim(), sectionId) Then
            ShowError("Section ID must be a valid integer.")
            Return
        End If

        ' Validate Section Name
        If String.IsNullOrEmpty(sectionName) Then
            ShowError("Section name cannot be empty.")
            Return
        End If

        ' Insert Query
        Dim query As String = "INSERT INTO sections (section_id, section_name) VALUES (@sectionId, @sectionName)"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@sectionId", sectionId)
                    command.Parameters.AddWithValue("@sectionName", sectionName)
                    command.ExecuteNonQuery()
                End Using
            End Using
            ShowSuccess("Section added successfully.")
            LoadSections()
            sectionCount.Text = GetSectionCount()
            sectionsCount.Text = GetSectionCount()

            sectionNameField.Text = ""
            sectionIdField.Text = ""
        Catch ex As MySqlException
            ShowError("Database error: " & ex.Message)
        End Try
    End Sub

    Private Sub sectionUpdateBtn_Click(sender As Object, e As EventArgs) Handles sectionUpdateBtn.Click
        Dim sectionName As String = sectionNameField.Text.Trim()
        Dim sectionId As Integer

        ' Validate Section ID
        If Not Integer.TryParse(sectionIdField.Text.Trim(), sectionId) Then
            ShowError("Section ID must be a valid integer.")
            Return
        End If

        ' Validate Section Name
        If String.IsNullOrEmpty(sectionName) Then
            ShowError("Section name cannot be empty.")
            Return
        End If

        ' Update Query
        Dim query As String = "UPDATE sections SET section_name = @sectionName WHERE section_id = @sectionId"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@sectionId", sectionId)
                    command.Parameters.AddWithValue("@sectionName", sectionName)
                    If command.ExecuteNonQuery() = 0 Then
                        ShowError("No section found with the given ID.")
                    Else
                        ShowSuccess("Section updated successfully.")
                        LoadSections()
                        sectionNameField.Text = ""
                        sectionIdField.Text = ""
                    End If
                End Using
            End Using
        Catch ex As MySqlException
            ShowError("Database error: " & ex.Message)
        End Try
    End Sub

    Private Sub sectionDeleteBtn_Click(sender As Object, e As EventArgs) Handles sectionDeleteBtn.Click
        Dim sectionId As Integer

        ' Validate Section ID
        If Not Integer.TryParse(sectionIdField.Text.Trim(), sectionId) Then
            ShowError("Section ID must be a valid integer.")
            Return
        End If

        ' Delete Query
        Dim query As String = "DELETE FROM sections WHERE section_id = @sectionId"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@sectionId", sectionId)
                    If command.ExecuteNonQuery() = 0 Then
                        ShowError("No section found with the given ID.")
                    Else
                        ShowSuccess("Section deleted successfully.")
                        LoadSections()
                        sectionNameField.Text = ""
                        sectionIdField.Text = ""
                        sectionCount.Text = GetSectionCount()
                        sectionsCount.Text = GetSectionCount()
                    End If
                End Using
            End Using
        Catch ex As MySqlException
            ShowError("Database error: " & ex.Message)
        End Try
    End Sub


    Private Sub ShowSuccess(message As String)
        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub sectionClearBtn_Click(sender As Object, e As EventArgs) Handles sectionClearBtn.Click
        sectionNameField.Text = ""
        sectionIdField.Text = ""

    End Sub

    Private Sub sectionIdField_KeyPress(sender As Object, e As KeyPressEventArgs) Handles sectionIdField.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub subjectGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles subjectGrid.CellContentClick

    End Sub

    ' //////////////////////////////////////////////////////////////////////////////////////






    Private Sub LoadSubjects()
        Dim query As String = "SELECT * FROM subjects"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        subjectGrid.Rows.Clear()
                        While reader.Read()
                            subjectGrid.Rows.Add(reader("subject_code").ToString(), reader("subject_title").ToString(), reader("curriculum_year").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As MySqlException
            ShowError("Database error: " & ex.Message)
        End Try
    End Sub

    Private Sub subjectAddBtn_Click(sender As Object, e As EventArgs) Handles subjectAddBtn.Click
        If ValidateSubjectFields() Then
            Dim subjectCode As Integer = Integer.Parse(subjectCodeField.Text)
            Dim subjectTitle As String = subjectNameField.Text.Trim()
            Dim curriculumYear As String = subjectYearField.Text.Trim()

            Dim query As String = "INSERT INTO subjects (subject_code, subject_title, curriculum_year) VALUES (@code, @title, @year)"
            Try
                Using conn As New MySqlConnection(connectionString)
                    conn.Open()
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@code", subjectCode)
                        cmd.Parameters.AddWithValue("@title", subjectTitle)
                        cmd.Parameters.AddWithValue("@year", curriculumYear)
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Subject added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadSubjects() ' Refresh the grid after adding a new subject
                        subjectCount.Text = GetSubjectCount()
                        subjectsCount.Text = GetSubjectCount()

                        subjectCodeField.Text = ""
                        subjectNameField.Text = ""
                        subjectYearField.Text = ""
                    End Using
                End Using
            Catch ex As MySqlException When ex.Number = 1062
                MessageBox.Show("The subject code already exists. Please use a unique code.", "Duplicate Key Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                ShowError($"An unexpected error occurred: {ex.Message}")
            End Try
        End If
    End Sub

    ' Updating a subject
    Private Sub subjectUpdateBtn_Click(sender As Object, e As EventArgs) Handles subjectUpdateBtn.Click
        If ValidateSubjectFields() Then
            Dim subjectCode As Integer = Integer.Parse(subjectCodeField.Text)
            Dim subjectTitle As String = subjectNameField.Text.Trim()
            Dim curriculumYear As String = subjectYearField.Text.Trim()

            Dim query As String = "UPDATE subjects SET subject_title = @title, curriculum_year = @year WHERE subject_code = @code"
            Try
                Using conn As New MySqlConnection(connectionString)
                    conn.Open()
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@code", subjectCode)
                        cmd.Parameters.AddWithValue("@title", subjectTitle)
                        cmd.Parameters.AddWithValue("@year", curriculumYear)
                        Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                        If rowsAffected > 0 Then
                            MessageBox.Show("Subject updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadSubjects() ' Refresh the grid after updating the subject
                            subjectCodeField.Text = ""
                            subjectNameField.Text = ""
                            subjectYearField.Text = ""
                        Else
                            MessageBox.Show("No record found with the given subject code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                ShowError($"An unexpected error occurred: {ex.Message}")
            End Try
        End If
    End Sub

    ' Deleting a subject
    Private Sub subjectDeleteBtn_Click(sender As Object, e As EventArgs) Handles subjectDeleteBtn.Click
        Dim subjectCode As Integer
        If String.IsNullOrEmpty(subjectCodeField.Text.Trim()) OrElse Not Integer.TryParse(subjectCodeField.Text, subjectCode) Then
            MessageBox.Show("Invalid subject code. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result = MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Dim query As String = "DELETE FROM subjects WHERE subject_code = @code"
            Try
                Using conn As New MySqlConnection(connectionString)
                    conn.Open()
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@code", subjectCode)
                        Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                        If rowsAffected > 0 Then
                            MessageBox.Show("Subject deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            LoadSubjects() ' Refresh the grid after deleting the subject
                            subjectCodeField.Text = ""
                            subjectNameField.Text = ""
                            subjectYearField.Text = ""
                            subjectCount.Text = GetSubjectCount()
                            subjectsCount.Text = GetSubjectCount()
                        Else
                            MessageBox.Show("No record found with the given subject code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                ShowError($"An unexpected error occurred: {ex.Message}")
            End Try
        End If
    End Sub


    Private Function ValidateSubjectFields() As Boolean
        If String.IsNullOrEmpty(subjectCodeField.Text.Trim()) OrElse
           String.IsNullOrEmpty(subjectNameField.Text.Trim()) OrElse
           String.IsNullOrEmpty(subjectYearField.Text.Trim()) Then
            MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If Not Integer.TryParse(subjectCodeField.Text, Nothing) Then
            MessageBox.Show("Invalid subject code. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Return True
    End Function

    ' Clearing input fields
    Private Sub subjectClearBtn_Click(sender As Object, e As EventArgs) Handles subjectClearBtn.Click
        subjectCodeField.Text = ""
        subjectNameField.Text = ""
        subjectYearField.Text = ""
    End Sub

    Private Sub subjectCodeField_KeyPress(sender As Object, e As KeyPressEventArgs) Handles subjectCodeField.KeyPress
        ' Allow only digit characters. If a non-digit character is pressed, consume the input.
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub subjectYearField_KeyPress(sender As Object, e As KeyPressEventArgs) Handles subjectYearField.KeyPress
        ' Allow only digit characters. If a non-digit character is pressed, consume the input.
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub





    '/////////////////////////////////////////////////////////////////////////////////////////////////////////



    Private Sub LoadStudents()
        Dim query As String = "SELECT * FROM students"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        studentGrid.Rows.Clear()
                        While reader.Read()
                            studentGrid.Rows.Add(reader("student_id").ToString(),
                                             reader("student_name").ToString(),
                                             reader("course_year_section").ToString(),
                                             reader("department").ToString(),
                                             reader("username").ToString(),
                                             reader("password").ToString(),
                                             reader("pin").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As MySqlException
            ShowError("Database error: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadSectionBox()
        ' Replace with your actual connection string

        Dim query As String = "SELECT section_name FROM sections"

        Using connection As New MySqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        ' Clear existing items in case this is called multiple times
                        studentSectionBox.Items.Clear()

                        While reader.Read()
                            ' Add each subject_title to the ComboBox
                            studentSectionBox.Items.Add(reader("section_name").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub studentAddBtn_Click(sender As Object, e As EventArgs) Handles studentAddBtn.Click
        Try
            ' Check if all fields are filled
            If Not AreFieldsFilled() Then
                MessageBox.Show("Please fill in all fields.")
                Return
            End If

            Dim studentId As String = studentIdField.Text
            Dim studentName As String = studentNameField.Text
            Dim courseYearSection As String = studentSectionBox.Text ' Get selected value from combobox
            Dim department As String = studentDepartmentField.Text
            Dim username As String = studentUsernameField.Text
            Dim password As String = studentPasswordField.Text
            Dim pinValue As String = studentPinField.Text

            ' Validate if the pin exists
            Dim queryCheckPin As String = "SELECT COUNT(*) FROM pins WHERE pin = @pin"
            Using conn As New MySqlConnection(connectionString),
              cmdCheckPin As New MySqlCommand(queryCheckPin, conn)

                cmdCheckPin.Parameters.AddWithValue("@pin", pinValue)
                conn.Open()

                Dim pinExists As Integer = CInt(cmdCheckPin.ExecuteScalar())
                conn.Close()

                If pinExists > 0 Then
                    MessageBox.Show("The pin already exists. Please use a different pin.")
                    Return
                End If
            End Using

            ' Insert the pin into pins table
            Dim queryInsertPin As String = "INSERT INTO pins (pin) VALUES (@pin)"
            Using conn As New MySqlConnection(connectionString),
              cmdInsertPin As New MySqlCommand(queryInsertPin, conn)

                cmdInsertPin.Parameters.AddWithValue("@pin", pinValue)
                conn.Open()
                cmdInsertPin.ExecuteNonQuery()
                conn.Close()
            End Using

            ' Insert the student record into students table
            Dim queryInsertStudent As String = "INSERT INTO students (student_id, student_name, course_year_section, department, username, password, pin) VALUES (@studentId, @studentName, @courseYearSection, @department, @username, @password, @pin)"
            Using conn As New MySqlConnection(connectionString),
              cmdInsertStudent As New MySqlCommand(queryInsertStudent, conn)

                cmdInsertStudent.Parameters.AddWithValue("@studentId", studentId)
                cmdInsertStudent.Parameters.AddWithValue("@studentName", studentName)
                cmdInsertStudent.Parameters.AddWithValue("@courseYearSection", courseYearSection)
                cmdInsertStudent.Parameters.AddWithValue("@department", department)
                cmdInsertStudent.Parameters.AddWithValue("@username", username)
                cmdInsertStudent.Parameters.AddWithValue("@password", password)
                cmdInsertStudent.Parameters.AddWithValue("@pin", pinValue)
                conn.Open()
                cmdInsertStudent.ExecuteNonQuery()
                conn.Close()

                MessageBox.Show("Student added successfully!")
                LoadStudents()
                studentCount.Text = GetStudentCount()
                studentsCount.Text = GetStudentCount()

                ' Clear the input fields
                studentIdField.Text = ""
                studentNameField.Text = ""
                studentSectionBox.SelectedIndex = -1 ' Reset combobox selection
                studentDepartmentField.Text = ""
                studentUsernameField.Text = ""
                studentPasswordField.Text = ""
                studentPinField.Text = ""
            End Using

        Catch ex As MySqlException
            ' Catch SQL-related errors
            MessageBox.Show("A database error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Catch all other errors
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub





    ' Update Button Implementation
    Private Sub studentUpdateBtn_Click(sender As Object, e As EventArgs) Handles studentUpdateBtn.Click
        Try
            ' Check if all fields are filled except the pin
            If String.IsNullOrEmpty(studentIdField.Text) OrElse String.IsNullOrEmpty(studentNameField.Text) OrElse
           String.IsNullOrEmpty(studentSectionBox.Text) OrElse String.IsNullOrEmpty(studentDepartmentField.Text) OrElse
           String.IsNullOrEmpty(studentUsernameField.Text) OrElse String.IsNullOrEmpty(studentPasswordField.Text) Then
                MessageBox.Show("Please fill in all fields.")
                Return
            End If

            Dim studentId As String = studentIdField.Text
            Dim studentName As String = studentNameField.Text
            Dim courseYearSection As String = studentSectionBox.Text
            Dim department As String = studentDepartmentField.Text
            Dim username As String = studentUsernameField.Text
            Dim password As String = studentPasswordField.Text

            ' Check if the student ID exists
            Dim queryCheckStudent As String = "SELECT COUNT(*) FROM students WHERE student_id = @studentId"
            Using conn As New MySqlConnection(connectionString),
              cmdCheckStudent As New MySqlCommand(queryCheckStudent, conn)

                cmdCheckStudent.Parameters.AddWithValue("@studentId", studentId)
                conn.Open()

                Dim studentExists As Integer = CInt(cmdCheckStudent.ExecuteScalar())
                conn.Close()

                If studentExists = 0 Then
                    MessageBox.Show("Student ID not found. Cannot update.")
                    Return
                End If
            End Using

            ' Update the student record, excluding the pin
            Dim queryUpdateStudent As String = "
        UPDATE students
        SET student_name = @studentName,
            course_year_section = @courseYearSection,
            department = @department,
            username = @username,
            password = @password
        WHERE student_id = @studentId"

            Using conn As New MySqlConnection(connectionString),
              cmdUpdateStudent As New MySqlCommand(queryUpdateStudent, conn)

                cmdUpdateStudent.Parameters.AddWithValue("@studentName", studentName)
                cmdUpdateStudent.Parameters.AddWithValue("@courseYearSection", courseYearSection)
                cmdUpdateStudent.Parameters.AddWithValue("@department", department)
                cmdUpdateStudent.Parameters.AddWithValue("@username", username)
                cmdUpdateStudent.Parameters.AddWithValue("@password", password)
                cmdUpdateStudent.Parameters.AddWithValue("@studentId", studentId)

                conn.Open()
                cmdUpdateStudent.ExecuteNonQuery()
                conn.Close()

                MessageBox.Show("Student updated successfully!")
                LoadStudents()

                ' Clear the input fields
                studentIdField.Text = ""
                studentNameField.Text = ""
                studentSectionBox.Text = ""
                studentDepartmentField.Text = ""
                studentUsernameField.Text = ""
                studentPasswordField.Text = ""
                studentPinField.Text = "" ' Optionally clear the pin field if it's part of the form
            End Using

        Catch ex As MySqlException
            ' Catch SQL-related errors
            MessageBox.Show("A database error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Catch all other errors
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub studentDeleteBtn_Click(sender As Object, e As EventArgs) Handles studentDeleteBtn.Click
        Try
            ' Ensure the student ID field is not empty
            If String.IsNullOrEmpty(studentIdField.Text) Then
                MessageBox.Show("Please enter the student ID to delete.")
                Return
            End If

            Dim studentId As String = studentIdField.Text
            Dim pinValue As String = String.Empty

            ' Confirm deletion
            Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If confirmResult = DialogResult.No Then
                Return
            End If

            ' Check if the student ID exists and retrieve the associated pin
            Dim queryCheckStudent As String = "SELECT pin FROM students WHERE student_id = @studentId"
            Using conn As New MySqlConnection(connectionString),
              cmdCheckStudent As New MySqlCommand(queryCheckStudent, conn)

                cmdCheckStudent.Parameters.AddWithValue("@studentId", studentId)
                conn.Open()

                Dim reader As MySqlDataReader = cmdCheckStudent.ExecuteReader()
                If reader.Read() Then
                    pinValue = reader("pin").ToString()
                Else
                    MessageBox.Show("Student ID not found.")
                    conn.Close()
                    Return
                End If

                conn.Close()
            End Using

            ' Delete the pin from the pins table
            If Not String.IsNullOrEmpty(pinValue) Then
                Dim queryDeletePin As String = "DELETE FROM pins WHERE pin = @pin"
                Using conn As New MySqlConnection(connectionString),
                  cmdDeletePin As New MySqlCommand(queryDeletePin, conn)

                    cmdDeletePin.Parameters.AddWithValue("@pin", pinValue)
                    conn.Open()
                    cmdDeletePin.ExecuteNonQuery()
                    conn.Close()
                End Using
            End If

            ' Delete the student record from the students table
            Dim queryDeleteStudent As String = "DELETE FROM students WHERE student_id = @studentId"
            Using conn As New MySqlConnection(connectionString),
              cmdDeleteStudent As New MySqlCommand(queryDeleteStudent, conn)

                cmdDeleteStudent.Parameters.AddWithValue("@studentId", studentId)
                conn.Open()
                cmdDeleteStudent.ExecuteNonQuery()
                conn.Close()

                MessageBox.Show("Student deleted successfully!")
                LoadStudents()
                studentCount.Text = GetStudentCount()
                studentsCount.Text = GetStudentCount()

                ' Clear the input fields
                studentIdField.Text = ""
            End Using

        Catch ex As MySqlException
            ' Catch SQL-related errors
            MessageBox.Show("A database error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Catch any other errors
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Function AreFieldsFilled() As Boolean
        Return Not String.IsNullOrEmpty(studentNameField.Text) AndAlso
               Not String.IsNullOrEmpty(studentSectionBox.Text) AndAlso
               Not String.IsNullOrEmpty(studentDepartmentField.Text) AndAlso
               Not String.IsNullOrEmpty(studentUsernameField.Text) AndAlso
               Not String.IsNullOrEmpty(studentPasswordField.Text) AndAlso
               Not String.IsNullOrEmpty(studentPinField.Text)
    End Function

    Private Sub studentClearBtn_Click(sender As Object, e As EventArgs) Handles studentClearBtn.Click
        studentIdField.Text = ""
        studentNameField.Text = ""
        studentDepartmentField.Text = ""
        studentUsernameField.Text = ""
        studentPasswordField.Text = ""
        studentPinField.Text = ""
    End Sub





    '////////////////////////////////////////////////////////////////////////////////////////




    Private Sub LoadProfessors()
        Dim query As String = "SELECT * FROM professors"
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        professorGrid.Rows.Clear()
                        While reader.Read()
                            professorGrid.Rows.Add(reader("faculty_id").ToString(),
                                               reader("name").ToString(),
                                               reader("department").ToString(),
                                               reader("username").ToString(),
                                               reader("password").ToString(),
                                               reader("pin").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As MySqlException
            ShowError("Database error: " & ex.Message)
        End Try
    End Sub

    Private Function AreFieldsFilledProf() As Boolean
        ' Check if all fields are filled (excluding the pin)
        Return Not (String.IsNullOrEmpty(professorIdField.Text) OrElse
                String.IsNullOrEmpty(professorNameField.Text) OrElse
                String.IsNullOrEmpty(professorDepartmentField.Text) OrElse
                String.IsNullOrEmpty(professorUsernameField.Text) OrElse
                String.IsNullOrEmpty(professorPasswordField.Text))
    End Function


    Private Sub professorIdField_KeyPress(sender As Object, e As KeyPressEventArgs) Handles professorIdField.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub professorAddBtn_Click(sender As Object, e As EventArgs) Handles professorAddBtn.Click
        Try
            ' Check if all fields are filled
            If Not AreFieldsFilledProf() Then
                MessageBox.Show("Please fill in all fields.")
                Return
            End If

            Dim facultyId As String = professorIdField.Text ' Assuming you have a textbox for the faculty ID
            Dim professorName As String = professorNameField.Text ' Assuming you have a textbox for the name
            Dim department As String = professorDepartmentField.Text ' Assuming you have a textbox for the department
            Dim username As String = professorUsernameField.Text ' Assuming you have a textbox for the username
            Dim password As String = professorPasswordField.Text ' Assuming you have a textbox for the password
            Dim pinValue As String = professorPinField.Text ' Assuming you have a textbox for the pin

            ' Validate if the pin exists
            Dim queryCheckPin As String = "SELECT COUNT(*) FROM pins WHERE pin = @pin"
            Using conn As New MySqlConnection(connectionString),
              cmdCheckPin As New MySqlCommand(queryCheckPin, conn)

                cmdCheckPin.Parameters.AddWithValue("@pin", pinValue)
                conn.Open()

                Dim pinExists As Integer = CInt(cmdCheckPin.ExecuteScalar())
                conn.Close()

                If pinExists > 0 Then
                    MessageBox.Show("The pin already exists. Please use a different pin.")
                    Return
                End If
            End Using

            ' Insert the pin into the pins table
            Dim queryInsertPin As String = "INSERT INTO pins (pin) VALUES (@pin)"
            Using conn As New MySqlConnection(connectionString),
              cmdInsertPin As New MySqlCommand(queryInsertPin, conn)

                cmdInsertPin.Parameters.AddWithValue("@pin", pinValue)
                conn.Open()
                cmdInsertPin.ExecuteNonQuery()
                conn.Close()
            End Using

            ' Insert the professor record
            Dim queryInsertProfessor As String = "INSERT INTO professors (faculty_id, name, department, username, password, pin) VALUES (@faculty_id, @name, @department, @username, @password, @pin)"
            Using conn As New MySqlConnection(connectionString),
              cmdInsertProfessor As New MySqlCommand(queryInsertProfessor, conn)

                cmdInsertProfessor.Parameters.AddWithValue("@faculty_id", facultyId)
                cmdInsertProfessor.Parameters.AddWithValue("@name", professorName)
                cmdInsertProfessor.Parameters.AddWithValue("@department", department)
                cmdInsertProfessor.Parameters.AddWithValue("@username", username)
                cmdInsertProfessor.Parameters.AddWithValue("@password", password)
                cmdInsertProfessor.Parameters.AddWithValue("@pin", pinValue)
                conn.Open()
                cmdInsertProfessor.ExecuteNonQuery()
                conn.Close()
            End Using

            MessageBox.Show("Professor added successfully!")
            LoadProfessors()
            professorCount.Text = GetProfessorCount()
            professorsCount.Text = GetProfessorCount()

            professorIdField.Text = ""
            professorNameField.Text = ""
            professorDepartmentField.Text = ""
            professorUsernameField.Text = ""
            professorPasswordField.Text = ""
            professorPinField.Text = ""

        Catch ex As MySqlException
            ' Handle any MySQL-specific errors
            MessageBox.Show("Database error: " & ex.Message)
        Catch ex As Exception
            ' Handle any general errors
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub professorUpdateBtn_Click(sender As Object, e As EventArgs) Handles professorUpdateBtn.Click
        ' Check if all fields are filled except the pin
        If Not AreFieldsFilledProf() Then
            MessageBox.Show("Please fill in all fields.")
            Return
        End If

        Dim facultyId As String = professorIdField.Text
        Dim professorName As String = professorNameField.Text
        Dim department As String = professorDepartmentField.Text
        Dim username As String = professorUsernameField.Text
        Dim password As String = professorPasswordField.Text

        ' Check if the professor ID exists
        Dim queryCheckProfessor As String = "SELECT COUNT(*) FROM professors WHERE faculty_id = @facultyId"
        Using conn As New MySqlConnection(connectionString),
          cmdCheckProfessor As New MySqlCommand(queryCheckProfessor, conn)

            cmdCheckProfessor.Parameters.AddWithValue("@facultyId", facultyId)
            conn.Open()

            Dim professorExists As Integer = CInt(cmdCheckProfessor.ExecuteScalar())
            conn.Close()

            If professorExists = 0 Then
                MessageBox.Show("Professor ID not found. Cannot update.")
                Return
            End If
        End Using

        ' Update the professor record, excluding the pin
        Dim queryUpdateProfessor As String = "
    UPDATE professors
    SET name = @professorName,
        department = @department,
        username = @username,
        password = @password
    WHERE faculty_id = @facultyId"

        Using conn As New MySqlConnection(connectionString),
          cmdUpdateProfessor As New MySqlCommand(queryUpdateProfessor, conn)

            cmdUpdateProfessor.Parameters.AddWithValue("@professorName", professorName)
            cmdUpdateProfessor.Parameters.AddWithValue("@department", department)
            cmdUpdateProfessor.Parameters.AddWithValue("@username", username)
            cmdUpdateProfessor.Parameters.AddWithValue("@password", password)
            cmdUpdateProfessor.Parameters.AddWithValue("@facultyId", facultyId)

            conn.Open()
            cmdUpdateProfessor.ExecuteNonQuery()
            conn.Close()

            MessageBox.Show("Professor updated successfully!")
            LoadProfessors()

            ' Clear the input fields
            professorIdField.Text = ""
            professorNameField.Text = ""
            professorDepartmentField.Text = ""
            professorUsernameField.Text = ""
            professorPasswordField.Text = ""
            professorPinField.Text = "" ' Optionally clear the pin field if it's part of the form
        End Using
    End Sub

    Private Sub professorDeleteBtn_Click(sender As Object, e As EventArgs) Handles professorDeleteBtn.Click
        Try
            ' Check if the professor ID is entered
            If String.IsNullOrEmpty(professorIdField.Text) Then
                MessageBox.Show("Please enter the professor ID to delete.")
                Return
            End If

            Dim professorId As String = professorIdField.Text
            Dim pinValue As String = String.Empty

            ' Check if the professor ID exists
            Dim queryCheckProfessor As String = "SELECT pin FROM professors WHERE faculty_id = @professorId"

            Using conn As New MySqlConnection(connectionString),
              cmdCheckProfessor As New MySqlCommand(queryCheckProfessor, conn)

                cmdCheckProfessor.Parameters.AddWithValue("@professorId", professorId)
                conn.Open()

                Dim reader As MySqlDataReader = cmdCheckProfessor.ExecuteReader()
                If reader.Read() Then
                    pinValue = reader("pin").ToString()
                Else
                    MessageBox.Show("Professor ID not found.")
                    conn.Close()
                    Return
                End If

                conn.Close()
            End Using

            ' Delete the pin from the pins table
            Dim queryDeletePin As String = "DELETE FROM pins WHERE pin = @pin"
            Using conn As New MySqlConnection(connectionString),
              cmdDeletePin As New MySqlCommand(queryDeletePin, conn)

                cmdDeletePin.Parameters.AddWithValue("@pin", pinValue)
                conn.Open()
                cmdDeletePin.ExecuteNonQuery()
                conn.Close()
            End Using

            ' Delete the professor record from the professors table
            Dim queryDeleteProfessor As String = "DELETE FROM professors WHERE faculty_id = @professorId"
            Using conn As New MySqlConnection(connectionString),
              cmdDeleteProfessor As New MySqlCommand(queryDeleteProfessor, conn)

                cmdDeleteProfessor.Parameters.AddWithValue("@professorId", professorId)
                conn.Open()
                cmdDeleteProfessor.ExecuteNonQuery()
                conn.Close()

                MessageBox.Show("Professor deleted successfully!")
                LoadProfessors()
                professorCount.Text = GetProfessorCount()
                professorsCount.Text = GetProfessorCount()

                ' Clear the input fields
                professorIdField.Text = ""
                professorNameField.Text = ""
                professorDepartmentField.Text = ""
                professorUsernameField.Text = ""
                professorPasswordField.Text = ""
                professorPinField.Text = "" ' Optionally clear the pin field if it's part of the form
            End Using

        Catch ex As MySqlException
            ' Handle any MySQL-specific errors (e.g., issues with database connection, query execution)
            MessageBox.Show("Database error: " & ex.Message)
        Catch ex As Exception
            ' Handle any general errors
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub professorClearBtn_Click(sender As Object, e As EventArgs) Handles professorClearBtn.Click
        professorIdField.Text = ""
        professorNameField.Text = ""
        professorDepartmentField.Text = ""
        professorUsernameField.Text = ""
        professorPasswordField.Text = ""
        professorPinField.Text = "" ' Optionally clear the pin field if it's part of the form
    End Sub


    '/////////////////////////////////////////////////////////


    Private Sub loadRecords()
        Using conn As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                conn.Open()

                ' SQL query to select class_id, professor's name, and subject's title
                Dim cmd As New MySqlCommand("
                SELECT c.class_id, p.name AS professor_name, s.subject_title
                FROM classes c
                JOIN professors p ON c.professor_id = p.faculty_id
                JOIN subjects s ON c.subject_code = s.subject_code", conn)

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                ' Assuming you have a DataGridView named 'classGridView' to load the records into
                classGridView.Rows.Clear() ' Clear existing rows before loading new data

                ' Load records into the DataGridView
                While reader.Read()
                    classGridView.Rows.Add(reader("class_id").ToString(), reader("professor_name").ToString(), reader("subject_title").ToString())
                End While
                reader.Close()

            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            Finally
                ' Close the connection
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub loadBoxes()
        Using conn As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                conn.Open()

                ' Load professors into classProfsField combo box
                Dim cmdProf As New MySqlCommand("SELECT faculty_id, name FROM professors", conn)
                Dim dtProf As New DataTable()
                Dim daProf As New MySqlDataAdapter(cmdProf)

                daProf.Fill(dtProf)
                classProfsField.DataSource = dtProf
                classProfsField.DisplayMember = "name" ' What is displayed
                classProfsField.ValueMember = "faculty_id" ' What is used as value

                ' Load subjects into classSubjectField combo box
                Dim cmdSub As New MySqlCommand("SELECT subject_code, subject_title FROM subjects", conn)
                Dim dtSub As New DataTable()
                Dim daSub As New MySqlDataAdapter(cmdSub)

                daSub.Fill(dtSub)
                classSubjectField.DataSource = dtSub
                classSubjectField.DisplayMember = "subject_title" ' What is displayed
                classSubjectField.ValueMember = "subject_code" ' What is used as value

            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            Finally
                ' Close the connection
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub classAddBtn_Click(sender As Object, e As EventArgs) Handles classAddBtn.Click
        If String.IsNullOrWhiteSpace(classIdField.Text) Then
            MessageBox.Show("Class ID cannot be empty. Please enter a valid Class ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim selectedClassId As Integer = Convert.ToInt32(classIdField.Text)
        Dim selectedProfId As Integer = Convert.ToInt32(classProfsField.SelectedValue)
        Dim selectedSubjectCode As Integer = Convert.ToInt32(classSubjectField.SelectedValue)
        MessageBox.Show("Selected Professor ID: " & selectedProfId & ", Selected Subject Code: " & selectedSubjectCode)

        ' Proceed with your insertion logic here
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New MySqlCommand("INSERT INTO classes (class_id, professor_id, subject_code) VALUES (@classId, @professorId, @subjectCode)", conn)
                cmd.Parameters.AddWithValue("@classId", selectedClassId) ' Replace with actual class ID if needed
                cmd.Parameters.AddWithValue("@professorId", selectedProfId)
                cmd.Parameters.AddWithValue("@subjectCode", selectedSubjectCode)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Record added successfully!")
                classCount.Text = GetClassCount()
                classesCount.Text = GetClassCount()

                loadRecords()

            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub classUpdateBtn_Click(sender As Object, e As EventArgs) Handles classUpdateBtn.Click
        If String.IsNullOrWhiteSpace(classIdField.Text) Then
            MessageBox.Show("Class ID cannot be empty. Please enter a valid Class ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim selectedClassId As Integer = Convert.ToInt32(classIdField.Text)
        Dim selectedProfId As Integer = Convert.ToInt32(classProfsField.SelectedValue)
        Dim selectedSubjectCode As Integer = Convert.ToInt32(classSubjectField.SelectedValue)


        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New MySqlCommand("UPDATE classes SET professor_id = @professorId, subject_code = @subjectCode WHERE class_id = @classId", conn)
                cmd.Parameters.AddWithValue("@classId", selectedClassId)
                cmd.Parameters.AddWithValue("@professorId", selectedProfId)
                cmd.Parameters.AddWithValue("@subjectCode", selectedSubjectCode)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Record updated successfully!")
            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub classDeleteBtn_Click(sender As Object, e As EventArgs) Handles classDeleteBtn.Click
        If String.IsNullOrWhiteSpace(classIdField.Text) Then
            MessageBox.Show("Class ID cannot be empty. Please enter a valid Class ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim selectedClassId As Integer = Convert.ToInt32(classIdField.Text)

        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM classes WHERE class_id = @classId", conn)
                cmd.Parameters.AddWithValue("@classId", selectedClassId)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Record deleted successfully!")
                classCount.Text = GetClassCount()
                classesCount.Text = GetClassCount()

                loadRecords()

            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Function GetProfessorCount() As Integer
        Return GetRowCount("professors")
    End Function

    Private Function GetClassCount() As Integer
        Return GetRowCount("classes")
    End Function

    Private Function GetStudentCount() As Integer
        Return GetRowCount("students")
    End Function

    Private Function GetSectionCount() As Integer
        Return GetRowCount("sections")
    End Function

    Private Function GetSubjectCount() As Integer
        Return GetRowCount("subjects")
    End Function

    ' Helper method to run the COUNT query and return the result as an integer
    Private Function GetRowCount(tableName As String) As Integer
        Dim count As Integer = 0

        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New MySqlCommand($"SELECT COUNT(*) FROM {tableName}", conn)
                count = Convert.ToInt32(cmd.ExecuteScalar())
            Catch ex As MySqlException
                MessageBox.Show("Error: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using

        Return count
    End Function

    Private Sub subjectPanel_Paint(sender As Object, e As PaintEventArgs) Handles subjectPanel.Paint

    End Sub

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        subjectsCount.Text = GetSubjectCount()
        sectionsCount.Text = GetSectionCount()
        studentsCount.Text = GetStudentCount()
        professorsCount.Text = GetProfessorCount()
        classesCount.Text = GetClassCount()
    End Sub

    Private Sub professorPanel_Paint(sender As Object, e As PaintEventArgs) Handles professorPanel.Paint

    End Sub

    Private Sub signOutBtn_Click(sender As Object, e As EventArgs) Handles signOutBtn.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to sign out?", "Sign Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Me.Close()
            ShowLoginForm()
        End If
    End Sub

    ' Method to handle showing the login form
    Private Sub ShowLoginForm()
        Dim loginPage As Form2 = DirectCast(Application.OpenForms("Form2"), Form2)
        If loginPage IsNot Nothing Then
            loginPage.Show()
        Else
            loginPage = New Form2()
            loginPage.Show()
        End If
    End Sub

    Private Sub Guna2CirclePictureBox1_Click(sender As Object, e As EventArgs) Handles Guna2CirclePictureBox1.Click

    End Sub

    Private Sub classClearnBtn_Click(sender As Object, e As EventArgs) Handles classClearnBtn.Click

        classProfsField.SelectedIndex = -1
        classSubjectField.SelectedIndex = -1
        classIdField.Text = ""
    End Sub

    Private Sub classesPanel_Paint(sender As Object, e As PaintEventArgs) Handles classesPanel.Paint

    End Sub
End Class
