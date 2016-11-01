using UnityEngine;

public sealed class HeroSpawnPoint : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject hero_prefab;

    [SerializeField]
    private bool spawn_on_awake;

    private HeroController spawned_hero;

    public float interval;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        if(spawn_on_awake)
        {
            Spawn();
        }

        StartCoroutine(crSpawn());
    }
    #endregion MonoBehaviour

    #region Red
    private System.Collections.IEnumerator crSpawn()
    {
        while(true)
        {
            yield return new WaitUntil(() => spawned_hero == null || spawned_hero.IsDead());

            yield return new WaitForSeconds(interval);

            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject hero_game_object = Instantiate(hero_prefab, transform.position, hero_prefab.transform.rotation) as GameObject;
        spawned_hero = hero_game_object.GetComponent<HeroController>();
    }
    #endregion Red
}
