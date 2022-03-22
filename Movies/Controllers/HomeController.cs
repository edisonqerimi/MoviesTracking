using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Models;
using Movies.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext context;
        private readonly IMovieRepo _movieRepo;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(AppDbContext context, IMovieRepo movieRepo, ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            _movieRepo = movieRepo;
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Explore()
        {
            var movies = new List<MoviesViewModel>();
            var userMovies = context.UserMovie.Where(u => u.UserID == userManager.GetUserId(User)).ToList();


            foreach (var movie in _movieRepo.GetAllMovies())
            {
                var newMovie = new MoviesViewModel()
                {
                    Id = movie.Id,
                    Date = movie.Date,
                    Description = movie.Description,
                    PhotoLink = movie.PhotoLink,
                    PhotoPath = movie.PhotoPath,
                    PhotoSource = movie.PhotoSource,
                    Rating = movie.Rating,
                    Duration=movie.Duration,
                    Title = movie.Title,
                    Added = userMovies.SingleOrDefault(m => m.MovieId == movie.Id) != null
                };
                movies.Add(newMovie);
            }


            return View(movies);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id, string backAction)
        {
            ViewBag.BackAction = backAction;
            var movie = _movieRepo.GetMovie(id);
            return View(movie);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            var movies = _movieRepo.GetAllMovies();
            return View(movies);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(int id)
        {
            _movieRepo.Remove(id);
            return RedirectToAction("Manage");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Watchlist()
        {
            //var movies = context.Users.Where(x => x.Id == userManager.GetUserId(User)).SelectMany(x => x.UserMovies).ToList();

            var movies = context.UserMovie.Include(m => m.Movie).Where(m => m.UserID == userManager.GetUserId(User)).ToList();

            return View(movies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromWatchlist([FromForm] int movieId, [FromForm] string returnAction)
        {
            if (ModelState.IsValid)
            {
                /*var movie = context.Movies.FirstOrDefault(m => m.Id == movieId);
                var user = context.Users.Include(u => u.Movies).Single(u => u.Id == userManager.GetUserId(User));
                user.Movies.Remove(movie);*/
                var userMovie = context.UserMovie.SingleOrDefault(u => u.MovieId == movieId);
                context.UserMovie.Remove(userMovie);
                context.SaveChanges();
            }
            return RedirectToAction(returnAction);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToWatchlist([FromForm] int movieId)
        {
            if (ModelState.IsValid)
            {
                /*var movie = context.Movies.FirstOrDefault(m => m.Id == movieId);
                var user = context.Users.Include(u => u.Movies).Single(u => u.Id == userManager.GetUserId(User));
                user.Movies.Add(movie);*/
                var userMovie = new UserMovie()
                {
                    MovieId = movieId,
                    UserID = userManager.GetUserId(User),
                    AddedDate = DateTime.Now,
                    
                };
                context.UserMovie.Add(userMovie);
                context.SaveChanges();
            }
            return RedirectToAction("Explore");
        }


     
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CreateViewModel newMovie)
        {

            if (ModelState.IsValid)
            {

                string uniqueFileName = ProcessUploadedFile(newMovie);
                var movie = new Movie()
                {
                    Title = newMovie.Title,
                    Rating = newMovie.Rating,
                    Description = newMovie.Description,
                    TrailerLink = newMovie.TrailerLink,
                    PhotoLink = newMovie.PhotoLink,
                    PhotoPath = uniqueFileName,
                    Date = newMovie.Date,
                    Duration = newMovie.Duration,
                    PhotoSource = newMovie.PhotoSource
                };
                _movieRepo.Add(movie);
                /*return RedirectToAction("Details", new { id = movie.Id });*/
                return RedirectToAction("Manage");
            }

            return View();

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var newMovie = _movieRepo.GetMovie(id);
            var editViewModel = new EditViewModel()
            {
                Title = newMovie.Title,
                Description = newMovie.Description,
                Rating = newMovie.Rating,
                ExistingPhotoPath = newMovie.PhotoPath,
                PhotoLink = newMovie.PhotoLink,
                TrailerLink = newMovie.TrailerLink,
                Date = newMovie.Date,
                Duration = newMovie.Duration,
                PhotoSource = newMovie.PhotoSource

            };
            return View(editViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {

            if (ModelState.IsValid)
            {

                var movie = _movieRepo.GetMovie(model.Id);

                movie.Title = model.Title;
                movie.Rating = model.Rating;
                movie.TrailerLink = model.TrailerLink;
                movie.Description = model.Description;
                movie.Date = model.Date;
                movie.Duration = model.Duration;

                if (model.PhotoSource == PhotoSource.Link && model.PhotoLink != null)
                {
                    movie.PhotoLink = model.PhotoLink;
                }
                else if (model.PhotoSource == PhotoSource.Upload && model.Photo != null)
                {
                    string photoPath = ProcessUploadedFile(model);
                    movie.PhotoPath = photoPath;
                }
                if (movie.PhotoPath == null && model.Photo == null)
                {
                    model.PhotoSource = PhotoSource.Link;
                }
                else if (model.PhotoLink == null)
                {
                    model.PhotoSource = PhotoSource.Upload;
                }

                movie.PhotoSource = model.PhotoSource;
                _movieRepo.Update(movie);

                return RedirectToAction("Manage");
            }
            return View();
        }
        private string ProcessUploadedFile(CreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
