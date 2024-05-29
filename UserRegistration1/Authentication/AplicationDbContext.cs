using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserRegistration1.Authentication
{
    public class AplicationDbContext:IdentityDbContext<AplicationUser>
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext>options):base(options)
        {
            
        }
    }
}
