using UnityEngine;

public interface iHealth
{
    float CurrentHealth();

    float MaxHealth();

    bool IsImmune();

    void SetImmunity(bool v_immune);

    bool IsImmortal();

    void SetImmortality(bool v_immortal);

    bool IsDead();

    bool Kill();

    void Resurrect(float hp);

    float Heal(float hp);

    float Injure(float hp);
}

public interface iMana
{
    float CurrentMana();

    float MaxMana();

    float AddMana(float amount);

    float SubtractMana(float amount);

    bool UseMana(float cost);
}

public interface iPosition
{
    Vector2 Position();

    Vector2 Position_Forward();

    Vector2 NearestPosition_Forward();

    Vector2 Position_BottomCenter();

    Vector2 NearestPosition_BottomCenter();

    Vector2 Position_TopCenter();

    Vector2 NearestPosition_TopCenter();

    Vector2 Position_Center();

    Vector2 ForwardDirection();

    bool Grounded(LayerMask layer_mask);
}

public interface iDimensions
{
    Vector2 Velocity();

    ObjectSize Size();
}

public interface iInstance
{
    string Name();

    string Tag();

    int InstanceID();

    bool IsActiveAndEnabled();

    bool IsInvisible();
}

public interface iLiving : iPosition, iDimensions, iInstance, iHealth
{
    bool IsLeader();
}
