using Microsoft.AspNetCore.Http;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.ViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage ="Movie Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Rating is required")]
        [Range(0,10,ErrorMessage ="Rating must be between 0 and 10")]
        public double Rating { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string TrailerLink { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public PhotoSource? PhotoSource { get; set; }
        public string PhotoLink { get; set; }
        [Required]
        //[Range(typeof(DateTime), "1/1/1900", "1/1/2999", ErrorMessage = "Date is out of Range")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public IFormFile Photo { get; set; }
    }
}
