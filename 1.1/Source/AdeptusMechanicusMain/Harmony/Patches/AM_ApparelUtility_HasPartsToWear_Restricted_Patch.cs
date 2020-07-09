﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    /*
    [HarmonyPatch(typeof(ApparelUtility), "HasPartsToWear")]
    public static class AM_ApparelUtility_HasPartsToWear_Restricted_Patch
    {
        [HarmonyPrefix]
        public static void Pre_(Pawn p, ThingDef apparel, ref bool __result)
        {
            if (apparel.HasModExtension<ApparelRestrictionDefExtension>())
            {

            }
        }

        [HarmonyPostfix]
        public static void Post_(Pawn p, ThingDef apparel, ref bool __result)
        {
            if (apparel.HasModExtension<ApparelRestrictionDefExtension>())
            {
            //    __result = false;

                ApparelRestrictionDefExtension defExtension = apparel.GetModExtension<ApparelRestrictionDefExtension>();
                if (defExtension!=null)
                {
                    bool gender = defExtension.gender == Gender.None || p.gender == defExtension.gender;
                    bool race = false;
                    bool hediff = false;
                    bool trait = false;
                    if (!defExtension.RaceDefs.NullOrEmpty())
                    {
                        race = defExtension.RaceDefs.Contains(p.def);
                    }
                    if (!defExtension.HediffDefs.NullOrEmpty())
                    {
                        hediff = p.health.hediffSet.hediffs.Any(x => defExtension.HediffDefs.Contains(x.def));
                    }
                    if (!defExtension.TraitDefs.NullOrEmpty())
                    {
                        trait = p.story.traits.allTraits.Any(x => defExtension.TraitDefs.Contains(x.def));
                    }
                    __result = gender && (race || hediff || trait);
                }
            }
        }
    }
    */
}
