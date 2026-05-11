using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace AppRazor.Pages;

public class SeedModel : PageModel
{
    private const int DefaultSeedCount = 100;

    private readonly IAdminService _adminService;

    public string? Message { get; set; }

    public SeedModel(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostSeed()
    {
        var response = await _adminService.SeedAsync(DefaultSeedCount);
        Message = $"Database seeded successfully. Seeded friends: {response.Item.Db.NrSeededFriends}.";
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveSeed()
    {
        var response = await _adminService.RemoveSeedAsync(true);
        Message = $"Seed removed successfully. Seeded friends left: {response.Item.Db.NrSeededFriends}.";
        return Page();
    }
}
