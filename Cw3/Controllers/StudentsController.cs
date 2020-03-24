using System;
using System.Collections.Generic;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Server=localhost,32770;Initial Catalog=s18706;User ID=sa;Password=Root1234";
        
        
        [HttpGet]
        public IActionResult GetStudent()
        {
            var list = new List<Student>();
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM student,enrollment,studies " +
                                    "WHERE student.idenrollment=enrollment.idenrollment AND studies.idstudy=enrollment.idstudy";
                
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.StudiesName = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();
                    list.Add(st);
                }
            }

            return Ok(list);
        }
        
        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student, enrollment where indexnumber=@index AND " + 
                                "enrollment.idenrollment = student.idenrollment";
                
                com.Parameters.AddWithValue("index", indexNumber);
        
                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    return Ok($"{dr["FirstName"].ToString()} {dr["LastName"].ToString()} " +
                              $"Semester = {dr["Semester"].ToString()} StartDate = {dr["StartDate"].ToString()}");
                }
        
            }
        
            return NotFound();
        }
        
        // private readonly IDbService _dbService;
        //
        // [HttpPut("{id}")]
        // public IActionResult PutStudent(int id)
        // {
        //     return Ok("Aktualizacja dokończona");
        // }
        //
        // [HttpDelete("{id}")]
        // public IActionResult DeleteStudent(int id)
        // {
        //     return Ok("Usuwanie ukończone");
        // }
        //
        // public StudentsController(IDbService dbService)
        // {
        //     _dbService = dbService;
        // }

    }
}