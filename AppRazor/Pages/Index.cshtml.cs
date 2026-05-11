using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Models.DTO;
using Services.Interfaces;

namespace AppRazor.Pages;

public class IndexModel : PageModel
{
    private readonly IFriendsService _friendsService;

    public List<IFriend> Friends { get; set; } = [];

    public IndexModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task OnGet()
    {
        var response = await _friendsService.ReadFriendsAsync(
            seeded: true,
            flat: false,
            filter: null,
            pageNumber: 0,
            pageSize: 20);

        Friends = response.PageItems?.ToList() ?? [];
    }
}
