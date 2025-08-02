using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface ICompanyServices
    {
        Task<bool> AddAsync(CompanyVM companyVm);
        Task<(bool, List<CompanyVM>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, CompanyVM?)> GetByIdAsync(int companyId);
        Task<bool> UpdateAsync(CompanyVM companyVm);
        Task<bool> DeleteAsync(int companyId);
        Task<bool> ExistsAsync(int companyId);
    }
}
