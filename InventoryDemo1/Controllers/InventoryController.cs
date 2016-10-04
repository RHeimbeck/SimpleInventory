using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InventoryDemo1.Models;

namespace InventoryDemo1.Controllers
{
    public class InventoryController : ApiController
    {
        private DictionaryInventoryRepository repository;
        public InventoryController()
        {
            // Initialize the data repository with a singleton instance so it stays static and 
            // other sources can access it.
            this.repository = DictionaryInventoryRepository.Instance;
        }

        #region Get
        // GET api/inventory
        // Get all items.
        public IEnumerable<InventoryItem> Get()
        {
            return repository.Get();
        }

        // GET api/inventory/{label}
        // Get a particular item specified by label
        public InventoryItem GetInventoryItem(string label)
        {
            return getInventoryItem(label);
        }
        #endregion

        #region Add item
        // POST api/inventory
        // Use http POST verb to add a new inventory item.
        public HttpResponseMessage PostInventoryItem(InventoryItem item)
        {
            // Validate the label and expiration values
            // We could also validate the Type field but there are no current specifications for it.
            if (String.IsNullOrEmpty(item.label) ||
                item.expiration < DateTime.Now)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            item = repository.Add(item);
            var response = Request.CreateResponse<InventoryItem>(HttpStatusCode.Created, item);
            response.Headers.Location = Request.RequestUri; // new Uri(Request.RequestUri, item.label);
            // ** Send Notification message to user
            System.Diagnostics.Debug.WriteLine(String.Format("Added new inventory item: {0}", item.label));
            return response;
        }
        #endregion

        #region Remove item
        // DELETE api/inventory/xxx
        // Use http DELETE verb to remove a specific item from the inventory, by label.
        // Returns the item details
        public InventoryItem DeleteInventoryItem(string label)
        {
            InventoryItem item = getInventoryItem(label);
            repository.Delete(label);
            // ** Send Notification message to user
            System.Diagnostics.Debug.WriteLine(String.Format("Removed inventory item: {0}", item.label));
            return item;
        }
        #endregion

        #region Private Methods
        // Common worker method for getting an InventoryItem if it is available.
        // Throws HttpResponseException if item cannot be found in the repository.
        // !Note! - be sure VS configuration is set to allow this type of exception during debugging, 
        // otherwise it looks like an unhandled exception though it is not if this class inherits from ApiController
        private InventoryItem getInventoryItem(string label)
        {
            InventoryItem item;
            if (!repository.TryGet(label, out item))
            {
                // return NoContent - the server processed the request but the resource was not found.
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }
            return item;
        }
        #endregion
    }
}
