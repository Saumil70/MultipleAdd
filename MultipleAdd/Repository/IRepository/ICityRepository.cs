


using MultipleAdd;
using MultipleAdd.Models;

namespace MultipleAdd.Repository.IRepository
{
    public interface ICityRepository : IRepository<City>
    {
        void Update(City obj);
      


    }
}
