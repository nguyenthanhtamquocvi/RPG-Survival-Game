using UnityEngine;

public class ItemPickup : Interactable
{

    [SerializeField]
    public WeaponItem weaponItem;

    public override void Interact(PlayerManger playerManger)
    {
        base.Interact(playerManger);
        PickUpItem(playerManger);
    }


    private void PickUpItem(PlayerManger playerManger)
    {
        PlayerLocomotion playerLocomotion;
        AnimationHandler animationHandler;
        playerLocomotion = playerManger.GetComponent<PlayerLocomotion>();
        animationHandler = playerManger.GetComponentInChildren<AnimationHandler>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animationHandler.PlayTargetAnimation("Pickup", true);
        PlayerInventory.instance.Equip(weaponItem);
        Destroy(gameObject);
    }
}