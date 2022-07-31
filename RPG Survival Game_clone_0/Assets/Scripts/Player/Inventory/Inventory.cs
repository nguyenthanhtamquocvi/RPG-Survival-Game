using Events;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Items/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private VoidEvent onInventoryItemUpdated = null;

        [SerializeField] private ItemSlot testItemSlot = new ItemSlot();

        public ItemContainer ItemContainer { get; } = new ItemContainer(20);

        public void OnEnable() => ItemContainer.OnIteamUpdated += onInventoryItemUpdated.Raise;

        public void OnDisable() => ItemContainer.OnIteamUpdated -= onInventoryItemUpdated.Raise;

        [ContextMenu("Test Add")]
        public void TestAdd()
        {
            ItemContainer.AddItem(testItemSlot);
        }
    }
}