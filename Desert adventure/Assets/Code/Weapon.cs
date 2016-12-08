using UnityEngine;

public abstract class Weapon {
   
    public enum WeaponType
    {
        Ranged,
        Melee
    };

    protected Animator c_playerAnimator;
    protected int c_damage;
    protected float c_range;
    protected WeaponType c_type;
    public WeaponType Type
    {
        get
        {
            return c_type;
        }
    }
    public float Range
    {
        get
        {
            return c_range;
        }
    }
    public int Damage
    {
        get
        {
            return c_damage;
        }
    }

    public Weapon(Animator p_playerAnimator, WeaponType p_type, int p_damage, float p_range)
    {
        c_playerAnimator = p_playerAnimator;
        c_damage = p_damage;
        c_range = p_range;
        c_type = p_type;
    }

    public virtual void Attack(Vector2 p_direction, Collider2D p_target)
    {
        if (p_target == null)
            return;

        if (p_target.gameObject.tag == "Skeleton")
            p_target.gameObject.GetComponent<SkeletonHealthManager>().ReceiveDamage(c_damage);
    }
}
