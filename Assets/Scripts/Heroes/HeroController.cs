using UnityEngine;
using System.Collections.Generic;
using System;

public class HeroController : MonoBehaviour, iLiving, iMana
{
    #region Variables
    const float DRAW_TIME = 2.0f;
    const float MIN_DISTANCE = 0.05f;

    #region Other
    public string _name;

    [Tooltip("True: Leads the allies.\nFalse: Searching for leader ally and then follows.")]
    public bool is_leader;

    [Tooltip("Distance at which hero will mark target.")]
    [Range(0.0f, 20.0f)]
    public float sight;

    [HideInInspector]
    public string enemy_tag;

    protected int instance_id;

    private Color native_sprite_color;

    private PositionOffsets position_offsets;

    private PositionOffsets nearest_position_offsets;

    private ObjectSize size;
    #endregion Other

    #region Weapons
    [Header("Weapons")]
    [Range(0.01f, 1.0f)]
    public float accuracy;

    public Vector2 weapon_offset;

    [Tooltip("List of weapon prefabs.")]
    public List<GameObject> weapons_on_spawn;

    [HideInInspector]
    public List<Weapon> available_weapons;

    [HideInInspector]
    public Weapon current_weapon;

    private Transform _weapons_group;

    const string WEAPONS_GROUP = "Weapons";
    #endregion Weapons

    #region Skills
    [Header("Skills")]
    [Tooltip("List of skill prefabs.")]
    public List<GameObject> skills_on_spawn;

    [HideInInspector]
    public List<HeroSkill> available_skills;
    
    protected HeroSkill last_used_skill;

    private Transform _skills_group;

    const string SKILLS_GROUP = "Skills";
    #endregion Skills

    #region Health
    [Header("Health")]
    [Range(0.0f, 1000.0f)]
    public float initial_hp;

    [Range(0.0f, 1000.0f)]
    public float initial_max_hp;

    [Range(0.0f, 3.0f)]
    public float immune_on_hit_duration;

    [Range(0.0f, 3.0f)]
    public float immune_on_resurrect_duration;

    protected Health _health;
    #endregion Health

    #region Mana
    [Header("Mana")]
    [Range(0.0f, 1000.0f)]
    public float initial_mp;

    [Range(0.0f, 1000.0f)]
    public float initial_max_mp;

    protected Mana _mana;
    #endregion Mana

    #region Statuses
    protected bool invisible;

    protected bool layer_shift;

    [HideInInspector]
    public bool can_move;

    [HideInInspector]
    public bool can_attack;
    #endregion Statuses

    #region Jumps
    [Header("Jumps")]
    public int jump_count;

    public float jump_force;

    // Jump counter
    private int jump_number;
    #endregion Jumps

    #region Movement
    [Header("Movement")]
    [Range(0, 10)]
    public float movement_speed;

    [Tooltip("Movement speed modifier applied when hero is sprinting.")]
    [Range(1, 5)]
    public float sprint_coefficient;

    private Vector2 forward_direction;

    private Vector2 velocity;

    private Vector2 current_position;

    private Vector2 previous_position;

    public bool able_to_levitate;
    #endregion Movement

    #region ComponentReferences
    protected SpriteRenderer _renderer;

    [HideInInspector]
    public Rigidbody2D _rigidbody;

    [HideInInspector]
    public Animator _animator;

    protected DestroyAfterDelay _destroy_after_delay;
    #endregion ComponentReferences

    #region AnimatorParameters
    protected int AP_velocity_x;
    protected int AP_velocity_y;
    protected int AP_movement_x;
    protected int AP_movement_y;
    protected int AP_sprint;
    protected int AP_jump;
    protected int AP_dead;
    protected int AP_hitted;

    private Vector2 APV_velocity_current;
    private Vector2 APV_velocity_previous;
    private Vector2 APV_movement_current;
    private Vector2 APV_movement_previous;
    #endregion AnimatorParameters

    #region Input
    private float input_axis_x;
    private float input_axis_y;
    private bool input_sprint;
    #endregion Input
    #endregion Variables

