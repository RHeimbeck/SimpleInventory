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
            HttpResponseException excpt = new HttpResponseException(System.Net.HttpStatusCode.OK);
            // Act
            try
            {
                // ask for an item that is not in the repository
                InventoryItem item = controller.GetInventoryItem("ginger");
            }
            catch(HttpResponseException e)
            {
                exceptionOccurred = true;
                excpt = e;
            }

            // Assert
            Assert.AreEqual(true, exceptionOccurred, "Returned an exception");
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, excpt.Response.StatusCode, "Status code: NotFound");
        }

        [TestMethod]
        public void GetAll()
        {
            // Arrange
            var controller = new InventoryController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            List<InventoryItem> results = new List<InventoryItem>(controller.Get());

            // Assert
            Assert.IsTrue(results.Count >= 3, "Should return a list of items");
        }

        [TestMethod]
        public void GetAll_Empty()
        {
            // Arrange
            var controller = new InventoryController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            InventoryItem[] results = new List<InventoryItem>(controller.Get()).ToArray();
            // Remove All items - maybe add a method to the data model or controller to do this.
            for(int i = 0; i < results.Length; i++)
            {
                controller.DeleteInventoryItem(results[i].label);
            }

            List<InventoryItem> newResults = new List<InventoryItem>(controller.Get());

            // Assert
            Assert.AreEqual(0, newResults.Count, "Should return a zero-length list");
        }
    }
}
