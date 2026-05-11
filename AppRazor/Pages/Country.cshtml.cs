using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace AppRazor.Pages;

public class CountryModel : PageModel
{
    private readonly IFriendsService _friendsService;

    public string Country { get; set; } = "";

    public List<CityOverview> Cities { get; set; } = [];

    public CountryModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task<IActionResult> OnGet(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return RedirectToPage("/Countries");
        }

        Country = country;

        var response = await _friendsService.ReadFriendsAsync(
            seeded: true,
            flat: false,
            filter: null,
            pageNumber: 0,
            pageSize: 1000);

        var friends = response.PageItems?.ToList() ?? [];

        Cities = friends
            .Where(f => f.Address != null &&
                        f.Address.Country == country &&
                        !string.IsNullOrWhiteSpace(f.Address.City))
            .GroupBy(f => f.Address.City)
            .Select(g => new CityOverview
            {
                City = g.Key,
                FriendCount = g.Count(),
                PetCount = g.Sum(f => f.Pets?.Count() ?? 0)
            })
            .OrderBy(c => c.City)
            .ToList();

        return Page();
    }
}

public class CityOverview
{
    public string City { get; set; } = "";
    public int FriendCount { get; set; }
    public int PetCount { get; set; }
}