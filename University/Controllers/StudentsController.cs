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
  }

}
