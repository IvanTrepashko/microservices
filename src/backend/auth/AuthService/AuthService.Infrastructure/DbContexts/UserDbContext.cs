using AuthService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.DbContexts;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) { }
