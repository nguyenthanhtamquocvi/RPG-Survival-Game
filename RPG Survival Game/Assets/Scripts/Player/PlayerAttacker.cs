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
        else if (lastAttack == weaponItem.attack2)
        {
            AnimationHandler.instance.PlayTargetAnimation(weaponItem.attack3, true);
            lastAttack = weaponItem.attack3;
        }
        else if (lastAttack == weaponItem.attack3)
        {
            AnimationHandler.instance.PlayTargetAnimation(weaponItem.attack4, true);
            lastAttack = weaponItem.attack4;
        }

    }

    public void HandleAttack(WeaponItem weaponItem)
    {
        AnimationHandler.instance.PlayTargetAnimation(weaponItem.attack1, true);
        lastAttack = weaponItem.attack1;

    }
}
