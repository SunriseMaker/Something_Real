using UnityEngine;

public class ExplosiveArrow : Projectile
{
    #region Variables
    [SerializeField]
    private GameObject explosion_prefab;

    [SerializeField]
    private Vector2 explosion_scale;
    #endregion Variables

    #region Red
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);

        Vector3 position = new Vector3(_collider.bounds.center.x, _collider.bounds.center.y, explosion_prefab.transform.position.z);
        GameObject _instance = Instantiate(explosion_prefab, position, Quaternion.Euler(Vector3.zero)) as GameObject;
        _instance.transform.localScale = new Vector3(explosion_scale.x, explosion_scale.y, 1.0f);
    }
    #endregion Red
}
