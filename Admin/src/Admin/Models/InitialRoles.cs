using System;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Models
{
    public class InitialRoles
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole("Admin"));
                context.SaveChanges();
            }
        }
    }
}