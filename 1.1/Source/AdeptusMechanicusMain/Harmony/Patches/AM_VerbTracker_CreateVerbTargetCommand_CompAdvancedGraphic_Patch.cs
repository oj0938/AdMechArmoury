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
using UnityEngine;
using AdeptusMechanicus.ExtensionMethods;
using System.Reflection;

namespace AdeptusMechanicus.HarmonyInstance
{
    /*
    [HarmonyPatch(typeof(VerbTracker), "CreateVerbTargetCommand")]
    public static class AM_VerbTracker_CreateVerbTargetCommand_CompAdvancedGraphic_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(VerbTracker __instance, Thing ownerThing, Verb verb, ref Command_VerbTarget __result)
        {
            if (__instance != null)
            {
                CompAdvancedGraphic advancedWeaponGraphic = ownerThing.TryGetComp<CompAdvancedGraphic>();
                if (advancedWeaponGraphic!=null)
                {
                    __result.icon = ownerThing.Graphic.MatSingleFor(ownerThing).mainTexture as Texture2D;
                    // __result.IconDrawColor = ownerThing.Graphic.MatSingleFor(ownerThing).mainTexture as Texture2D;
                }
                if (true)
                {
                    __result.icon = ownerThing.Graphic.MatSingleFor(ownerThing).mainTexture as Texture2D;
                }
            }
        }
    }
    */
}