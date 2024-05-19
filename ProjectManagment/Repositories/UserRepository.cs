using ProjectManagment.Data;
using System.Security.Claims;

namespace ProjectManagment.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}
