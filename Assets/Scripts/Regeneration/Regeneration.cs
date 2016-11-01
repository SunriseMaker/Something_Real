using UnityEngine;

public class Regeneration : MonoBehaviour
{
    #region Variables
    [Tooltip("Delay in seconds between two effects occur.\nThe lesser this number, the faster effect occurs.")]
    [SerializeField]
    private float pause_between_effects;

    protected iHealth ihealth;
    #endregion Variables

    #region MonoBehaviour
    protected virtual void Awake()
    {
        ihealth = GetComponent<iHealth>();
	}

    private void Start()
    {
        Debug.Assert(pause_between_effects != 0, "ASSERTION FAILED: pause_between_effects parameter is zero.");
        
        StartCoroutine(EffectLoop());
    }

    private System.Collections.IEnumerator EffectLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(pause_between_effects);

            if(!ihealth.IsDead())
            {
                Effect();
            }
        }
    }
    #endregion MonoBehaviour

    #region Red
    protected virtual void Effect()
    {
    }
    #endregion Red
}
