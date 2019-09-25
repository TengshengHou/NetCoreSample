using Contact.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Data
{
    public class ContactContext
    {
        private IMongoDatabase _database;
        private IMongoCollection<ContactBook> _collection;
        private AppSettings _appSettings;
        public ContactContext(IOptionsSnapshot<AppSettings> settings)
        {
            _appSettings = settings.Value;
            var client = new MongoClient(_appSettings.MongContactConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(_appSettings.MongContactDataBase);
            }
        }

        private void CheckAndCreateCollection(string collectionName)
        {
            var collectionList = _database.ListCollections().ToList();
            var conllectionNames = new List<String>();
            collectionList.ForEach(b => conllectionNames.Add(b["name"].AsString));
            if (conllectionNames.Contains(collectionName))
                _database.CreateCollection(collectionName);
        }

        /// <summary>
        /// 用户通讯录
        /// </summary>
        public IMongoCollection<ContactBook> ContactBooks
        {
            get
            {
                CheckAndCreateCollection("ContactBooks");
                return _database.GetCollection<ContactBook>("ContactBooks");
            }
        }

        /// <summary>
        /// 好友申请请求记录
        /// </summary>

        public IMongoCollection<ContactApplyRequest> ContactApplyRequest
        {
            get
            {
                CheckAndCreateCollection("ContactBooks");
                return _database.GetCollection<ContactApplyRequest>("ContactBooks");
            }
        }
    }
}
