using GlobalRequest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalRequest.Controllers;

[ApiController]
[Route("[controller]")]
public class DummyController : ControllerBase
{
    [HttpPost]
    public IActionResult GetMvc()
    {
        var result = new
        {
            InstanceFilesPresent = Request?.Form.Files.Any() ?? false,
            StaticFilesPresent = HttpContextHelper.Current.Request.Form.Files.Any(),
        };

        Console.WriteLine($"InstanceFilesPresent: {result.InstanceFilesPresent}");
        Console.WriteLine($"StaticFilesPresent: {result.StaticFilesPresent}");

        return Ok(result);
    }
}