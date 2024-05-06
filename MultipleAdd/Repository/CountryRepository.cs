using Microsoft.EntityFrameworkCore;
using MultipleAdd.Models;
using MultipleAdd.Repository.IRepository;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;



namespace MultipleAdd.Repository
{
    public class CountryRepository : Repository<Countries>, ICountryRepository
    {
        private UserEntites _db;
        public CountryRepository(UserEntites db) : base(db)
        {
            _db = db;
        }


        public void Update(Countries obj)
        {
            Update(obj);
        }




    }
}
