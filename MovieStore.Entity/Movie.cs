using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Entity
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public double Budget { get; set; }
        public double Rating { get; set; }
        public string Storyline { get; set; }
        public IEnumerable<MovieActor> MovieActors { get; set; }
        public IEnumerable<MovieDirector> MovieDirectors { get; set; }
    }
}
