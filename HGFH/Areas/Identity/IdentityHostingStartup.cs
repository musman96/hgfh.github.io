using System;
using HGFH.Data;
using HGFH.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(HGFH.Areas.Identity.IdentityHostingStartup))]
namespace HGFH.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //builder.ConfigureServices((context, services) => {
            //    services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(
            //    context.Configuration.GetConnectionString("SampleAppContextConnection")));
            //    services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            //        .AddDefaultUI()
            //        .AddEntityFrameworkStores<ApplicationDbContext>()
            //        .AddDefaultTokenProviders();
            //    services.AddScoped<IUserClaimsPrincipalFactory<SampleAppUser>,
            //        ApplicationUserClaimsPrincipalFactory
            //        >();
           // });
        }
    }
}