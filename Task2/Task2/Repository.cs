using System.Linq;
using MongoDB.Driver;


namespace Task2
{
    public class Repository
    {
        private readonly IMongoCollection<data> Collection;

        public Repository(string dbname, string connectionString)
        {
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(dbname);
            Collection = database.GetCollection<data>("Tender");
        }
        
        public void AddToDatabase(data data)
        {
            if(IsHashDifferent(data.Hash,data._id))
            {
                Collection.ReplaceOne<data>(t => t._embedded.Purchase.Id == data._embedded.Purchase.Id, data,
                    new UpdateOptions() { IsUpsert = true });
            }
        }

        public void RemoveFromDatabase(string id)
        {
            Collection.DeleteOne(t => t._embedded.Purchase.Id == id);
        }

        public bool IsElementExist(string id)
        {
            var tender = Collection.Find<data>(t => t._embedded.Purchase.Id == id);
            return tender.Any();
        }

        public bool IsHashDifferent(string hash, string id)
        {
            var tender = Collection.Find<data>(t=>t._id==id);
            if (tender.Any<data>()) 
            {
                return tender.First().Hash != hash;
            }
            else
            {
                return true;
            }
        }

    }
}
