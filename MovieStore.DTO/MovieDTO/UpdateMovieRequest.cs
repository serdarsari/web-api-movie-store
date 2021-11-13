using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.MovieDTO
{
    public class UpdateMovieRequest
    {
        public string Name { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public double Budget { get; set; }
        public double Rating { get; set; }
        public string Storyline { get; set; }
    }
}
