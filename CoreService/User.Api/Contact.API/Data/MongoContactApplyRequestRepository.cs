using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRepository
    {
        private readonly ContactContext _contactContext;
        public MongoContactApplyRequestRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        public async Task<bool> AddRequestAsync(ContactApplyRequest request, CancellationToken cancellationToken)
        {
            var fileter = Builders<ContactApplyRequest>.Filter.Where(r => r.UserId == request.UserId && r.ApplierID == request.ApplierID);
            if ((await _contactContext.ContactApplyRequest.CountDocumentsAsync(fileter)) > 0)
            {
                var update = Builders<ContactApplyRequest>.Update.Set(r => r.ApplyTime, DateTime.Now);
                //var options = new UpdateOptions { IsUpsert = true };
                var reslut = await _contactContext.ContactApplyRequest.UpdateOneAsync(fileter, update, null, cancellationToken);
                return reslut.MatchedCount == reslut.ModifiedCount && reslut.MatchedCount == 1;
            }
            await _contactContext.ContactApplyRequest.InsertOneAsync(request, null, cancellationToken);
            return true;

        }

        public async Task<bool> ApprovalAsync(int userId, int applierId, CancellationToken cancellationToken)
        {

           

            var fileter = Builders<ContactApplyRequest>.Filter.Where(r => r.UserId == userId && r.ApplierID == applierId);
            var update = Builders<ContactApplyRequest>.Update
                .Set(r => r.Approvaled, 1)
                .Set(r => r.HandleTime, DateTime.Now);
            var reslut = await _contactContext.ContactApplyRequest.UpdateOneAsync(fileter, update, null, cancellationToken);
            return reslut.MatchedCount == reslut.ModifiedCount && reslut.MatchedCount == 1;
        }

        public async Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            return (await _contactContext.ContactApplyRequest.FindAsync(a => a.UserId == userId)).ToList(cancellationToken);
        }
    }
}
