using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Entity
{
    public class Award
    {
        [Key]
        public int AwardId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public IEnumerable<ActorAwardWinner> ActorAwardWinners { get; set; }
        public IEnumerable<DirectorAwardWinner> DirectorAwardWinners { get; set; }
    }
}
