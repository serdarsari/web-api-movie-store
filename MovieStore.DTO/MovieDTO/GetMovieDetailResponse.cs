using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.MovieDTO
{
    public class GetMovieDetailResponse
    {
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public string Genre { get; set; }
        public double Budget { get; set; }
        public double Rating { get; set; }
        public string Storyline { get; set; }
        public List<string> Actors { get; set; }
        public List<string> Directors { get; set; }
        public string ErrorMessage { get; set; }

    }
}
