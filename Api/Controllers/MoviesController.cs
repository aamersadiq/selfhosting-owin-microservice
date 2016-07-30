using Data;
using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers
{
    [AllowAnonymous]
    public class MoviesController : ApiController
    {
        IMovieRepository _moviesRepository;
        public MoviesController(IMovieRepository moviesRepository) {
            _moviesRepository = moviesRepository;
        }

        public IEnumerable<MoviesLibrary.MovieData> Get()
        {
            return _moviesRepository.GetAll();
        }

        public MoviesLibrary.MovieData Get(int id)
        {
            return _moviesRepository.GetById(id);
        }

        public void Post(MoviesLibrary.MovieData movie)
        {
            _moviesRepository.Save(movie);
        }

        public void Put(MoviesLibrary.MovieData movie)
        {
            _moviesRepository.Save(movie);
        }

    }
}
