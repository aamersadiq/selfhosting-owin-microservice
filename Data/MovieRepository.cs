using Core;
using MoviesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Data
{
    [Chained]
    public class MovieRepository : IMovieRepository
    {
        MovieDataSource _moviesDataSource;

        int _cacheExpiryInHours;
        public MovieRepository()
        {
            _moviesDataSource = new MovieDataSource();
        }

        public List<MovieData> GetAll()
        {
            return _moviesDataSource.GetAllData();
        }

        public MovieData GetById(int id)
        {
            if (id <= 0) {
                return null;
            }

            return _moviesDataSource.GetDataById(id);
        }


        public void Save(MovieData movie)
        {
            if (movie.MovieId == 0) {
                _moviesDataSource.Create(movie);
            }
            else
            {
                _moviesDataSource.Update(movie);
            }
        }
    }
}
