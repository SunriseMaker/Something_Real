using UnityEngine;

public sealed class Weapon : MonoBehaviour
{
    #region Variables
    public bool projectile_weapon;

    public float pause_between_attacks;

    public Sprite HUD_sprite;

    public float recoil;

    private Animator _animator;

    private AttackData attack_data;

    private Vector2 aim;

    private bool can_attack;

    private int owner_instance_id;

    private Rigidbody2D owner_rigidbody;

    private static int AP_Attack;

    static Weapon()
    {
        AP_Attack = Animator.StringToHash("Attack");
    }
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        attack_data = projectile_weapon ? GetComponent<ProjectileAttackData>() : GetComponent<AttackData>();
        can_attack = true;
    }
    #endregion MonoBehaviour

    #region Red
    public void Attack(Vector2 v_aim)
    {
        if(can_attack)
        {
            aim = v_aim;
            AttackAnimation();
            StartCoroutine(TriggerCanAttack());
        }
    }

    public void SetOwnerData(Rigidbody2D v_rigidbody, int v_instance_id, string v_tag)
    {
        owner_rigidbody = v_rigidbody;
        owner_instance_id = v_instance_id;
        attack_data.ignore_tag = v_tag;
    }

    private void AttackAnimation()
    {
        _animator.SetTrigger(AP_Attack);
    }

    private void Recoil()
    {
        if (recoil != 0 && owner_rigidbody != null && !owner_rigidbody.isKinematic)
        {
            owner_rigidbody.AddForce(aim * -1 * recoil, ForceMode2D.Impulse);
        }
    }

    private void AttackSound()
    {
        if (attack_data.sound != null)
        {
            AudioSource.PlayClipAtPoint(attack_data.sound, transform.position, 0.5f);
        }
    }

    public void Fire()
    {
        if(projectile_weapon)
        {
            Vector3 position = transform.position + (Vector3)aim;
            __Attacks.SpawnProjectile((ProjectileAttackData)attack_data, aim, position);
        }
        else
        {
            __Attacks.PerformAttack(attack_data, aim, transform.position, owner_instance_id);
        }

        AttackSound();
        Recoil();
    }

    private System.Collections.IEnumerator TriggerCanAttack()
    {
        can_attack = false;

        yield return new WaitForSeconds(pause_between_attacks);

        can_attack = true;
    }
    #endregion Red
}
