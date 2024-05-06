


using Microsoft.AspNetCore.Identity;
using MultipleAdd.Models;
using MultipleAdd.Repository;
using MultipleAdd.Repository.IRepository;


namespace MultipleAdd.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private UserEntites _db;
        public ICountryRepository CountryRepository { get; private set; }

        public IStateRepository StateRepository { get; private set; }

        public ICityRepository CityRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }



        public UnitOfWork(UserEntites db)
        {
            _db = db;
            CountryRepository = new CountryRepository(_db);
            StateRepository = new StateRepository(_db);
            CityRepository = new  CityRepository(_db);
            UserRepository = new UserRepository(_db);

        }
        

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
