using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DTO.AwardDTO
{
    public class GetAwardsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
