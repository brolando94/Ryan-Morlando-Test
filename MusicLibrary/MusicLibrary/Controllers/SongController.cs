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
    public class SongController : ControllerBase
    {
        //Allow to store/read our connection string from the app settings via the configuration object 
        private readonly IConfiguration _configuration;
        public SongController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get data from a table
        [HttpGet]
        public JsonResult Get()
        {
            //build query
            string query = @"select SongId, SongName, Album from dbo.Song";

            //create new data table object 
            DataTable dt = new DataTable();

            // store connection string using configuration object
            string sqlDataSource = _configuration.GetConnectionString("MusicLibraryConnection");
            SqlDataReader myReader;

            //open the connection to the sql database and read in via query
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
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
        public JsonResult Post(Song Sng)
        {
            //build query
            string query = @"insert into dbo.Song (SongName, Album) values (@SongName, @Album)";

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
                    myCommand.Parameters.AddWithValue("@SongName", Sng.Name);
                    myCommand.Parameters.AddWithValue("@Album", Sng.Album);
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                    }
                    
                    catch (SqlException)
                    {
                        return new JsonResult("Can not have duplicate Song");
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
        public JsonResult Put(Song Sng)
        {
            //build query
            string query = @"Update dbo.Song Set SongName = @SongName, Album = @Album where SongId = @SongId";

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
                        myCommand.Parameters.AddWithValue("@SongId", Sng.Id);
                        myCommand.Parameters.AddWithValue("@SongName", Sng.Name);
                        myCommand.Parameters.AddWithValue("@Album", Sng.Album);
                    }
                    catch (SqlException)
                    {
                        return new JsonResult("SongId must only be a whole number");
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

        //Delete Method 
        [HttpDelete("{id}")]

        //Same outline as others but accepts the id as a parameter for a record to be deleted
        public JsonResult Delete(int id)
        {
            //build query
            string query = @"delete from dbo.Song where SongId = @SongId";

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
                    myCommand.Parameters.AddWithValue("@SongId", id);
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
