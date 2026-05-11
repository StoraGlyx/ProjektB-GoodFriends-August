using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;

namespace AppRazor.Pages;

public class FriendsModel : PageModel
{
    private readonly IFriendsService _friendsService;

    public string? Country { get; set; }
    public string? City { get; set; }

    public List<IFriend> Friends { get; set; } = [];

    public FriendsModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task OnGet(string? country, string? city)
    {
        Country = country;
        City = city;

        var response = await _friendsService.ReadFriendsAsync(
            seeded: true,
            flat: false,
            filter: null,
            pageNumber: 0,
            pageSize: 1000);

        var friends = response.PageItems?.ToList() ?? [];

        if (!string.IsNullOrWhiteSpace(country))
        {
            friends = friends
                .Where(f => f.Address != null && f.Address.Country == country)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            friends = friends
                .Where(f => f.Address != null && f.Address.City == city)
                .ToList();
        }

        Friends = friends
            .OrderBy(f => f.LastName)
            .ThenBy(f => f.FirstName)
            .ToList();
    }
}