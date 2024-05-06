




using MultipleAdd.Models;
using MultipleAdd.Repository.IRepository;



namespace MultipleAdd.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private UserEntites _db;
        public CityRepository(UserEntites db) : base(db)
        {
            _db = db;
        }
        public void Update(City obj)
        {
            Update(obj);
        }



    }
}
