using AutoMapper;
using Core.Mapping;
using Core.Services;
using Core.Services.Contracts;
using Core.ViewModels.Notification;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Test.Tests
{
    public class NotificationServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private INotificationService service;
        private IMapper mapper;
        private string userId;
        private string firstId;
        private string secondId;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddAutoMapper(cfg => cfg.AddProfile<NotificationProfile>())
                .AddSingleton<INotificationService, NotificationService>()
                .BuildServiceProvider();

            IRepository repository = serviceProvider.GetService<IRepository>();
            service = serviceProvider.GetService<INotificationService>();
            mapper = serviceProvider.GetService<IMapper>();

            await SeedDbAsync(repository);
        }

        [Test]
        public async Task GetUnreadCount_ReturnsCorrectly()
        {
            int expected = 2;
            int actual = await service.GetUnreadCount(userId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Read_ChecksCorrectly()
        {
            int expected = 1;

            await service.Read(firstId);
            int actual = await service.GetUnreadCount(userId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAll_ReturnsCorrectCount()
        {
            int expected = 2;

            NotificationDetailsModel[] all = await service.GetAll(userId) as NotificationDetailsModel[];
            int actual = all.Length;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAll_ReturnsCorrectly()
        {
            string firstExpectedTitle = "Title for test notification";
            string secondExpectedTitle = "Another title for test notification";
            string firstExpectedText = "This is the text for our test notification";
            string secondExpectedText = "Lorem ipsum x20";
            DateTime firstExpectedDate = new DateTime(2022, 3, 28);
            DateTime secondExpectedDate = new DateTime(2022, 3, 23);

            NotificationDetailsModel[] all = await service.GetAll(userId) as NotificationDetailsModel[];
            NotificationDetailsModel first = all[0];
            NotificationDetailsModel second = all[1];

            Assert.AreEqual(firstExpectedDate, first.Date);
            Assert.AreEqual(secondExpectedDate, second.Date);

            Assert.AreEqual(firstExpectedTitle, first.Title);
            Assert.AreEqual(secondExpectedTitle, second.Title);

            Assert.AreEqual(firstExpectedText, first.Text);
            Assert.AreEqual(secondExpectedText, second.Text);

            Assert.AreEqual(firstId, first.Id);
            Assert.AreEqual(secondId, second.Id);
        }

        [Test]
        public async Task Delete_DeletesCorrectly_SingleId()
        {
            int expected = 1;

            string[] ids = { firstId };
            await service.Delete(ids);

            NotificationDetailsModel[] all = await service.GetAll(userId) as NotificationDetailsModel[];
            int actual = all.Length;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Delete_DeletesCorrectly_MultipleIds()
        {
            int expected = 0;

            string[] ids = { firstId, secondId };
            await service.Delete(ids);

            NotificationDetailsModel[] all = await service.GetAll(userId) as NotificationDetailsModel[];
            int actual = all.Length;

            Assert.AreEqual(expected, actual);
        }

        private async Task SeedDbAsync(IRepository repository)
        {
            User user = new User()
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@abv.bg"
            };

            Notification firstNotification = new Notification()
            {
                Title = "Title for test notification",
                Text = "This is the text for our test notification",
                User = user,
                Date = new DateTime(2022, 3, 28),
                IsChecked = false
            };

            Notification secondNotification = new Notification()
            {
                Title = "Another title for test notification",
                Text = "Lorem ipsum x20",
                User = user,
                Date = new DateTime(2022, 3, 23),
                IsChecked = false
            };

            await repository.AddAsync(user);
            await repository.AddAsync(firstNotification);
            await repository.AddAsync(secondNotification);
            await repository.SaveChangesAsync();

            User userFromDb = await repository.All<User>()
                .FirstAsync();

            Notification firstNotificationFromDb = await repository.All<Notification>(n => n.Title == "Title for test notification")
                .FirstAsync();

            Notification secondNotificationFromDb = await repository.All<Notification>(n => n.Title == "Another title for test notification")
                .FirstAsync();

            firstId = firstNotificationFromDb.Id;
            secondId = secondNotificationFromDb.Id;

            string userId = userFromDb.Id;
            this.userId = userId;
        }
    }
}
