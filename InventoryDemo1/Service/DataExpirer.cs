using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryDemo1.Models;
using System.Threading;

namespace InventoryDemo1.Service
{
    public class DataExpirer
    {
        private const int THREAD_SLEEP = 1000; 
        private DictionaryInventoryRepository repository;
        public void CheckForExpiredData()
        {
            this.repository = DictionaryInventoryRepository.Instance;
            while(true)
            {
                InventoryItem[] expiredInventoryItems = this.repository.Get().Where(e => e.expiration < DateTime.Now).ToArray();
                // ****
                // Insert Repository Lock here
                // ****
                for(var i = 0; i < expiredInventoryItems.Length; i++)
                {
                    var label = expiredInventoryItems[i].label;
                    
                    System.Diagnostics.Debug.WriteLine(String.Format("Expired Item: {0} automatically removed from inventory", label));
                    this.repository.Delete(label);
                }
                // ****
                // Insert Repository UnLock here
                // ****
                Thread.Sleep(1000);   
            }
        }
    }
}