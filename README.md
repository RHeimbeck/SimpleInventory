# SimpleInventory
ASP.Net simple inventory system with REST API

To Run this App:
1. Download to local repository
2. Open with Visual Studio (Used VS 2015 to create it)
3. Run with Chrome (or other) browser to see simple web test page, or
4. Run unit tests.

Assumptions for this Simple Inventory ASP.Net application
1. Inventory items are only specified by their "label" attribute.
2. There is no implementation of inventory count for a particular item.  Adding a new item with the same label as an existing item will over-write the exisiting item.  Removing a particular item removes the one and only item with this label.
3. The system does not implement Authorization or Authentication at this time.  Anyone can access, add, or remove any item.
4. The simple data repository does not support a locking mechanism at this time so there could be potential data clashes between subscribers to this singleton data store.  I.E. the mechanism for removing expired items might remove an item at the same time that an external client is requesting it.  The Data Locking mechanism would have to be implemented for this to be a production worthy system.
5. The mechanism for automatically removing expired items runs as a separate, stand-alone thread.  This thread wakes-up every second to check the inventory for expired items.  If an item has expired, it is removed from the data store, and a debug console message is displayed for notification.  This 'expiring' mechanism does not take into account how many items are in the inventory so it could potentialy take a significant amount of time and resources to run if the data store contained a large amount of items.  Ideally this mechanism would have built-in throttling so that it would not tie-up computer resources and the data repository.
6. The REST API for the inventory system uses the basic HTTP verbs to provide CRUD data access (minus the 'U' at this time.).

# API
GET api/inventory  - returns all items in the inventory
GET api/inventory/{label}  - get a specific inventory item by label if it exists
POST api/inventory/{item} - add an item to the inventory by {item}.  Item should contain these field : (string)label, (DateTime)expiration, (string)type.
DELETE api/inventory/{label} - remove an item from inventory if it exists.
