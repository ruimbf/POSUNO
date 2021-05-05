using Microsoft.EntityFrameworkCore;
using POSUNO.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSUNO.Api.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckUserAsync();
            await CheckCostumersAsync();
            await CheckProductsAsync();
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
                    _context.Costumers.Add(new Entities.Customer
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

        private async Task CheckUserAsync()
        {
            if (!_context.Users.Any())
            {
                _context.Users.Add(new Entities.User { Email = "ruif.sps@gmail.com", FirstName = "Rui", LastName = "Ferreira", Password = "123456" });
                _context.Users.Add(new Entities.User { Email = "catyffarinha@gmail.com", FirstName = "Catarina", LastName = "Farinha", Password = "123456" });
            }
            await _context.SaveChangesAsync();
        }
    }
}
