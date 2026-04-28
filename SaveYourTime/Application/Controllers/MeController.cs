using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Application.Controllers;

public class MeController : ControllerBase
{
    [HttpPost("[controller]/[action]")]
    public async Task<IActionResult> MyFirstGet([FromBody] MyClass myClass)
    {
        return Ok(new MyClassResponse(myClass.Id.ToString(), myClass.Id+123));
    }
}

public class MyClass
{
    public int Id { get; set; }
}

public record MyClassResponse(string Response, int Smth);