using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Mytest.Modules;

namespace Mytest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"SELECT DEPARTMENT_ID,DEPARTMENT_NAME FROM DBO.DEPARTMENT";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con=new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com= new SqlCommand(query,con))
                {
                    reader = com.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult(dataTable);
        }

        [HttpPost]

        public JsonResult Post(Department dep)
        {
            string query = @"INSERT INTO DBO.DEPARTMENT VALUES (@DEPARTMENT_NAME)";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@DEPARTMENT_NAME", dep.DEPARTMENT_NAME);
                    reader = com.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Added successfully");
        }

        [HttpPut]

        public JsonResult Put(Department dep)
        {
            string query = @"UPDATE  DBO.DEPARTMENT SET DEPARTMENT_NAME= @DEPARTMENT_NAME 
                            WHERE DEPARTMENT_ID=@DEPARTMENT_ID";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@DEPARTMENT_ID", dep.DEPARTMENT_ID);
                    com.Parameters.AddWithValue("@DEPARTMENT_NAME", dep.DEPARTMENT_NAME);
                    reader = com.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete ("{id}")]

        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM  DBO.DEPARTMENT  
                            WHERE DEPARTMENT_ID=@DEPARTMENT_ID";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@DEPARTMENT_ID", id);
                    
                    reader = com.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Deleted successfully");
        }
    }
}
