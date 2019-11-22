using Contact.API.Data;
using Contact.API.integrationEvents.Events;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.integrationEvents.EventHanding
{
    public class UserProfileChanagedEventHandler : ICapSubscribe
    {
        private IContactRepository _contactRepository;
        public UserProfileChanagedEventHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        [CapSubscribe("finbook.userapi.userprofilechanged")]
        public async Task UpdateContactInfo(UserProfileChangedEvent @event)
        {
            var token = new CancellationToken();
            await _contactRepository.UpdateContactionInfoAsync(new UserIdentity
            {
                Avatar = @event.Avatar,
                Company = @event.Company,
                Name = @event.Name,
                Title = @event.Title,
                UserId = @event.userID
            }, token);
        }
    }
}