    #region MonoBehaviour
    protected virtual void Awake()
    {
        #region Assertions
        string assertion_prefix = "ASSERTION FAILED: " + name;
        Debug.Assert(movement_speed > 0, assertion_prefix + "movement_speed must be positive.");
        Debug.Assert(sprint_coefficient > 0, assertion_prefix + "sprint_coefficient must be positive.");
        Debug.Assert(initial_mp <= initial_max_mp, assertion_prefix + "initial_mp is greater than initial_max_mp.");
        Debug.Assert(initial_hp <= initial_max_hp, assertion_prefix + "initial_hp is greater than initial_max_hp.");
        Debug.Assert(initial_hp > 0, assertion_prefix + "initial_hp is not positive.");
        Debug.Assert(jump_count == 0 || jump_force > 0, assertion_prefix+"jump_force is zero");
        #endregion Assertions

        #region ComponentReferences
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _destroy_after_delay = GetComponent<DestroyAfterDelay>();
        #endregion ComponentReferences

        #region AnimatorParameters
        // These parameters must exist in every hero's animator
        AP_velocity_x = Animator.StringToHash("Velocity_x");
        AP_velocity_y = Animator.StringToHash("Velocity_y");
        AP_movement_x = Animator.StringToHash("Movement_x");
        AP_movement_y = Animator.StringToHash("Movement_y");
        AP_sprint = Animator.StringToHash("Sprint");
        AP_jump = Animator.StringToHash("Jump");
        AP_dead = Animator.StringToHash("Dead");
        AP_hitted = Animator.StringToHash("Hitted");
        #endregion AnimatorParameters

        #region Statuses
        can_move = true;
        can_attack = true;
        forward_direction = Vector2.right;
        #endregion Statuses

        #region Other
        enemy_tag = tag == "Red" ? "Blue" : "Red";

        _health = new Health(initial_max_hp, initial_hp);
        _mana = new Mana(initial_max_mp, initial_mp);

        instance_id = gameObject.GetInstanceID();
        native_sprite_color = _renderer.color;

        CalculateOffsets();

        InitializeSkills();
        Debug.Assert(available_skills.Count > 0, "Hero " + name + " has no skills.");

        InitializeWeapons();
        #endregion Other
    }

    private void Update()
    {
        CalculateVelocity();
    }

    private void FixedUpdate()
    {
        Movement();

        JumpOver();
    }

    private void Movement()
    {
        const float AMORTIZATION = 10.0f;

        if (can_move)
        {
            float speed;
            float delta_time = Time.deltaTime;

            #region axis_x
            if (input_axis_x != 0)
            {
                speed = input_axis_x * movement_speed;

                if (input_sprint)
                {
                    speed *= sprint_coefficient;
                }

                TurnToMovingDirection(input_axis_x);

                // Amortizing the forces affecting X axis

                float axis_x_sign = Math.Sign(input_axis_x);
                float rigidbody_velocity_x_sign = Math.Sign(_rigidbody.velocity.x);

                if (rigidbody_velocity_x_sign != 0 && rigidbody_velocity_x_sign != axis_x_sign)
                {
                    float amortization = input_axis_x * AMORTIZATION * delta_time;

                    float new_rigidbody_velocity_x = _rigidbody.velocity.x + amortization;

                    if (Math.Sign(new_rigidbody_velocity_x) != rigidbody_velocity_x_sign)
                    {
                        new_rigidbody_velocity_x = 0;
                    }

                    _rigidbody.velocity = new Vector2(new_rigidbody_velocity_x, _rigidbody.velocity.y);
                }

                // Checking for walls

                //Debug.DrawRay(NearestPosition_Forward(), ForwardDirection() * MIN_DISTANCE, Color.red, DRAW_TIME);
                if (Physics2D.Raycast(NearestPosition_Forward(), ForwardDirection(), MIN_DISTANCE, GameData.LayerMasks.Default).collider == null)
                {
                    transform.Translate(speed * delta_time, 0, 0, Space.World);
                }
            }
            #endregion axis_x

            #region axis_y
            if (input_axis_y != 0 && able_to_levitate)
            {
                speed = input_axis_y * movement_speed;

                if (input_sprint)
                {
                    speed *= sprint_coefficient;
                }

                // Ignoring gravitation
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

                transform.Translate(Vector2.up * speed * delta_time, Space.World);
            }
            #endregion axis_y
        }

        APV_movement_current = new Vector2(Math.Abs(input_axis_x), Math.Abs(input_axis_y));

        #region AnimatorParametersUpdate
        if (APV_movement_current != APV_movement_previous)
        {
            _animator.SetFloat(AP_movement_x, APV_movement_current.x);
            _animator.SetFloat(AP_movement_y, APV_movement_current.y);

            APV_movement_previous = APV_movement_current;
            APV_movement_current = Vector2.zero;
        }
        #endregion AnimatorParametersUpdate

        ResetInput();
    }

