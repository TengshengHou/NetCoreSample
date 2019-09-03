using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;

namespace Contact.API.Data
{
    public class ContactApplyRequestRepository : IContactApplyRequestRepository
    {
        private readonly ContactContext _contactContext;
        public ContactApplyRequestRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        public Task<bool> AddRequestAsync(ContactApplyRequest contactApplyRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApprovalAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetRequestListAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
