using ExceptionLibrary;
using MongoDB.Bson;
using MongoDB.Driver;
using NutriLensClassLibrary.Models;
using System.Collections;
using System.Xml.Linq;

namespace NutriLensWebApp.Entities
{
    /// <summary>
    /// AppDbContext próprio para as entidades do MongoDB
    /// </summary>
    public static class AppMongoDbContext
    {
        private static string _connectionUri;
        private static string _database;

        public static void SetMongoContext(string connectionUri, string database)
        {
            _connectionUri = connectionUri;
            _database = database;
        }

        public static bool GetDatabaseConnectionStatus(out string message)
        {
            try
            {
                Client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                message = "Status: OK";
                return true;
            }
            catch(Exception ex)
            {
                message = $"Status: FALHA. {ExceptionManager.ExceptionMessage(ex)}";
                return false;
            }
        }

        private static IMongoClient? _client;

        private static IMongoClient Client
        {
            get
            {
                _client ??= new MongoClient(_connectionUri);
                return _client;
            }
        }

        private static IMongoCollection<TbcaItem>? _tbcaItem;

        public static IMongoCollection<TbcaItem> TbcaItem
        {
            get
            {
                _tbcaItem ??= Client.GetDatabase(_database).GetCollection<TbcaItem>("TbcaFoodItemsCollection");
                return _tbcaItem;
            }
        }

        private static IMongoCollection<Login>? _login;

        public static IMongoCollection<Login> Login
        {
            get
            {
                _login ??= Client.GetDatabase(_database).GetCollection<Login>("LoginCollection");
                return _login;
            }
        }

        private static IMongoCollection<UserInfo>? _userInfo;

        public static IMongoCollection<UserInfo> UserInfo
        {
            get
            {
                _userInfo ??= Client.GetDatabase(_database).GetCollection<UserInfo>("UserInfoCollection");
                return _userInfo;
            }
        }

        private static IMongoCollection<BarcodeItem>? _barcodeItem;

        public static IMongoCollection<BarcodeItem> BarcodeItem
        {
            get
            {
                _barcodeItem ??= Client.GetDatabase(_database).GetCollection<BarcodeItem>("BarCodeProductsCollection");
                return _barcodeItem;
            }
        }

        private static IMongoCollection<MongoImage>? _mongoImage;

        public static IMongoCollection<MongoImage> MongoImage
        {
            get
            {
                _mongoImage ??= Client.GetDatabase(_database).GetCollection<MongoImage>("TestCollection");
                return _mongoImage;
            }
        }
    }
}
