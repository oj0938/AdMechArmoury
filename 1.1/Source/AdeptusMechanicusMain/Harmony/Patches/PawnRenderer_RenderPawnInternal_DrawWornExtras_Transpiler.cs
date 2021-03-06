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
using System.Reflection.Emit;
using UnityEngine;
using System.Reflection;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal", new Type[] { typeof(Vector3), typeof(float), typeof(bool), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(bool), typeof(bool), typeof(bool) }), HarmonyPriority(Priority.Last)]
    public static class PawnRenderer_RenderPawnInternal_DrawWornExtras_Transpiler
    {
        private static readonly Type patchType = typeof(HarmonyPatches);
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            List<CodeInstruction> instructionList = instructions.ToList();

            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[index: i];
                if (i > 1 && instructionList[index: i -1].OperandIs(AccessTools.Method(type: typeof(Graphics), name: nameof(Graphics.DrawMesh), parameters: new []{typeof(Mesh), typeof(Vector3), typeof(Quaternion), typeof(Material), typeof(Int32)})) && (i+1) < instructionList.Count /* && instructionList[index: i + 1].opcode == OpCodes.Brtrue_S*/)
                {
                    yield return instruction; // portrait
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_1);
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_0);
                    yield return new CodeInstruction(opcode: OpCodes.Ldfld, operand: AccessTools.Field(type: typeof(PawnRenderer), name: "pawn"));
                    yield return new CodeInstruction(opcode: OpCodes.Ldloc_0);             // quat
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_S, operand: 4); // bodyfacing
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_S, operand: 9); //invisible
                    yield return new CodeInstruction(opcode: OpCodes.Ldloc_1);             // Mesh
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_S, operand: 5); // bodyfacing
                    yield return new CodeInstruction(opcode: OpCodes.Call,    operand: typeof(PawnRenderer_RenderPawnInternal_DrawWornExtras_Transpiler).GetMethod("DrawAddons"));

                    instruction = new CodeInstruction(opcode: OpCodes.Ldarg_S, operand: 7);
                }

                yield return instruction;
            }
        }

        public static void DrawAddons( bool portrait, Vector3 vector, Pawn pawn, Quaternion quat, Rot4 bodyFacing, bool invisible, Mesh mesh, Rot4 headfacing)
        {
            if (invisible) return;
            Vector2 size  = mesh?.bounds.size ?? (portrait ? MeshPool.humanlikeBodySet.MeshAt(bodyFacing).bounds.size : pawn.Drawer.renderer.graphics.nakedGraphic.MeshAt(bodyFacing).bounds.size);
            if (pawn.apparel != null && pawn.apparel.WornApparelCount > 0)
            {
                
                if (AdeptusIntergrationUtil.enabled_AlienRaces)
                {
                    AlienRacesPatch(pawn, bodyFacing, out size, portrait);
                }
                else
                {
                    size = new Vector2(1.5f, 1.5f);
                }
                
                List<Apparel> worn = pawn.apparel.WornApparel;
                for (int wa = 0; wa < worn.Count; wa++)
                {
                    Apparel apparel = worn[wa];
                    for (int i = 0; i < apparel.AllComps.Count; i++)
                    {
                        CompPauldronDrawer Pauldron = apparel.AllComps[i] as CompPauldronDrawer;
                        if (Pauldron != null)
                        {
                            Vector3 center = vector + (quat * Pauldron.GetOffsetFor(bodyFacing, false));
                            if (Pauldron.activeEntries.NullOrEmpty())
                            {
                                Pauldron.Initialize();
                            }
                            foreach (ShoulderPadEntry entry in Pauldron.activeEntries)
                            {
                                //    entry.Drawer = Pauldron;
                                if (entry.apparel == null)
                                {
                                    entry.apparel = apparel;
                                }
                                if (entry.Drawer == null)
                                {
                                    Log.Warning("Warning! Drawer null");
                                }
                                if (entry.ShouldDrawEntry(portrait, bodyFacing, size, out Graphic pauldronMat, out Mesh pauldronMesh, out Vector3 offset))
                                {
                                    GenDraw.DrawMeshNowOrLater
                                        (
                                            // pauldronMesh,
                                            GetPawnMesh(portrait, pawn, bodyFacing, !Pauldron.onHead),
                                            center + (quat * offset),
                                            quat,
                                            OverrideMaterialIfNeeded(pauldronMat.MatAt(bodyFacing), pawn),
                                            portrait
                                        );
                                }
                            }
                        }
                        CompApparelExtraDrawer ExtraDrawer = apparel.AllComps[i] as CompApparelExtraDrawer;
                        if (ExtraDrawer != null)
                        {
                            Vector3 drawAt = vector;
                            if (!ExtraDrawer.Props.ExtrasEntries.NullOrEmpty())
                            {
                                if (ExtraDrawer.ShouldDrawExtra(pawn, apparel, bodyFacing, out Material extraMat))
                                {
                                    if (ExtraDrawer.onHead)
                                    {
                                        drawAt = vector + quat * pawn.Drawer.renderer.BaseHeadOffsetAt(headfacing);
                                    }
                                    drawAt += quat * ExtraDrawer.GetAltitudeOffset(bodyFacing, ExtraDrawer.ExtraPartEntry);
                                    Material material = OverrideMaterialIfNeeded(extraMat, pawn);
                                    GenDraw.DrawMeshNowOrLater(mesh, drawAt, quat, material, portrait);
                                    //    vector.y += CompApparelExtaDrawer.MinClippingDistance;
                                }
                            }
                        }
                    }

                }
            }
            if (!pawn.Dead)
            {
                for (int hd = 0; hd < pawn.health.hediffSet.hediffs.Count; hd++)
                {
                    Vector3 drawAt = vector;
                    HediffWithComps hediff = pawn.health.hediffSet.hediffs[hd] as HediffWithComps;
                    if (hediff != null)
                    {
                        for (int i = 0; i < hediff.comps.Count; i++)
                        {
                            HediffComp_DrawImplant_AdMech drawer = hediff.comps[i] as HediffComp_DrawImplant_AdMech;
                            if (drawer != null)
                            {
                                Material material = null;
                                if (drawer.implantDrawProps.implantDrawerType != ImplantDrawerType.Head)
                                {
                                    drawAt.y += 0.005f;
                                    if (bodyFacing == Rot4.South && drawer.implantDrawProps.implantDrawerType == ImplantDrawerType.Backpack)
                                    {
                                        drawAt.y -= 0.3f;
                                    }
                                    material = drawer.ImplantMaterial(pawn, bodyFacing);
                                    //    GenDraw.DrawMeshNowOrLater(mesh, drawAt, quat, material, portrait);
                                }
                                else
                                {
                                    if (!pawn.Downed && !pawn.Dead && drawer.implantDrawProps.useHeadOffset)
                                    {
                                        drawAt = vector + pawn.Drawer.renderer.BaseHeadOffsetAt(headfacing);
                                    }
                                    else
                                    {
                                        if (pawn.Downed || pawn.Dead && drawer.implantDrawProps.useHeadOffset)
                                        {
                                            drawAt.y = vector.y + pawn.Drawer.renderer.BaseHeadOffsetAt(headfacing).y;
                                        }
                                    }
                                    drawAt.y += 0.005f;
                                    material = drawer.ImplantMaterial(pawn, headfacing);
                                    //    GenDraw.DrawMeshNowOrLater(mesh, drawAt, quat, material, portrait);
                                }

                                if (material != null)
                                {
                                    //    GenDraw.DrawMeshNowOrLater(mesh, drawAt , quat, material, portrait);

                                    material = OverrideMaterialIfNeeded(material, pawn);
                                    //                                                                                        Angle calculation to not pick the shortest, taken from Quaternion.Angle and modified
                                    GenDraw.DrawMeshNowOrLater(mesh: mesh, loc: drawAt + drawer.offsetVector().RotatedBy(angle: Mathf.Acos(f: Quaternion.Dot(a: Quaternion.identity, b: quat)) * 2f * 57.29578f),
                                        quat: quat, mat: material, drawNow: portrait);

                                    drawAt.y += HediffComp_DrawImplant_AdMech.MinClippingDistance;
                                }
                            }
                            HediffComp_Shield _Shield;
                            if ((_Shield = hediff.comps[i] as HediffComp_Shield) != null)
                            {
                                _Shield.DrawWornExtras();
                            }
                        }
                    }
                }
            }
        
        }

        // Token: 0x06000F45 RID: 3909 RVA: 0x00057D14 File Offset: 0x00055F14
        private static Material OverrideMaterialIfNeeded(Material original, Pawn pawn)
        {
            Material baseMat = pawn.IsInvisible() ? InvisibilityMatPool.GetInvisibleMat(original) : original;
            return pawn.Drawer.renderer.graphics.flasher.GetDamagedMat(baseMat);
        }
        
        public static void AlienRacesPatch(Pawn pawn, Rot4 bodyFacing, out Vector2 size, bool portrait)
        {
            AlienRace.ThingDef_AlienRace alienDef = pawn.def as AlienRace.ThingDef_AlienRace;
            Vector2 d;
            if (alienDef != null)
            {
                AlienRace.AlienPartGenerator.AlienComp comp = pawn.TryGetComp<AlienRace.AlienPartGenerator.AlienComp>();
                if (comp != null)
                {
                //    d = (portrait ? comp.alienPortraitGraphics.bodySet.MeshAt(bodyFacing).bounds.size : comp.alienGraphics.bodySet.MeshAt(bodyFacing).bounds.size);
                    d = (portrait ? comp.customPortraitDrawSize : comp.customDrawSize);

                    size = new Vector2(d.x * 1.5f, d.y * 1.5f);
                    return;
                }
            }
            size = new Vector2(1.5f, 1.5f);
            return;
        }


        // Token: 0x06000082 RID: 130 RVA: 0x00008950 File Offset: 0x00006B50
        public static Mesh GetPawnMesh(bool portrait, Pawn pawn, Rot4 facing, bool wantsBody)
        {

            if (AdeptusIntergrationUtil.enabled_AlienRaces)
            {
                return GetAlienPawnMesh(portrait, pawn, facing, wantsBody);
            }
            if (!wantsBody)
            {
                return MeshPool.humanlikeHeadSet.MeshAt(facing);
            }
            return MeshPool.humanlikeBodySet.MeshAt(facing);
        }

        // Token: 0x06000082 RID: 130 RVA: 0x00008950 File Offset: 0x00006B50
        public static Mesh GetAlienPawnMesh(bool portrait, Pawn pawn, Rot4 facing, bool wantsBody)
        {

            AlienRace.AlienPartGenerator.AlienComp comp = pawn.GetComp<AlienRace.AlienPartGenerator.AlienComp>();
            if (comp == null)
            {
                if (!wantsBody)
                {
                    return MeshPool.humanlikeHeadSet.MeshAt(facing);
                }
                return MeshPool.humanlikeBodySet.MeshAt(facing);
            }
            else if (!portrait)
            {
                if (!wantsBody)
                {
                    return comp.alienHeadGraphics.headSet.MeshAt(facing);
                }
                return comp.alienGraphics.bodySet.MeshAt(facing);
            }
            else
            {
                if (!wantsBody)
                {
                    return comp.alienPortraitHeadGraphics.headSet.MeshAt(facing);
                }
                return comp.alienPortraitGraphics.bodySet.MeshAt(facing);
            }
        }
    }
}
