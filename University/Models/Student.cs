using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace University.Models
{
  public class Student
  {

    private int _studentId;
    public string _studentName { get; set; }
    public string _studentNumber { get; set; }

    public Student (string studentName, string studentNumber)
    {
      // _studentId = studentId;
      _studentName = studentName;
      _studentNumber = studentNumber;
    }

    public int GetStudentId()
    {
      return _studentId;
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name, number FROM students;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        // int clientId = rdr.GetInt32(0);
        // string clientName = rdr.GetString(1);
        // string clientPhone = rdr.GetString(2);

        Student newStudent = new Student(
          rdr.GetString(1),
          rdr.GetString(2)
        );
        newStudent._studentId = rdr.GetInt32(0);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetStudentId().Equals(newStudent.GetStudentId());
        bool nameEquality = this._studentName.Equals(newStudent._studentName);
        return (idEquality && nameEquality);
      }
    }

    public int Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, number) VALUES (@name, @number);";
      cmd.Parameters.AddWithValue("@name", _studentName);
      cmd.Parameters.AddWithValue("@number", _studentNumber);
      cmd.ExecuteNonQuery();
      _studentId = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return _studentId;
    }

    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int StudentsId = 0;
      string StudentName = "";
      string StudentNumber = "";
      while(rdr.Read())
      {
        StudentsId = rdr.GetInt32(0);
        StudentName = rdr.GetString(1);
        StudentNumber = rdr.GetString(2);
      }
      Student newStudent = new Student(StudentName, StudentNumber);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newStudent;
    }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO enrollments (students_id, courses_id) VALUES (@StudentsId, @CoursesId);";

      MySqlParameter students_id = new MySqlParameter();
      students_id.ParameterName = "@StudentsId";
      students_id.Value = _studentId;
      cmd.Parameters.Add(students_id);

      MySqlParameter courses_id = new MySqlParameter();
      courses_id.ParameterName = "@CoursesId";
      courses_id.Value = newCourse.GetCourseId();
      cmd.Parameters.Add(courses_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Course> GetCourses()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

        cmd.CommandText = @"SELECT courses.* FROM students
            JOIN enrollments ON (students.id = enrollments.students_id)
            JOIN courses ON (enrollments.courses_id = courses.id)
            WHERE students.id = @StudentsId;";

        MySqlParameter studentIdParameter = new MySqlParameter();
        studentIdParameter.ParameterName = "@StudentsId";
        studentIdParameter.Value = _studentId;
        cmd.Parameters.Add(studentIdParameter);

        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<Course> courses = new List<Course>{};
        while(rdr.Read())
        {
          // int coursesId = rdr.GetInt32(0);
          string courseName = rdr.GetString(1);
          string courseNumber = rdr.GetString(2);
          Course newCourse = new Course(courseNumber, courseName);
          courses.Add(newCourse);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return courses;
    }


  }
}