    private void ResetInput()
    {
        input_axis_x = 0.0f;
        input_axis_y = 0.0f;
        input_sprint = false;
    }

    public void Move(float v_axis_x, float v_axis_y, bool v_sprint)
    {
        input_axis_x = v_axis_x;
        input_axis_y = v_axis_y;
        input_sprint = v_sprint;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Landing nullifies the counter of jumps
        
        if(Grounded(GameData.LayerMasks.Obstacles))
        {
            jump_number = 0;
        }
    }
    #endregion MonoBehaviour

    #region Red
    private void InitializeSkills()
    {
        _skills_group = new GameObject(SKILLS_GROUP).transform;
        _skills_group.SetParent(transform, true);

        available_skills = new List<HeroSkill>();

        foreach (GameObject go in skills_on_spawn)
        {
            GameObject instance_gameobject = __General.InstantiatePrefab(go, go.name, Vector3.zero, Quaternion.Euler(Vector3.zero), _skills_group, false);
            HeroSkill instance_skill = instance_gameobject.GetComponent<HeroSkill>();
            instance_skill.SetHeroReference(this);
            available_skills.Add(instance_skill);
        }
    }

    protected System.Collections.IEnumerator LayerShift(int from_layer, int to_layer, float seconds)
    {
        layer_shift = true;
        gameObject.layer = to_layer;

        yield return new WaitForSeconds(seconds);

        gameObject.layer = from_layer;
        layer_shift = false;
    }

    protected virtual void OnDeath()
    {
        gameObject.layer = GameData.Layers.Limbo;

        jump_number = 0;

        Animation_Death();

        if (_destroy_after_delay != null)
        {
            _destroy_after_delay.enabled = true;
        }
    }

    public Vector2 Aim()
    {
        const float MAX_ACCURACY_OFFSET_Y = 0.25f;

        Vector2 aim = forward_direction;

        iLiving target = __Targets.FindTarget(this, enemy_tag, null, true);

        if (target != null)
        {
            Vector2 position = Position();
            Vector2 target_position = target.Position();

            if (__Targets.LookingAtTarget(forward_direction, position, target_position))
            {
                aim = __Targets.TargetDirectionNormalized(position, target_position);
            }
        }

        if (accuracy != 1)
        {
            float y_modifier = __General.RandomChoice() ? 1.0f : -1.0f;
            aim.y += (1.0f - accuracy) * y_modifier * MAX_ACCURACY_OFFSET_Y;
        }

        return aim;
    }

    private void JumpOver()
    {
        const float TIME_IN_VOID = 0.15f;

        // Jump over the platform

        if (layer_shift || jump_number == 0 || velocity.y <= 0)
        {
            return;
        }

        float probable_distance = TIME_IN_VOID * velocity.y;

        if (probable_distance < Size().height)
        {
            return;
        }

        Vector2 foot_position = Position_BottomCenter();

        Vector2 destination_point = foot_position + Vector2.up * probable_distance;

        //Debug.DrawLine(foot_position, destination_point, Color.red, DRAW_TIME);
        RaycastHit2D hit = Physics2D.Linecast(foot_position, destination_point, GameData.LayerMasks.Default);

        if (hit.collider != null)
        {
            StartCoroutine(LayerShift(GameData.Layers.Living, GameData.Layers.Void, TIME_IN_VOID));
        }
    }
    #endregion Red

    #region Animations
    protected virtual void Animation_Death()
    {
        _animator.SetBool(AP_dead, true);
    }

