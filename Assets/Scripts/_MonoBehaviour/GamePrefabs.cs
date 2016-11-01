using UnityEngine;

public class GamePrefabs : MonoBehaviour
{
    [Header("Emotions")]
    [SerializeField]
    private GameObject emotion_happiness;

    [SerializeField]
    private GameObject emotion_sadness;

    [SerializeField]
    private GameObject emotion_alertness;

    [SerializeField]
    private GameObject emotion_surprise;

    [Header("Particles")]
    [SerializeField]
    private GameObject particles_jump;

    [Header("SFX")]
    [SerializeField]
    private AudioClip sfx_pickup;

    [Header("Other")]
    [SerializeField]
    private GameObject loading_screen;

    [SerializeField]
    private GameObject pause_menu;

    private void Awake()
    {
        #region Emotions
        Emotions.happiness = emotion_happiness;
        Emotions.sadness = emotion_sadness;
        Emotions.alertness = emotion_alertness;
        Emotions.surprise = emotion_surprise;
        #endregion Emotions

        #region Particles
        Particles.jump = particles_jump;
        #endregion Particles

        #region SFX
        SFX.pickup = sfx_pickup;
        #endregion SFX

        #region Other
        Other.loading_screen = loading_screen;
        Other.pause_menu = pause_menu;
        #endregion Other
    }

    #region StaticClasses
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

    public static class SFX
    {
        public static AudioClip pickup;
    }

    public static class Other
    {
        public static GameObject loading_screen;
        public static GameObject pause_menu;
    }
    #endregion StaticClasses
}
