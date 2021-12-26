using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace PortSearchMatchModule
{
    public class MongoService
    {
        public static async Task PostAsync()
        {
            var connectionString = "mongodb+srv://vlad:1111@cluster0.baq47.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
            
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("Logistic");

            string text = System.IO.File.ReadAllText("D:\\ports.json");

            var document = BsonSerializer.Deserialize<BsonArray>(text);
            var collection = database.GetCollection<BsonDocument>("Ports");

            foreach (var port in document.Values)
            {
                // var a= BsonSerializer.Deserialize<BsonArray>(port.AsString);
                await collection.InsertOneAsync(port.ToBsonDocument());
            }
        }

        public static async Task<List<BsonDocument>> GetAllCollectionAsync()
        {
            var connectionString = "mongodb+srv://vlad:1111@cluster0.baq47.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("Logistic");

            var portCollections = database.GetCollection<BsonDocument>("Ports");
            var portBsonDocuments = await portCollections.Find(new BsonDocument()).ToListAsync();

            return portBsonDocuments;
        }

        public static async Task ShowAllPorts()
        {
            var ports = await MongoService.GetAllCollectionAsync();

            var portNames = new List<string>();

            foreach (var port in ports)
            {
                portNames = new List<string>();
                var a = port.Elements.ToList();

                var portCode = a[1].Value.AsString;
                var portNameType = a[2].Value.BsonType;

                if (portNameType == BsonType.Array)
                {
                    portNames = a[2].Value.AsBsonArray.Select(p => p.AsString).ToList();
                }
                else if (portNameType == BsonType.String)
                {
                    portNames.Add(a[2].Value.AsString);
                }

                Console.WriteLine($"portCode: {portCode}. portNames: {string.Join(", ", portNames)}");
            }
        }
    }
}
