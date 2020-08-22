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
using System.Text.RegularExpressions;

namespace AdeptusMechanicus.HarmonyInstance
{

    [HarmonyPatch(typeof(BackCompatibility), "BackCompatibleDefName")]
    public static class BackCompatibility_BackCompatibleDefName_Patch
    {
        [HarmonyPostfix]
        public static void BackCompatibleDefName_Postfix(Type defType, string defName, bool forDefInjections, ref string __result)
        {
            if (GenDefDatabase.GetDefSilentFail(defType, defName, false) == null)
            {
                string newName = string.Empty;
            //    Log.Message(string.Format("Checking for replacement for {0} Type: {1}", defName, defType));
                if (defType == typeof(ThingDef))
                {
                    if (defName == "OGAM_Apparel_SkitariiLegionnaireHelmet")
                    {
                        newName = "OGAM_Apparel_SkitariiLegionnaireHelmet_TOGGLEDEF_Hooded";
                    }
                    else
                    if (defName == "OGAM_Apparel_SkitariiPrimusHelmet")
                    {
                        newName = "OGAM_Apparel_SkitariiPrimusHelmet_TOGGLEDEF_Unhooded";
                    }
                    else
                    if (defName.Contains("OGT_Gun_TKroot"))
                    {
                        newName = Regex.Replace(defName, "OGT_Gun_TKroot", "OGK_Gun_Kroot");
                    }
                    else
                    if (defName.Contains("Alien_Kroot"))
                    {
                        newName = "OG_Alien_Kroot";
                    }
                    else
                    if (defName.Contains("Alien_Tau"))
                    {
                        newName ="OG_Alien_Tau";
                    }
                    else
                    if (defName.Contains("Alien_Ork"))
                    {
                        newName ="OG_Alien_Ork";
                    }
                    else
                    if (defName.Contains("Cybork"))
                    {
                        newName ="OG_Alien_Cybork";
                    }
                    else
                    if (defName.Contains("Alien_Cybork"))
                    {
                        newName ="OG_Alien_Cybork";
                    }
                    else
                    if (defName.Contains("Alien_Grot"))
                    {
                        newName ="OG_Alien_Grot";
                    }
                    else
                    if (defName.Contains("Alien_Eldar"))
                    {
                        newName ="OG_Alien_Eldar";
                    }
                    else
                    if (defName.Contains("AttackSquig"))
                    {
                        newName ="OG_Squig_Ork";
                    }
                    else
                    if (defName.Contains("Squig"))
                    {
                        newName ="OG_Squig";
                    }
                    else
                    if (defName.Contains("Snotling"))
                    {
                        newName ="OG_Ork_Snotling";
                    }


                    if (defName == "OG_Human_Imperial" || defName == "OG_Human_ELT")
                    {
                        newName = "Human";
                    }
                    if (defName == "Corpse_OG_Human_Imperial" || defName == "Corpse_OG_Human_ELT")
                    {
                        newName = "Corpse_Human";
                    }
                    if (defName == "Tau_Kroot_Warrior")
                    {
                        newName = "OG_Alien_Kroot";
                    }
                    if (defName == "Corpse_Tau_Kroot_Warrior")
                    {
                        newName = "Corpse_OG_Alien_Kroot";
                    }

                    if (defName != __result)
                    {
                        if (defName.Contains("Corpse"))
                        {
                            newName ="Corpse_" + __result;
                        }
                        if (defName.Contains("Meat"))
                        {
                            if (defName.Contains("Cyborg_Ork"))
                            {
                                newName ="Meat_OG_Alien_Ork";
                            }
                            else
                            if (defName.Contains("Squig"))
                            {
                                newName ="Meat_OG_Squig";
                            }
                            else
                            newName ="Meat_" + __result;
                        }
                    }
                    if (defName.Contains("OrkGrog"))
                    {
                        newName ="OG_Ork_Grog";
                    }
                    if (defName.Contains("Plant_OrkFungus"))
                    {
                        newName ="OG_Plant_OrkoidFungus";
                    }
                    if (defName.Contains("Meat_Cyborg_Ork"))
                    {
                        newName ="Meat_OG_Alien_Ork";
                    }
                    if (defName.Contains("CadianFlakArmour"))
                    {
                        newName ="OGIG_Apparel_FlakArmour";
                    }
                    if (defName.Contains("Rosarius"))
                    {
                        newName =DefDatabase<ThingDef>.AllDefs.Where(x => x.defName.Contains("Rosarius")).ToList().First().defName;
                    }
                    if (defName.Contains("IronHalo"))
                    {
                        newName =DefDatabase<ThingDef>.AllDefs.Where(x => x.defName.Contains("IronHalo")).ToList().First().defName;
                    }
                    if (defName.Contains("PuritySealA"))
                    {
                        newName =DefDatabase<ThingDef>.AllDefs.Where(x => x.defName.Contains("PuritySealA")).ToList().First().defName;
                    }
                    if (defName.Contains("Apparel_TribalKroot"))
                    {
                        newName ="OGK_Apparel_TribalKroot";
                    }
                    
                    if (defName.Contains("OGT_CombatHelmet"))
                    {
                        newName ="OGT_Apparel_CombatHelmet";
                    }
                    if (defName.Contains("OGT_CombatArmour"))
                    {
                        newName ="OGT_Apparel_CombatArmour";
                    }
                    if (defName.Contains("OGE_Apparel_RuneArmour"))
                    {
                        newName ="OGE_Apparel_RuneArmourWarlock";
                    }
                    if (defName.Contains("OGCDWarpRift"))
                    {
                        newName =DefDatabase<ThingDef>.AllDefs.Where(x=> x.defName.Contains("Warp") && x.defName.Contains("Rift")).RandomElement().defName;
                    }

                    if (defName.Contains("OGC_Gun_C"))
                    {
                        newName = Regex.Replace(defName, "OGC_Gun_C", "OGC_Gun_");
                    }
                    if (defName.Contains("OGC_Melee_C"))
                    {
                        newName = Regex.Replace(defName, "OGC_Melee_C", "OGC_Melee_");
                    }

                    if (defName.Contains("OGE_Gun_E"))
                    {
                        newName = Regex.Replace(defName, "OGE_Gun_E", "OGE_Gun_");
                    }
                    if (defName.Contains("OGE_Melee_E"))
                    {
                        newName = Regex.Replace(defName, "OGE_Melee_E", "OGE_Melee_");
                    }

                    if (defName.Contains("OGN_Gun_N"))
                    {
                        newName = Regex.Replace(defName, "OGN_Gun_N", "OGN_Gun_");
                    }
                    if (defName.Contains("OGN_Melee_N"))
                    {
                        newName = Regex.Replace(defName, "OGN_Melee_N", "OGN_Melee_");
                    }

                    if (defName.Contains("OGO_Gun_O"))
                    {
                        newName = Regex.Replace(defName, "OGO_Gun_O", "OGO_Gun_");
                    }
                    if (defName.Contains("OGO_Melee_O"))
                    {
                        newName = Regex.Replace(defName, "OGO_Melee_O", "OGO_Melee_");
                    }

                    if (defName.Contains("OGT_Gun_T"))
                    {
                        newName = Regex.Replace(defName, "OGT_Gun_T", "OGT_Gun_");
                    }
                    if (defName.Contains("OGT_Melee_T"))
                    {
                        newName = Regex.Replace(defName, "OGT_Melee_T", "OGT_Melee_");
                    }
                    /*
                    if (defName.Contains("OGT_Gun_") && defName.Contains("NeutronBlaster"))
                    {

                    }
                    */
                }
                if (defType == typeof(FactionDef))
                {
                    if (defName == "OGChaosDeamonFaction")
                    {
                        newName ="OG_Chaos_Deamon_Faction";
                    }
                    if (defName == "MechanicusFaction")
                    {
                        newName ="OG_Mechanicus_Faction";
                    }
                    if (defName == "NecronFaction")
                    {
                        newName ="OG_Necron_Faction";
                    }
                    // Eldar Factions
                    if (defName == "EldarFaction")
                    {
                        newName ="OG_Eldar_Craftworld_Faction";
                    }
                    if (defName == "EldarPlayerColony")
                    {
                        newName ="OG_Eldar_Player_Craftworld";
                    }
                    // Ork factions
                    if (defName == "OrkFaction")
                    {
                        newName ="OG_Ork_Tek_Faction";
                    }
                    if (defName == "FeralOrkFaction")
                    {
                        newName ="OG_Ork_Feral_Faction";
                    }
                    if (defName.Contains("Ork") && defName.Contains("Player"))
                    {
                        if (defName.Contains("Trib"))
                        {
                            newName ="OG_Ork_PlayerTribe";
                        }
                        else
                        {
                            newName ="OG_Ork_PlayerColony";
                        }
                    }
                    if (defName == "RokOrkz")
                    {
                        newName ="OG_Ork_Rok";
                    }
                    if (defName == "HulkOrkz")
                    {
                        newName ="OG_Ork_Hulk";
                    }

                    // Tau factions
                    if (defName == "TauFaction")
                    {
                        newName ="OG_Tau_Faction";
                    }
                    if (defName == "TauPlayerColony")
                    {
                        newName ="OG_Tau_Player";
                    }

                    // Kroot factions
                    if (defName == "KrootFaction")
                    {
                        newName ="OG_Kroot_Faction";
                    }
                    if (defName == "KrootPlayerColonyTribal")
                    {
                        newName ="OG_Kroot_Player_Tribal";
                    }

                }
                if (defType == typeof(PawnKindDef))
                {
                    List<PawnKindDef> list;
                    if (defName.Contains("Ork"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Ork")).ToList();
                        if (defName.Contains("Choppa"))
                        {
                            list = list.Where(x => x.defName.Contains("Choppa")).ToList();
                        }
                        else
                        if (defName.Contains("Shoota"))
                        {
                            list = list.Where(x => x.defName.Contains("Shoota")).ToList();
                        }
                        else
                        if (defName.Contains("Slugga"))
                        {
                            list = list.Where(x => x.defName.Contains("Slugga")).ToList();
                        }

                        if (defName.Contains("Mek"))
                        {
                            list = list.Where(x => x.defName.Contains("Mek")).ToList();
                        }
                        if (defName.Contains("Warboss"))
                        {
                            list = list.Where(x => x.defName.Contains("Warboss")).ToList();
                        }
                        else
                        if (defName.Contains("Nob"))
                        {
                            list = list.Where(x => x.defName.Contains("Nob")).ToList();
                        }
                        else
                        {
                            list = list.Where(x => !x.defName.Contains("Nob") && !x.defName.Contains("Warboss")).ToList();
                        }
                        newName =list.RandomElement().defName;
                        
                    }
                    if (defName.Contains("Grot"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Grot")).ToList();
                        if (defName.Contains("Colonist"))
                        {
                            list = list.Where(x => x.defName.Contains("Colonist")).ToList();
                        }
                        newName =list.RandomElement().defName;
                    }
                    if (defName.Contains("Snotling"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Snotling")).ToList();
                        
                        newName =list.RandomElement().defName;
                    }
                    if (defName.Contains("Squig"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Squig")).ToList();
                        
                        newName =list.RandomElement().defName;
                    }
                    if (defName.Contains("Tau"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Tau")).ToList();
                        if (defName.Contains("Aun"))
                        {
                            list = list.Where(x => x.defName.Contains("Aun")).ToList();
                        }
                        if (defName.Contains("Shas"))
                        {
                            list = list.Where(x => x.defName.Contains("Shas")).ToList();
                        }
                        if (defName.Contains("Kor"))
                        {
                            list = list.Where(x => x.defName.Contains("Kor")).ToList();
                        }
                        if (defName.Contains("Por"))
                        {
                            list = list.Where(x => x.defName.Contains("Por")).ToList();
                        }
                        if (defName.Contains("Fio"))
                        {
                            list = list.Where(x => x.defName.Contains("Fio")).ToList();
                        }
                        if (defName.Contains("Colonist"))
                        {
                            list = list.Where(x => x.defName.Contains("Colonist")).ToList();
                        }
                        newName =list.RandomElement().defName;
                    }
                    if (defName.Contains("Kroot"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Kroot")).ToList();
                        if (defName.Contains("Shaper"))
                        {
                            list = list.Where(x => x.defName.Contains("Shaper")).ToList();
                        }
                        if (defName.Contains("Colonist"))
                        {
                            list = list.Where(x => x.defName.Contains("Colonist")).ToList();
                        }
                        newName =list.RandomElement().defName;
                    }
                    if (defName.Contains("Guevesa"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Guevesa")).ToList();

                        if (defName.Contains("Colonist"))
                        {
                            list = list.Where(x => x.defName.Contains("Colonist")).ToList();
                        }
                        newName =list.RandomElement().defName;
                    }
                    if (defName.Contains("Eldar"))
                    {
                        list = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.defName.Contains("Eldar")).ToList();
                        if (defName.Contains("Exarch"))
                        {
                            list = list.Where(x => x.defName.Contains("Exarch")).ToList();
                        }
                        if (defName.Contains("Colonist"))
                        {
                            list = list.Where(x => x.defName.Contains("Colonist")).ToList();
                        }
                        newName =list.RandomElement().defName;
                    }

                }
                if (defType == typeof(ResearchProjectDef))
                {
                    // Common Reseach renames
                    if (defName == "WRPowerWeapons" || defName == "OG_Weapons_Power_Imperial")
                    {
                        newName = "OG_Common_Tech_Weapons_Powered";
                    }
                    if (defName == "ImperialSpecialPowerWeapons" || defName == "OG_Weapons_SpecialPower_Imperial")
                    {
                        newName = "OG_Common_Tech_Weapons_Powered_Special";
                    }
                    if (defName == "WRForceWeapons" || defName == "OG_Weapons_Force_Imperial")
                    {
                        newName = "OG_Common_Tech_Weapons_Force";
                    }
                    if (defName == "ImperialSpecialWeapons" || defName == "OG_Weapons_Special_Imperial")
                    {
                        newName = "OG_Common_Tech_Weapons_Special";
                    }
                    if (defName == "ImperialHeavyWeapons" || defName == "OG_Weapons_Heavy_Imperial")
                    {
                        newName = "OG_Common_Tech_Weapons_Heavy";
                    }
                    if (defName == "WRImpLasTech" || defName == "OG_Weapons_Laser_Imperial")
                    {
                        newName = "OG_Common_Tech_Weapons_Laser";
                    }
                    if (defName == "WRImpBoltTech" || defName == "OG_Weapons_Bolter_Imperial")
                    {
                        newName = "OG_Imperial_Tech_Weapons_Bolt";
                    }
                    if (defName == "WRImpPlasmaTech" || defName == "OG_Weapons_Plasma_Imperial")
                    {
                        newName = "OG_Common_Tech_Weapons_Plasma";
                    }
                    // Imperial Reseach renames
                    if (defName == "ImperialTechBase" || defName == "OG_Tech_Base_Imperial" || defName == "OG_Imperial_Tech_Base_T1")
                    {
                        newName = "OG_Imperial_Tech_Base_T0";
                    }
                    /*
                    if (defName == "OG_Weapons_Base_Imperial")
                    {
                        newName = "OG_Imperial_Tech_Weapons_T1";
                    }
                    */
                    if (defName == "WGRConversionField" || defName == "OG_Wargear_ConversionField_Imperial")
                    {
                        newName ="OG_Imperial_Tech_Wargear_Shield";
                    }
                    if (defName == "ARBasicServoSkull" || defName == "OG_Wargear_ServoSkull_Imperial")
                    {
                        newName ="OG_Imperial_Tech_Wargear_ServoSkull";
                    }
                    // Mechanicus Reseach renames
                    if (defName == "MechanicusTechBase" || defName == "OG_Tech_Base_Mechanicus")
                    {
                        newName = "OG_Mechanicus_Tech_Base_T1";
                    }
                    if (defName == "WRRadiumWeapons" || defName == "OG_Weapons_Radium_Mechanicus")
                    {
                        newName = "OG_Mechanicus_Tech_Weapons_Radium";
                    }
                    if (defName == "WRMechAdvBallistics" || defName == "OG_Weapons_AdvBallistics_Mechanicus")
                    {
                        newName = "OG_Mechanicus_Tech_Weapons_AdvancedBallistics";
                    }
                    /*
                    if (defName == "WRMechanicusPlasma" || defName == "OG_Weapons_Plasma_Mechanicus")
                    {
                        newName = "OG_Mechanicus_Tech_Weapons_Ranged_T3";
                    }
                    */
                    // Eldar Reseach renames
                    if (defName == "EldarTechBase")
                    {
                        newName = "OG_Eldar_Tech_Base_T1";
                    }
                    if (defName == "EldarBasicWeaponsTech" || defName == "OG_Eldar_Tech_Weapons_Ranged_T1")
                    {
                        newName = "OG_Eldar_Tech_Weapons_Shuriken"; 
                    }
                    if (defName == "EldarAdvancedWeapons" || defName == "OG_Eldar_Tech_Weapons_Ranged_T2")
                    {
                        newName = "OG_Aeldari_Tech_Weapons_Monofilament";
                    }
                    if (defName == "EldarHeavyWeapons" || defName == "OG_Eldar_Tech_Weapons_Ranged_T3")
                    {
                        newName = "OG_Eldar_Tech_Weapons_Vortex";
                    }
                    if (defName == "EldarWraithTech")
                    {
                        newName = "OG_Eldar_Tech_Base_T2";
                    }
                    /*
                    if (defName == "EldarBasicMeleeTech")
                    {
                        newName = "OG_Eldar_Tech_Weapons_Melee_T1"; 
                    }
                    */
                    if (defName == "EldarPowerWeapons")
                    {
                        newName = "OG_Common_Tech_Weapons_Powered";
                    }
                    if (defName == "EldarAdvancedMelee" || defName == "OG_Eldar_Tech_Weapons_Melee_T3")
                    {
                        newName = "OG_Eldar_Tech_Weapons_Witchblade";
                    }
                    if (defName == "EldarArmour" || defName == "OG_Eldar_Tech_Apparel_Armour_T1")
                    {
                        newName = "OG_Aeldari_Tech_Apparel_Armour_T1"; 
                    }
                    if (defName == "EldarAspectArmour")
                    {
                        newName = "OG_Eldar_Tech_Apparel_Armour_T2";
                    }
                    if (defName == "EldarAdvancedArmour")
                    {
                        newName = "OG_Eldar_Tech_Apparel_Armour_T3";
                    }

                    // Tau Reseach renames TauTechBase
                    if (defName == "TauTechBase")
                    {
                        newName = "OG_Tau_Tech_Base_T1";
                    }
                    if (defName == "TauPlasmaTech" || defName == "OG_Tau_Tech_Weapons_Ranged_T1")
                    {
                        newName = "OG_Tau_Tech_Weapons_PlasmaPulse";
                    }
                    if (defName == "TauAdvancedWeapons" || defName == "OG_Tau_Tech_Weapons_Ranged_T2")
                    {
                        newName = "OG_Tau_Tech_Weapons_Railgun";
                    }
                    if (defName == "TauIonTech" || defName == "OG_Tau_Tech_Weapons_Ranged_T3")
                    {
                        newName = "OG_Tau_Tech_Weapons_Ion";
                    }
                    if (defName == "TauArmour")
                    {
                        newName = "OG_Tau_Tech_Apparel_Armour_T1";
                    }
                    if (defName == "TauDroneTech")
                    {
                        newName = "OG_Tau_Tech_Wargear_Drone";
                    }
                    if (defName == "TauShieldTech")
                    {
                        newName = "OG_Tau_Tech_Wargear_Shield";
                    }

                    // Ork Reseach renames
                    if (defName == "OrkTekBase")
                    {
                        newName = "OG_Ork_Tech_Base_T1";
                    }
                    if (defName == "OrkishBrutality")
                    {
                        newName = "OG_Ork_Tech_Weapons_Melee_T1";
                    }
                    if (defName == "OrkishExtremeBrutality")
                    {
                        newName = "OG_Ork_Tech_Weapons_Melee_T2";
                    }
                    if (defName == "OrkishPowerField")
                    {
                        newName = "OG_Ork_Tech_Weapons_Melee_T3";
                    }
                    if (defName == "OrkishCunning")
                    {
                        newName = "OG_Ork_Tech_Weapons_Ranged_T1";
                    }
                    if (defName == "OrkishIntenseCunning")
                    {
                        newName = "OG_Ork_Tech_Weapons_Ranged_T2";
                    }
                    if (defName == "OrkishMekTek")
                    {
                        newName = "OG_Ork_Tech_Base_T2";
                    }
                    if (defName == "OrkishBigMekBrainz")
                    {
                        newName = "OG_Ork_Tech_Base_T3";
                    }
                    if (defName == "OrkishArmour")
                    {
                        newName = "OG_Ork_Tech_Apparel_Armour_T1";
                    }
                    if (defName == "OrkishEavyArmour")
                    {
                        newName = "OG_Ork_Tech_Apparel_Armour_T2";
                    }
                    if (defName == "OrkishMegaArmour")
                    {
                        newName = "OG_Ork_Tech_Apparel_Armour_T3";
                    }
                }
                if (defType == typeof(HediffDef))
                {
                    if (defName == "HyperactiveNymuneOrgan")
                    {
                        newName ="OG_Kroot_Mutation_HyperactiveNymuneOrgan";
                    }
                }
                if (!newName.NullOrEmpty())
                {
                    __result = newName;
                }
                if (defName == __result)
                {
                //    Log.Warning(string.Format("AMA No replacement found for: {0} T:{1}", defName, defType));
                }
                else
                {
                //    Log.Message(string.Format("Replacement found: {0} T:{1}", __result, defType));
                }
            }
        }
    }

}