using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Entity
{
    public class Actor
    {
        [Key]
        public int ActorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public IEnumerable<ActorAwardWinner> ActorAwardWinners { get; set; }
        public IEnumerable<MovieActor> MovieActors { get; set; }
    }
}
