using Movies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models
{
    public interface IMovieRepo
    {
        Movie GetMovie(int id);
        IEnumerable<Movie> GetAllMovies();
        Movie Add(Movie movie);
        Movie Remove(int id);
        Movie Update(Movie movieChanges);
    }
}
