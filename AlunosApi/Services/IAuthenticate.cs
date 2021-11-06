using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(string email, string password);
        Task LogOut();
    }
}
