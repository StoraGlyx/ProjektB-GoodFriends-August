using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Interfaces;
using Services.Interfaces;

namespace AppRazor.Pages;

public class FriendDetailsModel : PageModel
{
    private readonly IFriendsService _friendsService;
    private readonly IPetsService _petsService;

    public IFriend? Friend { get; set; }

    public FriendDetailsModel(
        IFriendsService friendsService,
        IPetsService petsService)
    {
        _friendsService = friendsService;
        _petsService = petsService;
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

    public async Task<IActionResult> OnPostDeletePet(Guid friendId, Guid petId)
    {
        await _petsService.DeletePetAsync(petId);

        return RedirectToPage("/FriendDetails", new { id = friendId });
    }
}