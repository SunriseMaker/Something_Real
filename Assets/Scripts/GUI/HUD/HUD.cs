using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region Variables
    #region Health
    [Header("Health")]
    public Sprite health_slider_icon_alive;
    public Sprite health_slider_icon_dead;

    private Image health_slider_handle_image;
    private Slider health_slider;
    private Text health_text;
    #endregion HealthBar

    [Header("Mana")]
    #region Mana
    private Slider mana_slider;
    private Text mana_text;
    #endregion Mana

    [Header("Weapons")]
    #region Weapons
    private Image weapon_image;
    #endregion Weapons
    #endregion Variables

    [Header("Active Skills")]
    public Image[] skill_images;

    #region MonoBehaviour
    private void Awake()
    {
        health_slider = transform.FindChild("HUD_Health_Slider").GetComponent<Slider>();
        health_text = transform.FindChild("HUD_Health_Text").GetComponent<Text>();

        mana_slider = transform.FindChild("HUD_Mana_Slider").GetComponent<Slider>();
        mana_text = transform.FindChild("HUD_Mana_Text").GetComponent<Text>();

        weapon_image = transform.FindChild("HUD_Weapon_Image").GetComponent<Image>();

        health_slider_handle_image = health_slider.handleRect.GetComponent<Image>();
    }
    #endregion MonoBehaviour

    #region Red
    public void UpdateHealth(float current_health, float max_health)
    {
        health_slider_handle_image.sprite = current_health > 0 ? health_slider_icon_alive : health_slider_icon_dead;
        health_slider.value = current_health / max_health;
        health_text.text = ((int)current_health).ToString();
    }

    public void UpdateMana(float current_mana, float max_mana)
    {
        mana_slider.value = current_mana / max_mana;
        mana_text.text = ((int)current_mana).ToString();
    }

    public void UpdateWeapon(Sprite weapon_sprite)
    {
        weapon_image.sprite = weapon_sprite;

        weapon_image.canvasRenderer.SetAlpha(weapon_sprite == null ? 0.0f : 1.0f);
    }

    public void UpdateActiveSkills(System.Collections.Generic.List<HeroSkill> skill_list)
    {
        foreach (Image i in skill_images)
        {
            i.sprite = null;
            i.canvasRenderer.SetAlpha(0);
        }

        for (int i = 0; i < skill_list.Count; i++)
        {
            HeroSkill s = skill_list[i];

            skill_images[i].sprite = s.HUD_sprite;
            skill_images[i].canvasRenderer.SetAlpha(1);
        }
    }
    #endregion Red
}
