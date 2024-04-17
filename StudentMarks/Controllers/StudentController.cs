using Microsoft.AspNetCore.Mvc;
using Student.Model;
using System.Collections.Generic;
using System.Linq;

namespace StudentMarks.Controllers
{   


    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly Students _Student;
        private readonly Marksheet _Marksheet;
        public StudentController() 
        {
            _Student = new Students();
             _Marksheet = new Marksheet();  
        }
        private readonly List<Students> students = new List<Students>
        {
            new Students { StudentId = 1, StudentName = "Aksita Das", StudentJoinDate = new DateTime(2020, 1, 1), Class = "A" },
            new Students { StudentId = 2, StudentName = "priya Kumari", StudentJoinDate = new DateTime(2019, 3, 15), Class = "B" },
            new Students { StudentId = 3, StudentName = "lovina Rachel", StudentJoinDate = new DateTime(2019, 3, 15), Class = "c" },
            new Students { StudentId = 4, StudentName = "Rupa Kanongoo", StudentJoinDate = new DateTime(2019, 3, 15), Class = "B" },
            new Students { StudentId = 5, StudentName = "Sonali Sahoo", StudentJoinDate = new DateTime(2019, 3, 15), Class = "A" },
            new Students { StudentId = 6, StudentName = "Rishab Jain", StudentJoinDate = new DateTime(2019, 3, 15), Class = "B" },
            new Students { StudentId = 7, StudentName = "Jack D", StudentJoinDate = new DateTime(2019, 3, 15), Class = "C" },
            
        };

       
       
        private readonly List<Marksheet> markSheets = new List<Marksheet>
        {    
            new Marksheet { MarkSheetId = 1, StudentId = 1, SubjectTotalMark = 100, MarksObtained = 85 },
            new Marksheet { MarkSheetId = 2, StudentId = 1, SubjectTotalMark = 100, MarksObtained = 92 },
            new Marksheet { MarkSheetId = 4, StudentId = 2, SubjectTotalMark = 100, MarksObtained = 78 },
            new Marksheet { MarkSheetId = 5, StudentId = 7, SubjectTotalMark = 100, MarksObtained = 54 },
            new Marksheet { MarkSheetId = 6, StudentId = 3, SubjectTotalMark = 100, MarksObtained = 89 },
            new Marksheet { MarkSheetId = 7, StudentId = 4, SubjectTotalMark = 100, MarksObtained = 45 },
            new Marksheet { MarkSheetId = 8, StudentId = 2, SubjectTotalMark = 100, MarksObtained = 80 },
            new Marksheet { MarkSheetId = 9, StudentId = 6, SubjectTotalMark = 100, MarksObtained = 67 },
            new Marksheet { MarkSheetId = 10, StudentId = 5, SubjectTotalMark = 100, MarksObtained = 65 },
            new Marksheet { MarkSheetId = 11, StudentId = 7, SubjectTotalMark = 100, MarksObtained = 78 },
            new Marksheet { MarkSheetId = 12, StudentId = 3, SubjectTotalMark = 100, MarksObtained = 90 }
           //how to add multiple uniqe mashksheetId to one studentId?having diffrent marsksheetId refering to diffrent subjects
        };

        public double TotalMarkObtained { get; private set; }
        public double TotalMarks { get; private set; }

        // 1. GetTotalMarkObtained()
        [HttpGet("GetTotalMarkObtained/{studentId}")]
        public IActionResult GetTotalMarkObtained(int studentId)
        {
            var student = students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
                return NotFound();

            var totalMarkObtained = markSheets
                .Where(m => m.StudentId == studentId)
                .Sum(m => m.MarksObtained);

            return Ok(totalMarkObtained);
        }
        // 2. GetTotalPercentageObtained(StudentId)
        [HttpGet("GetTotalPercentageObtained/{studentId}")]
        public IActionResult GetTotalPercentageObtained(int studentId)
        {
            var student = students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
                return NotFound();

            var markSheets = this.markSheets.Where(m => m.StudentId == studentId);
            var totalMarksObtained = markSheets.Sum(m => m.MarksObtained);
            var totalMaxMarks = markSheets.Sum(m => m.SubjectTotalMark);
            var percentage = (totalMarksObtained * 100.0) / totalMaxMarks;

            return Ok(percentage);
        }

        // 3. GetAllMarksById()
        [HttpGet("GetAllMarksById/{studentId}")]
        public IActionResult GetAllMarksById(int studentId)
        {
            var student = students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
                return NotFound();

            var markDetails = markSheets
                .Where(m => m.StudentId == studentId)
                .Select(m => new
                {
                    SubjectTotalMark = m.SubjectTotalMark,
                    MarksObtained = m.MarksObtained
                });

            return Ok(markDetails);
        }


        // 4. AddMarks()
        [HttpPost("AddMarks")]
        public IActionResult AddMarks([FromBody] Marksheet markSheet)
        {
            markSheets.Add(markSheet);
            return Ok();
        }

        // 5. UpdateMarks()
        [HttpPut("UpdateMarks")]
        public IActionResult UpdateMarks([FromBody] Marksheet markSheet)
        {
            var existingMarkSheet = markSheets.FirstOrDefault(m => m.MarkSheetId == markSheet.MarkSheetId);
            if (existingMarkSheet == null)
                return NotFound();

            existingMarkSheet.MarksObtained = markSheet.MarksObtained;
            return Ok();
        }

        //// 6. GetStudentList(with total mark and percentage)
        //[HttpGet("GetStudentList")]
        //public IActionResult GetStudentList()
        //{
        //    var studentList = students.Select(s => new
        //    {
        //        s.StudentName,
        //        TotalMark = markSheets.Where(m => m.StudentId == s.StudentId).Sum(m => m.SubjectTotalMark),
        //        TotalMarkObtained = markSheets.Where(m => m.StudentId == s.StudentId).Sum(m => m.MarksObtained),
        //        //Totalpercentage = (TotalMarkObtained * 100.0) / TotalMarks
        //         // TotalPercentage = GetTotalPercentageObtained(s.StudentId).Value
        //      });

        //    return Ok(studentList);
        //}

        [HttpGet("GetStudentList")]
        public IActionResult GetStudentList()
        {
            var studentList = students.Select(s => new
            {
                s.StudentName,
                TotalMark = markSheets.Where(m => m.StudentId == s.StudentId).Sum(m => m.SubjectTotalMark),
                TotalMarkObtained = markSheets.Where(m => m.StudentId == s.StudentId).Sum(m => m.MarksObtained),
                TotalPercentage = CalculatePercentage(
                    markSheets.Where(m => m.StudentId == s.StudentId).Sum(m => m.MarksObtained),
                    markSheets.Where(m => m.StudentId == s.StudentId).Sum(m => m.SubjectTotalMark)
                )
            });

            return Ok(studentList);
        }

        private double CalculatePercentage(int marksObtained, int totalMarks)
        {
            if (totalMarks == 0)
                return 0;

            return (double)marksObtained / totalMarks * 100;
        }

        // 7. GetTopThree(int class)
        [HttpGet("GetTopThree/{classId}")]
        public IActionResult GetTopThree(string classId)
        {
            var topStudents = students
                .Where(s => s.Class == classId)
                .Select(s => new
                {
                    Student = s,
                    TotalMarkObtained = markSheets.Where(m => m.StudentId == s.StudentId).Sum(m => m.MarksObtained)
                })
                .OrderByDescending(s => s.TotalMarkObtained)
                .Take(3)
                .Select(s => s.Student);

            return Ok(topStudents);
        }
    }
}