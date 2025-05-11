using Microsoft.AspNetCore.Mvc;

namespace MicroserviceB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JustBController : ControllerBase
{
    [HttpPost("process")]
    public IActionResult ProcessData([FromBody] string message)
    {
        Console.WriteLine($"[B] Received: {message}");
        return Ok($"[B] Processed: {message}");
    }
}
