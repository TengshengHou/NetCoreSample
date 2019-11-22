using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.integrationEvents.Events
{
    public class UserProfileChangedEvent
    {
        public int userID { get; set; }
        public string Name { get; set; }
        //公司
        public string Company { get; set; }
        //职位
        public string Title { get; set; }
        //头像地址
        public string Avatar { get; set; }
    }
}
