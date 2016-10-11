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
    const string LAYER_DEFAULT = "Default";
    const string LAYER_LIVING = "Living";
    const string LAYER_LIMBO = "Limbo";
    const string LAYER_VOID = "Void";
    const string LAYER_PHYSICAL_OBJECTS = "PhysicalObjects";
    const string LAYER_INTERACTABLE_OBJECTS = "InteractableObjects";
    #endregion Names

    #region Variables
    [HideInInspector]
    public static int main_menu_id;

    [Header("GameTime")]
    public Vector3 initial_current_time;

    public float initial_time_speed;

    [Header("Singletons")]
    public GameObject singleton_event_system;
    public GameObject singleton_game_events;
    public GameObject singleton_main_camera;

    [Header("Emotions")]
    public GameObject emotion_happiness;

    public GameObject emotion_sadness;

    public GameObject emotion_alertness;

    public GameObject emotion_surprise;

    [Header("Particles")]
    public GameObject particles_jump;

    [Header("SFX")]
    public AudioClip sfx_pickup;

    [Header("Other")]
    public GameObject hud_available_skill;
    public GameObject loading_screen;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        main_menu_id = 0;

        #region Time
        Time.game_paused = false;
        Time.normal_time_scale = 1.0f;
        UnityEngine.Time.timeScale = Time.normal_time_scale;

        Time.normal_fixed_delta_time = 0.02f;
        UnityEngine.Time.fixedDeltaTime = Time.normal_fixed_delta_time;

        Time.now = __Time.Vector3_To_DateTime(DateTime.MinValue, initial_current_time);
        Time.time_speed = initial_time_speed;
        #endregion Time

        #region Singletons
        AssignSingleton<UnityEngine.EventSystems.EventSystem>(ref Singletons.event_system, singleton_event_system, OBJECT_EVENT_SYSTEM);
        AssignSingleton<GameEvents>(ref Singletons.game_events, singleton_game_events, OBJECT_GAME_EVENTS);
        AssignSingleton<PlayerCamera>(ref Singletons.main_camera, singleton_main_camera, OBJECT_MAIN_CAMERA);
        #endregion Singletons

        #region Layers
        Layers.Default = LayerMask.NameToLayer(LAYER_DEFAULT);
        Layers.Living = LayerMask.NameToLayer(LAYER_LIVING);
        Layers.Limbo = LayerMask.NameToLayer(LAYER_LIMBO);
        Layers.Void = LayerMask.NameToLayer(LAYER_VOID);
        #endregion Layers

        #region LayerMasks
        LayerMasks.Default = LayerMask.GetMask(LAYER_DEFAULT);
        LayerMasks.Obstacles = LayerMask.GetMask(LAYER_DEFAULT, LAYER_PHYSICAL_OBJECTS);
        LayerMasks.Targets = LayerMask.GetMask(LAYER_DEFAULT, LAYER_PHYSICAL_OBJECTS, LAYER_LIVING);
        LayerMasks.IntaractableObjects = LayerMask.GetMask(LAYER_INTERACTABLE_OBJECTS);
        #endregion LayerMasks

        #region Emotions
        Emotions.happiness = emotion_happiness;
        Emotions.sadness = emotion_sadness;
        Emotions.alertness = emotion_alertness;
        Emotions.surprise = emotion_surprise;
        #endregion Emotions

        #region Other
        Other.hud_available_skill = hud_available_skill;
        Other.loading_screen = loading_screen;
        #endregion Other

        #region Particles
        Particles.jump = particles_jump;
        #endregion Particles

        #region SFX
        SFX.pickup = sfx_pickup;
        #endregion SFX
    }

    private void FixedUpdate()
    {
        Time.now = Time.now.AddSeconds(UnityEngine.Time.deltaTime * Time.time_speed);
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
    public static class Layers
    {
        public static int Default;
        public static int Living;
        public static int Limbo;
        public static int Void;
    }

    public static class LayerMasks
    {
        public static int Default;
        public static int Obstacles;
        public static int Targets;
        public static int IntaractableObjects;
    }

    public static class Time
    {
        public static DateTime now;
        public static float time_speed;
        public static bool game_paused;
        public static float normal_fixed_delta_time;
        public static float normal_time_scale;
    }

    public static class Singletons
    {
        public static UnityEngine.EventSystems.EventSystem event_system;
        public static PlayerCamera main_camera;
        public static GameEvents game_events;
    }

    public static class Emotions
    {
        public static GameObject happiness;
        public static GameObject sadness;
        public static GameObject alertness;
        public static GameObject surprise;
    }

    public static class Particles
    {
        public static GameObject jump;
    }

    public static class Other
    {
        public static GameObject hud_available_skill;
        public static GameObject loading_screen;
    }

    public static class SFX
    {
        public static AudioClip pickup;
    }
    #endregion StaticClasses
}
