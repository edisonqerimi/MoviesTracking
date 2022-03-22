using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.ViewModels
{
    public class MoviesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string PhotoLink { get; set; }
        public string PhotoPath { get; set; }
        public PhotoSource? PhotoSource { get; set; }
        public bool Added { get; set; }
        public string Duration { get; set; }
    }
}
