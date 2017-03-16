using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZzukBot.Constants;
using ZzukBot.ExtensionFramework.Classes;
using ZzukBot.Game.Statics;
using ZzukBot.Objects;

//I can't seem to get OnBuff() to work, just putting the buffs to be maintained into DefensiveSpells for now.

public class ShadyPriest : CustomClass
{
	bool useTouchOfWeakness = true;
	int healthP = 65;
	bool useShadowForm = true;

    public override void Dispose() { }
    public override bool Load() { return true;  }
    public override bool OnBuff() 
        {
		return true; 
        }


    public string[] drinkNames = {"Refreshing Spring Water", "Ice Cold Milk",
            "Melon Juice", "Moonberry Juice",
            "Sweet Nectar", "Morning Glory Dew", "Conjured Purified Water",
            "Conjured Spring Water", "Conjured Mineral Water", "Conjured Sparkling Water",
            "Conjured Crystal Water" };

    public void SelectDrink()
        {
            if (Inventory.Instance.GetItemCount("Morning Glory Dew") != 0)
                Local.Drink(drinkNames[5]);
            else if (Inventory.Instance.GetItemCount("Sweet Nectar") != 0)
                Local.Drink(drinkNames[4]);
            else if (Inventory.Instance.GetItemCount("Moonberry Juice") != 0)
                Local.Drink(drinkNames[3]);
            else if (Inventory.Instance.GetItemCount("Melon Juice") != 0)
                Local.Drink(drinkNames[2]);
            else if (Inventory.Instance.GetItemCount("Ice Cold Milk") != 0)
                Local.Drink(drinkNames[1]);
            else if (Inventory.Instance.GetItemCount("Refreshing Spring Water") != 0)
                Local.Drink(drinkNames[0]);
            else if (Inventory.Instance.GetItemCount("Conjured Purified Water") != 0)
                Local.Drink(drinkNames[6]);
            else if (Inventory.Instance.GetItemCount("Conjured Spring Water") != 0)
                Local.Drink(drinkNames[7]);
            else if (Inventory.Instance.GetItemCount("Conjured Mineral Water") != 0)
                Local.Drink(drinkNames[8]);
            else if (Inventory.Instance.GetItemCount("Conjured Sparkling Water") != 0)
                Local.Drink(drinkNames[9]);
            else if (Inventory.Instance.GetItemCount("Conjured Crystal Water") != 0)
                Local.Drink(drinkNames[10]);        
            }

    public void PullPriority()
       {

            if (Spell.Instance.GetSpellRank("Mind Blast") != 0 && Spell.Instance.GetSpellRank("Power Word: Shield") != 0)
            {
                if (Spell.Instance.IsSpellReady("Mind Blast"))
                {
                    if (Spell.Instance.IsSpellReady("Power Word Shield") && !Local.GotDebuff("Weakened Soul"))
                    {
                        Spell.Instance.Cast("Power Word: Shield");
                    }

                    Spell.Instance.Cast("Mind Blast");
                }
            }
            else if (Spell.Instance.GetSpellRank("Shadow Word: Pain") != 0)
            {
                if (Spell.Instance.IsSpellReady("Power Word Shield") && !Local.GotDebuff("Weakened Soul"))
                    {
                        Spell.Instance.Cast("Power Word: Shield");
                    }

                if (!Target.GotDebuff("Shadow Word: Pain"))
                {
                    Spell.Instance.Cast("Shadow Word: Pain");
                }
            }
            else
            {   // to ensure this works from level 1.
                Spell.Instance.Cast("Smite");
            }
        }

    public void GotWand()
    {
        if(useShadowForm)
        {
            if(Local.ManaPercent < 10 || Target.HealthPercent < healthP)
            {
                
                    Spell.Instance.StartWand();
               
            }
        }
    }

    public void NoWand()
    {
        if (Local.ManaPercent > 60)
        {
            Spell.Instance.Cast("Smite");
        }
        else if (Target.IsFleeing)
        {
            Spell.Instance.Cast("Smite");
        }
        else
        {

            Spell.Instance.Attack();
        }
    }

