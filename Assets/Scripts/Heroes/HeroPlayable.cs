using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HeroPlayable : MonoBehaviour
{
    const int ACTIVE_SKILLS_COUNT = 3;

    #region Variables
    [HideInInspector]
    public HeroController hero;

    [HideInInspector]
    public List<HeroSkill> active_skills;

    private HUD hud;

    #region UserInput
    private float userinput_x;
    private float userinput_y;
    private bool userinput_sprint;
    private bool userinput_jump;
    private bool userinput_skill1;
    private bool userinput_skill2;
    private bool userinput_skill3;
    #endregion UserInput
    #endregion Variables

    #region MonoBehaviour
    protected virtual void Awake()
    {
        hero = GetComponent<HeroController>();
    }

    protected virtual void Start()
    {
        InitializeActiveSkills();

        GameObject hud_gameobject = GameObject.Find("HUD");

        if (hud_gameobject != null)
        {
            hud = hud_gameobject.GetComponent<HUD>();
            hud.SetHeroReference(this);
            HUDUpdateActiveSkills();
        }
    }

    private void OnEnable()
    {
        HUDUpdateActiveSkills();
    }

    private void Update()
    {
        if (!GameData.Time.game_paused && !hero.IsDead())
        {
            UserInput();
            EntityControl();
        }
    }
    #endregion MonoBehaviour

    #region Red
    public virtual void UserInput()
    {
        userinput_x = Input.GetAxis("Horizontal");
        userinput_y = Input.GetAxis("Vertical");
        userinput_sprint = Input.GetAxis("Sprint") != 0;
        userinput_jump = Input.GetButtonDown("Jump");

        userinput_skill1 = Input.GetAxis("Skill1") != 0;
        userinput_skill2 = Input.GetAxis("Skill2") != 0;
        userinput_skill3 = Input.GetAxis("Skill3") != 0;
    }

    public void EntityControl()
    {
        if (userinput_x != 0 || userinput_y != 0)
        {
            hero.Move(userinput_x, userinput_y, userinput_sprint);
        }

        if (userinput_jump)
        {
            if (userinput_y == -1.0f)
            {
                hero.JumpDown();
            }
            else
            {
                hero.Jump();
            }
        }

        if (userinput_skill1)
        {
            UseActiveSkill(1);
        }

        if (userinput_skill2)
        {
            UseActiveSkill(2);
        }

        if (userinput_skill3)
        {
            UseActiveSkill(3);
        }
    }

    private void InitializeActiveSkills()
    {
        #region ActiveSkillsList
        active_skills = new List<HeroSkill>();

        int k = hero.available_skills.Count;

        if (k > ACTIVE_SKILLS_COUNT)
        {
            k = ACTIVE_SKILLS_COUNT;
        }

        if (k > 0)
        {
            active_skills.AddRange(hero.available_skills.Take(k));
        }
        #endregion ActiveSkillsList
    }

    private void UseActiveSkill(int index)
    {
        if (index > active_skills.Count)
        {
            return;
        }
        
        active_skills[index - 1].Use();
    }

    private void HUDUpdateActiveSkills()
    {
        if(hud!=null)
        {
            hud.UpdateActiveSkills();
        }
    }

    public void NextSkill(int skill_number)
    {
        if(skill_number> ACTIVE_SKILLS_COUNT || skill_number>active_skills.Count)
        {
            return;
        }

        skill_number--;

        int next_skill_index = 0;

        int current_skill_index = hero.available_skills.IndexOf(active_skills[skill_number]);

        if (current_skill_index != -1 && current_skill_index != hero.available_skills.Count - 1)
        {
            next_skill_index = current_skill_index + 1;
        }

        active_skills[skill_number] = hero.available_skills[next_skill_index];

        HUDUpdateActiveSkills();
    }
    #endregion Red
}
