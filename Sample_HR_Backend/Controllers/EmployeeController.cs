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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Mytest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"SELECT EMPLOYEE_ID,EMPLOYEE_NAME,
            CONVERT(VARCHAR(10),JOINED_DATE,120)AS JOINED_DATE,DEPARTMENT_NAME,PHOTO FROM DBO.EMPLOYEE";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(query, con))
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

        public JsonResult Post(Employee emp)
        {
            string query = @"INSERT INTO DBO.EMPLOYEE 
            (EMPLOYEE_NAME,JOINED_DATE,DEPARTMENT_NAME,PHOTO)
            VALUES (@EMPLOYEE_NAME,@JOINED_DATE,@DEPARTMENT_NAME,@PHOTO)";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@EMPLOYEE_NAME", emp.EMPLOYEE_NAME);
                    com.Parameters.AddWithValue("@JOINED_DATE", emp.JOINED_DATE);
                    com.Parameters.AddWithValue("@DEPARTMENT_NAME", emp.DEPARTMENT_NAME);
                    com.Parameters.AddWithValue("@PHOTO", emp.PHOTO);
                    reader = com.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Added successfully");
        }

        [HttpPut]

        public JsonResult Put(Employee emp)
        {
            string query = @"UPDATE  DBO.EMPLOYEE SET EMPLOYEE_NAME= @EMPLOYEE_NAME,
                            JOINED_DATE=@JOINED_DATE,DEPARTMENT_NAME=@DEPARTMENT_NAME,PHOTO=@PHOTO
                            WHERE EMPLOYEE_ID=@EMPLOYEE_ID";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@EMPLOYEE_ID", emp.EMPLOYEE_ID);
                    com.Parameters.AddWithValue("@EMPLOYEE_NAME", emp.EMPLOYEE_NAME);
                    com.Parameters.AddWithValue("@JOINED_DATE", emp.JOINED_DATE);
                    com.Parameters.AddWithValue("@DEPARTMENT_NAME", emp.DEPARTMENT_NAME);
                    com.Parameters.AddWithValue("@PHOTO", emp.PHOTO);
                    reader = com.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM  DBO.EMPLOYEE  
                            WHERE EMPLOYEE_ID=@EMPLOYEE_ID";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@EMPLOYEE_ID", id);

                    reader = com.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Deleted successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try 
            {
                var HttpRequest = Request.Form;
                var PostedFile = HttpRequest.Files[0];
                string filename = PostedFile.FileName;
                var PhysicalPath = _env.ContentRootPath + "/Photos/" + filename;
                using (var stream=new FileStream(PhysicalPath,FileMode.Create))
                {

                    PostedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("123.png");
            }

        }
    }
}
