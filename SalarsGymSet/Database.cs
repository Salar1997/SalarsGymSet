using MongoDB.Bson;
using MongoDB.Driver;
using SalarsGymSet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalarsGymSet
{
    public class Database
    {
        private const string EXERCISE_COLLECTION = "Exercise";
        private const string ACCOUNT_COLLECTION = "Accounts";


        private readonly IMongoDatabase _db;
        public Database()
        {
            var client = new MongoClient();
            _db = client.GetDatabase("test");
        }

        internal bool IfIdAccountExists(Account account)
        {
            var collection = _db.GetCollection<Account>(ACCOUNT_COLLECTION);
            List<Account> accounts = collection.Find(acc => true).ToList();
            foreach (var acc in accounts)
            {
                if (acc.UserName.ToLower() == account.UserName.ToLower() && acc.Password.ToLower() == acc.Password.ToLower())
                    return true;
            }
            return false;
        }

        internal Gymset GetGymSetById(ObjectId id)
        {
            var collection = _db.GetCollection<Gymset>(EXERCISE_COLLECTION);
            return collection.Find(gyset => gyset.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// save a gymset
        /// </summary>
        /// <param name="newGymSet"></param>
        internal void SaveGymSet(Gymset newGymSet)
        {
            var collection = _db.GetCollection<Gymset>(EXERCISE_COLLECTION);
            collection.InsertOne(newGymSet);
        }

        internal List<Gymset> GetGymSetForUser(string user)
        {
            var collection = _db.GetCollection<Gymset>(EXERCISE_COLLECTION);
            return collection.Find(gyset => gyset.User.ToLower() == user.ToLower()).ToList();
        }

        internal List<Gymset> GymSetFilter(string userName, string priority)
        {
            var collection = _db.GetCollection<Gymset>(EXERCISE_COLLECTION);
            return collection.Find(gyset => gyset.User.ToLower() == userName.ToLower() && gyset.Difficulty == priority).ToList();
        }

        internal void DeleteGymSet(ObjectId id)
        {
            var collection = _db.GetCollection<Gymset>(EXERCISE_COLLECTION);
            collection.DeleteOne(gyset => gyset.Id == id);
        }

        internal void UpdatePrivateTrainerGymSet(string user)
        {
            var collection = _db.GetCollection<Account>(ACCOUNT_COLLECTION);
            var filter = Builders<Account>.Filter.Eq("UserName", user);

            var updatePrivateTrainerGymset = Builders<Account>.Update
                .Set("privateTrainer", true);
            collection.UpdateOne(filter, updatePrivateTrainerGymset);
        }

        internal Account GetAccount(string user)
        {
            var collection = _db.GetCollection<Account>(ACCOUNT_COLLECTION);
            return collection.Find(acc => acc.UserName.ToLower() == user.ToLower()).FirstOrDefault();

        }

        internal void EditGymSet(ObjectId id, string exercise, string set, string difficulty)
        {
            var collection = _db.GetCollection<Gymset>(EXERCISE_COLLECTION);
            var filter = Builders<Gymset>.Filter.Eq("Id", id);
            var updateName = Builders<Gymset>.Update
                .Set("Exercise", exercise)
                .Set("Set", set)
                .Set("Difficulty", difficulty);
            collection.UpdateOne(filter, updateName);
        }

        internal void CreateAccount(Account account)
        {
            var collection = _db.GetCollection<Account>(ACCOUNT_COLLECTION);
            collection.InsertOne(account);
        }

    }
}
