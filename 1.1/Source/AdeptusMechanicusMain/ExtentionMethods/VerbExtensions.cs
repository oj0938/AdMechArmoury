﻿using System;
using System.Collections.Generic;
using System.Linq;
using AdeptusMechanicus.settings;
using RimWorld;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{

    public static class VerbExtensions
    {
        public static GunVerbEntry SpecialRules(this Verb verb)
        {
            GunVerbEntry entry = null;
            if (verb.verbProps.LaunchesProjectile)
            {
                if (verb.EquipmentSource != null)
                {
                    CompWeapon_GunSpecialRules GunExt = verb.EquipmentSource.TryGetComp<CompWeapon_GunSpecialRules>();
                    if (GunExt != null)
                    {
                        entry = GunExt.GunVerbs[GunExt.CurMode];
                    }
                }
                if (verb.HediffCompSource != null && !verb.IsMeleeAttack)
                {
                //    Log.Message("ownerVerb.HediffCompSource");
                    HediffComp_VerbGiverExtra _VGE = verb.HediffCompSource as HediffComp_VerbGiverExtra;
                    if (_VGE!=null)
                    {
                        entry = _VGE.Props.verbEntrys[_VGE.VerbProperties.IndexOf(verb.verbProps)] ?? entry;
                    }
                }
            }
            return entry;
        }

        public static bool RapidFire(this Verb verb, float num, out bool InRange, out float modifier)
        {
            modifier = num;
            InRange = false;
            bool result = verb.SpecialRules() != null && verb.SpecialRules().RapidFire && AMSettings.Instance.AllowRapidFire;
            if (result)
            {
                InRange = verb.SpecialRules().RapidFire && verb.CurrentTarget.Cell.InHorDistOf(verb.Caster.Position, verb.verbProps.range / 2);
                if (InRange)
                {
                    float reduction = ((verb.verbProps.burstShotCount - 1) * verb.verbProps.ticksBetweenBurstShots).TicksToSeconds() / 4;
                    reduction += num / 2;
                    modifier -= reduction;
                }
            }
            return result;
        }

        public static bool UserEffect(this Verb verb)
        {
            bool result = verb.SpecialRules() != null && verb.SpecialRules().EffectsUser && AMSettings.Instance.AllowUserEffects;
            return result;
        }
        
        public static bool UserEffect(this Verb verb, out float Chance, out HediffDef Effect, out StatDef ResistStat, out List<string> ImmuneList)
        {
            Chance = 0;
            Effect = null;
            ResistStat = null;
            ImmuneList = new List<string>();
            bool result = verb.SpecialRules() != null && verb.SpecialRules().EffectsUser && AMSettings.Instance.AllowUserEffects;
            if (result)
            {
                Effect = verb.SpecialRules().UserEffect;
                ResistStat = verb.SpecialRules().ResistEffectStat;
                Chance = verb.SpecialRules().EffectsUserChance;
                ImmuneList = verb.SpecialRules().UserEffectImmuneList;
                if (verb.SpecialRules().UserEffectImmuneList.Contains(verb.caster.def.defName) || verb.caster == null || !verb.CasterIsPawn)
                {
                    return false;
                }

            }
            return result;
        }

        public static bool TwinLinked(this Verb verb)
        {
            bool result = verb.SpecialRules() !=null && verb.SpecialRules().TwinLinked;
            return result;
        }

        public static bool MultiShot(this Verb verb)
        {
            bool result = verb.SpecialRules() != null && verb.SpecialRules().Multishot && AMSettings.Instance.AllowMultiShot;
            return result;
        }
        public static bool MultiShot(this Verb verb, out int Extras)
        {
            Extras = 0;
            bool result = verb.SpecialRules() != null && verb.SpecialRules().Multishot && AMSettings.Instance.AllowMultiShot;
            if (result)
            {
                Extras = verb.SpecialRules().ScattershotCount;
            }
            return result;
        }

        public static bool GetsHot(this Verb verb)
        {
            bool result = verb.SpecialRules() != null && verb.SpecialRules().GetsHot;
            return result;
        }

        public static bool GetsHot(this Verb verb,out bool GetsHotCrit, out float GetsHotCritChance, out bool GetsHotCritExplosion, out float GetsHotCritExplosionChance, out bool canDamageWeapon, out float extraWeaponDamage)
        {
            GunVerbEntry entry = verb.SpecialRules();
            bool GetsHot = false;
            if (entry == null || !AMSettings.Instance.AllowGetsHot)
            {
                GetsHotCrit = false;
                GetsHotCritChance = 0f;
                GetsHotCritExplosion = false;
                GetsHotCritExplosionChance = 0f;
                canDamageWeapon = false;
                extraWeaponDamage = 0f;
            //    Log.Message("no SpecialRules detected");
                return GetsHot;
            }
            GetsHot = entry.GetsHot;
            GetsHotCrit = entry.GetsHotCrit;
            GetsHotCritChance = entry.GetsHotCritChance;
            GetsHotCritExplosion = entry.GetsHotCritExplosion;
            GetsHotCritExplosionChance = entry.GetsHotCritExplosionChance;
            canDamageWeapon = entry.HotDamageWeapon;
            extraWeaponDamage = entry.HotDamage;
            return GetsHot;
        }

        public static bool Jams(this Verb verb)
        {
            bool result = verb.SpecialRules() != null && verb.SpecialRules().Jams;
            return result;
        }
        public static bool Jams(this Verb verb, out bool canDamageWeapon, out float extraWeaponDamage)
        {
            GunVerbEntry entry = verb.SpecialRules();
            bool Jams = false;
            if (entry == null || !AMSettings.Instance.AllowJams)
            {
                canDamageWeapon = false;
                extraWeaponDamage = 0f;
           //     Log.Message("no SpecialRules detected");
                return Jams;
            }
            Jams = entry.Jams;
            canDamageWeapon = entry.JamsDamageWeapon;
            extraWeaponDamage = entry.JamDamage;
            return Jams;
        }

        public static bool powerWeapon(this Verb verb)
        {
            return verb.GetDamageDef().powerWeapon();
        }

        public static bool witchbladeWeapon(this Verb verb)
        {
            return verb.GetDamageDef().witchbladeWeapon();
        }

        public static bool forceWeapon(this Verb verb)
        {
            return verb.GetDamageDef().forceWeapon();
        }

        public static bool rendingWeapon(this Verb verb)
        {
            return verb.GetDamageDef().rendingWeapon();
        }

    }

}
