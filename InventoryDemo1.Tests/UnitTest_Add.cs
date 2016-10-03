using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using InventoryDemo1.Controllers;
using InventoryDemo1.Models;

namespace InventoryDemo1.Tests
{
    [TestClass]
    public class UnitTest_Add
    {
        [TestMethod]
        public void AddItem_Positive()
        {
            // Arrange
            var controller = new InventoryController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            // Check to see if a specific item already exits - though it should not.
            bool exceptionOccurred = false;
            HttpResponseException excpt = new HttpResponseException(System.Net.HttpStatusCode.OK);
            InventoryItem item;
            // Act
            try
            {
                // ask for an item that is not in the repository
                item = controller.GetInventoryItem("bananas");
            }
            catch (HttpResponseException e)
            {
                exceptionOccurred = true;
                excpt = e;
            }

            // Assert
            Assert.AreEqual(true, exceptionOccurred, "Returned an exception");
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, excpt.Response.StatusCode, "Status code: NotFound");

            // Now add the item and check again.  This time it should be there
            string requestUri = "http://localhost:1234/api/inventory/bananas";
            controller.Request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            InventoryItem bananas = new InventoryItem { label = "bananas", expiration = new DateTime(2020, 10, 1), type = "Chaquita" };
            HttpResponseMessage resp = controller.PostInventoryItem(bananas);
            item = controller.GetInventoryItem("bananas");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Created, resp.StatusCode, "Status code: Created");
            Assert.AreEqual(requestUri.ToString(), resp.Headers.Location.ToString(), "Correct api assest location");
            Assert.AreEqual("bananas", item.label, "Found the newly created item");
        }
    }
}
