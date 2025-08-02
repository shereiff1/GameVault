using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.BLL.Services.Implementation
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ICompanyRepo _companyRepo;
        private readonly IMapper _mapper;

        public CompanyServices(ICompanyRepo companyRepo, IMapper mapper)
        {
            _companyRepo = companyRepo;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CompanyVM companyVm)
        {
            try
            {
                var company = _mapper.Map<Company>(companyVm);
                return await _companyRepo.AddAsync(company);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int companyId)
        {
            return await _companyRepo.DeleteAsync(companyId);
        }

        public async Task<bool> ExistsAsync(int companyId)
        {
            return await _companyRepo.ExistsAsync(companyId);
        }

        public async Task<(bool, List<CompanyVM>?)> GetAllAsync(bool includeDeleted = false)
        {
            var (success, companies) = await _companyRepo.GetAllAsync(includeDeleted);
            if (!success || companies == null)
                return (false, null);

            var vmList = _mapper.Map<List<CompanyVM>>(companies);
            return (true, vmList);
        }

        public async Task<(bool, CompanyVM?)> GetByIdAsync(int companyId)
        {
            var (success, company) = await _companyRepo.GetByIdAsync(companyId);
            if (!success || company == null)
                return (false, null);

            var vm = _mapper.Map<CompanyVM>(company);
            return (true, vm);
        }

        public async Task<bool> UpdateAsync(CompanyVM companyVm)
        {
            try
            {
                var company = _mapper.Map<Company>(companyVm);
                return await _companyRepo.UpdateAsync(company);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
