using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Server.Controllers
{
    [Produces("application/json")]
    
    [Route("api/File")]
    public class FileController : Controller
    {
        // GET: api/File
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/File/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/File
        [HttpPost]
        public void Post()
        {

        }
        
        // PUT: api/File/
        [HttpPut()]
        [Consumes("application/json")]
        public async Task<UploadFile> Put([FromQuery]string name, [FromQuery]long totalSize)
        {
            using (var conn = GetConnection())
            {
                await conn.OpenAsync();
                var q = 
@"insert into Files
(Name, TotalSize)
output Inserted.FileId
Values
(@name, @size)
";
                using (var query = new SqlCommand(q, conn))
                {
                    query.Parameters.AddWithValue("@name", name);
                    query.Parameters.AddWithValue("@size", totalSize);


                    var fileId = (await query.ExecuteScalarAsync()).ToString();

                    return new UploadFile() { FileId = fileId, Name=name, TotalSize=totalSize, IsComplete=false, Path=null };
                }
            }
        }
        
        
        private SqlConnection GetConnection()
        {
            return new SqlConnection(Startup.Configuration.GetConnectionString("fileDB"));
        }
    }
}
