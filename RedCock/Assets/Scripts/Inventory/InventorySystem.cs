using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedCock.Inventory
{
    public class InventorySystem
    {
        private List<InventoryItem> inventoryItems;

        public void Initialize()
        {
            inventoryItems = new();
        }
    }
}