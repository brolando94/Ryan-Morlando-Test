using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicLibrary.Models;
using System.Data;
using System.Data.SqlClient;

namespace MusicLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        //Allow to store/read our connection string from the app settings via the configuration object 
        private readonly IConfiguration _configuration;
        public ArtistController (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get data from a table
        [HttpGet]
        public JsonResult Get()
        {
            //build query
            string query = @"select ArtistId, ArtistName from dbo.Artist";

            //create new data table object 
            DataTable dt = new DataTable();

            // store connection string using configuration object
            string sqlDataSource = _configuration.GetConnectionString("MusicLibraryConnection");
            SqlDataReader myReader;

            //open the connection to the sql database and read in via query
            using(SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            //Return result in json format
            return new JsonResult(dt);
        }

        //Add data to a Table
        [HttpPost]

        //Same as Get but pass the model Object into the post method
        public JsonResult Post(Artist Art)
        {
            //build query
            string query = @"insert into dbo.Artist values (@ArtistName)";

            //create new data table object 
            DataTable dt = new DataTable();

            //store connection
            string sqlDataSource = _configuration.GetConnectionString("MusicLibraryConnection");
            SqlDataReader myReader;

            //open the connection to the sql database
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    //Adds the passed in object to the query
                    myCommand.Parameters.AddWithValue("@ArtistName", Art.Name);
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                    }
                    catch (SqlException)
                    {
                        return new JsonResult("Can not have duplicate artist");
                    }
                    dt.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            //Return Add Successful Message
            return new JsonResult("Add Successful");
        }

        //Put Method to update records
        [HttpPut]

        //Same as post but it allows the record to be updated Based on the id
        public JsonResult Put(Artist Art)
        {
            //build query
            string query = @"Update dbo.Artist Set ArtistName = @ArtistName where ArtistId = @ArtistId";

            //create new data table object
            DataTable dt = new DataTable();

            //store connection
            string sqlDataSource = _configuration.GetConnectionString("MusicLibraryConnection");
            SqlDataReader myReader;

            //open the connection to the sql database
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                //update the record with the values passed in based on the value of the id 
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    try
                    {
                        myCommand.Parameters.AddWithValue("@ArtistId", Art.Id);
                        myCommand.Parameters.AddWithValue("@ArtistName", Art.Name);
                    }
                    catch(SqlException)
                    {
                        return new JsonResult("ArtistId must only be a whole number");
                    }
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            //Return Update Successful message
            return new JsonResult("Upadate Successful");
        }

        //Delete Method.. Accepts Id in url
        [HttpDelete("{id}")]

        //Same outline as others but accepts the id as a parameter for a record to be deleted
        public JsonResult Delete(int id)
        {
            //build query
            string query = @"delete from dbo.Artist where ArtistId = @ArtistId";

            //create new data table object
            DataTable dt = new DataTable();

            //store connection
            string sqlDataSource = _configuration.GetConnectionString("MusicLibraryConnection");
            SqlDataReader myReader;

            //open the connection to the sql database
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                //add the id to the query to be deleted
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ArtistId", id);
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            //Return delete successful message
            return new JsonResult("Delete Successful");
        }

    }
}
