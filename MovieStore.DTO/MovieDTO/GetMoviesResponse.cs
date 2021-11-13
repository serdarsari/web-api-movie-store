using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.MovieDTO
{
    public class GetMoviesResponse
    {
        public int TotalMovies { get; set; }
        public string NextPage { get; set; }
        public List<MovieResponse> Movies { get; set; }
    }
}
