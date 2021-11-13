using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Entity
{
    public class MovieDirector
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int DirectorId { get; set; }
    }
}
