using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Entity
{
    public class ActorAwardWinner
    {
        [Key]
        public int Id { get; set; }
        public int AwardId { get; set; }
        public int ActorId { get; set; }
    }
}
