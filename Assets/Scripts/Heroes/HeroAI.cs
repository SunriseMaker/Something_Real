using System;
using UnityEngine;
using System.Collections.Generic;

public class HeroAI : MonoBehaviour
{
    protected delegate System.Collections.IEnumerator TriggerStatus();

    const float PAUSE_BETWEEN_SEARCHES = 1.0f;
    
    const float JUMP_IF_DISTANCE_X = 2.5f;

    const float JUMP_IF_DISTANCE_Y = 2.5f;

    const float CLOSEST_DISTANCE = 1.0f;

    #region Variables
    [Header("Behaviour")]
    [Tooltip("Stands still, doesn't move")]
    public bool idle;

    public bool mark_target_only_if_in_gaze_direction;

    [Range(0.0f, 100.0f)]
    [Tooltip("Percentage of health which determines that it's level is critical.")]
    public float critical_hp_percent;

    protected float critical_hp;

    [Header("Distance")]
    [Range(0.0f, 15.0f)]
    [Tooltip("Maximum distance at which hero will attack enemy.")]
    public float attack_distance;

    [Tooltip("Hero will loose target at this distance.")]
    [Range(5.0f, 50.0f)]
    public float lost_target_distance;

    [Header("Timing")]
    [Range(0.1f, 1.0f)]
    public float pause_between_attacks;

    [Range(0.1f, 1.0f)]
    public float pause_between_jumps;

    protected HeroController hero;

    protected iLiving enemy;

    protected iLiving leader;

    protected Vector2 hero_position;

    protected float hero_height;

    protected bool hero_is_leader;

    protected float roam_direction;

    protected bool can_search_for_leader;

    protected bool can_search_for_enemy;

    protected bool can_attack;

    protected bool can_defend;

    protected bool can_jump;

    protected List<HeroSkill> attack_skills = new List<HeroSkill>();

    protected List<HeroSkill> defence_skills = new List<HeroSkill>();

    private bool on_critical_hp_done;
    #endregion Variables

    #region MonoBehaviour
    protected virtual void Start()
    {
        hero = GetComponent<HeroController>();

        #region Assertions
        Debug.Assert(attack_distance >= CLOSEST_DISTANCE, "Hero " + name + " is unable to attack because attack_distance less than closest_distance.");
        Debug.Assert(lost_target_distance >= hero.sight, "Hero " + name + " sight is greater than lost_target_distance.");
        #endregion Assertions

        if (hero.available_skills.Count>0)
        {
            attack_skills = hero.available_skills.FindAll((a) => a.skill_type == HeroSkill.SkillType.Attack);
            defence_skills = hero.available_skills.FindAll((a) => a.skill_type == HeroSkill.SkillType.Defence);
        }

        hero.SelectRandomWeapon();

        can_search_for_leader = true;

        can_search_for_enemy = true;

        can_attack = true;

        can_defend = true;

        can_jump = true;

        critical_hp = hero.MaxHealth() * critical_hp_percent * 0.01f;

        on_critical_hp_done = (critical_hp == 0);

        roam_direction = __General.RandomChoice() ? 1.0f : -1.0f;

        hero_height = hero.Size().height;

        hero_is_leader = hero.IsLeader();
    }

    protected virtual void FixedUpdate()
    {
        if (hero.IsDead())
        {
            return;
        }

        hero_position = hero.Position();

        #region CriticalHP
        if (!on_critical_hp_done && hero.CurrentHealth()<=critical_hp)
        {
            OnCriticalHP();

            on_critical_hp_done = true;
        }
        #endregion CriticalHP

        #region Enemy
        if (enemy == null && can_search_for_enemy)
        {
            StartCoroutine(TriggerCanSearchForEnemy());

            enemy = __Targets.FindTarget(hero, hero.enemy_tag, null, mark_target_only_if_in_gaze_direction);

            if(enemy!=null)
            {
                Emotion(GameData.Prefabs.emotion_alertness);
            }
        }

        if(enemy!=null)
        {
            if (CheckTarget(ref enemy, hero.enemy_tag, null, true, true, GameData.Prefabs.emotion_happiness))
            {
                Pursue(enemy, true);
                return;
            }
        }
        #endregion Enemy

        #region Leader
        if (!hero_is_leader)
        {
            if (leader == null && can_search_for_leader)
            {
                StartCoroutine(TriggerCanSearchForLeader());
                leader = __Targets.FindTarget(hero, hero.tag, true, mark_target_only_if_in_gaze_direction);

                if (leader != null)
                {
                    Emotion(GameData.Prefabs.emotion_happiness);
                }
            }

            if(leader != null)
            {
                if (CheckTarget(ref leader, hero.tag, true, false, false, GameData.Prefabs.emotion_sadness))
                {
                    Pursue(leader, false);
                    return;
                }
            }
        }
        #endregion Leader

        Roam();
    }
    #endregion MonoBehaviour

    #region Red
    protected void Emotion(GameObject emotion_prefab)
    {
        __General.InstantiatePrefab(emotion_prefab, hero_position + Vector2.up * 2, Quaternion.Euler(Vector3.zero), hero.transform, false);
    }