    protected virtual void Animation_Hit()
    {
        _animator.SetTrigger(AP_hitted);
    }

    public void SetAnimatorTrigger(string trigger_name)
    {
        _animator.SetTrigger(trigger_name);
    }
    #endregion Animations

    #region Movement
    public void TurnAround()
    {
        forward_direction *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void TurnToMovingDirection(float direction)
    {
        if(direction==0)
        {
            return;
        }

        float direction_normalized = Math.Sign(direction);

        if (direction_normalized != ForwardDirection().x)
        {
            TurnAround();
        }
    }

    private void CalculateVelocity()
    {
        current_position = Position_Center();

        velocity = (current_position - previous_position) / Time.deltaTime;

        previous_position = current_position;

        if (movement_speed == 0)
        {
            APV_velocity_current = Vector2.zero;
        }
        else
        {
            APV_velocity_current = new Vector2(Math.Abs(velocity.x / movement_speed), Math.Abs(velocity.y / movement_speed));
        }

        if (APV_velocity_current != APV_velocity_previous)
        {
            _animator.SetFloat(AP_velocity_x, APV_velocity_current.x);
            _animator.SetFloat(AP_velocity_y, APV_velocity_current.y);

            APV_velocity_previous = APV_velocity_current;
        }
    }

    private void CalculateOffsets()
    {
        float min_y = float.PositiveInfinity;
        float max_y = float.NegativeInfinity;

        float min_x = float.PositiveInfinity;
        float max_x = float.NegativeInfinity;

        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            min_y = Math.Min(min_y, col.bounds.min.y);
            max_y = Math.Max(max_y, col.bounds.max.y);

            min_x = Math.Min(min_x, col.bounds.min.x);
            max_x = Math.Max(max_x, col.bounds.max.x);
        }

        float min_offset_y = min_y - transform.position.y;
        float max_offset_y = max_y - transform.position.y;
        float center_offset_y = (min_offset_y + max_offset_y) / 2;

        float min_offset_x = min_x - transform.position.x;
        float max_offset_x = max_x - transform.position.x;
        float center_offset_x = (min_offset_x + max_offset_x) / 2;

        size = new ObjectSize(max_x - min_x, max_y - min_y);

        Vector2 center_offset = new Vector2(center_offset_x, center_offset_y);
        Vector2 bottom_center_offset = new Vector2(center_offset_x, min_offset_y);
        Vector2 top_center_offset = new Vector2(center_offset_x, max_offset_y);
        Vector2 left_center_offset = new Vector2(min_offset_x, center_offset_y);
        Vector2 right_center_offset = new Vector2(max_offset_x, center_offset_y);

        position_offsets = new PositionOffsets(center_offset, top_center_offset, bottom_center_offset, left_center_offset, right_center_offset);

        nearest_position_offsets = new PositionOffsets(
            center_offset,
            top_center_offset + Vector2.up * MIN_DISTANCE,
            bottom_center_offset + Vector2.down * MIN_DISTANCE,
            left_center_offset + Vector2.left * MIN_DISTANCE,
            right_center_offset + Vector2.right * MIN_DISTANCE
            );
    }
    #endregion Movement

    #region Weapons
    private void InitializeWeapons()
    {
        _weapons_group = new GameObject(WEAPONS_GROUP).transform;
        _weapons_group.SetParent(transform, true);

        Vector3 position = transform.position + (Vector3)weapon_offset;

        available_weapons = new List<Weapon>();

        foreach (GameObject go in weapons_on_spawn)
        {
            GameObject instance_gameobject = __General.InstantiatePrefab(go, go.name, position, Quaternion.Euler(Vector3.zero), _weapons_group, true);
            Weapon instance_weapon = instance_gameobject.GetComponent<Weapon>();
            instance_weapon.SetOwnerData(_rigidbody, instance_id, tag);
            available_weapons.Add(instance_weapon);
        }

        SelectNextWeapon();
    }

