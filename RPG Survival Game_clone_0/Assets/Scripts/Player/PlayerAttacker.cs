using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    public string lastAttack;

    public void HandleWeaponCombo(WeaponItem weaponItem)
    {
        AnimationHandler.instance.anim.SetBool("canDoCombo", false);
        if (lastAttack == weaponItem.attack1)
        {
            AnimationHandler.instance.PlayTargetAnimation(weaponItem.attack2, true);
            lastAttack = weaponItem.attack2;
        }

    }

    public void HandleAttack(WeaponItem weaponItem)
    {
        AnimationHandler.instance.PlayTargetAnimation(weaponItem.attack1, true);
        lastAttack = weaponItem.attack1;

    }
}
