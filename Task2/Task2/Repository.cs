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

        public static IMongoDatabase GetDatabase(string dbname)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Task2"].ConnectionString;
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(dbname);
            return database;
        }
        
        public static void AddToDatabase(MiniTender data, IMongoDatabase database)
        {
            var collection = database.GetCollection<MiniTender>("Tender");
            collection.InsertOne(data);
        }

        public static void RemoveFromDatabase(string id, IMongoDatabase database)
        {
            var collection = database.GetCollection<MiniTender>("Tender");
            collection.DeleteOne(t => t.Id == id);
        }

        public static bool IsElementExist(string id, IMongoDatabase database)
        {
            var collection = database.GetCollection<MiniTender>("Tender");
            var tender = collection.Find<MiniTender>(t => t.Id == id);
            return tender.Count()!=0;
        }

    }
}