    public void OffensiveSpells()
    {
        //PWS 
        if(!Local.GotAura("Power Word: Shield") && !Local.GotDebuff("Weakened Soul") && Spell.Instance.GetSpellRank("Power Word: Shield") != 0 && Spell.Instance.IsSpellReady("Power Word:Shield"))
        {
            Spell.Instance.Cast("Power Word: Shield");
        }

        if(!Local.GotAura("Shadowform") && Spell.Instance.GetSpellRank("Shadowform") != 0)
        {
            Spell.Instance.Cast("Shadowform");
        }

        if(Spell.Instance.GetSpellRank("Shadow Word: Pain") != 0 && Target.HealthPercent >= 5 && Local.ManaPercent >= 10)
        {
            if(!Target.GotDebuff("Shadow Word: Pain"))
            {
                Spell.Instance.Cast("Shadow Word: Pain");
            }
        }

        if(Target.HealthPercent >= 95 && Spell.Instance.GetSpellRank("Mind Blast") != 0 && Spell.Instance.IsSpellReady("Mind Blast"))
        {
            Spell.Instance.Cast("Mind Blast");
        }

        if(Spell.Instance.GetSpellRank("Mind Flay") != 0)
        {
            if(Spell.Instance.IsSpellReady("Mind Flay") && Target.HealthPercent >= healthP && Local.ManaPercent >= 10)
            {
				Spell.Instance.CastWait("Mind Flay", 2500);
            }
        }
    }

    public void DefensiveSpells()
    {
        if((!Local.GotAura("Power Word: Shield") && !Local.GotDebuff("Weakened Soul")) || Local.HealthPercent <= 45)
        {
            if(Spell.Instance.GetSpellRank("Power Word: Shield") != 0)
            {
                if(!Local.GotAura("Power Word: Shield") && !Local.GotDebuff("Weakened Soul"))
                {
                    Spell.Instance.Cast("Power Word: Shield");
                }
            }
        }

		if (Spell.Instance.GetSpellRank("Inner Fire") != 0)
		{
			if (!Local.GotAura("Inner Fire"))
			{
				Spell.Instance.Cast("Inner Fire");
				
			}
		}
		if (Spell.Instance.GetSpellRank("Power Word: Fortitude") != 0)
		{
			if (!Local.GotAura("Power Word: Fortitude"))
			{
				Spell.Instance.Cast("Power Word: Fortitude");
				
			}
		}
		if (Spell.Instance.GetSpellRank("Shadowform") != 0 && !Local.GotAura("Shadowform"))
		{
			Spell.Instance.Cast("Shadowform");
			
		}

		if (Local.HealthPercent <= 45 && Local.ManaPercent >= 40)
        {
            if(Local.GotAura("Shadowform"))
                {
                    Spell.Instance.Cast("Shadowform");
                }

            if(Spell.Instance.GetSpellRank("Flash Heal") != 0 && Local.ManaPercent >= 55)
            {
                Spell.Instance.CastWait("Flash Heal", 1500);
            }
            else if (Spell.Instance.GetSpellRank("Heal") != 0)
            {
                Spell.Instance.CastWait("Heal", 1500);
            }
            else
            {
                Spell.Instance.CastWait("Lesser Heal", 1500);
            }
        }
    }

    
    public override void OnFight()
    {
        bool canWand = Local.IsWandEquipped();
        DefensiveSpells();
        OffensiveSpells();

        
        if (canWand && Spell.Instance.IsSpellReady("Shadow Word: Pain"))
        {
			
            GotWand();
        }

        if (!canWand)
        {
            NoWand();
        }
    }

    public override void OnPull()
    {
		PullPriority();
    }

    public override void OnRest()
    {   //According to v3 api, Drink() Method checks whether drinking is already taking place. No more checks needed?
        if(Local.ManaPercent < 20)
            SelectDrink();
            return;
    }

    public override void ShowGui() {}
    public override void Unload() {}

    public WoWUnit Target {get{return ObjectManager.Instance.Target;}}
    public LocalPlayer Local {get{return ObjectManager.Instance.Player;}}

    public override string Author {get{return "sensgates";}}
    public override string Name {get{return "ShadyPriest";}}
    public override int Version {get{return 1;}}
    public override Enums.ClassId Class {get {return Enums.ClassId.Priest;}}
    public override bool SuppressBotMovement {get{return false;}}
    public override float CombatDistance {get{return 25.0f;}}
}