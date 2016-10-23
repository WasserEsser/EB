using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteroidsBlitzcrank
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using Microsoft.Win32;

    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static AIHeroClient User = Player.Instance;

        private static Spell.Skillshot Q;
        private static Spell.Active W;
        private static Spell.Active E;
        private static Spell.Active R;

        private static Menu BlitzcrankMenu, ComboMenu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (User.ChampionName != "Blitzcrank")
            {
                return;
            }
            Q = new Spell.Skillshot(spellSlot: SpellSlot.Q, spellRange: 925, skillShotType: SkillShotType.Linear, castDelay: 220, spellSpeed: 1750, spellWidth: 70)
            { AllowedCollisionCount = 0 };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 150);
            R = new Spell.Active(SpellSlot.R, 600);


            BlitzcrankMenu = MainMenu.AddMenu("SteroidsBlitzcrank", "SteroidsBlitzcrank");
            ComboMenu = BlitzcrankMenu.AddSubMenu("Combo");

            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("R", new CheckBox("Use R"));

            Game.OnTick += Game_OnTick;
        }
        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

        if (target == null)
            {
                return;
            }
            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                var Qpred = Q.GetPrediction(target);
                if (target.IsValidTarget(Q.Range) && Q.IsReady() && Qpred.HitChance >= HitChance.High)
                {
                    Q.Cast(target);
                    {
                        if (ComboMenu["W"].Cast<CheckBox>().CurrentValue)
                        {
                            W.Cast();
                            {
                                if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
                                    if (target.IsValidTarget(E.Range) && E.IsReady())
                                    {
                                        E.Cast();
                                        {
                                            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
                                                if (target.IsValidTarget(R.Range) && R.IsReady())
                                                {
                                                    R.Cast();
                                                }
                                        }
                                    }
                            }
                        }
                    }
                }
            }
        }
    }
}
