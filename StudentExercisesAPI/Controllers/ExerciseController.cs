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
    //Specify route to call from API
    [Route("api/[controller]")]
    [ApiController]
    //Create class for controller
    public class ExerciseController : ControllerBase
    {
        // Shows connection to db       vvvvvv
        private readonly IConfiguration _config;

        public ExerciseController(IConfiguration config)
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
                e.Id,
                e.Name,
                e.Language
            FROM Exercise e
            ";
            if (q != null)
            {
                string isQ = $@"
                    AND e.Name LIKE '%{q}%'
                    OR e.Language LIKE '%{q}%'
                ";
                sql = $"{sql} {isQ}";
            }
            Console.WriteLine(sql);
            //Using database 
            using (IDbConnection conn = Connection)
            {

                IEnumerable<Exercise> exercises = await conn.QueryAsync<Exercise>(
                    sql
                );
                return Ok(exercises);
            }
        }

    }

}