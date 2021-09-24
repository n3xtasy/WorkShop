using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSAPI.Models.User;

namespace WSAPI.Context
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext()
        {
            Database.EnsureCreated();
        }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=workshop;Username=postgres;Password=Oliver15243");
        }
    }
    public class UserRepository
    {
        private UserContext userContext;

        public UserRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<Task> CreateAsync(CreateUserViewModel createUserViewModel)
        {
            await userContext.Users.AddAsync(new User()
            {
                Email = createUserViewModel.Email,
                Password = createUserViewModel.Password,
                RegistrationDate = DateTime.Now,
                Role = "User"
            });

            await userContext.SaveChangesAsync();

            return Task.CompletedTask;
        }
        public async Task<User> GetUserByAuthenticationDataAsync(AuthenticateUserViewModel userData)
        {
            try
            {
                var user = await userContext.Users.FirstOrDefaultAsync(u => u.Email == userData.Email && u.Password == userData.Password);
                
                return user;
            }
            catch
            {
                return null;
            }
        }
        public bool Exist(CreateUserViewModel createUserViewModel)
        {
            return userContext.Users.Where(u => u.Email == createUserViewModel.Email).Count() != 0;
        }

    }
    public class User
    { 
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Role { get; set; }
    }
}
