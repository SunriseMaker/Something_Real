using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Variables
    protected ProjectileAttackData attack_data;

    protected Rigidbody2D _rigidbody;

    protected Collider2D _collider;

    protected Vector2 launch_direction;

    protected float speed_effect;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        if (attack_data.life_time > 0)
        {
            Destroy(gameObject, attack_data.life_time);
        }
    }

    private void FixedUpdate()
    {
        if(speed_effect > 0 && _rigidbody.isKinematic)
        {
            transform.Translate(launch_direction * speed_effect * Time.deltaTime, Space.World);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        __Attacks.PerformAttack(attack_data, Vector2.zero, _collider.bounds.center, GetInstanceID(), "");

        switch (attack_data.behaviour)
        {
            case ProjectileAttackData.ProjectileImpactBehaviour.penetrate:
                break;

            case ProjectileAttackData.ProjectileImpactBehaviour.destroy:
                Destroy(gameObject);
                break;

            case ProjectileAttackData.ProjectileImpactBehaviour.stick:
                Stick(col);
                break;
        }
    }
    #endregion MonoBehaviour

    #region Red
    public virtual void Launch(Vector2 v_direction, ProjectileAttackData v_attack_data)
    {
        if (v_attack_data.override_y != 0)
        {
            v_direction = new Vector2(v_direction.x, v_attack_data.override_y);
        }

        launch_direction = v_direction;

        attack_data = v_attack_data;

        speed_effect = attack_data.speed_effect;

        _rigidbody.isKinematic = attack_data.is_kinematic;
        
        if(!_rigidbody.isKinematic)
        {
            _rigidbody.AddForce(launch_direction * speed_effect, ForceMode2D.Impulse);
        }
    }

    protected virtual void Stick(Collider2D col)
    {
        speed_effect = 0.0f;

        _rigidbody.isKinematic = true;
        
        _collider.enabled = false;

        transform.SetParent(col.transform, true);
    }

    protected virtual Vector2 CurrentDirection()
    {
        return _rigidbody.isKinematic ? launch_direction : new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y).normalized;
    }
    #endregion Red
}
