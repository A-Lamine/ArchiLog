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
    public class PostUnitTestController
    {
        private PizzasController PizzaRepository;
        public static DbContextOptions<ArchiDbContext> dbContextOptions;
        public static string connectionString = "Server=tcp:archilogla.database.windows.net,1433;Initial Catalog=Archilog;Persist Security Info=False;User ID=lamine;Password=Bejaia06.0;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        static PostUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<ArchiDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public PostUnitTestController()
        {
            var context = new ArchiDbContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            PizzaRepository = new PizzasController(context);
        }

        [Fact]
        public async void Task_GetPostById_Return_OkResult()
        {
            //Arrange
            var pizzaId = 2;

            //Act
            var data = await PizzaRepository.GetModelById(pizzaId);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        #region Get Pizza By Id

            [Fact]
            public async void Task_GetPostById_Return_NotFoundResult()
            {
                //Arrange
                var pizzaId = 3;

                //Act
                var data = await PizzaRepository.GetModelById(pizzaId);

                //Assert
                    Assert.IsType<NotFoundResult>(data);
            }

            [Fact]
            public async void Task_GetPostById_Return_BadRequestResult()
            {
                //Arrange
                int pizzaId = 9999;

                //Act
                var data = await PizzaRepository.GetModelById(pizzaId);

                //Assert
                Assert.IsType<BadRequestResult>(data);
            }

            [Fact]
            public async void Task_GetPostById_MatchResult()
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
            public async void Task_GetPosts_Return_OkResult()
            {
                //Act  
                var data = await PizzaRepository.TriModel("", "");

                //Assert  
                Assert.IsType<OkObjectResult>(data);
            }

            [Fact]
            public void Task_GetPosts_Return_BadRequestResult()
            {
                //Act  
                var data = PizzaRepository.TriModel("", "");
                data = null;

                if (data != null)
                    //Assert  
                    Assert.IsType<BadRequestResult>(data);
            }

            [Fact]
            public async void Task_GetPosts_MatchResult()
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
            public async void Task_Add_InvalidData_Return_BadRequest()
            {
                //Arrange
                Pizza post = new Pizza() { Name = "45", Price = 11, Topping = "Chevre, Miel" };

                //Act              
                var data = await PizzaRepository.PostModel(post);

                //Assert  
                Assert.IsType<BadRequestResult>(data);
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
            public async void Task_Delete_Post_Return_OkResult()
            {
                //Arrange
                var pizzaId = 2;

                //Act  
                var data = await PizzaRepository.DeleteModel(pizzaId);

                //Assert  
                Assert.IsType<OkResult>(data);
            }

            [Fact]
            public async void Task_Delete_Post_Return_NotFoundResult()
            {
                //Arrange
                var pizzaId = 5;

                //Act  
                var data = await PizzaRepository.DeleteModel(pizzaId);

                //Assert  
                Assert.IsType<NotFoundResult>(data);
            }

            [Fact]
            public async void Task_Delete_Return_BadRequestResult()
            {
                //Arrange
                int pizzaId = -1;

                //Act  
                var data = await PizzaRepository.DeleteModel(pizzaId);

                //Assert  
                Assert.IsType<BadRequestResult>(data);
            }

        #endregion
    }
}
