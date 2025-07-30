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