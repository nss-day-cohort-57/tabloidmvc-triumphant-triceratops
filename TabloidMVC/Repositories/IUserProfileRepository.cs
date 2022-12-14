using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        void AddUserProfile (UserProfile profile);
        UserProfile GetUserProfileById(int id);
        List<UserProfile> GetAll();
    }
}