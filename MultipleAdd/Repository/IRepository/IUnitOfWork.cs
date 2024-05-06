

namespace MultipleAdd.Repository.IRepository
{
    public interface IUnitOfWork
    {

        IUserRepository UserRepository { get; }
        ICountryRepository CountryRepository { get; }
        IStateRepository  StateRepository { get; }
        ICityRepository CityRepository { get; } 
        void Save();
    }
}
