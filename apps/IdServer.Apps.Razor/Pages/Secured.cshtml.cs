using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdServer.Apps.Mvc.Pages;

[Authorize]
public class SecuredModel : PageModel
{
    private readonly ILogger<SecuredModel> _logger;

    public SecuredModel(ILogger<SecuredModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        _logger.LogInformation("secured");
    }
}
