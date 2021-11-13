using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Entity
{
    public class Director
    {
        [Key]
        public int DirectorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public IEnumerable<DirectorAwardWinner> DirectorAwardWinners { get; set; }
        public IEnumerable<MovieDirector> MovieDirectors { get; set; }
    }
}
