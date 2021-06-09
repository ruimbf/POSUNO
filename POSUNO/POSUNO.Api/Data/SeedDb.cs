using Microsoft.EntityFrameworkCore;
using POSUNO.Api.Data.Entities;
using POSUNO.Api.Enums;
using POSUNO.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSUNO.Api.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("Rui", "Ferreira", "ruif.sps@gmail.com", "322 311 4620");
            await CheckUserAsync("Caty", "Farinha", "catyffarinha@gmail.com", "322 311 4620");
            
            await CheckCostumersAsync();
            await CheckProductsAsync();
        }



        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, UserType.Admin.ToString());
                await _userHelper.AddUserToRoleAsync(user, UserType.User.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }
        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                Random random = new Random();
                User user = await _context.Users.FirstOrDefaultAsync();

                for (int i = 0; i < 200; i++)
                {
                    _context.Products.Add(new Entities.Product { 
                        Name = $"Produto {i}", 
                        Description = $"Produto {i}", 
                        Price = random.Next(5, 1000), 
                        Stock = random.Next(0, 500),
                        IsActive = true,
                        User = user 
                    });
                }

            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckCostumersAsync()
        {
            if (!_context.Products.Any())
            {
                Random random = new Random();
                User user = await _context.Users.FirstOrDefaultAsync();

                for (int i = 0; i < 50; i++)
                {
                    _context.Customers.Add(new Entities.Customer
                    {
                        FirstName = $"Cliente {i}",
                        LastName = $"Apelido {i}",
                        Address = $"Morada Cliente {i}",
                        Email = $"Email{i}@mail.pt",
                        IsActive = true,
                        PhoneNumber = $"999 000 0{i}",
                        User = user
                    });
                }

            }
            await _context.SaveChangesAsync();
        }

        //private async Task CheckUserAsync()
        //{
        //    if (!_context.Users.Any())
        //    {
        //        _context.Users.Add(new Entities.User { Email = "ruif.sps@gmail.com", FirstName = "Rui", LastName = "Ferreira", Password = "123456" });
        //        _context.Users.Add(new Entities.User { Email = "catyffarinha@gmail.com", FirstName = "Catarina", LastName = "Farinha", Password = "123456" });
        //    }
        //    await _context.SaveChangesAsync();
        //}
    }
}
