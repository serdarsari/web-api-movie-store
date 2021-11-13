using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.ActorDTO
{
    public class GetActorsResponse
    {
        public int TotalActors { get; set; }
        public string NextPage { get; set; }
        public List<ActorResponse> Actors { get; set; }
    }
}
