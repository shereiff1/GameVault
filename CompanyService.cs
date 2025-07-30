public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public Task<IEnumerable<Company>> GetAllCompaniesAsync() => _companyRepository.GetAllAsync();
    public Task<Company> GetCompanyByIdAsync(int id) => _companyRepository.GetByIdAsync(id);
    public Task CreateCompanyAsync(Company company) => _companyRepository.AddAsync(company);
    public Task UpdateCompanyAsync(Company company) => _companyRepository.UpdateAsync(company);
    public Task DeleteCompanyAsync(int id) => _companyRepository.DeleteAsync(id);
}