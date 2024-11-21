using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Common.Models
{
    public record PageRequest
    {
        public int PageNumber { get; init; } = 1;
        public int TakeCount { get; init; } = 10;
        public int SkipCount =>(PageNumber-1)* TakeCount;

    }
}
