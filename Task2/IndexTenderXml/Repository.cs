using System.Linq;
using MongoDB.Driver;

namespace IndexTenderXml
{
    public class Repository
    {
        private readonly IMongoCollection<data> Collection;

        /// <summary>
        /// Конструктор класса Repository
        /// </summary>
        /// <param name="dbname">Название базы данных</param>
        /// <param name="connectionString">Строка соединения</param>
        public Repository(string dbname, string connectionString)
        {
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(dbname);
            Collection = database.GetCollection<data>("Tender");
        }
        
        /// <summary>
        /// Добавление объекта data в базу данных
        /// </summary>
        /// <param name="data">Объет, которые необходимо добавить</param>
        public void AddToDatabase(data data)
        {
            if(IsHashDifferent(data.Hash,data._id))
            {
                Collection.ReplaceOne<data>(t => t._embedded.Purchase.Id == data._embedded.Purchase.Id, data,
                    new UpdateOptions() { IsUpsert = true });
            }
        }

        /// <summary>
        /// Удаление объекта из базы данных по его ID
        /// </summary>
        /// <param name="id">ID удаляемого объекта</param>
        public void RemoveFromDatabase(string id)
        {
            Collection.DeleteOne(t => t._id == id);
        }

        /// <summary>
        /// Проверка на существование объекта с текущим id
        /// </summary>
        /// <param name="id">id проверяемого объекта</param>
        public bool IsElementExist(string id)
        {
            var tender = Collection.Find<data>(t => t._embedded.Purchase.Id == id);
            return tender.Any();
        }
        /// <summary>
        /// Проверка на различие хеша текущего объекта с хешем объекта, хранящимся в базе данных
        /// </summary>
        /// <param name="hash">Хеш проверяемого объекта</param>
        /// <param name="id">id объектов, хеш которых сравнивается</param>
        public bool IsHashDifferent(string hash, string id)
        {
            var tender = Collection.Find<data>(t=>t._id==id);
            
            if (tender.Any<data>()) 
            {
                return tender.First<data>().Hash != hash;
            }
            else
            {
                return true;
            }
        }

    }
}
