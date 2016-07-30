using MoviesLibrary;
using System.Collections.Generic;

namespace Data
{
    public interface IMovieRepository
    {
        List<MovieData> GetAll();
        MovieData GetById(int id);
        void Save(MovieData movie);
    }
}
