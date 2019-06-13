using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BankProject.Models;
using System.Linq;

namespace BankProject.Infrastructure
{
    public class CustomPasswordValidator : PasswordValidator<AppUser> // Implementuje możliwość customowej obsługi haseł gotowy schemat z MS'a
    {
        public override async Task<IdentityResult> ValidateAsync(       // Implementacja motedy interfejsu IPasswordValidator
                UserManager<AppUser> manager, AppUser user, string password)
        {
            IdentityResult result = await base.ValidateAsync(manager,
                user, password);

            List<IdentityError> errors = result.Succeeded ?
                new List<IdentityError>() : result.Errors.ToList();

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Hasło nie może zawierać nazwy użytkownika."
                });
            }

            if (password.Contains("12345"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsSequence",
                    Description = "Hasło nie może zawierać sekwencji liczbowej."
                });
            }

            return errors.Count == 0 ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());
        }
    }
}