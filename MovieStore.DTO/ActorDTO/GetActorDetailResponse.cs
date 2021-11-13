using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.ActorDTO
{
    public class GetActorDetailResponse
    {
        public string FullName { get; set; }
        public string Born { get; set; }
        public string Biography { get; set; }
        public List<string> Awards { get; set; }
        public List<string> Movies { get; set; }
        public string ErrorMessage { get; set; }
    }
}
