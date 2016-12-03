using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;

namespace Task2
{
    public class Repository
    {
        public IMongoDatabase Database { get; }
        private readonly IMongoCollection<data> Collection;

        public Repository(string dbname, string connectionString)
        {
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(dbname);
            Collection = database.GetCollection<data>("Tender");
        }
        public IMongoDatabase GetDatabase(string dbname)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Task2"].ConnectionString;
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(dbname);
            return database;
        }
        
        public void AddToDatabase(data data)
        {
            Collection.InsertOne(data);
        }

        public void RemoveFromDatabase(string id)
        {
            Collection.DeleteOne(t => t._embedded.Purchase.Id == id);
        }

        public bool IsElementExist(string id)
        {
            var tender = Collection.Find<data>(t => t._embedded.Purchase.Id == id);
            return tender.Count()!=0;
        }

        public bool IsHashDifferent(string hash)
        {
            var data = Collection.Find<data>(t => t.Hash == hash);
            return data.Count() == 0;
        }

    }
}
