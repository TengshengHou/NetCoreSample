using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Model
{
    public class UserTag
    {
        public int UserId { get; set; }
        public int Tag { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
