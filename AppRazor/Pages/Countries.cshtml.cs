using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;

namespace AppRazor.Pages;

public class CountriesModel : PageModel
{
    private readonly IFriendsService _friendsService;

    public List<CountryOverview> Countries { get; set; } = [];

    public CountriesModel(IFriendsService friendsService)
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
            pageSize: 1000);

        var friends = response.PageItems?.ToList() ?? [];

        Countries = friends
            .Where(f => f.Address != null && !string.IsNullOrWhiteSpace(f.Address.Country))
            .GroupBy(f => f.Address.Country)
            .Select(g => new CountryOverview
            {
                Country = g.Key,
                FriendCount = g.Count()
            })
            .OrderBy(c => c.Country)
            .ToList();
    }
}

public class CountryOverview
{
    public string Country { get; set; } = "";
    public int FriendCount { get; set; }
}