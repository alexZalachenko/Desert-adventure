using UnityEngine;

public class WeaponManager : MonoBehaviour {

    [SerializeField]
    private BoxCollider2D c_playerBoxCollider;
    [SerializeField]
    private SpriteRenderer c_playerSpriteRenderer;
    [SerializeField]
    public float c_meleeRange;

    Weapon c_rangedWeapon;
    Weapon c_meleeWeapon;

    void Start()
    {
        c_rangedWeapon = new Gun(gameObject.GetComponent<Animator>());
        c_meleeWeapon = new Knife(gameObject.GetComponent<Animator>());
    }

    public void Attack()
    {
        Vector2 t_raycastDirection;
        if (c_playerSpriteRenderer.flipX)//if true player is looking to the left
            t_raycastDirection = Vector2.left;
        else
            t_raycastDirection = Vector2.right;

        //disable the player's collider so the raycast doesn't hit it
        c_playerBoxCollider.enabled = false;
        //we will raycast to decide if shoot or better melee
        RaycastHit2D t_raycastHit = Physics2D.Raycast(transform.position, t_raycastDirection, c_meleeRange);
        c_playerBoxCollider.enabled = true;
        if(t_raycastHit.collider == null)
            c_rangedWeapon.Attack(Vector2.right, t_raycastHit.collider);
        else
            c_meleeWeapon.Attack(Vector2.left, t_raycastHit.collider);
    }

    private void EquipWeapon(Weapon p_weaponToEquip)
    {
        if (p_weaponToEquip.Type == Weapon.WeaponType.Melee)
            c_meleeWeapon = p_weaponToEquip;
        else
            c_rangedWeapon = p_weaponToEquip;
    }
}
