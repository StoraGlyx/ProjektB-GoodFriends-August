using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Services.Interfaces;

namespace AppRazor.Pages;

public class EditAddressModel : PageModel
{
    private readonly IFriendsService _friendsService;
    private readonly IAddressesService _addressesService;

    [BindProperty]
    public AddressCuDto Address { get; set; } = new();

    public Guid FriendId { get; set; }

    public EditAddressModel(
        IFriendsService friendsService,
        IAddressesService addressesService)
    {
        _friendsService = friendsService;
        _addressesService = addressesService;
    }

    public async Task<IActionResult> OnGet(Guid friendId)
    {
        var response = await _friendsService.ReadFriendAsync(friendId, flat: false);

        if (response.Item == null || response.Item.Address == null)
        {
            return RedirectToPage("/Friends");
        }

        FriendId = friendId;

        Address = new AddressCuDto
        {
            AddressId = response.Item.Address.AddressId,
            StreetAddress = response.Item.Address.StreetAddress,
            ZipCode = response.Item.Address.ZipCode,
            City = response.Item.Address.City,
            Country = response.Item.Address.Country,
            FriendsId = new List<Guid> { response.Item.FriendId }
        };

        return Page();
    }

    public async Task<IActionResult> OnPost(Guid friendId)
    {
        FriendId = friendId;
        Address.FriendsId = new List<Guid> { friendId };

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _addressesService.UpdateAddressAsync(Address);

        return RedirectToPage("/FriendDetails", new { id = friendId });
    }
}