    private int GetWeaponIndex(string weapon_name)
    {
        if (available_weapons.Count == 0)
        {
            return -1;
        }

        int weapon_index;

        if (weapon_name == "")
        {
            weapon_index = 0;

            int current_weapon_index = available_weapons.IndexOf(current_weapon);

            if (current_weapon_index != -1 && current_weapon_index != available_weapons.Count - 1)
            {
                weapon_index = current_weapon_index + 1;
            }
        }
        else
        {
            weapon_index = available_weapons.FindIndex((a) => a.name == weapon_name);
        }
        
        return weapon_index;
    }

    private void SelectWeapon(int index)
    {
        if (index == -1)
        {
            return;
        }

        if (current_weapon != null)
        {
            current_weapon.gameObject.SetActive(false);
        }

        current_weapon = available_weapons[index];

        current_weapon.gameObject.SetActive(true);
    }

    public void SelectRandomWeapon()
    {
        int weapon_count = available_weapons.Count;

        if (weapon_count > 0)
        {
            int random_index = UnityEngine.Random.Range(0, weapon_count);
            SelectWeapon(random_index);
        }
    }

    public void SelectNextWeapon()
    {
        SelectWeapon(GetWeaponIndex(""));
    }
    #endregion Weapons

    #region Skills
    public void SetSkill(HeroSkill v_skill)
    {
        last_used_skill = v_skill;
    }

    public void SkillEffect()
    {
        last_used_skill.Effect();
    }
    #endregion Skills

    #region Abilities
    public System.Collections.IEnumerator Invulnerability(float duration)
    {
        StartCoroutine(__VisualEffects.Blink(_renderer, duration, 0.1f, native_sprite_color));

        if (duration>0)
        {
            _health.SetImmunity(true);

            yield return new WaitForSeconds(duration);

            _health.SetImmunity(false);
        }
    }

    public System.Collections.IEnumerator Invisibility(float duration, float alpha)
    {
        invisible = true;

        StartCoroutine(__VisualEffects.Transparency(_renderer, duration, alpha));

        yield return new WaitForSeconds(duration);

        invisible = false;
    }

    public void WeaponAttack()
    {
        if (current_weapon != null)
        {
            current_weapon.Attack(Aim());
        }
    }

    public void PerformAttack(AttackData attack_data)
    {
        attack_data.ignore_tag = tag;
        __Attacks.PerformAttack(attack_data, Aim(), NearestPosition_Forward(), instance_id);
    }

    public void SpawnProjectile(ProjectileAttackData attack_data)
    {
        attack_data.ignore_tag = tag;
        __Attacks.SpawnProjectile(attack_data, Aim(), Position_Forward());
    }

    public bool Jump()
    {
        bool jumped = false;
        
        if (jump_number < jump_count)
        {
            Vector2 lowest_point = NearestPosition_BottomCenter();

            Instantiate(
                GameData.Particles.jump,
                new Vector3(lowest_point.x, lowest_point.y, GameData.Particles.jump.transform.position.z),
                GameData.Particles.jump.transform.rotation
                );

            _animator.SetTrigger(AP_jump);

            jump_number++;

            // Ignoring gravitation
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

            _rigidbody.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);

            jumped = true;
        }

