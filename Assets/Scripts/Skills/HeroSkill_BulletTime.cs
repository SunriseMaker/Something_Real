using UnityEngine;

public class HeroSkill_BulletTime : HeroSkill
{
    #region Variables
    [Range(2.0f, 10.0f)]
    public float effect_duration;

    [Range(1.0f, 10.0f)]
    public float world_slow;
    #endregion Variables

    #region Red
    public override void Effect()
    {
        base.Effect();

        float real_time_duration = effect_duration / world_slow;

        if(world_slow>1.0f)
        {
            StartCoroutine(__Time.SlowMotion(world_slow, real_time_duration));

            StartCoroutine(BulletTime(real_time_duration));
        }
    }

    protected override float CalcDelay()
    {
        return base_delay / world_slow;
    }

    private System.Collections.IEnumerator BulletTime(float duration)
    {
        float initial_movement_speed = hero.movement_speed;
        float initial_gravity_scale = hero._rigidbody.gravityScale;
        float initial_jump_force = hero.jump_force;

        hero._animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        hero.movement_speed *= world_slow;
        hero._rigidbody.gravityScale *= world_slow;
        hero.jump_force *= world_slow;

        yield return new WaitForSeconds(duration);

        hero._rigidbody.velocity = Vector2.zero;
        hero._animator.updateMode = AnimatorUpdateMode.Normal;
        hero.movement_speed = initial_movement_speed;
        hero._rigidbody.gravityScale = initial_gravity_scale;
        
        hero.jump_force = initial_jump_force;
    }
    #endregion Red
}
