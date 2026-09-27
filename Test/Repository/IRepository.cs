using Test.model;

namespace Test.Repository
{
    public interface IRepository
    {
        Task<List<Employee>> GetAll();

        Task<Employee?> GetById(int id);

        Task<int> Add(Employee employee);

        Task<bool> Update(int id, Employee employee);

        Task<bool> Delete(int id);
    }
}
