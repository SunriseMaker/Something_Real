using UnityEngine;

public sealed class HealthRegeneration : Regeneration
{
    #region Variables
    [Tooltip("How much HP will be restored at a time.")]
    [SerializeField]
    private float hp_amount;
    #endregion Variables

    #region Red
    protected override void Effect()
    {
        ihealth.Heal(hp_amount);
    }
    #endregion Red
}
