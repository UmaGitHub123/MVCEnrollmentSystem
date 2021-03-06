﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EnrollmentApp.Models;
using EnrollmentApp.DAL;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace EnrollmentApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public override string UserName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            StudentRepository student = new StudentRepository();

            Student dem = student.GetSingleStudent(null, UserName);
            if (dem == null)
            {
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here

                return userIdentity;
            }
            else
            {
                string usernamees = dem.FullName;

                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here

                userIdentity.AddClaim(new Claim("FullName", usernamees));

                return userIdentity;
            }
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<EnrollmentApp.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<EnrollmentApp.Models.Course> Courses { get; set; }
    }
}