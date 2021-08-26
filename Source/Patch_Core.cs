using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace ButterfishHairModdingPlus
{
    class Patch_Core
    {
        [HarmonyAfter(new string[] { "babies.and.children.continued", "babies.and.children.continued.13", "children.and.pregnancy" })]
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
        public static void RecalcRootLocY(PawnRenderer __instance, ref Vector3 rootLoc, PawnRenderFlags flags)
        {
            /*Pawn pawn = __instance.graphics.pawn;

            if (!flags.FlagSet(PawnRenderFlags.Portrait) && pawn.Spawned)
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
            */
        }

        [HarmonyAfter(new string[] { "babies.and.children.continued", "babies.and.children.continued.13", "children.and.pregnancy" })]
        public static void DrawBackHairLayer(PawnRenderer __instance,
                                             Vector3 rootLoc,
                                             Vector3 headOffset,
                                             float angle,
                                             Rot4 headFacing,
                                             RotDrawMode bodyDrawType,
                                             PawnRenderFlags flags)
        {
            PawnGraphicSet graphics = __instance.graphics;
            Graphic_Multi_BHair hairGraphicExtended = graphics.hairGraphic as Graphic_Multi_BHair;

            //check if ShellFullyCoversHead
            bool shellFullyCoversHead = false;
            List<ApparelGraphicRecord> apparelGraphics = graphics.apparelGraphics;
            for (int i = 0; i < apparelGraphics.Count; i++)
            {
                if (apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell && apparelGraphics[i].sourceApparel.def.apparel.shellCoversHead)
                {
                    shellFullyCoversHead = true;
                }
            }

            //if has head and hair graphics
            if (graphics.headGraphic != null && hairGraphicExtended != null && !shellFullyCoversHead)
            {
                Material hairMat = hairGraphicExtended.BackMatAt(headFacing);

                if (hairMat != null)
                {
                    Vector3 onHeadLoc = rootLoc + headOffset;

                    bool hideHair = false;
                    if (flags.FlagSet(PawnRenderFlags.Headgear) && (!flags.FlagSet(PawnRenderFlags.Portrait) || !Prefs.HatsOnlyOnMap || flags.FlagSet(PawnRenderFlags.StylingStation)))
                    {
                        for (int i = 0; i < apparelGraphics.Count; i++)
                        {
                            if (apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover)
                            {
                                if (!apparelGraphics[i].sourceApparel.def.apparel.hatRenderedFrontOfFace && !apparelGraphics[i].sourceApparel.def.apparel.forceRenderUnderHair)
                                {
                                    if (HarmonyPatches_BHair.loadedShowHair)
                                    {
                                        //hideHair = Compat_ShowHair.SHCompat_ShouldHideHair(graphics.pawn, apparelGraphics[i].sourceApparel.def, flags.FlagSet(PawnRenderFlags.Portrait));
                                        hideHair = Patch_ShowHair.SHCompat_ShouldHideHair();
                                    }
                                    else if (HarmonyPatches_BHair.loadedHatDisplaySelection)
                                    {
                                        hideHair = Patch_HatDisplaySelection.HDCompat_ShouldHideHair(graphics.pawn, apparelGraphics[i].sourceApparel.def.defName);
                                    }
                                    else
                                    {
                                        hideHair = true;
                                    }
                                }
                            }
                        }
                    }

                    if (!hideHair && bodyDrawType != RotDrawMode.Dessicated && !flags.FlagSet(PawnRenderFlags.HeadStump))
                    {
                        //get modified hairMat
                        if (!flags.FlagSet(PawnRenderFlags.Portrait) && graphics.pawn.IsInvisible())
                        {
                            hairMat = InvisibilityMatPool.GetInvisibleMat(hairMat);
                        }
                        if (!flags.FlagSet(PawnRenderFlags.Cache))
                        {
                            hairMat = graphics.flasher.GetDamagedMat(hairMat);
                        }

                        //get hairMesh
                        Mesh hairMesh = null;
                        if (HarmonyPatches_BHair.loadedAlienRace)
                        {
                            //use modified hair mesh after processed by Alien Race
                            hairMesh = Patch_AlienRace.ARCompat_GetCopiedMesh();
                        }

                        if (HarmonyPatches_BHair.loadedBabiesAndChildren)
                        {
                            //use modified hair mesh after processed by Babies And Children
                            hairMesh = Patch_BabiesAndChildren.BCCompat_GetCopiedMesh();
                        }

                        if (HarmonyPatches_BHair.loadedRimWorldChildren)
                        {
                            //use modified hair mesh after processed by RimWorldChildren
                            hairMesh = Patch_RimWorldChildren.RCCompat_GetCopiedMesh();
                            hairMat = Patch_RimWorldChildren.RCCompat_ModifyHairForChild(hairMat, graphics.pawn);

                            //alternate calling method for manual calling
                            //hairMesh = Compat_RimWorldChildren.RCCompat_GetModifiedPawnHairMesh(graphics, graphics.pawn, headFacing);
                        }

                        if (hairMesh == null)
                        {
                            //default
                            hairMesh = graphics.HairMeshSet.MeshAt(headFacing);
                        }

                        GenDraw.DrawMeshNowOrLater(mesh: hairMesh, loc: onHeadLoc, quat: Quaternion.AngleAxis(angle, Vector3.up), mat: hairMat, drawNow: flags.FlagSet(PawnRenderFlags.DrawNow));
                    }
                }
            }
        }

        //fixes issue of portraits of pawns with gradient hairs having blank portraits on pawn selection screen
        public static void PreloadCacheBugfix()
        {
            /*
             * for (int i = 0; i < Find.GameInitData.startingAndOptionalPawns.Count; i++)
            {
                Pawn pawn = Find.GameInitData.startingAndOptionalPawns[i];
                PortraitsCache.Get(pawn, new Vector2(70f, 110f));
                PortraitsCache.Clear();
            }
            */
        }

        //fixes issue of portraits of pawns having blank portraits on first load of the game after boot
        public static void OnLoadPortraitsBugfix(ColonistBar __instance)
        {
            /*
             * if (HarmonyPatches_BHair.colonistBarFirstDraw)
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
            */
        }
    }
}
