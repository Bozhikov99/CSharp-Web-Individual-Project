using Core.Mapping;
using Core.Services;
using Core.Services.Contracts;
using Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class GenreServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddSingleton<IRepository, Repository>()
                .AddAutoMapper(cfg => cfg.AddProfile<GenreProfile>())
                .AddSingleton<IGenreService, GenreService>()
                .BuildServiceProvider();
        }

        [Test]
        public void FirstTest()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
