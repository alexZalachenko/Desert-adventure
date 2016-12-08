using UnityEngine;

public class WeaponManager : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer c_playerSpriteRenderer;
    [SerializeField]
    private LayerMask c_attackMask;

    private Vector3 c_raycastOffset;

    Weapon c_rangedWeapon;
    Weapon c_meleeWeapon;

    void Start()
    {
        c_rangedWeapon = new Gun(gameObject.GetComponent<Animator>());
        c_meleeWeapon = new Knife(gameObject.GetComponent<Animator>());
        c_raycastOffset = new Vector3(0, 0.25f, 0);
    }

    public void Attack()
    {
        Vector2 t_raycastDirection;
        if (c_playerSpriteRenderer.flipX)//if true player is looking to the left
            t_raycastDirection = Vector2.left;
        else
            t_raycastDirection = Vector2.right;

        //we will raycast to decide if shoot or better melee
        RaycastHit2D t_raycastHit = Physics2D.Raycast(transform.position + c_raycastOffset, t_raycastDirection, c_rangedWeapon.Range, c_attackMask);
        if(t_raycastHit.collider == null)
            c_rangedWeapon.Attack(t_raycastDirection, t_raycastHit.collider);
        else
        {
            if (t_raycastHit.distance < c_meleeWeapon.Range)
                c_meleeWeapon.Attack(t_raycastDirection, t_raycastHit.collider);
            else
                c_rangedWeapon.Attack(t_raycastDirection, t_raycastHit.collider);
        }
    }

    private void EquipWeapon(Weapon p_weaponToEquip)
    {
        if (p_weaponToEquip.Type == Weapon.WeaponType.Melee)
            c_meleeWeapon = p_weaponToEquip;
        else
            c_rangedWeapon = p_weaponToEquip;
    }
}
