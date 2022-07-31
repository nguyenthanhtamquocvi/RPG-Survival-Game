using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public interface IItemContainer
    {
        public ItemSlot AddItem(ItemSlot itemSlot);

        void RemoveItem(ItemSlot itemSlot);

        void RemoveAt(int slotIndex);

        void Swap(int indexOne, int indexTwo);

        bool HasItem(InventoryItem item);

        int GetTotalQuanlity(InventoryItem item);
    }
}