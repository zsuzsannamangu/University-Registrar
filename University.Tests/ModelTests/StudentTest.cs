[TestMethod]
    public void AddStudent_AddsStudentToCourse_StudentList()
    {
      //Arrange
      Course testCourse = new Course("Intro to CS");
      testCourse.Save();
      Student testStudent = new Student("Lilian");
      testStudent.Save();

      //Act
      testCourse.AddStudent(testStudent);

      List<Student> result = testCourse.GetStudents();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
