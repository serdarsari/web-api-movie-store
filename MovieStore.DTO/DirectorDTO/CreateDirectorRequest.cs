using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.DirectorDTO
{
    public class CreateDirectorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public List<int> AwardsIds { get; set; }
        public List<int> MoviesIds { get; set; }
    }
}
