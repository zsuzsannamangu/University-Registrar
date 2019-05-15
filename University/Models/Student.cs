using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace University.Models
{
  public class Student
  {

    public int _id { get; set; }
    public string _name { get; set; }
    public string _number { get; set; }

    public Student (string name, string number)
    {
      // _id = id;
      _name = name;
      _number = number;
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
        newStudent._id = rdr.GetInt32(0);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public int Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, number) VALUES (@name, @number);";
      cmd.Parameters.AddWithValue("@name", _name);
      cmd.Parameters.AddWithValue("@number", _number);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return _id;
    }
  }
}
