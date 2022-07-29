using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDrop : Interactable
{
    #region Singleton
    public static itemDrop instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public WeaponItem item;
 
    public override void Interact(PlayerManger playerManger)
    {
        base.Interact(playerManger);
        PickupDropItem(playerManger);
    }


    public void AddItem(WeaponItem weaponItem)
    {
        item = weaponItem;

    }
    public void PickupDropItem(PlayerManger playerManger)
    {
        PlayerLocomotion playerLocomotion;
        AnimationHandler animationHandler;
        playerLocomotion = playerManger.GetComponent<PlayerLocomotion>();
        animationHandler = playerManger.GetComponentInChildren<AnimationHandler>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animationHandler.PlayTargetAnimation("Pickup", true);
        PlayerInventory.instance.Equip(item);
        Destroy(gameObject);
    }
}
