using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesAPI.Data;

namespace StudentExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        
            // Shows connection to db       vvvvvv
            private readonly IConfiguration _config;

            public InstructorController(IConfiguration config)
            {
                _config = config;
            }
            // Create connection to sql
            public IDbConnection Connection
            {
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }
            [HttpGet]
            public async Task<IActionResult> Get(string q)
            {
                //Query sql server to select (a,b,c,) from table
                string sql = @"
            SELECT
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.Specialty,
                i.Cohort
            FROM Instructor i
            ";
                if (q != null)
                {
                    string isQ = $@"
                    AND e.FirstName LIKE '%{q}%'
                    OR e.Cohort LIKE '%{q}%'
                ";
                    sql = $"{sql} {isQ}";
                }
                Console.WriteLine(sql);
                //Using database 
                using (IDbConnection conn = Connection)
                {

                    IEnumerable<Instructor> instructors = await conn.QueryAsync<Instructor>(
                        sql
                    );
                    return Ok(instructors);
                }
            }

        }
    }
