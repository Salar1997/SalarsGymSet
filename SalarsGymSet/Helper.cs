using MongoDB.Bson;
using SalarsGymSet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalarsGymSet
{
    public class Helper
    {

        private readonly Database db = new Database();

        internal bool IfAccountExists(Account account)
        {
            return db.IfIdAccountExists(account);
        }

        internal void CreateAccount(Account account)
        {
            db.CreateAccount(account);
        }

        internal List<Gymset> GetGymSetForUser(string name)
        {
            return db.GetGymSetForUser(name);
        }

        internal void SaveGymSet(Gymset gymset)
        {
            db.SaveGymSet(gymset);
        }

        internal Gymset GetGymSetById(string id)
        {
            ObjectId gymsetId = new ObjectId(id);
            return db.GetGymSetById(gymsetId);
        }

        internal List<Gymset> GetGymSetByPriority(string userName, string priority = null)
        {
            List<Gymset> gymsets = db.GetGymSetForUser(userName);
            string[] priorities = { "Hard", "Meduim", "Easy" };
            if (priority == "Easy")
            {
                Array.Reverse(priorities);
            }
            return gymsets.OrderBy(p => Array.IndexOf(priorities, p.Difficulty)).ToList();

        }

        internal List<Gymset> GetFilteredGymSet(string userName, string priority)
        {
            return db.GymSetFilter(userName, priority);
        }

        internal void EditGymSet(string id, string exercise, string description, string priority)
        {
            ObjectId gymsetId = new ObjectId(id);
            db.EditGymSet(gymsetId, exercise, description, priority);
        }

        internal void DeleteGymSetById(string id)
        {
            ObjectId gymsetId = new ObjectId(id);
            db.DeleteGymSet(gymsetId);
        }

        internal Account GetAcoount(string user)
        {
            return db.GetAccount(user);
        }

        internal void UpdatePrivateTrainerGymSet(string user)
        {
            db.UpdatePrivateTrainerGymSet(user);

        }
    }
}
