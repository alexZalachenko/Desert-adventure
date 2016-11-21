using UnityEngine;
using System.Collections;

public class Knife : Weapon {

    public Knife(Animator p_playerAnimator): base(p_playerAnimator)
    {
    }

    public override void Attack(Vector2 p_direction, Collider2D p_target)
    {
        c_playerAnimator.SetTrigger("Melee");
    }
}
