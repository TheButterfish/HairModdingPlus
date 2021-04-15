using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace ButterfishHairModdingPlus
{
    class Patch_Core
    {
        [HarmonyAfter(new string[] { "babies.and.children.continued", "children.and.pregnancy" })]
        public static void UseModifiedGraphicParams(PawnGraphicSet __instance)
        {
            if (__instance.pawn.RaceProps.Humanlike)
            {
                if (HarmonyPatches_BHair.loadedBabiesAndChildren)
                {
                    if (Patch_BabiesAndChildren.BCCompat_IsYoungerThanChild(__instance.pawn))
                    {
                        __instance.hairGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Humanlike/null", ShaderDatabase.Cutout, Vector2.one, Color.white);
                        return;
                    }
                }

                string hairTexturePath = __instance.pawn.story.hairDef.texPath;

                HairColor2_Comp comp = __instance.pawn.GetComp<HairColor2_Comp>();
                Color hairColor2 = Color.white;
                if (comp != null)
                {
                    hairColor2 = comp.HairColorTwoExpo.hairColor2;
                }

                if (HarmonyPatches_BHair.loadedGradientHair)
                {
                    Color gradientColor = Compat_GradientHair.GHCompat_TryGetGradientColor(__instance.pawn);
                    hairTexturePath += ":" + gradientColor.a.ToString() + ":" + gradientColor.r.ToString() + ":" + gradientColor.g.ToString() + ":" + gradientColor.b.ToString();
                    hairTexturePath += Compat_GradientHair.GHCompat_TryGetGradientPath(__instance.pawn);
                }

                __instance.hairGraphic = GraphicDatabase.Get<Graphic_Multi_BHair>(hairTexturePath, ShaderDatabase.CutoutComplex, Vector2.one, __instance.pawn.story.hairColor, hairColor2);
            }
        }

        //credits to Killface for the basis of this snippet of code
        //IMPORTANT: In RimWorld, the "y" variable refers to depth, not height/vertical axis.
        //           Instead, height/vertical axis is stored in "z" variable.
        [HarmonyAfter(new string[] { "children.and.pregnancy" })]
        public static void RecalcRootLocY(PawnRenderer __instance, ref Vector3 rootLoc, bool portrait)
        {
            Pawn pawn = __instance.graphics.pawn;

            if (!portrait && pawn.Spawned)
            {
                Vector3 loc = rootLoc;
                CellRect viewRect = Find.CameraDriver.CurrentViewRect;
                viewRect = viewRect.ExpandedBy(1);

                List<Pawn> pawns = new List<Pawn>();
                foreach (Pawn otherPawn in pawn.Map.mapPawns.AllPawnsSpawned)
                {
                    if (!viewRect.Contains(otherPawn.Position)) { continue; }
                    if (otherPawn == pawn) { continue; }
                    if (otherPawn.DrawPos.x < loc.x - 0.5f) { continue; }
                    if (otherPawn.DrawPos.x > loc.x + 0.5f) { continue; }
                    if (otherPawn.DrawPos.z <= loc.z) { continue; }

                    pawns.Add(otherPawn);
                }

                if (!pawns.NullOrEmpty())
                {
                    float pawnOffset = 0.05f * pawns.Count;
                    loc.y += pawnOffset;
                }

                rootLoc = loc;
            }
        }

        [HarmonyAfter(new string[] { "babies.and.children.continued", "children.and.pregnancy" })]
        public static void DrawBackHairLayer(PawnRenderer __instance,
                                             ref Vector3 rootLoc,
                                             ref float angle,
                                             ref Rot4 headFacing,
                                             ref RotDrawMode bodyDrawType,
                                             ref bool portrait,
                                             ref bool headStump)
        {
            PawnGraphicSet graphics = __instance.graphics;
            if (!graphics.AllResolved)
            {
                graphics.ResolveAllGraphics();
            }
            Graphic_Multi_BHair hairGraphicExtended = graphics.hairGraphic as Graphic_Multi_BHair;

            //if has head and hair graphics
            if (graphics.headGraphic != null && hairGraphicExtended != null)
            {
                Material hairMat = hairGraphicExtended.BackMatAt(headFacing);

                if (hairMat != null)
                {
                    //-------------------------REPLICATED VANILLA CODE-------------------------
                    Quaternion quaternion = Quaternion.AngleAxis(angle, new Vector3(0f, 1f, 0f));
                    Vector3 b = quaternion * __instance.BaseHeadOffsetAt(headFacing);
                    Vector3 loc2 = rootLoc + b;
                    //-------------------------REPLICATED VANILLA CODE-------------------------


                    //loc2.y -= 0.0303030312f;    //changed from original, used to be +=


                    bool hideHair = false;
                    if (!portrait || !Prefs.HatsOnlyOnMap)
                    {
                        List<ApparelGraphicRecord> apparelGraphics = graphics.apparelGraphics;
                        for (int j = 0; j < apparelGraphics.Count; j++)
                        {
                            if (apparelGraphics[j].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead)
                            {
                                if (!apparelGraphics[j].sourceApparel.def.apparel.hatRenderedFrontOfFace)
                                {
                                    if (HarmonyPatches_BHair.loadedShowHair)
                                    {
                                        hideHair = Compat_ShowHair.SHCompat_ShouldHideHair(graphics.pawn, apparelGraphics[j].sourceApparel.def, portrait);
                                    }
                                    else if (HarmonyPatches_BHair.loadedHatDisplaySelection)
                                    {
                                        hideHair = Compat_HatDisplaySelection.HDCompat_ShouldHideHair(graphics.pawn, apparelGraphics[j].sourceApparel.def.defName);
                                    }
                                    else
                                    {
                                        hideHair = true;
                                    }
                                }
                            }
                        }
                    }

                    if (!hideHair && bodyDrawType != RotDrawMode.Dessicated && !headStump)
                    {
                        if (graphics.pawn.IsInvisible())
                        {
                            hairMat = InvisibilityMatPool.GetInvisibleMat(hairMat);
                        }
                        Material resultMat = graphics.flasher.GetDamagedMat(hairMat);

                        Mesh hairMesh = null;
                        if (HarmonyPatches_BHair.loadedAlienRace)
                        {
                            //use modified hair mesh after processed by Alien Race/Babies And Children
                            hairMesh = Patch_AlienRace.ARCompat_GetCopiedMesh();
                        }

                        if (HarmonyPatches_BHair.loadedRimWorldChildren)
                        {
                            //use modified hair mesh after processed by RimWorldChildren
                            hairMesh = Patch_RimWorldChildren.RCCompat_GetCopiedMesh();
                            resultMat = Patch_RimWorldChildren.RCCompat_ModifyHairForChild(resultMat, graphics.pawn);

                            //alternate calling method for manual calling
                            //hairMesh = Compat_RimWorldChildren.RCCompat_GetModifiedPawnHairMesh(graphics, graphics.pawn, headFacing);
                        }

                        if (hairMesh == null)
                        {
                            //default
                            hairMesh = graphics.HairMeshSet.MeshAt(headFacing);
                        }
                        
                        GenDraw.DrawMeshNowOrLater(mesh: hairMesh, mat: resultMat, loc: loc2, quat: quaternion, drawNow: portrait);
                    }
                }
            }
        }

        //fixes issue of portraits of pawns with gradient hairs having blank portraits on pawn selection screen
        public static void PreloadCacheBugfix()
        {
            for (int i = 0; i < Find.GameInitData.startingAndOptionalPawns.Count; i++)
            {
                Pawn pawn = Find.GameInitData.startingAndOptionalPawns[i];
                PortraitsCache.Get(pawn, new Vector2(70f, 110f));
                PortraitsCache.Clear();
            }
        }

        //fixes issue of portraits of pawns having blank portraits on first load of the game after boot
        public static void OnLoadPortraitsBugfix(ColonistBar __instance)
        {
            if (HarmonyPatches_BHair.colonistBarFirstDraw)
            {
                __instance.MarkColonistsDirty();

                List<Pawn> pawnList = __instance.GetColonistsInOrder();
                foreach (Pawn pawn in pawnList)
                {
                    pawn.Drawer.renderer.graphics.ResolveAllGraphics();
                    PortraitsCache.SetDirty(pawn);
                }
                
                HarmonyPatches_BHair.colonistBarFirstDraw = false;
            }
        }
    }
}
