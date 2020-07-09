﻿using AdeptusMechanicus.ExtensionMethods;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    public class CompProperties_Weapon_GunSpecialRules : CompProperties_WargearWeapon
    {
        public CompProperties_Weapon_GunSpecialRules()
        {
            this.compClass = typeof(CompWeapon_GunSpecialRules);
        }
        public bool HeavyWeapon = false;
        public float HeavyWeaponSetupTime = 4f;
        public List<GunVerbEntry> VerbEntries;
        public bool TyranidBurstBodySize = false;
    }

    public class CompWeapon_GunSpecialRules : CompWargearWeapon
    {
        public new CompProperties_Weapon_GunSpecialRules Props => (CompProperties_Weapon_GunSpecialRules)props;

        public List<GunVerbEntry> VerbEntries => Props.VerbEntries;
        public List<GunVerbEntry> gunVerbs;
        public List<GunVerbEntry> GunVerbs
        {
            get
            {
                if (this.gunVerbs.NullOrEmpty())
                {
                    this.gunVerbs = new List<GunVerbEntry>();
                }
                foreach (VerbProperties verb in this.parent.def.Verbs)
                {
                    int index = this.parent.def.Verbs.IndexOf(verb);

                    if (VerbEntries[index] == null)
                    {
                        VerbEntries.Add(new GunVerbEntry());
                    }
                    if (VerbEntries[index].VerbProps==null)
                    {
                        VerbEntries[index].VerbProps = verb;
                    }
                    gunVerbs.Add(VerbEntries[index]);
                }
                return gunVerbs;
            }
        }

        public CompToggleFireMode fireMode => this.parent.TryGetComp<CompToggleFireMode>();
        public int CurMode => fireMode != null ? fireMode.fireMode : 0;
        public bool HeavyWeapon => Props.HeavyWeapon;
        public bool TyranidBurstBodySize => Props.TyranidBurstBodySize;
        public bool TwinLinked => Props.VerbEntries[CurMode].TwinLinked;
        public bool RapidFire => Props.VerbEntries[CurMode].RapidFire;
        public bool GetsHot => Props.VerbEntries[CurMode].GetsHot;
        public bool HotDamageWeapon => Props.VerbEntries[CurMode].HotDamageWeapon;
        public bool GetsHotCrit => Props.VerbEntries[CurMode].GetsHotCrit;
        public bool GetsHotCritExplosion => Props.VerbEntries[CurMode].GetsHotCritExplosion;
        public bool Jams => Props.VerbEntries[CurMode].Jams;
        public bool JamsDamageWeapon => Props.VerbEntries[CurMode].JamsDamageWeapon;
        public bool Multishot => Props.VerbEntries[CurMode].Multishot;
        public bool EffectsUser => Props.VerbEntries[CurMode].EffectsUser;
        public bool Rending => Props.VerbEntries[CurMode].Rending;
        public Reliability reliability =>  Props.VerbEntries[CurMode].reliability;
        public float HeavyWeaponSetupTime => Props.HeavyWeaponSetupTime;
        public float HotDamage => Props.VerbEntries[CurMode].HotDamage;
        public float GetsHotCritChance => Props.VerbEntries[CurMode].GetsHotCritChance;
        public float GetsHotCritExplosionChance => Props.VerbEntries[CurMode].GetsHotCritExplosionChance;
        public float JamDamage => Props.VerbEntries[CurMode].JamDamage;
        public float EffectsUserChance => Props.VerbEntries[CurMode].EffectsUserChance;
        public float RendingChance => Props.VerbEntries[CurMode].RendingChance;
        public StatDef ResistEffectStat => Props.VerbEntries[CurMode].ResistEffectStat;
        public HediffDef UserEffect=> Props.VerbEntries[CurMode].UserEffect;
        public List<string> UserEffectImmuneList =>  Props.VerbEntries[CurMode].UserEffectImmuneList;
        public ResearchProjectDef requiredResearch => Props.VerbEntries[CurMode].requiredResearch;
        public int ScattershotCount => Props.VerbEntries[CurMode].ScattershotCount;
        public bool MeltaWeapon => fireMode != null ? Props.VerbEntries[fireMode.fireMode].VerbProps.defaultProjectile.projectile.Melta() : this.parent.def.Verbs[0].defaultProjectile.projectile.Melta();
        public bool VolkiteWeapon => fireMode != null ? Props.VerbEntries[fireMode.fireMode].VerbProps.defaultProjectile.projectile.Volkite() : this.parent.def.Verbs[0].defaultProjectile.projectile.Volkite();
        public bool GaussWeapon => fireMode != null ? Props.VerbEntries[fireMode.fireMode].VerbProps.defaultProjectile.projectile.Gauss() : this.parent.def.Verbs[0].defaultProjectile.projectile.Gauss();
        public bool HaywireWeapon => fireMode != null ? Props.VerbEntries[fireMode.fireMode].VerbProps.defaultProjectile.projectile.Haywire() : this.parent.def.Verbs[0].defaultProjectile.projectile.Haywire();
        public bool TeslaWeapon => fireMode != null ? Props.VerbEntries[fireMode.fireMode].VerbProps.defaultProjectile.projectile.Tesla() : this.parent.def.Verbs[0].defaultProjectile.projectile.Tesla();
        public bool ConversionWeapon => fireMode != null ? Props.VerbEntries[fireMode.fireMode].VerbProps.defaultProjectile.projectile.Conversion() : this.parent.def.Verbs[0].defaultProjectile.projectile.Conversion();
        public int LastMovedTick
        {
            get
            {

                if (CasterPawn == null)
                {
                    return 0;
                }
                if (!CasterPawn.pather.MovedRecently(HeavyWeaponSetupTime.SecondsToTicks()))
                {
                    return int.MaxValue;
                }
                traverse = Traverse.Create(CasterPawn.pather);
                lastmovedTick = (int)lastMovedTick.GetValue(CasterPawn.pather);

                return lastmovedTick;
            }
        }
        public int ticksHere
        {
            get
            {
                return Find.TickManager.TicksGame - LastMovedTick;
            }
        }
        Traverse traverse;
        public int lastmovedTick;
        public static FieldInfo lastMovedTick = typeof(Pawn_PathFollower).GetField("lastMovedTick", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

        public override void CompTick()
        {
            base.CompTick();
            if (CasterPawn != lastWearer)
            {
                lastWearer = CasterPawn;
            }
        }


        public Reliability Reliability
        {
            get
            {
                return this.reliability;
            }
        }

        public override string CompInspectStringExtra()
        {
            string str = "Special Rules:";
            string str2 = string.Empty;
            if (fireMode!=null)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Fire Modes" : str2 + ", Fire Modes";
            }
            if (EffectsUser)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Affects User" : str2 + ", Affects User";
            }
            if (RapidFire)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Rapid Fire" : str2 + ", Rapid Fire";
            }
            if (GetsHot)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Gets Hot" : str2 + ", Gets Hot";
            }
            else
            if (Jams)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Jams" : str2 + ", Jams";
            }
            if (TwinLinked)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Twin-Linked" : str2 + ", Twin-Linked";
            }
            if (Multishot)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Scattershot" : str2 + ", Scattershot";
            }
            if (Rending)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Rending Weapon" : str2 + ", Rending Weapon";
            }
            if (MeltaWeapon)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Melta Weapon" : str2 + ", Melta Weapon";
            }
            if (VolkiteWeapon)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Volkite Weapon" : str2 + ", Volkite Weapon";
            }
            if (ConversionWeapon)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Conversion Weapon" : str2 + ", Conversion Weapon";
            }
            if (HaywireWeapon)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Haywire Weapon" : str2 + ", Haywire Weapon";
            }
            /*
            if (GaussWeapon)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Gauss Weapon" : str2 + ", Gauss Weapon";
            }
            */
            if (TeslaWeapon)
            {
                str2 = str2.NullOrEmpty() ? str2 + " Tesla Weapon" : str2 + ", Tesla Weapon";
            }
            return str2.NullOrEmpty() ? null : str + str2;
        }

        public override string GetDescriptionPart()
        {

            string str = string.Empty;
            str = str + "Special Rules:";
            if (fireMode!=null)
            {
                str = str + string.Format("\nFire Modes: ");
                foreach (VerbProperties mode in parent.def.Verbs)
                {
                    if (parent.def.Verbs.IndexOf(mode) == 0)
                    {
                        str = str + string.Format("\n Primary: {0}", mode.label ?? mode.defaultProjectile.label);
                    }
                    if (parent.def.Verbs.IndexOf(mode) != 0)
                    {
                        str = str + string.Format("\n Secondry: {0}", mode.label ?? mode.defaultProjectile.label);
                    }
                }
                str = str + string.Format("\n Current Mode: {0} \n", fireMode.Active.label ?? fireMode.Active.defaultProjectile.label);
            }
            if (RapidFire)
            {
                float reductionbase = ((this.GunVerbs[this.CurMode].VerbProps.burstShotCount - 1) * this.GunVerbs[this.CurMode].VerbProps.ticksBetweenBurstShots).TicksToSeconds() / 4;
                float warmup = this.GunVerbs[this.CurMode].VerbProps.warmupTime;
                float cooldown = parent.GetStatValue(StatDefOf.RangedWeapon_Cooldown);
                float Cycle = cooldown + warmup + (reductionbase * 4);
                float warmupreduction = (warmup / 2) + reductionbase;
                float cooldownreduction = (cooldown / 2) + reductionbase;
                float warmupReduction = (warmupreduction / warmup) * 100;
                float cooldownReduction = (cooldownreduction / cooldown) * 100;
                float newCycle = (cooldown - cooldownreduction) + (warmup - warmupreduction) + (reductionbase * 4);
                str = str + string.Format("\n RapidFire: Full Firing Cycle time reduced from {5} to {6} when firing at targets within {4} cells.\nWarmup reduced by {0}% ({1} seconds) and Cooldown by {2}% ({3} seconds).\n", warmupReduction.ToStringByStyle(ToStringStyle.FloatMaxOne), warmup - warmupreduction, cooldownReduction.ToStringByStyle(ToStringStyle.FloatMaxOne), cooldown - cooldownreduction, compEquippable.VerbTracker.PrimaryVerb.verbProps.range/2, Cycle, newCycle);
            }
            if (GetsHot)
            {
                string reliabilityString;
                float failChance;
                StatPart_Reliability.GetReliability(this, out reliabilityString, out failChance);
                str = str + string.Format("\n Gets Hot: This {0} is {1} and has a {2} chance to overheat per shot fired.",parent.Label, reliabilityString, (failChance/100).ToStringPercent());
                if (HotDamageWeapon)
                {
                    str = str + string.Format(" Overheats cause {0} damage to the {1}.",HotDamage, parent.def.label);
                }
                if (GetsHotCrit)
                {
                    str = str + string.Format("it has a {0} chance to critically overheat, causing more damage to user and weapon.",(GetsHotCritChance/100).ToStringPercent());
                    if (GetsHotCritExplosion)
                    {
                        str = str + string.Format("Critical overheats have a {0} chance of cuasing the weapon to explode.", (GetsHotCritExplosionChance/100).ToStringPercent());
                    }
                }
                str = str + "\n";
            }
            if (Jams)
            {
                string reliabilityString;
                float failChance;
                StatPart_Reliability.GetReliability(this, out reliabilityString, out failChance);
                str = str + string.Format("\n Jams: This {0} is {1} and has a {2} chance to jam per shot fired.", parent.Label, reliabilityString, (failChance/100).ToStringPercent());
                if (JamsDamageWeapon)
                {
                    str = str + string.Format(" Jamming causes {0} damage to the {1}.", JamDamage, parent.def.label);
                }
                str = str + "\n";
            }
            if (TwinLinked)
            {
                str = str + "\n Twin-Linked: Fires two projectiles per shot";
            }
            if (Multishot)
            {
                str = str + string.Format("\n Scatter-Shot: Fires {0} perjectiles per shot", ScattershotCount);
            }
            if (Rending)
            {
                str = str + string.Format("\n Rending: has a {0} chance to ignore all armour", RendingChance);
            }
            if (MeltaWeapon)
            {
                str = str + string.Format("\n Melta: Damage Vs Buildings and AP doubled when firing at targets within {0} cells. \n", compEquippable.VerbTracker.PrimaryVerb.verbProps.range / 2);
            }
            if (VolkiteWeapon)
            {
                str = str + string.Format("\n Volite: AP begins to drop off when firing at targets over {0} cells. \n", compEquippable.VerbTracker.PrimaryVerb.verbProps.range / 2);
            }
            if (ConversionWeapon)
            {
                str = str + string.Format("\n Conversion: Damage and AP increase the further the target is away. \n");
            }
            if (HaywireWeapon)
            {
                str = str + "\n Haywire Weapon";
                str = str + string.Format("\n Haywire: additional EMP damage. \n");
            }
            /*
            if (GaussWeapon)
            {
                str = str + "\n Gauss Weapon";
                str = str + string.Format("\n Conversion: Damage and AP increase the further the target is away. \n");
            }
            */
            if (TeslaWeapon)
            {
                str = str + "\n Tesla Weapon";
                str = str + string.Format("\n Tesla: Bolt can arc to up to 3 nearby targets. \n");
            }
            return str;
            return base.GetDescriptionPart();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
        }

    }
}
