
using MultipleAdd;
using MultipleAdd.Models;



namespace MultipleAdd.Repository.IRepository
{
    public interface IStateRepository : IRepository<State>
    {
        void Update(State obj);


    }
}
