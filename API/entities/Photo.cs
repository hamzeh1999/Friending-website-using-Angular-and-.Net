using System.ComponentModel.DataAnnotations.Schema;

namespace API.entities
{
    [Table("Photo")]
    
    public class Photo
    {
        public int id{get; set;}
        public string url{get; set;}
        public bool isMan{get; set;}
        public string publicId{get; set;}
    
        public AppUser AppUser{get; set;}

        public int appUserId{get; set;}
    }
}