using UnityEngine;

public sealed class HeroSpawnPoint : MonoBehaviour
{
    public GameObject hero_prefab;
    private HeroController spawned_hero;

    public float interval;

    public bool spawn_on_awake;

    private float timer;

    private void Awake()
    {
        timer = 0.0f;

        if (spawn_on_awake)
        {
            Spawn();
        }
    }

    private void FixedUpdate()
    {
        if(interval>0)
        {
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                Spawn();
                timer = 0.0f;
            }
        }
    }

    public void Spawn()
    {
        if(hero_prefab!=null && (spawned_hero == null || spawned_hero.IsDead()))
        {
            GameObject hero_game_object = Instantiate(hero_prefab, transform.position, hero_prefab.transform.rotation) as GameObject;
            spawned_hero = hero_game_object.GetComponent<HeroController>();
        }
        
    }
}
