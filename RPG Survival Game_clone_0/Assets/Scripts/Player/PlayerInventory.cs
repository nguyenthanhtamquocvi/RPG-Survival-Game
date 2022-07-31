using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    private PlayerLocomotion playerLocomotion;
    private WeaponSlotManager weaponSlotManager;

    public WeaponItem rightWeapon;
    public WeaponItem unarmedWeapon;
    public WeaponItem[] weaponInRightHandSlots = new WeaponItem[1];
    public List<Item> inventory;    

    public Transform boxDropTransform;
    public Rigidbody Box;

    private void Awake()
    {
        instance = this;
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        weaponInRightHandSlots = new WeaponItem[numSlots];

        rightWeapon = unarmedWeapon;
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon);
    }

    private void Update()
    {
        if (InputHandler.instance.d_input)
            UnEquipAll();
    }

    public void Equip(WeaponItem weaponItem)
    {
        rightWeapon = weaponItem;
        int slotIndex = (int)weaponItem.equipSlot;

        WeaponItem oldItem = null;

        if (weaponInRightHandSlots[slotIndex] != null)
        {
            oldItem = weaponInRightHandSlots[slotIndex];
            inventory.Add(oldItem);
        }
        weaponInRightHandSlots[slotIndex] = weaponItem;
        weaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[slotIndex]);
    }

    public void UnEquip(int slotIndex)
    {
        if (weaponInRightHandSlots[slotIndex] != null)
        {
            WeaponItem oldItem = weaponInRightHandSlots[slotIndex];
            weaponInRightHandSlots[slotIndex] = null;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon);
            DropItem(oldItem);
            
        }
    }

    public void DropItem(WeaponItem weaponItem)
    {
            Rigidbody dropBox;
            dropBox = Instantiate(Box, boxDropTransform.position * 1, boxDropTransform.rotation) as Rigidbody;
            dropBox.AddForce(boxDropTransform.forward * 300);
            itemDrop.instance.AddItem(weaponItem);
    }

    public void UnEquipAll()
    {
        for (int i = 0; i < weaponInRightHandSlots.Length; i++)
        {
            UnEquip(i);
        }
    }
}