using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.MovieDTO
{
    public class CreateMovieRequest
    {
        public string Name { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public double Budget { get; set; }
        public double Rating { get; set; }
        public string Storyline { get; set; }
        public List<int> ActorsIds { get; set; }
        public List<int> DirectorsIds { get; set; }
    }
}
