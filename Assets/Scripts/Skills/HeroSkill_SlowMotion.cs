using UnityEngine;

public class HeroSkill_SlowMotion : HeroSkill
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
        Debug.Assert(world_slow > 1.0f);

        base.Effect();

        float real_time_duration = effect_duration / world_slow;
        StartCoroutine(__Time.SlowMotion(world_slow, real_time_duration));
        StartCoroutine(Singletons.main_camera.TwirlEffect(0.5f, 0.025f, 360f));
    }

    protected override float CalcDelay()
    {
        return base_delay / world_slow;
    }
    #endregion Red
}
