using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;

namespace AppRazor.Pages;

public class FriendDetailsModel : PageModel
{
    private readonly IFriendsService _friendsService;

    public IFriend? Friend { get; set; }

    public FriendDetailsModel(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task<IActionResult> OnGet(Guid id)
    {
        var response = await _friendsService.ReadFriendAsync(id, flat: false);

        Friend = response.Item;

        if (Friend == null)
        {
            return RedirectToPage("/Friends");
        }

        return Page();
    }
}