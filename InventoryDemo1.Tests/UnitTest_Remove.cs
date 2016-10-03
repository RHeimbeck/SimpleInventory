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
    public class UnitTest_Remove
    {
        [TestMethod]
        public void RemoveItem_Positive()
        {
            // Arrange
            var controller = new InventoryController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            DictionaryInventoryRepository repository = DictionaryInventoryRepository.Instance;

            repository.Add(new InventoryItem { label = "soup", expiration = new DateTime(2016, 10, 1), type = "Campbells" });

            // Act
            InventoryItem[] results = new List<InventoryItem>(controller.Get()).ToArray();

            // Assert
            Assert.IsTrue(results.Length >= 1, "Should return a list of items");

            // Remove All items - maybe add a method to the data model or controller to do this.
            for (int i = 0; i < results.Length; i++)
            {
                controller.DeleteInventoryItem(results[i].label);
            }

            List<InventoryItem> newResults = new List<InventoryItem>(controller.Get());

            // Assert
            Assert.AreEqual(0, newResults.Count, "Should Now return a zero-length list");
        }
    }
}
