using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
  public class CoursesController : Controller
  {
    [HttpGet("/courses")]
    public ActionResult Index()
    {
      List<Course> allCourses = Course.GetAll();
      return View(allCourses);
    }

    [HttpGet("/courses/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/courses")]
    public ActionResult Create(string name, string number)
    {
      Course newCourse = new Course(name, number);
      newCourse.Save();
      List<Course> allCourses = Course.GetAll();
      return View("Index", allCourses);
    }

    [HttpGet("/courses/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Course selectedCourse = Course.Find(id);
      List<Student> courseStudents = selectedCourse.GetStudents();
      List<Student> allStudents = Student.GetAll();
      model.Add("selectedCourse", selectedCourse);
      model.Add("courseStudents", courseStudents);
      model.Add("allStudents", allStudents);
      return View(model);
    }

    [HttpPost("/courses/{coursesId}/students/new")]
    public ActionResult AddStudent(int coursesId, int studentsId)
    {
      Course course = Course.Find(coursesId);
      Student student = Student.Find(studentsId);
      course.AddStudent(student);
      return RedirectToAction("Show",  new { id = coursesId });
    }

  }

}
