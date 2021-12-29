namespace MusicLibrary.Models
{
    public class Song:Artist
    { 
        //id and name derived from artist.. add album
        public string Album { get; set; }

    }
}
