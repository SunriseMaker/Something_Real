using UnityEngine;

public sealed class DestroyAfterDelay : MonoBehaviour
{
    public float delay;
	
	private void Start()
    {
        Destroy(gameObject, delay);
	}
}
