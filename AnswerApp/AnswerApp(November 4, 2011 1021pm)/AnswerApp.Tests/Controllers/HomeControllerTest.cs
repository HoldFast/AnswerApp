using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnswerApp;
using AnswerApp.Controllers;
using AnswerApp.Models;

namespace AnswerApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            HomeModel model = new HomeModel();
            ViewResult result = controller.Index(model) as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

        [TestMethod]
        public void Pay()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            //ViewResult result = controller.Pay(new SelectModel()) as ViewResult;

            // Assert
            //Assert.IsNotNull(result);
        }
    }
}
