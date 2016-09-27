using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController player = col.transform.root.GetComponent<PlayerController>();
        
        if(player != null)
        {
            player.SetCheckPoint(transform.position);
        }
    }
}
