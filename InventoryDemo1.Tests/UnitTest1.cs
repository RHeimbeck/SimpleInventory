using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using InventoryDemo1.Controllers;
using InventoryDemo1.Models;

namespace InventoryDemo1.Tests
{
    [TestClass]
    public class UnitTest_Get    {
        [TestMethod]
        public void GetItem_Positive()
        {
            // Arrange
            var controller = new InventoryController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            InventoryItem item = controller.GetInventoryItem("nuts");

            // Assert
            Assert.AreEqual("nuts", item.label);
            Assert.AreEqual(new DateTime(2016, 9, 30), item.expiration);
            Assert.AreEqual("Planters", item.type);
        }

        [TestMethod]
        public void GetItem_NotFound()
        {
            // Arrange
            var controller = new InventoryController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            bool exceptionOccurred = false;
            // Act
            try
            {
                // ask for an item that is not in the repository
                InventoryItem item = controller.GetInventoryItem("ginger");
            }
            catch(HttpResponseException e)
            {
                exceptionOccurred = true;
            }

            // Assert
            Assert.AreEqual(true, exceptionOccurred);
        }

        [TestMethod]
        public void GetAll()
        {
            // Arrange
            var controller = new InventoryController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            bool exceptionOccurred = false;

            // Act
            IEnumerable<InventoryItem> results = controller.Get();

            // Assert
            Assert.AreEqual(true, exceptionOccurred);
        }
    }
}
