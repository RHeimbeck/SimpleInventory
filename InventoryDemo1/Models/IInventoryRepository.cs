using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDemo1.Models
{
    public interface IInventoryRepository
    {
        IEnumerable<InventoryItem> Get();
        bool TryGet(string label, out InventoryItem item);
        InventoryItem Add(InventoryItem item);
        InventoryItem Update(InventoryItem item);
        bool Delete(string label);
    }
}