    protected bool CheckTarget(ref iLiving target, string target_tag, bool? target_is_leader, bool lost_if_too_far, bool lost_if_invisible, GameObject emotion_if_dead)
    {
        // Player changed hero
        if (!target.IsActiveAndEnabled())
        {
            target = __Targets.FindTarget(hero, target_tag, target_is_leader, false);

            if (target == null)
            {
                return false;
            }
        }

        if (lost_if_too_far && __Targets.TargetDistance(hero_position, target.Position()) >= lost_target_distance)
        {
            target = null;
            return false;
        }

        if (target.IsDead())
        {
            if(emotion_if_dead!=null)
            {
                Emotion(emotion_if_dead);
            }
            
            enemy = null;
            return false;
        }

        if (lost_if_invisible && enemy.IsInvisible())
        {
            Emotion(GameData.Prefabs.emotion_surprise);
            enemy = null;
            return false;
        }

        return true;
    }

    protected void Roam()
    {
        const float ROAM_SPEED = 1.0f;

        Vector2 nearest_position_forward = hero.NearestPosition_Forward();
        Vector2 forward_direction = hero.ForwardDirection();
        Vector2 forward_down_direction = forward_direction + Vector2.down;

        //Debug.DrawRay(nearest_position_forward, forward_direction, Color.blue, 2.0f);
        //Debug.DrawRay(nearest_position_forward, forward_down_direction, Color.blue, 2.0f);

        if (
            Physics2D.Raycast(nearest_position_forward, forward_down_direction, hero_height, GameData.LayerMasks.Obstacles).collider == null
            ||
            Physics2D.Raycast(nearest_position_forward, forward_direction, 1.0f, GameData.LayerMasks.Obstacles).collider != null
            )
        {
            roam_direction *= -1;
            hero.TurnAround();
        }
        else
        {
            hero.Move(ROAM_SPEED * roam_direction, 0.0f, false);
        }
    }

    protected void Pursue(iLiving target, bool attack)
    {
        const float DEEP_DOWN = 10.0f;

        Vector2 target_position = target.Position();

        Vector2 target_direction = __Targets.TargetDirection(hero_position, target_position);

        Vector2 target_direction_input = __Targets.TargetDirection01(hero_position, target_position);

        Vector2 absolute_target_direction = new Vector2(Math.Abs(target_direction.x), Math.Abs(target_direction.y));

        float target_distance = __Targets.TargetDistance(hero_position, target_position);

        TurnToTarget(target_position);

        if (!idle)
        {
            if (absolute_target_direction.x > CLOSEST_DISTANCE)
            {
                hero.Move(target_direction_input.x, 0, false);

                // Checking dangers

                Vector2 ray_direction;
                float ray_length;

                if (hero.Grounded(GameData.LayerMasks.Obstacles))
                {
                    ray_direction= hero.ForwardDirection() + Vector2.down;
                    ray_length = hero_height;
                }
                else
                {
                    ray_direction = Vector2.down;
                    ray_length = DEEP_DOWN;
                }

                //Debug.DrawRay(hero_position, ray_direction * ray_length, Color.blue, 2.0f);
                if (Physics2D.Raycast(hero_position, ray_direction, ray_length, GameData.LayerMasks.Obstacles).collider == null)
                {
                    Jump();
                }
            }

            if (absolute_target_direction.y > JUMP_IF_DISTANCE_Y && absolute_target_direction.x < JUMP_IF_DISTANCE_X)
            {
                if (target_direction.y > 0)
                {
                    Jump();
                }
                else
                {
                    hero.JumpDown();
                }
            }
        }

        if (attack && can_attack && target_distance <= attack_distance)
        {
            RandomSkill(attack_skills, TriggerCanAttack);
        }
    }

    protected void Jump()
    {
        if (can_jump && hero.Jump())
        {
            StartCoroutine(TriggerCanJump());
        }
    }

    protected void TurnToTarget(Vector2 target_position)
    {
        if (!__Targets.LookingAtTarget(hero.ForwardDirection(), hero_position, target_position))
        {
            hero.TurnAround();
        }
    }

    protected void RandomSkill(List<HeroSkill> skill_list, TriggerStatus trigger)
    {
        if (skill_list.Count == 0)
        {
            return;
        }

        int index = UnityEngine.Random.Range(0, skill_list.Count);

        skill_list[index].Use();

        StartCoroutine(trigger());
    }

    protected virtual void OnCriticalHP()
    {
        RandomSkill(defence_skills, TriggerCanDefend);
    }
    #endregion Red

    #region Triggers
    protected System.Collections.IEnumerator TriggerCanSearchForEnemy()
    {
        can_search_for_enemy = false;

        yield return new WaitForSeconds(PAUSE_BETWEEN_SEARCHES);

        can_search_for_enemy = true;
    }

    protected System.Collections.IEnumerator TriggerCanSearchForLeader()
    {
        can_search_for_leader = false;

        yield return new WaitForSeconds(PAUSE_BETWEEN_SEARCHES);

        can_search_for_leader = true;
    }

    protected System.Collections.IEnumerator TriggerCanJump()
    {
        can_jump = false;

        yield return new WaitForSeconds(pause_between_jumps);

        can_jump = true;
    }

    protected System.Collections.IEnumerator TriggerCanAttack()
    {
        can_attack = false;

        yield return new WaitForSeconds(pause_between_attacks);

        can_attack = true;
    }

    protected System.Collections.IEnumerator TriggerCanDefend()
    {
        can_defend = false;

        yield return new WaitForSeconds(pause_between_attacks);

        can_defend = true;
    }
    #endregion Triggers
}
