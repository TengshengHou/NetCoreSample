using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
using MongoDB.Driver;

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
            //var list = _contactContext.
        }

        public Task<bool> ApprovalAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            return (await _contactContext.ContactApplyRequest.FindAsync(a => a.UserId == userId)).ToList(cancellationToken);
        }
    }
}
