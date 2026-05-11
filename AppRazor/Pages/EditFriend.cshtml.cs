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

        Friend = new FriendCuDto
        {
            FriendId = response.Item.FriendId,
            FirstName = response.Item.FirstName,
            LastName = response.Item.LastName,
            Birthday = response.Item.Birthday
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var response = await _friendsService.UpdateFriendAsync(Friend);

        return RedirectToPage("/FriendDetails", new { id = Friend.FriendId });
    }
}