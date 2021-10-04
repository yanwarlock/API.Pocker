using API.Pocker.Controllers;
using API.Pocker.Models;
using API.Pocker.Models.Cards;
using API.Pocker.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace API.Pocker.xUnitTest.ControllerTest
{
   public class CardsControllerTest
    {
        [Fact]
        public async Task Test_PostCards()
        {
            var db = ServicesFactory.CreateDb();
            var mapper = ServicesFactory.CreateMapper();
            var service = new CardsService(db, mapper);
            var controller = new CardsController(service);

            var request = new CardsRequest
            {
                Value = 112233,
            };

            var response =  await controller.Post(request);

            Assert.IsType<CreatedAtActionResult>(response);
        }
        [Fact]
        public async Task Test_GetCards()
        {
            var db = ServicesFactory.CreateDb();
            var mapper = ServicesFactory.CreateMapper();
            var service = new CardsService(db, mapper);
            var controller = new CardsController(service);

            var request = new CardsRequest
            {
                Value = 112233,
            };
            var response = await controller.Post(request);

            var getResponse = await controller.Get("id_not_valido");
            Assert.IsType<NotFoundObjectResult>(getResponse);
        }

        [Fact]
        public async Task Test_DeleteCards()
        {
            var db = ServicesFactory.CreateDb();
            var mapper = ServicesFactory.CreateMapper();
            var service = new CardsService(db, mapper);
            var controller = new CardsController(service);

            var request = new CardsRequest
            {
                Value = 112233,
            };
            var responseCreate = await service.CreateAsync(request);

            var responseGet = await controller.Get(responseCreate.Data.Id);
            var responceDelete = await controller.Delete(responseCreate.Data.Id);

            Assert.IsType<OkObjectResult>(responseGet);
            Assert.IsType<OkObjectResult>(responceDelete);
        }
    }
}
