
using MultipleAdd.Models;
using MultipleAdd.Repository;
using MultipleAdd.Repository.IRepository;


namespace MultipleAdd.Repository
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        private UserEntites _db;
        public StateRepository(UserEntites db) : base(db)
        {
            _db = db;
        }
        public void Update(State obj)
        {
            Update(obj);
        }

    }
}
