using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface ICompanyRepo
    {
        Task<bool> AddAsync(Company company);
        Task<(bool, List<Company>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, Company?)> GetByIdAsync(int companyId);
        Task<bool> UpdateAsync(Company company);
        Task<bool> DeleteAsync(int companyId);
        Task<bool> ExistsAsync(int companyId);
    }
}
