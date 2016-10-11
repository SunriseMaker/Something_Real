using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Heroes")]
    public List<GameObject> heroes_on_spawn = new List<GameObject>();

    [HideInInspector]
    private List<HeroController> available_heroes = new List<HeroController>();

    [HideInInspector]
    public HeroController current_hero;

    [Header("Pause menu")]
    [Tooltip("Pause menu prefab.")]

    const string HEROES_GROUP = "Heroes";
    public GameObject pause_menu;

    [Header("HUD")]
    [Tooltip("HUD prefab.")]
    public HUD hud;

    [Tooltip("Time in seconds between updates of HUD elements.")]
    [Range(0.1f, 1.0f)]
    public float hud_update_rate;

    private Vector2 checkpoint_position;

    #region Inventory
    private Inventory _inventory;
    private InventoryCell current_inventory_cell;
    #endregion Inventory

    #region UserInput
    private float userinput_horizontal_look;
    private float userinput_vertical_look;
    private bool userinput_change_character;
    private bool userinput_change_weapon;
    private int userinput_change_item;
    bool userinput_change_item_axis_is_in_use;
    private bool userinput_submit;
    private bool userinput_use;
    private bool userinput_cancel;
    private bool userinput_menu;
    #endregion UserInput
    #endregion Variables

    #region MonoBehaviour
    private void Start()
    {
        GameData.Singletons.main_camera.SetPlayerReference(this);

        checkpoint_position = transform.position;

        InstantiateHeroes();

        Debug.Assert(available_heroes.Count != 0, "ASSERTION FAILED: available_heroes collection is empty.");
        Debug.Assert(!available_heroes.Exists((a) => a == null), "ASSERTION FAILED: Unassigned entry in available_heroes collection.");

        InstantiateHUD();

        InstantiateInventory();
    }

    private void Update()
    {
        UserInput();
        EntityControl();
    }
    #endregion MonoBehaviour

    #region Inventory
    public void AddInventoryItem(GameObject inventory_item_prefab, InventoryItem inventory_item_component, int quantity)
    {
        _inventory.AddItem(inventory_item_prefab, inventory_item_component, quantity);

        if(current_inventory_cell==null)
        {
            SelectNextInventoryCell();
        }
    }

    private void InstantiateInventory()
    {
        _inventory = new Inventory(transform);
    }

    private void SelectNextInventoryCell()
    {
        current_inventory_cell = _inventory.GetNextInventoryCell(current_inventory_cell, userinput_change_item);
    }

    private bool UseCurrentItemInCell()
    {
        bool used = false;

        if (current_inventory_cell != null)
        {
            used = _inventory.UseItemInCell(ref current_inventory_cell, current_hero);
        }

        if (current_inventory_cell == null)
        {
            SelectNextInventoryCell();
        }

        return used;
    }
    #endregion Inventory

    #region Red
    private void InstantiateHUD()
    {
        hud = Instantiate(hud);
        hud.name = "HUD";

        StartCoroutine(UpdateHUD(hud_update_rate));
    }

    private System.Collections.IEnumerator UpdateHUD(float rate)
    {
        while (true)
        {
            if (current_hero != null)
            {
                hud.UpdateHealth(current_hero.CurrentHealth(), current_hero.MaxHealth());

                hud.UpdateMana(current_hero.CurrentMana(), current_hero.MaxMana());

                hud.UpdateWeapon(current_hero.current_weapon);

                hud.UpdateItem(current_inventory_cell);
            }

            yield return new WaitForSeconds(rate);
        }
    }

    private void TeleportHero(HeroController hero, Vector2 destination)
    {
        if(hero!=null)
        {
            hero.transform.position = destination;
        }
    }

    private void ResurrectHero(float hp)
    {
        current_hero.Resurrect(hp);
        TeleportHero(current_hero, checkpoint_position);
    }

    private bool CanChangeHero()
    {
        return
            Time.timeScale==GameData.Time.normal_time_scale
            &&
            !current_hero.IsImmune()
            &&
            !current_hero.IsInvisible()
        ;
    }

    private void InstantiateHeroes()
    {
        GameObject heroes_group = new GameObject(HEROES_GROUP);
        heroes_group.transform.SetParent(transform, true);

        foreach (GameObject go in heroes_on_spawn)
        {
            GameObject instance_gameobject = __General.InstantiatePrefab(go, go.name, Vector3.zero, Quaternion.Euler(Vector3.zero), transform, true);
            instance_gameobject.transform.SetParent(heroes_group.transform, true);
            HeroController instance_herocontroller = instance_gameobject.GetComponent<HeroController>();
            available_heroes.Add(instance_herocontroller);
        }

        SelectNextHero();
    }

    private void SelectNextHero()
    {
        int next_hero_index = 0;

        if (current_hero != null)
        {
            int current_hero_index = available_heroes.IndexOf(current_hero);

            if (current_hero_index != -1 && current_hero_index != available_heroes.Count - 1)
            {
                next_hero_index = current_hero_index + 1;
            }
        }

        HeroController next_hero = available_heroes[next_hero_index];

        Vector2 hero_position;

        if(current_hero==null)
        {
            hero_position = transform.position;
        }
        else
        {
            hero_position = current_hero.transform.position;

            if (next_hero.ForwardDirection() != current_hero.ForwardDirection())
            {
                next_hero.TurnAround();
            }

            current_hero.gameObject.SetActive(false);
        }

        TeleportHero(next_hero, hero_position);
        next_hero.gameObject.SetActive(true);
        current_hero = next_hero;
    }

    private void PauseMenu()
    {
        Instantiate(pause_menu);
    }

    private void LookInput(ref float userinput_look, string axis_name, float accuracy, float min_value, float max_value)
    {
        bool modified = false;

        float look = Input.GetAxis(axis_name);

        if (look != 0)
        {
            userinput_look += look * Time.deltaTime;
            modified = true;
        }
        else
        {
            // Decreasing to zero
            if (Math.Abs(userinput_look) > accuracy)
            {
                userinput_look -= Math.Sign(userinput_look) * Time.deltaTime;
                modified = true;
            }
        }
        if (modified)
        {
            userinput_look = Math.Max(min_value, userinput_look);
            userinput_look = Math.Min(max_value, userinput_look);
        }
    }

    public void UserInput()
    {
        LookInput(ref userinput_vertical_look, "VerticalLook", 0.05f, -0.35f, 0.35f);

        userinput_submit = Input.GetButtonDown("Submit");
        userinput_use = Input.GetButtonDown("Use");
        userinput_cancel = Input.GetButtonDown("Cancel");
        userinput_menu = Input.GetButtonDown("Menu");

        userinput_change_character = Input.GetButtonDown("ChangeCharacter");
        userinput_change_weapon = Input.GetButtonDown("ChangeWeapon");

        float axis_value_change_item = Input.GetAxis("ChangeItem");

        userinput_change_item = 0;

        if (axis_value_change_item==0)
        {
            userinput_change_item_axis_is_in_use = false;
        }
        else
        {
            if (!userinput_change_item_axis_is_in_use)
            {
                userinput_change_item = System.Math.Sign(axis_value_change_item);
                
                userinput_change_item_axis_is_in_use = true;
            }
        }
    }

    public void EntityControl()
    {
        if (GameData.Time.game_paused)
        {
            return;
        }

        if (userinput_menu)
        {
            PauseMenu();
            return;
        }

        if (current_hero == null)
        {
            return;
        }

        if (current_hero.IsDead())
        {
            if (userinput_submit)
            {
                ResurrectHero(current_hero.initial_hp);
            }
        }
        else
        {
            if (userinput_change_character && CanChangeHero())
            {
                SelectNextHero();
            }

            if (userinput_change_weapon)
            {
                current_hero.SelectNextWeapon();
            }

            if(userinput_change_item!=0)
            {
                SelectNextInventoryCell();
            }

            if(userinput_use)
            {
                UseCurrentItemInCell();
            }
        }
    }

    public void SetCheckPoint(Vector2 v_checkpoint_position)
    {
        checkpoint_position = v_checkpoint_position;
    }

    public Vector2 HeroPosition()
    {
        Vector2 ret;

        if (current_hero == null)
        {
            ret = transform.position;
        }
        else
        {
            ret = current_hero.Position();
        }

        return ret;
    }

    public Vector2 HeroForwardDirection()
    {
        Vector2 ret;

        if (current_hero == null)
        {
            ret = Vector2.right;
        }
        else
        {
            ret = current_hero.ForwardDirection();
        }

        return ret;
    }

    public Vector2 HeroVelocity()
    {
        Vector2 ret;

        if (current_hero == null)
        {
            ret = Vector2.zero;
        }
        else
        {
            ret = current_hero.Velocity();
        }

        return ret;
    }

    public Vector2 HeroLook()
    {
        return new Vector2(userinput_horizontal_look, userinput_vertical_look);
    }
    #endregion Red
}
