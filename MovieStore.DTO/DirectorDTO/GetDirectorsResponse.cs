using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.DirectorDTO
{
    public class GetDirectorsResponse
    {
        public int TotalDirectors { get; set; }
        public string NextPage { get; set; }
        public List<DirectorResponse> Directors { get; set; }
    }
}
