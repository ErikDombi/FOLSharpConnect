using FOLSharpConnect.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FOLSharpConnect.API.Controllers;

[ApiController, Route("/api/v1/")]
public class FOLv1Controller : Controller
{
    [Route("upcoming")]
    public async Task<IActionResult> Upcoming(string username, string password)
    {
        var cal = await ICSFetecher.FetchICS(username, password);
        var events =
            cal.Events
                .Where(x => x.DtEnd.AsUtc.Date >= DateTime.UtcNow)
                .OrderBy(x => x.DtEnd)
                .Select(x => new CalEvent(x));
        
        return Ok(JsonConvert.SerializeObject(events, Formatting.Indented));
    }
    
    [Route("today")]
    public async Task<IActionResult> Today(string username, string password)
    {
        var cal = await ICSFetecher.FetchICS(username, password);
        var events =
            cal.Events
                .Where(x => x.DtEnd.AsUtc.Date == DateTime.UtcNow.AddDays(1).Date)
                .OrderBy(x => x.DtEnd)
                .Select(x => new CalEvent(x));
        
        return Ok(JsonConvert.SerializeObject(events, Formatting.Indented));
    }
    
    [Route("tomorrow")]
    public async Task<IActionResult> Tomorrow(string username, string password)
    {
        var cal = await ICSFetecher.FetchICS(username, password);
        var events =
            cal.Events
                .Where(x => x.DtEnd.AsUtc.Date == DateTime.UtcNow.AddDays(2).Date)
                .OrderBy(x => x.DtEnd)
                .Select(x => new CalEvent(x));
        
        return Ok(JsonConvert.SerializeObject(events, Formatting.Indented));
    }
}