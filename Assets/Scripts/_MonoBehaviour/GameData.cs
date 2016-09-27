using System;
using UnityEngine;

// GameData contains static classes with fields which initialize in Awake method and update in FixedUpdate method 

public class GameData : MonoBehaviour
{
    #region Names
    const string OBJECT_MAIN_CAMERA = "MainCamera";
    const string OBJECT_EVENT_SYSTEM = "EventSystem";
    const string LAYER_DEFAULT = "Default";
    const string LAYER_LIVING = "Living";
    const string LAYER_LIMBO = "Limbo";
    const string LAYER_VOID = "Void";
    const string LAYER_PHYSICALOBJECTS = "PhysicalObjects";
    #endregion Names

    #region Variables
    [Header("GameTime")]
    public Vector3 initial_current_time;

    public float initial_time_speed;

    [Header("Singletons")]
    public GameObject prefab_event_system;
    public GameObject prefab_main_camera;

    [Header("PREFABS", order = 0)]
    [Header("Emotions", order = 1)]
    public GameObject prefab_emotion_happiness;
    public GameObject prefab_emotion_sadness;
    public GameObject prefab_emotion_alertness;
    public GameObject prefab_emotion_surprise;
    [Header("HUD")]
    public GameObject prefab_hud_available_skill;
    [Header("Other")]
    public GameObject prefab_jump_particles;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        #region Time
        Time.game_paused = false;
        Time.game_speed = UnityEngine.Time.timeScale;
        Time.now = __Time.Vector3_To_DateTime(DateTime.MinValue, initial_current_time);
        Time.time_speed = initial_time_speed;
        #endregion Time

        #region Layers
        Layers.Default = LayerMask.NameToLayer(LAYER_DEFAULT);
        Layers.Living = LayerMask.NameToLayer(LAYER_LIVING);
        Layers.Limbo = LayerMask.NameToLayer(LAYER_LIMBO);
        Layers.Void = LayerMask.NameToLayer(LAYER_VOID);
        #endregion Layers

        #region LayerMasks
        LayerMasks.Default = LayerMask.GetMask(LAYER_DEFAULT);
        LayerMasks.Obstacles = LayerMask.GetMask(LAYER_DEFAULT, LAYER_PHYSICALOBJECTS);
        LayerMasks.Targets = LayerMask.GetMask(LAYER_DEFAULT, LAYER_PHYSICALOBJECTS, LAYER_LIVING);
        #endregion LayerMasks

        #region Singletons
        #region EventSystem
        Debug.Assert(prefab_event_system != null, "ASSERTION FAILED: EventSystem prefab is not assigned.");

        GameObject event_system_gameobject = GameObject.Find(OBJECT_EVENT_SYSTEM);

        if(event_system_gameobject==null)
        {
            event_system_gameobject = Instantiate(prefab_event_system) as GameObject;
            event_system_gameobject.name = OBJECT_EVENT_SYSTEM;
        }

        Singletons.event_system = event_system_gameobject.GetComponent<UnityEngine.EventSystems.EventSystem>();
        Debug.Assert(Singletons.event_system != null, "ASSERTION FAILED: Singletons.event_system is null.");
        #endregion EventSystem

        #region MainCamera
        Debug.Assert(prefab_event_system != null, "ASSERTION FAILED: MainCamera prefab is not assigned.");

        GameObject main_camera_gameobject = GameObject.Find(OBJECT_MAIN_CAMERA);

        if(main_camera_gameobject==null)
        {
            main_camera_gameobject = Instantiate(prefab_main_camera) as GameObject;
            main_camera_gameobject.name = OBJECT_MAIN_CAMERA;
        }

        Singletons.main_camera = main_camera_gameobject.GetComponent<CameraFollowingPlayer>();
        Debug.Assert(Singletons.main_camera != null, "ASSERTION FAILED: Singletons.main_camera is null.");
        #endregion MainCamera
        #endregion Singletons

        #region Prefabs
        Prefabs.emotion_happiness = prefab_emotion_happiness;
        Prefabs.emotion_sadness = prefab_emotion_sadness;
        Prefabs.emotion_alertness = prefab_emotion_alertness;
        Prefabs.emotion_surprise = prefab_emotion_surprise;
        Prefabs.hud_available_skill = prefab_hud_available_skill;
        Prefabs.jump_particles = prefab_jump_particles;
        #endregion Prefabs
    }

    private void FixedUpdate()
    {
        Time.now = Time.now.AddSeconds(UnityEngine.Time.deltaTime * Time.time_speed);
    }
    #endregion MonoBehaviours

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
    }

    public static class Time
    {
        public static DateTime now;
        public static float time_speed;
        public static bool game_paused;
        public static float game_speed;
    }

    public static class Singletons
    {
        public static UnityEngine.EventSystems.EventSystem event_system;
        public static CameraFollowingPlayer main_camera;
    }

    public static class Prefabs
    {
        public static GameObject emotion_happiness;
        public static GameObject emotion_sadness;
        public static GameObject emotion_alertness;
        public static GameObject emotion_surprise;
        public static GameObject hud_available_skill;
        public static GameObject jump_particles;
    }
    #endregion StaticClasses
}
