using UnityEngine;

public sealed class ManaRegeneration : Regeneration
{
    #region Variables
    private iMana _mana;

    [Tooltip("How much MP will be restored at a time.")]
    public float mp_amount;
    #endregion Variables

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();

        _mana = GetComponent<iMana>();
    }
    #endregion MonoBehaviour

    #region Red
    protected override void Effect()
    {
        _mana.AddMana(mp_amount);
    }
    #endregion Red	
}
