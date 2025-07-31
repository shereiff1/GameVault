
public class Company
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyInfo { get; set; }
}

public class History
{
    public int HistoryId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public double PlaytimeHours { get; set; }

    public int UserId { get; set; }
    public int GameId { get; set; }
}

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company> GetByIdAsync(int id);
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task DeleteAsync(int id);
}


public interface IHistoryRepository
{
    Task<IEnumerable<History>> GetAllAsync();
    Task<History> GetByIdAsync(int id);
    Task AddAsync(History history);
    Task UpdateAsync(History history);
    Task DeleteAsync(int id);
}


public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task<Company> GetByIdAsync(int id)
    {
        return await _context.Companies.FindAsync(id);
    }

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}


public class HistoryRepository : IHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public HistoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<History>> GetAllAsync()
    {
        return await _context.Histories.ToListAsync();
    }

    public async Task<History> GetByIdAsync(int id)
    {
        return await _context.Histories.FindAsync(id);
    }

    public async Task AddAsync(History history)
    {
        await _context.Histories.AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(History history)
    {
        _context.Histories.Update(history);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var history = await _context.Histories.FindAsync(id);
        if (history != null)
        {
            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();
        }
    }
}

public interface ICompanyService
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync();
    Task<Company> GetCompanyByIdAsync(int id);
    Task CreateCompanyAsync(Company company);
    Task UpdateCompanyAsync(Company company);
    Task DeleteCompanyAsync(int id);
}

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


public interface IHistoryService
{
    Task<IEnumerable<History>> GetAllHistoriesAsync();
    Task<History> GetHistoryByIdAsync(int id);
    Task CreateHistoryAsync(History history);
    Task UpdateHistoryAsync(History history);
    Task DeleteHistoryAsync(int id);
}


public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;

    public HistoryService(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public Task<IEnumerable<History>> GetAllHistoriesAsync() => _historyRepository.GetAllAsync();
    public Task<History> GetHistoryByIdAsync(int id) => _historyRepository.GetByIdAsync(id);
    public Task CreateHistoryAsync(History history) => _historyRepository.AddAsync(history);
    public Task UpdateHistoryAsync(History history) => _historyRepository.UpdateAsync(history);
    public Task DeleteHistoryAsync(int id) => _historyRepository.DeleteAsync(id);
}


[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _companyService.GetAllCompaniesAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var company = await _companyService.GetCompanyByIdAsync(id);
        return company == null ? NotFound() : Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Company company)
    {
        await _companyService.CreateCompanyAsync(company);
        return CreatedAtAction(nameof(Get), new { id = company.CompanyId }, company);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Company company)
    {
        if (id != company.CompanyId) return BadRequest();
        await _companyService.UpdateCompanyAsync(company);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _companyService.DeleteCompanyAsync(id);
        return NoContent();
    }
}

// Controllers/HistoryController.cs
[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;

    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _historyService.GetAllHistoriesAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var history = await _historyService.GetHistoryByIdAsync(id);
        return history == null ? NotFound() : Ok(history);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] History history)
    {
        await _historyService.CreateHistoryAsync(history);
        return CreatedAtAction(nameof(Get), new { id = history.HistoryId }, history);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] History history)
    {
        if (id != history.HistoryId) return BadRequest();
        await _historyService.UpdateHistoryAsync(history);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _historyService.DeleteHistoryAsync(id);
        return NoContent();
    }
}

services.AddScoped<ICompanyRepository, CompanyRepository>();
services.AddScoped<IHistoryRepository, HistoryRepository>();
services.AddScoped<ICompanyService, CompanyService>();
services.AddScoped<IHistoryService, HistoryService>();

