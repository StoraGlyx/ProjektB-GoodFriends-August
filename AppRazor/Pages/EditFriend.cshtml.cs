using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Services.Interfaces;

namespace AppRazor.Pages;

public class EditFriendModel : PageModel
{
    private readonly IFriendsService _friendsService;

    [BindProperty]
    public FriendCuDto Friend { get; set; } = new();

    public EditFriendModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {
        var response = await _friendsService.ReadFriendAsync(id, flat: false);

        if (response.Item == null)
        {
            return RedirectToPage("/Friends");
        }

        Friend = new FriendCuDto(response.Item);

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var existing = await _friendsService.ReadFriendAsync(Friend.FriendId!.Value, flat: false);

        if (existing.Item == null)
        {
            return RedirectToPage("/Friends");
        }

        Friend.AddressId = existing.Item.Address?.AddressId;
        Friend.PetsId = existing.Item.Pets?.Select(p => p.PetId).ToList();
        Friend.QuotesId = existing.Item.Quotes?.Select(q => q.QuoteId).ToList();

        if (string.IsNullOrWhiteSpace(Friend.Email))
        {
            Friend.Email = existing.Item.Email;
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _friendsService.UpdateFriendAsync(Friend);

        return RedirectToPage("/FriendDetails", new { id = Friend.FriendId });
    }
}