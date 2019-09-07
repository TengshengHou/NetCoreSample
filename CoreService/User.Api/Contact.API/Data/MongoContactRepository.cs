using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Models;
using MongoDB.Driver;
namespace Contact.API.Data
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly ContactContext _contactContext;
        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }

        public async Task<bool> AddContacAsync(int userId, BaseUserInfo baseUserInfo, CancellationToken cancellationToken)
        {

            if (_contactContext.ContactBooks.CountDocuments(c => c.UserId == userId) == 0)
            {
                await _contactContext.ContactBooks.InsertOneAsync(new ContactBook()
                {
                     UserId=userId
                });

            }

            var filter = Builders<ContactBook>.Filter.Eq(c => c.UserId, userId);
            UpdateDefinition<ContactBook> update = Builders<ContactBook>.Update.AddToSet(c => c.Contacts, new Models.Contact()
            {
                UserId = baseUserInfo.UserId,
                Avatar = baseUserInfo.Avatar,
                Company = baseUserInfo.Company,
                Name = baseUserInfo.Name,
                Title = baseUserInfo.Title,
            });
            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);
            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
        }

        public async Task<bool> UpdateContactionInfoAsync(BaseUserInfo baseUserInfo, CancellationToken cancellationToken)
        {

            //var  filterDefinition=Builders<ContactBook>
            var contackBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == baseUserInfo.UserId, null, cancellationToken)).FirstOrDefault();
            if (contackBook == null)
                return true;

            var contactIds = contackBook.Contacts.Select(a => a.UserId);
            var fileter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.In(c => c.UserId, contactIds),
                Builders<ContactBook>.Filter.ElemMatch(c => c.Contacts, contact => contact.UserId == baseUserInfo.UserId)
                );
            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Name", baseUserInfo.Name)
                .Set("Contacts.$.Avatar", baseUserInfo.Avatar)
                .Set("Contacts.$.Company", baseUserInfo.Company)
                .Set("Contacts.$.title", baseUserInfo.Title);
            var updateResult = _contactContext.ContactBooks.UpdateMany(fileter, update);
            return updateResult.MatchedCount == updateResult.ModifiedCount;
        }
    }
}
