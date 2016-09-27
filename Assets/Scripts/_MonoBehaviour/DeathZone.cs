using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private void OnTriggerEnter2D (Collider2D col)
    {
        iHealth ihealth = col.GetComponent<iHealth>();

        if (ihealth!=null && !ihealth.IsDead())
        {
            ihealth.Kill();
        }
	}
}
