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
  }

}
