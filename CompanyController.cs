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