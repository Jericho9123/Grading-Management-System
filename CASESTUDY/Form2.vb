Imports MySqlConnector


Public Class Form2
    Private ReadOnly connectionString As String = "Server=localhost;Port=3308;Database=casestudy;Uid=root;Pwd=redmercy44;"


    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Establish a connection when the form loads
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()
                MessageBox.Show("Connection successful!")
            End Using
        Catch ex As MySqlException
            MessageBox.Show("Error connecting to the database: " & ex.Message)
        End Try
    End Sub

    Private Sub showPasswordBox_CheckedChanged(sender As Object, e As EventArgs) Handles showPasswordBox.CheckedChanged
        If showPasswordBox.Checked Then
            passwordField.UseSystemPasswordChar = True
        Else
            passwordField.UseSystemPasswordChar = False
        End If
    End Sub


    Dim roleToCheck As String
    Public Property id As String
    Public Property namee As String
    Public Property section As String
    Public Property department As String



    Private Sub signInBtn_Click(sender As Object, e As EventArgs) Handles signInBtn.Click
        If String.IsNullOrWhiteSpace(usernameField.Text) OrElse
           String.IsNullOrWhiteSpace(passwordField.Text) OrElse
           String.IsNullOrWhiteSpace(roleBox.Text) Then
            MessageBox.Show("Please fill out all fields before signing in.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        roleToCheck = roleBox.Text

        If roleToCheck = "Student" Then
            Dim query As String = "SELECT * FROM students WHERE username = @username AND password = @password"
            Try
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@username", usernameField.Text)
                        command.Parameters.AddWithValue("@password", passwordField.Text)

                        Using reader As MySqlDataReader = command.ExecuteReader()
                            If reader.HasRows Then
                                reader.Read()
                                namee = reader("student_name").ToString()
                                id = reader("student_id").ToString()
                                section = reader("course_year_section").ToString()
                                department = reader("department").ToString()
                                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Dim studentForm As New Form1
                                studentForm.nameLabel.Text = namee
                                studentForm.idLabel.Text = id
                                studentForm.sectionLabel.Text = section
                                studentForm.departmentLabel.Text = department
                                studentForm.Show()
                                Me.Hide()



                            Else
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If

        If roleToCheck = "Faculty Member" Then
            Dim query As String = "SELECT * FROM professors WHERE username = @username AND password = @password"
            Try
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@username", usernameField.Text)
                        command.Parameters.AddWithValue("@password", passwordField.Text)

                        Using reader As MySqlDataReader = command.ExecuteReader()
                            If reader.HasRows Then
                                reader.Read()
                                namee = reader("name").ToString()
                                id = reader("faculty_id").ToString()
                                department = reader("department").ToString()

                                Dim facultyForm As New Form3
                                facultyForm.nameLabel.Text = namee
                                facultyForm.idLabel.Text = id
                                facultyForm.departmentLabel.Text = department
                                facultyForm.Show()
                                Me.Hide()
                                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        If roleToCheck = "Administrator" Then
            Dim query As String = "SELECT * FROM admins WHERE username = @username AND password = @password"
            Try
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@username", usernameField.Text)
                        command.Parameters.AddWithValue("@password", passwordField.Text)

                        Using reader As MySqlDataReader = command.ExecuteReader()
                            If reader.HasRows Then
                                reader.Read()
                                namee = reader("name").ToString()
                                id = reader("admin_id").ToString()


                                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Dim adminForm As New Form4
                                adminForm.nameLabel.Text = namee
                                adminForm.idLabel.Text = id
                                adminForm.Show()

                                Me.Hide()
                            Else
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

    End Sub

    Private Sub pinFIeld_KeyPress(sender As Object, e As KeyPressEventArgs) Handles pinField.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Block the input
        End If
    End Sub

    Private Sub backBtn_Click(sender As Object, e As EventArgs) Handles backBtn.Click
        signInPanel.Visible = True
        forgotPasswordPanel.Visible = False
    End Sub

    Private Sub showPasswordBox1_CheckedChanged(sender As Object, e As EventArgs) Handles showPasswordBox1.CheckedChanged
        If showPasswordBox1.Checked Then
            newPasswordField.UseSystemPasswordChar = True
            confirmPasswordField.UseSystemPasswordChar = True
        Else
            newPasswordField.UseSystemPasswordChar = False
            confirmPasswordField.UseSystemPasswordChar = False
        End If


    End Sub


    Private Sub forgotPasswordLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles forgotPasswordLink.LinkClicked
        ' Prompt the user for their email
        Dim email As String = InputBox("Enter your email address:", "Forgot Password")

        ' Validate the email input
        If String.IsNullOrEmpty(email) Then
            MessageBox.Show("Email address cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if the email exists in either professors or admins
        Dim emailExists As Boolean = False
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                ' SQL query to check the email in both professors and admins
                Dim query As String = "SELECT COUNT(*) FROM ( " &
                                  "SELECT email FROM professors WHERE email = @Email " &
                                  "UNION ALL " &
                                  "SELECT email FROM admins WHERE email = @Email " &
                                  ") AS CombinedEmails"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Email", email)
                    emailExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0
                End Using
            Catch ex As Exception
                MessageBox.Show("An error occurred while checking the email: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
        End Using

        ' Handle the result
        If emailExists Then
            ' Proceed to forgot password panel
            signInPanel.Visible = False
            forgotPasswordPanel.Visible = True
        Else
            MessageBox.Show("The entered email address does not exist in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub



    Dim fetchedStudentId As String ' Declare the variable as String
    Dim fetchedProfessorId As String

    Private Sub nextBtn_Click(sender As Object, e As EventArgs) Handles nextBtn.Click


        If roleToCheck = "Student" Then
            Dim query As String = "SELECT student_id FROM students WHERE pin = @pin"

            Try
                ' Establish connection to the database
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()

                    ' Create the command with the query and parameters
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@pin", pinField.Text)

                        ' Execute the query and check if the pin exists
                        Using reader As MySqlDataReader = command.ExecuteReader()
                            If reader.HasRows Then
                                reader.Read() ' Move to the first row if it exists
                                fetchedStudentId = reader("student_id").ToString() ' Convert to String and store the student ID

                                ' Hide the current panel and show the password change panel
                                forgotPasswordPanel.Visible = False
                                changePasswordPanel.Visible = True
                            Else
                                ' Pin not found, show an error message
                                MessageBox.Show("Pin not found. Please check your pin and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If


        If roleToCheck = "Faculty Member" Then
            Dim query As String = "SELECT faculty_id FROM professors WHERE pin = @pin"

            Try
                ' Establish connection to the database
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()

                    ' Create the command with the query and parameters
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@pin", pinField.Text)

                        ' Execute the query and check if the pin exists
                        Using reader As MySqlDataReader = command.ExecuteReader()
                            If reader.HasRows Then
                                reader.Read() ' Move to the first row if it exists
                                fetchedProfessorId = reader("faculty_id").ToString() ' Convert to String and store the student ID

                                ' Hide the current panel and show the password change panel
                                forgotPasswordPanel.Visible = False
                                changePasswordPanel.Visible = True
                            Else
                                ' Pin not found, show an error message
                                MessageBox.Show("Pin not found. Please check your pin and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

    End Sub

    Private Sub changePasswordBtn_Click(sender As Object, e As EventArgs) Handles changePasswordBtn.Click
        If String.IsNullOrWhiteSpace(newPasswordField.Text) OrElse
           String.IsNullOrWhiteSpace(confirmPasswordField.Text) Then
            MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If newPasswordField.Text <> confirmPasswordField.Text Then
            MessageBox.Show("Passwords do not match. Please re-enter.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If roleToCheck = "Student" Then
            Dim query As String = "UPDATE students SET password = @password WHERE student_id = @student_id"

            Try
                ' Establish connection to the database
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()

                    ' Create the command with the query and parameters
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@password", confirmPasswordField.Text)
                        command.Parameters.AddWithValue("@student_id", fetchedStudentId)

                        ' Execute the update query
                        Dim rowsAffected As Integer = command.ExecuteNonQuery()

                        If rowsAffected > 0 Then
                            MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            changePasswordPanel.Visible = False
                            signInPanel.Visible = True
                        Else
                            MessageBox.Show("Failed to update the password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If

        If roleToCheck = "Faculty Member" Then
            Dim query As String = "UPDATE professors SET password = @password WHERE faculty_id = @faculty_id"

            Try
                ' Establish connection to the database
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()

                    ' Create the command with the query and parameters
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@password", confirmPasswordField.Text)
                        command.Parameters.AddWithValue("@faculty_id", fetchedProfessorId)

                        ' Execute the update query
                        Dim rowsAffected As Integer = command.ExecuteNonQuery()

                        If rowsAffected > 0 Then
                            MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            changePasswordPanel.Visible = False
                            signInPanel.Visible = True
                        Else
                            MessageBox.Show("Failed to update the password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub backBtn1_Click(sender As Object, e As EventArgs) Handles backBtn1.Click
        changePasswordPanel.Visible = False
        forgotPasswordPanel.Visible = True
    End Sub

    Private Sub changePasswordPanel_Paint(sender As Object, e As PaintEventArgs) Handles changePasswordPanel.Paint

    End Sub

    Private Sub roleBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles roleBox.SelectedIndexChanged

    End Sub

    Private Sub signInPanel_Paint(sender As Object, e As PaintEventArgs) Handles signInPanel.Paint

    End Sub
End Class
