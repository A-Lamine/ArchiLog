using System;
using System.Collections.Generic;
using ArchiAPI.Controllers;
using ArchiAPI.Data;
using ArchiAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ArchiTest
{
    public class PizzaUnitTestController
    {
        private PizzasController PizzaRepository;
        public static DbContextOptions<ArchiDbContext> dbContextOptions;
        public static string connectionString = "Server=tcp:ecandotti.database.windows.net,1433;Initial Catalog=archilog;Persist Security Info=False;User ID=superenzo;Password=Superpassword13;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        static PizzaUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<ArchiDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public PizzaUnitTestController()
        {
            var context = new ArchiDbContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            PizzaRepository = new PizzasController(context);
        }

        #region Get Pizza By Id

            [Fact]
            public async void Task_GetPizzaById_Return_OkResult()
            {
                //Arrange
                var pizzaId = 1;

                //Act
                var data = await PizzaRepository.GetModelById(pizzaId);

                //Assert
                Assert.IsType<OkObjectResult>(data);
            }

            [Fact]
            public async void Task_GetPizzaById_Return_NotFoundResult()
            {
                //Arrange
                var pizzaId = 9999;

                //Act
                var data = await PizzaRepository.GetModelById(pizzaId);

                //Assert
                    Assert.IsType<NotFoundResult>(data);
            }

            [Fact]
            public async void Task_GetPizzaById_MatchResult()
            {
                //Arrange
                int pizzaId = 1;

                //Act
                var data = await PizzaRepository.GetModelById(pizzaId);

                //Assert
                Assert.IsType<OkObjectResult>(data);

                var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
                var pizza = okResult.Value.Should().BeAssignableTo<Pizza>().Subject;

                Assert.Equal("Test Name 1", pizza.Name);
                Assert.Equal("Test Topping 1", pizza.Topping);
            }

        #endregion

        #region Get All Pizza 

            [Fact]
            public async void Task_GetPizzas_Return_OkResult()
            {
                //Act  
                var data = await PizzaRepository.TriModel("", "");

                //Assert  
                Assert.IsType<OkObjectResult>(data);
            }

            [Fact]
            public async void Task_GetPizzas_MatchResult()
            {
                //Act  
                var data = await PizzaRepository.TriModel("", "");

                //Assert  
                Assert.IsType<OkObjectResult>(data);

                var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
                var pizza = okResult.Value.Should().BeAssignableTo<List<Pizza>>().Subject;

                Assert.Equal("Test Name 1", pizza[0].Name);
                Assert.Equal("Test Topping 1", pizza[0].Topping);

                Assert.Equal("Test Name 2", pizza[1].Name);
                Assert.Equal("Test Topping 2", pizza[1].Topping);
            }

        #endregion

        #region Add New Pizza  

            [Fact]
            public async void Task_Add_ValidData_Return_OkResult()
            {
                //Arrange  
                var pizza = new Pizza() { Name = "Chevre Miel", Price = 11, Topping = "Chevre, Miel" };

                //Act  
                var data = await PizzaRepository.PostModel(pizza);

                //Assert  
                Assert.IsType<OkObjectResult>(data);
            }

            [Fact]
            public async void Task_Add_ValidData_MatchResult()
            {
                //Arrange
                var pizza = new Pizza() { Name = "Chevre avec Miel", Price = 11, Topping = "Chevre, Miel" };

                //Act  
                var data = await PizzaRepository.PostModel(pizza);

                //Assert  
                Assert.IsType<OkObjectResult>(data);

                var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

                Assert.Equal(3, okResult.Value);
            }

        #endregion

        #region Delete Pizza  

            [Fact]
            public async void Task_Delete_Pizza_Return_OkResult()
            {
                //Arrange
                var pizzaId = 1;

                //Act  
                var data = await PizzaRepository.DeleteModel(pizzaId);

                //Assert  
                Assert.IsType<OkResult>(data);
            }

            [Fact]
            public async void Task_Delete_Pizza_Return_NotFoundResult()
            {
                //Arrange
                var pizzaId = 9999;

                //Act  
                var data = await PizzaRepository.DeleteModel(pizzaId);

                //Assert  
                Assert.IsType<NotFoundResult>(data);
            }

        #endregion
    }
}
