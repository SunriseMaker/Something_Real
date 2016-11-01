using UnityEngine;

public sealed class BouncyPlatform : MonoBehaviour
{
    [SerializeField]
    private float jump_height;

    [SerializeField]
    private bool amortization;

	private void OnCollisionEnter2D(Collision2D col)
    {
        // This check prevents dead bodies from bouncing :)
        if (col.gameObject.layer!=LayerID.Limbo)
        {
            __Physics.AddForce(col.gameObject, Vector2.up * jump_height, ForceMode2D.Impulse);
        }
    }
}
