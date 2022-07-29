using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot rightHandSlot;
    private DamageCollider rightHandDamageCollider;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSLot in weaponHolderSlots)
        {
            rightHandSlot = weaponSLot;
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        rightHandSlot.LoadWeaponModel(weaponItem);
        LoadWeaponDamageCollider();

        if (weaponItem != null)
        {
            animator.CrossFade(weaponItem.armIdle, 0.2f);
        }
        else
        {
            animator.CrossFade("Arm_Empty", 0.2f);
        }
    }

    public void LoadWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }
}