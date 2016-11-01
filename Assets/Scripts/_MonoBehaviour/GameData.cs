using System;
using UnityEngine;

// This class is a centralized reference storage
// GameData contains static classes with fields which initialize in Awake method and update in FixedUpdate method 

public class GameData : MonoBehaviour
{
    #region Names
    const string OBJECT_MAIN_CAMERA = "MainCamera";
    const string OBJECT_EVENT_SYSTEM = "EventSystem";
    const string OBJECT_GAME_EVENTS = "GameEvents";
    const string OBJECT_HUD = "HUD";
    #endregion Names

    #region Variables
    [HideInInspector]
    public static int main_menu_id;

    [Header("GameTime")]
    [SerializeField]
    private Vector3 initial_current_time;

    [SerializeField]
    private float initial_time_speed;

    [Header("Singletons")]
    [SerializeField]
    private GameObject singleton_event_system;

    [SerializeField]
    private GameObject singleton_game_events;

    [SerializeField]
    private GameObject singleton_main_camera;

    [SerializeField]
    private GameObject singleton_hud;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        main_menu_id = 0;
        
        #region Time
        GameTime.game_paused = false;
        GameTime.normal_time_scale = 1.0f;
        Time.timeScale = GameTime.normal_time_scale;

        GameTime.normal_fixed_delta_time = 0.02f;
        Time.fixedDeltaTime = GameTime.normal_fixed_delta_time;

        GameTime.now = __Time.Vector3_To_DateTime(DateTime.MinValue, initial_current_time);
        GameTime.time_speed = initial_time_speed;
        #endregion Time

        #region Singletons
        AssignSingleton<UnityEngine.EventSystems.EventSystem>(ref Singletons.event_system, singleton_event_system, OBJECT_EVENT_SYSTEM);
        AssignSingleton<GameEvents>(ref Singletons.game_events, singleton_game_events, OBJECT_GAME_EVENTS);
        AssignSingleton<PlayerCamera>(ref Singletons.main_camera, singleton_main_camera, OBJECT_MAIN_CAMERA);
        AssignSingleton<HUD>(ref Singletons.hud, singleton_hud, OBJECT_HUD);
        #endregion Singletons
    }

    private void FixedUpdate()
    {
        GameTime.now = GameTime.now.AddSeconds(Time.deltaTime * GameTime.time_speed);
    }
    #endregion MonoBehaviours

    #region Red
    private void AssignSingleton<T>(ref T singleton, GameObject prefab_gameobject, string prefab_name)
    {
        Debug.Assert(prefab_gameobject != null);

        GameObject singleton_gameobject = GameObject.Find(prefab_name);

        if (singleton_gameobject == null)
        {
            singleton_gameobject = Instantiate(prefab_gameobject) as GameObject;
            singleton_gameobject.name = prefab_name;
        }

        singleton = singleton_gameobject.GetComponent<T>();

        Debug.Assert(singleton != null);
    }
    #endregion Red

    #region StaticClasses
    public static class GameTime
    {
        public static DateTime now;
        public static float time_speed;
        public static bool game_paused;
        public static float normal_fixed_delta_time;
        public static float normal_time_scale;
    }
    #endregion StaticClasses
}
