



using MultipleAdd;
using MultipleAdd.Models;

namespace MultipleAdd.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Countries>
    {
       void Update(Countries obj);

    }
}
