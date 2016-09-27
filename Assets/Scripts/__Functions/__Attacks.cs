using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public static class __Attacks
{
    public static void PerformAttack(AttackData attack_data, Vector2 aim, Vector2 attacker_position, int instance_id_ignored, string tag_ignored)
    {
        RaycastHit2D[] raycasts;

        if (attack_data.radius > 0)
        {
            raycasts = Physics2D.CircleCastAll(attacker_position, attack_data.radius, aim, attack_data.range, GameData.LayerMasks.Targets);
        }
        else
        {
            raycasts = Physics2D.RaycastAll(attacker_position, aim, attack_data.range, GameData.LayerMasks.Targets);
        }

        var q1 = from q in raycasts
                 where
                    q.collider.tag!=tag_ignored
                    &&
                    q.collider.gameObject.GetInstanceID() != instance_id_ignored
                 select new
                 {
                     game_object = q.collider.gameObject,
                     living = q.collider.GetComponent<iLiving>(),
                     rigidbody2d = q.collider.gameObject.GetComponent<Rigidbody2D>(),
                     distance = q.distance
                 }
                 ;

        q1 = q1.OrderBy((a) => a.distance);

        List<int> targets_injured = new List<int>();

        int target_id;
        Vector2 impact_force_direction;

        foreach (var q in q1)
        {
            if (q.living == null)
            {
                // Obstacle
                impact_force_direction = __Targets.TargetDirectionNormalized(attacker_position, q.game_object.transform.position);

                ApplyImpactForce(attack_data, impact_force_direction, q.rigidbody2d);
                
                if (!attack_data.penetrate_walls)
                {
                    break;
                }
            }
            else
            {
                // Living
                if (attack_data.radius==0 && (!attack_data.penetrate_enemies && targets_injured.Count > 0))
                {
                    break;
                }
                else
                {
                    target_id = q.game_object.GetInstanceID();

                    // Ensure that the target hasn't been hitted formerly (because entity can have more than one collider)
                    if (!targets_injured.Exists((a) => a == target_id))
                    {
                        q.living.Injure(attack_data.damage);
                        
                        targets_injured.Add(target_id);

                        if (q.living.IsDead() || attack_data.always_apply_impact_force)
                        {
                            impact_force_direction = __Targets.TargetDirectionNormalized(attacker_position, q.living.Position());
                            ApplyImpactForce(attack_data, impact_force_direction, q.rigidbody2d);
                        }
                    }
                }
            }
        }
    }

    public static void SpawnProjectile(ProjectileAttackData projectile_attack_data, Vector2 launch_direction, Vector3 position)
    {
        Vector3 object_position = new Vector3(position.x, position.y, projectile_attack_data.projectile_prefab.transform.position.z);

        GameObject p = UnityEngine.Object.Instantiate(
            projectile_attack_data.projectile_prefab,
            object_position,
            projectile_attack_data.projectile_prefab.transform.rotation
            ) as GameObject;

        // Turning the projectile at shot direction
        p.transform.localScale = new Vector3(p.transform.localScale.x * Math.Sign(launch_direction.x), p.transform.localScale.y, projectile_attack_data.projectile_prefab.transform.localScale.z);

        p.GetComponent<Projectile>().Launch(launch_direction, projectile_attack_data);
    }

    public static void ApplyImpactForce(AttackData attack_data, Vector2 force_direction, Rigidbody2D target_rigidbody)
    {
        if (
            attack_data.impact_force_amount != 0
            && force_direction != Vector2.zero
            && target_rigidbody != null
            && !target_rigidbody.isKinematic
            )
        {
            target_rigidbody.AddForce(force_direction * attack_data.impact_force_amount, ForceMode2D.Impulse);
        }
    }
}
