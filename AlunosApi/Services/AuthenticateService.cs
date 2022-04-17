using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticateService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false); // 3° param: não deseja persistir o Cookie de entrada no navegador após o mesmo ser fechado
                                                                                                                    // 4° param: não deseja que a conta do usuário seja bloqueada caso a conexão falhar
            return result.Succeeded; // true se sucesso - false caso não
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            //Cria um usuário e retorna OK

            var appUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(appUser, password);

            if (result.Succeeded) 
            {
                await _signInManager.SignInAsync(appUser, isPersistent: false);
            }

            return result.Succeeded;
        }
    }
}
