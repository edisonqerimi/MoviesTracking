using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class UserMovie
    {
        public int MovieId { get; set; }
        public string UserID { get; set; }
        public Movie Movie { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
