using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(AppUser user, UserManager<AppUser> userManager);
    }
}
