using UnityEngine;
using System;
using System.Linq;

public static class __Targets
{
    public static iLiving FindTarget(HeroController seeker, string target_tag, bool? target_is_leader, bool only_in_gaze_direction)
    {
        iLiving ret = null;

        Vector2 attacker_position = seeker.Position();
        Vector2 attacker_forward_direction = seeker.ForwardDirection();

        // Limiting objects quantity to minimize calculations by testing the area only in attacker's sight distance
        Collider2D[] objects_in_radius = Physics2D.OverlapCircleAll(attacker_position, seeker.sight, LayerMaskID.Targets);

        var targets =
            from
                q
            in
                (
                from
                    q
                in
                    objects_in_radius
                where
                    q.tag == target_tag
                select
                    q.GetComponent<iLiving>()
                )
            where
                q != null
                && !q.IsDead()
                && !q.IsInvisible()
                && q.InstanceID() != seeker.InstanceID()
            select new
            {
                living = q,
                distance = Vector2.Distance(attacker_position, q.Position())
            }
        ;

        if (only_in_gaze_direction)
        {
            targets = targets.Where((a) => __Targets.LookingAtTarget(attacker_forward_direction, attacker_position, a.living.Position()));
        }

        if(target_is_leader!=null)
        {
            targets = targets.Where((a) => (a.living.IsLeader() == target_is_leader));
        }

        targets = targets.OrderBy((a) => a.distance);

        bool done = false;

        foreach (var t in targets)
        {
            // No sorting needed, because:
            // >> The colliders in the array are sorted in order of distance from the origin point.

            RaycastHit2D[] objects_in_sight_line =
                Physics2D.RaycastAll(attacker_position, __Targets.TargetDirectionNormalized(attacker_position, t.living.Position()), seeker.sight, LayerMaskID.Targets);

            foreach (RaycastHit2D r in objects_in_sight_line)
            {
                iLiving living = r.collider.GetComponent<iLiving>();

                // An obstacle
                if(living==null)
                {
                    break;
                }

                // Nearest target in sight
                if(living==t.living)
                {
                    ret = t.living;
                    done = true;
                    break;
                }
            }

            if(done)
            {
                break;
            }
        }

        return ret;
    }

    #region TargetDistance
    public static float TargetDistance(Vector2 attacker_position, Vector2 target_position)
    {
        return Vector2.Distance(attacker_position, target_position);
    }

    public static float TargetDistance(iPosition attacker, iPosition target)
    {
        return TargetDistance(attacker.Position(), target.Position());
    }
    #endregion TargetDistance

    #region LookingAtTarget
    public static bool LookingAtTarget(Vector2 attacker_forward_direction, Vector2 attacker_position, Vector2 target_position)
    {
        return (attacker_forward_direction.x == TargetDirection01(attacker_position, target_position).x);
    }

    public static bool LookingAtTarget(iPosition attacker, iPosition target)
    {
        bool looking_at_target = false;

        if (target != null)
        {
            looking_at_target = LookingAtTarget(attacker.ForwardDirection(), attacker.Position(), target.Position());
        }

        return looking_at_target;
    }
    #endregion LookingAtTarget

    #region Directions
    #region TargetDirection
    public static Vector2 TargetDirection(Vector2 attacker_position, Vector2 target_position)
    {
        return target_position - attacker_position;
    }

    public static Vector2 TargetDirection(iPosition attacker, iPosition target)
    {
        return TargetDirection(target.Position(), attacker.Position());
    }
    #endregion TargetDirection

    #region TargetDirectionNormalized
    public static Vector2 TargetDirectionNormalized(Vector2 attacker_position, Vector2 target_position)
    {
        return TargetDirection(attacker_position, target_position).normalized;
    }

    public static Vector2 TargetDirectionNormalized(iPosition attacker, iPosition target)
    {
        return TargetDirectionNormalized(attacker.Position(), target.Position());
    }
    #endregion TargetDirectionNormalized

    #region TargetDirection01
    public static Vector2 TargetDirection01(Vector2 attacker_position, Vector2 target_position)
    {
        float direction_x = attacker_position.x < target_position.x ? 1.0f : -1.0f;
        float direction_y = attacker_position.y < target_position.y ? 1.0f : -1.0f;

        return new Vector2(direction_x, direction_y);
    }

    public static Vector2 TargetDirection01(iPosition attacker, iPosition target)
    {
        return __Targets.TargetDirection01(attacker.Position(), target.Position());
    }
    #endregion TargetDirection01
    #endregion Directions
}
