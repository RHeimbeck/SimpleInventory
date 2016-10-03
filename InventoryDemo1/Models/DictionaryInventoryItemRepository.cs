using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryDemo1.Models
{
    public sealed class DictionaryInventoryRepository : IInventoryRepository
    {
        // Create this "in-memory" simple data repository as a Singleton pattern.
        // This allows multiple 'subscribers' to each access and share the same data set
        // !Note! At this time there is NOT a provision to Lock the data to prevent various clashes such
        // as one subscriber deleting a record as another one is reading it.
        // Adding a Locking mechanism is a "soon to be added" feature.
        private Dictionary<string, InventoryItem> inventoryItems = new Dictionary<string, InventoryItem>();
        private static volatile DictionaryInventoryRepository instance;
        private static object syncRoot = new Object();
        private static object dataAccess = new Object();

        private DictionaryInventoryRepository() {
            // Seed the Inventory to have some initial data
            inventoryItems.Add("soup", new InventoryItem{ label = "soup", expiration = new DateTime(2016, 10, 1), type = "Campbells" } );
            inventoryItems.Add("nuts", new InventoryItem { label = "nuts", expiration = new DateTime(2016, 9, 30), type = "Planters" });
            inventoryItems.Add("kitchensink", new InventoryItem { label = "kitchensink", expiration = new DateTime(2016, 9, 29), type = "Elgin" });
        }

        public static DictionaryInventoryRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    // prevent multiple threads from being the one to create this singelton.
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DictionaryInventoryRepository();
                    }
                }

                return instance;
            }
        }

        public IEnumerable<InventoryItem> Get()
        {
            return inventoryItems.Values.OrderBy(inventoryItem => inventoryItem.label);
        }

        public bool TryGet(string label, out InventoryItem item)
        {
            return inventoryItems.TryGetValue(label, out item);
        }

        public InventoryItem Add(InventoryItem item)
        {
            lock (dataAccess) // only allow one thread at a time to add, modify, or remove data
            {
                inventoryItems[item.label] = item;
            }
            return item;
        }

        public InventoryItem Update(InventoryItem item)
        {
            lock (dataAccess) // only allow one thread at a time to add, modify, or remove data
            {
                inventoryItems[item.label] = item;
            }
            return item;
        }

        public bool Delete(string label)
        {
            bool status;
            lock (dataAccess) // only allow one thread at a time to add, modify, or remove data
            {
                status = inventoryItems.Remove(label);
            }
            return status;
        }
    }
}