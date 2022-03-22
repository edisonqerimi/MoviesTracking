using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class Movie
    {

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        public double Rating { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string TrailerLink { get; set; }
        public string PhotoLink { get; set; }
        public string PhotoPath { get; set; }
        [Required]
        public PhotoSource? PhotoSource { get; set; }
        [Required]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2999", ErrorMessage = "Date is out of Range")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required]
        public string Duration { get; set; }
        public List<UserMovie> UserMovies { get; set; }
        //public List<ApplicationUser> Users { get; set; }



    }
}