        return jumped;
    }

    public void JumpDown()
    {
        // Hero should pass distance (which equals to his height) through current collider because of gravity being on "Void" layer

        if (layer_shift)
        {
            return;
        }

        bool can_jump_down = false;

        if (Grounded(GameData.LayerMasks.Default))
        {
            Vector2 touchdown_point = Position_BottomCenter() + Vector2.down * Size().height;

            Debug.DrawRay(touchdown_point, Vector2.down * MIN_DISTANCE, Color.red, DRAW_TIME);
            can_jump_down = Physics2D.Raycast(touchdown_point, Vector2.down, MIN_DISTANCE, GameData.LayerMasks.Obstacles).collider == null;
        }

        if (can_jump_down)
        {
            _animator.SetTrigger(AP_jump);
            StartCoroutine(LayerShift(GameData.Layers.Living, GameData.Layers.Void, 0.75f / _rigidbody.gravityScale));
        }
    }
    #endregion Abilities

    #region iLiving
    public bool IsLeader()
    {
        return is_leader;
    }

    #region iPosition
    public Vector2 Position()
    {
        return current_position;
    }

    public Vector2 Position_Forward()
    {
        Vector2 offset = ForwardDirection().x > 0 ? position_offsets.right_center : position_offsets.left_center;

        return (Vector2)transform.position + offset;
    }

    public Vector2 NearestPosition_Forward()
    {
        Vector2 offset = ForwardDirection().x > 0 ? nearest_position_offsets.right_center : nearest_position_offsets.left_center;

        return (Vector2)transform.position + offset;
    }

    public Vector2 Position_BottomCenter()
    {
        return (Vector2)transform.position + position_offsets.bottom_center;
    }

    public Vector2 NearestPosition_BottomCenter()
    {
        return (Vector2)transform.position + nearest_position_offsets.bottom_center;
    }

    public Vector2 Position_TopCenter()
    {
        return (Vector2)transform.position + position_offsets.top_center;
    }

    public Vector2 NearestPosition_TopCenter()
    {
        return (Vector2)transform.position + nearest_position_offsets.top_center;
    }

    public Vector2 Position_Center()
    {
        return (Vector2)transform.position + position_offsets.center;
    }

    public Vector2 ForwardDirection()
    {
        return forward_direction;
    }

    public bool Grounded(LayerMask layer_mask)
    {
        Vector2 bottom_point = Position_BottomCenter() + Vector2.up * MIN_DISTANCE;
        //Debug.DrawRay(bottom_point, Vector2.down * MIN_DISTANCE * 2, Color.red, DRAW_TIME);
        Collider2D col = Physics2D.Raycast(bottom_point, Vector2.down, MIN_DISTANCE * 2, layer_mask).collider;
        return col != null;
    }
    #endregion iPosition

    #region iDimensions
    public Vector2 Velocity()
    {
        return velocity;
    }

    public ObjectSize Size()
    {
        return size;
    }
    #endregion iDimensions

    #region iHealth
    public float CurrentHealth()
    {
        return _health.CurrentHealth();
    }

    public float MaxHealth()
    {
        return _health.MaxHealth();
    }

    public bool IsImmune()
    {
        return _health.IsImmune();
    }

    public void SetImmunity(bool v_immune)
    {
        _health.SetImmunity(v_immune);
    }

    public bool IsImmortal()
    {
        return _health.IsImmortal();
    }

    public void SetImmortality(bool v_immortal)
    {
        _health.SetImmortality(v_immortal);
    }

    public bool IsDead()
    {
        return _health.IsDead();
    }

    public bool Kill()
    {
        bool killed = _health.Kill();

        if (killed)
        {
            OnDeath();
        }

        return killed;
    }

    public virtual void Resurrect(float hp)
    {
        _health.Resurrect(hp);
        _mana.AddMana(_mana.max_mp);
        _rigidbody.velocity = Vector2.zero;
        gameObject.layer = GameData.Layers.Living;
        Invulnerability(immune_on_resurrect_duration);
        _animator.SetBool(AP_dead, false);
    }

    public virtual float Heal(float hp)
    {
        return _health.Heal(hp);
    }

    public virtual float Injure(float hp)
    {
        float delta_health = _health.Injure(hp);

        if (delta_health != 0)
        {
            if (IsDead())
            {
                OnDeath();
            }
            else
            {
                StartCoroutine(Invulnerability(immune_on_hit_duration));
                Animation_Hit();
            }
        }

        return delta_health;
    }
    #endregion iHealth

    #region iInstance
    public string Name()
    {
        return name;
    }

    public string Tag()
    {
        return tag;
    }

    public int InstanceID()
    {
        return instance_id;
    }

    public bool IsActiveAndEnabled()
    {
        return isActiveAndEnabled;
    }

    public bool IsInvisible()
    {
        return invisible;
    }
    #endregion iInstance
    #endregion iLiving

    #region iMana
    public float AddMana(float amount)
    {
        return _mana.AddMana(amount);
    }

    public float SubtractMana(float amount)
    {
        return _mana.SubtractMana(amount);
    }

    public bool UseMana(float cost)
    {
        return _mana.UseMana(cost);
    }

    public float CurrentMana()
    {
        return _mana.CurrentMana();
    }

    public float MaxMana()
    {
        return _mana.MaxMana();
    }
    #endregion iMana
}
