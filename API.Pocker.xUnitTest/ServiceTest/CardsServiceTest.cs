using API.Pocker.Models.Cards;
using API.Pocker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace API.Pocker.xUnitTest.ServiceTest
{
    public class CardsServiceTest
    {
        [Fact]
        public async Task CreateAsync()
        {
            var db = ServicesFactory.CreateDb();
            var mapper = ServicesFactory.CreateMapper();
            var service = new CardsService(db, mapper);
            var request = new CardsRequest
            {
                Value = 112233,
            };

            var create = await service.CreateAsync(request);

            Assert.True(create.Succeeded == true);
            Assert.True(create.Data.Id != null);
        }
        [Fact]
        public async Task DeleteAsync()
        {
            var db = ServicesFactory.CreateDb();
            var mapper = ServicesFactory.CreateMapper();
            var service = new CardsService(db, mapper);
            var request = new CardsRequest
            {
                Value = 112233,
            };

            var insert = await service.CreateAsync(request);

            var delete = await service.DeleteAsync(insert.Data.Id);
            Assert.True(delete.Succeeded == true);
            Assert.True(!db.Cards.Select(c => c.Id == insert.Data.Id).Any());
        }
        [Fact]
        public async Task GetAllAsync()
        {
            var db = ServicesFactory.CreateDb();
            var mapper = ServicesFactory.CreateMapper();
            var service = new CardsService(db, mapper);

            for (int i = 0; i < 10; i++)
            {
                var request = new CardsRequest
                {
                    Value = i+1,
                };
                await service.CreateAsync(request);
            }

            var getAll = await service.GetAllAsync();
            Assert.True(getAll.Succeeded == true);
            Assert.True(db.Cards.Count() == getAll.Data.Count);
        }
    }
}
