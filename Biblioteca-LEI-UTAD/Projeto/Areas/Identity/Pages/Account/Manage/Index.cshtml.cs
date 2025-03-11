// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trabalho.Areas.Data;

namespace Trabalho.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public string Username { get; set; }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Role")]
            public string Role { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Phone]
            [Display(Name = "PhoneNumber2")]
            public string PhoneNumber2 { get; set; }

            [Phone]
            [Display(Name = "PhoneNumber3")]
            public string PhoneNumber3 { get; set; }

        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Nome = user.Nome,
                Role = user.Role,
                Address = user.Address,
                PhoneNumber = phoneNumber,
                PhoneNumber2 = user.PhoneNumber2,
                PhoneNumber3 = user.PhoneNumber3
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.Nome != user.Nome)
            {
                user.Nome = Input.Nome;
            }

            if (Input.Role != user.Role)
            {
                user.Role = Input.Role;
            }

            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }

            if (Input.PhoneNumber2 != user.PhoneNumber2)
            {
                user.PhoneNumber2 = Input.PhoneNumber2;
            }

            if (Input.PhoneNumber3 != user.PhoneNumber3)
            {
                user.PhoneNumber3 = Input.PhoneNumber3;
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
