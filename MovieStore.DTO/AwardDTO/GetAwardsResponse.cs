using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.AwardDTO
{
    public class GetAwardsResponse
    {
        public int TotalAwards { get; set; }
        public string NextPage { get; set; }
        public List<AwardResponse> Awards { get; set; }
    }
}
