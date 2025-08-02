using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace GameVault.DAL.Repository.Implementation
{
    public class CompanyRepo : ICompanyRepo
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Company company)
        {
            try
            {
                await _context.companies.AddAsync(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int companyId)
        {
            try
            {
                var company = await _context.companies.FirstOrDefaultAsync(c => c.CompanyId == companyId);
                if (company == null) return false;

                company.MarkAsDeleted();
                _context.companies.Update(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<(bool, List<Company>?)> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var companies = includeDeleted
                    ? await _context.companies
                        .Include(c => c.Games)
                        .Include(c => c.Inventory)
                        .ToListAsync()
                    : await _context.companies
                        .Where(c => !c.IsDeleted)
                        .Include(c => c.Games)
                        .Include(c => c.Inventory)
                        .ToListAsync();

                return (true, companies);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<(bool, Company?)> GetByIdAsync(int companyId)
        {
            try
            {
                var company = await _context.companies
                    .Include(c => c.Games)
                    .Include(c => c.Inventory)
                    .FirstOrDefaultAsync(c => c.CompanyId == companyId && !c.IsDeleted);

                return (company != null, company);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            try
            {
                _context.companies.Update(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(int companyId)
        {
            try
            {
                return await _context.companies.AnyAsync(c => c.CompanyId == companyId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
