using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
  public class StudentsController : Controller
  {
    [HttpGet("/students")]
    public ActionResult Index()
    {
      List<Student> allStudents = Student.GetAll();
      return View(allStudents);
    }

    [HttpGet("/students/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/students")]
    public ActionResult Create(string name, string number)
    {
      Student newStudent = new Student(name, number);
      newStudent.Save();
      List<Student> allStudents = Student.GetAll();
      return View("Index", allStudents);
    }

    [HttpGet("/students/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Student selectedStudent = Student.Find(id);
      List<Course> studentCourses = selectedStudent.GetCourses();
      List<Course> allCourses = Course.GetAll();
      model.Add("student", selectedStudent);
      model.Add("studentCourses", studentCourses);
      model.Add("allCourses", allCourses);
      return View(model);
    }

    [HttpPost("/students/{studentsId}/courses/new")]
    public ActionResult AddCourse(int studentsId, int coursesId)
    {
      Student student = Student.Find(studentsId);
      Course course = Course.Find(coursesId);
      student.AddCourse(course);
      return RedirectToAction("Show",  new { id = studentsId });
    }

  }

}
