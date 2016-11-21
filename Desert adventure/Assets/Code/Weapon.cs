using UnityEngine;
using System.Collections;

public abstract class Weapon {
   
    public enum WeaponType
    {
        Ranged,
        Melee
    };

    protected Animator c_playerAnimator;
    public float c_damage;
    public WeaponType c_type;
    public WeaponType Type
    {
        set
        {
            c_type = value;
        }
        get
        {
            return c_type;
        }
    }

    public Weapon(Animator p_playerAnimator)
    {
        c_playerAnimator = p_playerAnimator;
    }

    public abstract void Attack(Vector2 p_direction, Collider2D p_target);
}
