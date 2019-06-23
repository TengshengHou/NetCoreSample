using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api
{
    public class UserOperationException:Exception
    {
        public UserOperationException(string msg):base(msg)
        {

        }
    }
}
