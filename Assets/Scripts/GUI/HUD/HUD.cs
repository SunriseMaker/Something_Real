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
    #endregion Health

    #region Mana
    [Header("Mana")]
    private Slider mana_slider;
    private Text mana_text;
    #endregion Mana

    #region Items
    [Header("Items")]
    private Image item_image;
    private Text item_text;
    #endregion Items

    #region Weapons
    [Header("Weapons")]
    private Image weapon_image;
    #endregion Weapons

    #region Skills
    [Header("Active Skills")]
    public Image[] skill_images;
    #endregion Skills

    private HeroPlayable hero_playable;

    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        health_slider = transform.FindChild("HUD_Health_Slider").GetComponent<Slider>();
        health_text = transform.FindChild("HUD_Health_Text").GetComponent<Text>();

        mana_slider = transform.FindChild("HUD_Mana_Slider").GetComponent<Slider>();
        mana_text = transform.FindChild("HUD_Mana_Text").GetComponent<Text>();

        item_image = transform.FindChild("HUD_Item_Image").GetComponent<Image>();
        item_text = transform.FindChild("HUD_Item_Text").GetComponent<Text>();

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

    public void UpdateItem(InventoryCell inventory_cell)
    {
        float alpha = 0;

        if(inventory_cell!=null)
        {
            alpha = 1;

            item_image.sprite = inventory_cell.inventory_item.hud_sprite;
            item_text.text = inventory_cell.quantity.ToString();
        }

        item_text.canvasRenderer.SetAlpha(alpha);
        item_image.canvasRenderer.SetAlpha(alpha);
    }

    public void UpdateWeapon(Weapon weapon)
    {
        float alpha = 0;

        if(weapon!=null)
        {
            alpha = 1;
            weapon_image.sprite = weapon.HUD_sprite;
        }

        weapon_image.canvasRenderer.SetAlpha(alpha);
    }

    public void UpdateActiveSkills()
    {
        foreach (Image i in skill_images)
        {
            i.sprite = null;
            i.canvasRenderer.SetAlpha(0);
        }

        for (int i = 0; i < hero_playable.active_skills.Count; i++)
        {
            HeroSkill s = hero_playable.active_skills[i];

            skill_images[i].sprite = s.HUD_sprite;
            skill_images[i].canvasRenderer.SetAlpha(1);
        }
    }

    public void SetHeroReference (HeroPlayable v_hero_playable)
    {
        hero_playable = v_hero_playable;
    }

    public void ChangeActiveSkill(int skill_number)
    {
        if(hero_playable!=null)
        {
            hero_playable.NextSkill(skill_number);
        }
    }
    #endregion Red
}
