using UnityEngine;
using System.Collections;

public class Gun : Weapon{
    
    public Gun(Animator p_playerAnimator): base(p_playerAnimator, WeaponType.Ranged, 1, 6)
    {
    }
    public override void Attack(Vector2 p_direction, Collider2D p_target)
    {
        base.Attack(p_direction, p_target);
        c_playerAnimator.SetTrigger("Shoot");
        SoundManager.Instance.PlaySound(SoundManager.EffectsCips.Shooting);
    }
}
