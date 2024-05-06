

using MultipleAdd.Models;


namespace MultipleAdd.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {


        IEnumerable<User> UserIndex();
        void UserAdd(User obj);
        void Update(User obj);
    

    }
}
