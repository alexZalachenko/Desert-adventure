using UnityEngine;

public class Knife : Weapon {

    public Knife(Animator p_playerAnimator) : base(p_playerAnimator, WeaponType.Melee, 1, 1)
    {
    }

    public override void Attack(Vector2 p_direction, Collider2D p_target)
    {
        base.Attack(p_direction, p_target);
        c_playerAnimator.SetTrigger("Melee");
        SoundManager.Instance.PlaySound(SoundManager.EffectsCips.Knifing);
    }
}
