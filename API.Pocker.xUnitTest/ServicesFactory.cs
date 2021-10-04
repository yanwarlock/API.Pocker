using API.Pocker.Data;
using API.Pocker.Mapping;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace API.Pocker.xUnitTest
{
    internal static class ServicesFactory
    {
        internal static IMapper CreateMapper() => CreateAutoMapperConfiguration().CreateMapper();
        private static MapperConfiguration CreateAutoMapperConfiguration() =>
           new MapperConfiguration(mc => { mc.AddProfile(new MapProfile()); });
        internal static ApplicationDbContext CreateDb() =>
           new ApplicationDbContext(CreateNewContextOptions());

        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();


            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
