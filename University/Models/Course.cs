using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace University.Models
{
  public class Course
  {

    public int _courseId;
    public string _courseName { get; set; }
    public string _courseNumber { get; set; }

    public Course (string courseName, string courseNumber)
    {
      // _courseId = courseId;
      _courseName = courseName;
      _courseNumber = courseNumber;
    }

    public int GetCourseId()
    {
      return _courseId;
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name, number FROM courses;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        // int clientId = rdr.GetInt32(0);
        // string clientName = rdr.GetString(1);
        // string clientPhone = rdr.GetString(2);

        Course newCourse = new Course(
          rdr.GetString(1),
          rdr.GetString(2)
        );
        newCourse._courseId = rdr.GetInt32(0);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetCourseId().Equals(newCourse.GetCourseId());
        bool nameEquality = this._courseName.Equals(newCourse._courseName);
        return (idEquality && nameEquality);
      }
    }


    public int Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (name, number) VALUES (@name, @number);";
      cmd.Parameters.AddWithValue("@name", _courseName);
      cmd.Parameters.AddWithValue("@number", _courseNumber);
      cmd.ExecuteNonQuery();
      _courseId = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return _courseId;
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int coursesId = 0;
      string courseName = "";
      string courseNumber = "";
      // We remove the line setting a itemStudentId value here.
      while(rdr.Read())
      {
        coursesId = rdr.GetInt32(0);
        courseName = rdr.GetString(1);
        courseNumber = rdr.GetString(2);
        // We no longer read the itemCategoryId here, either.
      }
      // Constructor below no longer includes a itemCategoryId parameter:
      Course newCourse = new Course(courseName, courseNumber);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCourse;
    }


    //This adds an entry to the enrollments join table containing IDs for relevant Student and Course objects.
    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO enrollments (students_id, courses_id) VALUES (@StudentsId, @CoursesId);";
      // cmd.Parameters.AddWithValue("@StudentsId", _id);
      // cmd.Parameters.AddWithValue("@CoursesId", _id);

      MySqlParameter students_id = new MySqlParameter();
      students_id.ParameterName = "@StudentsId";
      students_id.Value = newStudent.GetStudentId();
      cmd.Parameters.Add(students_id);

      MySqlParameter courses_id = new MySqlParameter();
      courses_id.ParameterName = "@CoursesId";
      courses_id.Value = _courseId;
      cmd.Parameters.Add(courses_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Student> GetStudents()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT students.* FROM courses
          JOIN enrollments ON (courses.id = enrollments.courses_id)
          JOIN students ON (enrollments.students_id = students.id)
          WHERE courses.id = @CoursesId;";

      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@CoursesId";
      courseIdParameter.Value = _courseId;
      cmd.Parameters.Add(courseIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Student> students = new List<Student>{};
      while(rdr.Read())
      {
        // int studentsId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        string studentNumber = rdr.GetString(2);
        Student newStudent = new Student(studentNumber, studentName);
        students.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return students;
    }
  }
}
