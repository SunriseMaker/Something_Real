using UnityEngine;

public sealed class HeroSpawnPoint : MonoBehaviour
{
    public GameObject hero_prefab;
    private HeroController spawned_hero;

    public bool spawn_on_awake;

    public float interval;

    private void Awake()
    {
        if(spawn_on_awake)
        {
            Spawn();
        }

        StartCoroutine(crSpawn());
    }

    private System.Collections.IEnumerator crSpawn()
    {
        while(true)
        {
            yield return new WaitUntil(() => spawned_hero == null || spawned_hero.IsDead());

            yield return new WaitForSeconds(interval);

            Spawn();
        }
    }

    public void Spawn()
    {
        GameObject hero_game_object = Instantiate(hero_prefab, transform.position, hero_prefab.transform.rotation) as GameObject;
        spawned_hero = hero_game_object.GetComponent<HeroController>();
    }
}
