using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon Item")]
public class WeaponItem : Item
{
    public EquipmentSlot equipSlot;
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Weapon Idle Animation")]
    public string armIdle;

    [Header("Weapon Attack Animation")]
    public string attack1;
    public string attack2;
    public string attack3;
    public string attack4;


    public override void Use()
    {
        base.Use();
    }
}
public enum EquipmentSlot { weapon }