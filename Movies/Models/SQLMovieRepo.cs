
using Movies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Movies.Models
{
    public class SQLMovieRepo : IMovieRepo
    {
        private readonly AppDbContext context;

        public SQLMovieRepo(AppDbContext context)
        {
            this.context = context;
        }
        public Movie Add(Movie movie)
        {
            
            context.Movies.Add(movie);
            context.SaveChanges();
            return movie;
        }

        /*public List<WatchlistViewModel> GetWatchlist()
        {  
           List<WatchlistViewModel>  data = (from movie in context.Movies
                         join userMovie in context.UserMovie on movie.Id equals userMovie.MovieId
                         join user in context.Users on userMovie.UserId equals user.Id
                         select new
                         {
                             UserId = user.Id,
                             MovieId = movie.Id,
                             Username = user.UserName,
                             Title = movie.Title,
                             Date = movie.Date,
                             Description = movie.Description,
                             Rating = movie.Rating,
                             PhotoLink = movie.PhotoLink,
                             PhotoPath = movie.PhotoPath,
                             PhotoSource = movie.PhotoSource
                         }).AsEnumerable()
                         .Select(x=> new WatchlistViewModel {
                             UserId = x.UserId,
                             MovieId = x.MovieId,
                             Username = x.Username,
                             Title = x.Title,
                             Date = x.Date,
                             Description = x.Description,
                             Rating = x.Rating,
                             PhotoLink = x.PhotoLink,
                             PhotoPath = x.PhotoPath,
                             PhotoSource = x.PhotoSource
                         }).ToList();
            return data;
        }*/

       /* public UserMovie AddToWatchlist(UserMovie userMovie)
        {
            context.UserMovie.Add(userMovie);
            context.SaveChanges();
            return userMovie;
        }
        public UserMovie RemoveFromWatchlist(int id,string userId)
        {
            
            var userMovie = context.UserMovie.Find(id,userId);
            context.Remove(userMovie);
            context.SaveChanges();
            return (userMovie);
        }*/
        public IEnumerable<Movie> GetAllMovies()
        {
            return context.Movies;
        }


        public Movie GetMovie(int id)
        {

            return context.Movies.Find(id);
        }

        public Movie Remove(int id)
        {
            var movie = context.Movies.Find(id);
            if (movie != null)
            {
                context.Movies.Remove(movie);
                context.SaveChanges();
            }
            return movie;
        }

        public Movie Update(Movie movieChanges)
        {
            var movie = context.Movies.Attach(movieChanges);
            movie.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return movieChanges;
        }
    }
}
