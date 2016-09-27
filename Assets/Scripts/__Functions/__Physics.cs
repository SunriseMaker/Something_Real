using UnityEngine;

public static class __Physics
{
    public static void AddForce(GameObject game_object, Vector2 force, ForceMode2D force_mode=ForceMode2D.Impulse)
    {
        Rigidbody2D _rigidbody = game_object.GetComponent<Rigidbody2D>();

        if (_rigidbody != null && !_rigidbody.isKinematic)
        {
            _rigidbody.AddForce(force, force_mode);
        }
    }
}
