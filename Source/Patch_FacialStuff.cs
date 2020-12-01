using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    class Patch_FacialStuff
    {
        public static float tempBaseDrawLocY = 0;

        public static void FSCompat_GetBaseDrawLocY(ref Vector3 rootLoc)
        {
            tempBaseDrawLocY = rootLoc.y;
        }

        public static void FSCompat_DrawBackHairLayer(object __instance,
                                                      ref Vector3 hairLoc,
                                                      RotDrawMode bodyDrawType,
                                                      Quaternion headQuat,
                                                      ref bool renderBody,
                                                      ref bool portrait)
        //FacialStuff.HumanHeadDrawer __instance
        {
            Type t_HumanHeadDrawer = GenTypes.GetTypeInAnyAssembly("FacialStuff.HumanHeadDrawer");
            try
            {
                if (t_HumanHeadDrawer != null)
                {
                    FacialStuff.HumanHeadDrawer this_HumanHeadDrawer = (FacialStuff.HumanHeadDrawer)__instance;
                    //-------------------------REPLICATED FACIAL STUFF CODE-------------------------
                    PawnGraphicSet curGraphics = Traverse.Create(this_HumanHeadDrawer).Field("Graphics").GetValue<PawnGraphicSet>();
                    if (!curGraphics.AllResolved)
                    {
                        curGraphics.ResolveAllGraphics();
                    }

                    Graphic_Multi_BHair hairGraphicExtended = curGraphics.hairGraphic as Graphic_Multi_BHair;
                    if (hairGraphicExtended != null)
                    {
                        Mesh hairMesh = this_HumanHeadDrawer.GetPawnHairMesh(portrait);
                        Rot4 curHeadFacing = Traverse.Create(this_HumanHeadDrawer).Field("HeadFacing").GetValue<Rot4>();
                        Material hairMat = hairGraphicExtended.BackMatAt(curHeadFacing);

                        if (hairMat != null)
                        {
                            List<ApparelGraphicRecord> apparelGraphics = curGraphics.apparelGraphics;
                            List<ApparelGraphicRecord> headgearGraphics = null;
                            if (!apparelGraphics.NullOrEmpty())
                            {
                                headgearGraphics = apparelGraphics
                                                  .Where(x => x.sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead ||
                                                              x.sourceApparel.def.apparel.LastLayer == DefDatabase<ApparelLayerDef>.GetNamedSilentFail("OnHead") ||
                                                              x.sourceApparel.def.apparel.LastLayer == DefDatabase<ApparelLayerDef>.GetNamedSilentFail("StrappedHead") ||
                                                              x.sourceApparel.def.apparel.LastLayer == DefDatabase<ApparelLayerDef>.GetNamedSilentFail("MiddleHead")).ToList();
                            }

                            FacialStuff.CompBodyAnimator animator = this_HumanHeadDrawer.CompAnimator;

                            bool noRenderGoggles = FacialStuff.Controller.settings.FilterHats;

                            bool showRoyalHeadgear = this_HumanHeadDrawer.Pawn.royalty?.MostSeniorTitle != null && FacialStuff.Controller.settings.ShowRoyalHeadgear;
                            bool noRenderRoofed = animator != null && animator.HideHat && !showRoyalHeadgear;
                            bool noRenderBed = FacialStuff.Controller.settings.HideHatInBed && !renderBody && !showRoyalHeadgear;
                            //-------------------------REPLICATED FACIAL STUFF CODE-------------------------



                            hairLoc.y = tempBaseDrawLocY;
                            tempBaseDrawLocY = 0;



                            if (!headgearGraphics.NullOrEmpty())
                            {
                                //-------------------------REPLICATED FACIAL STUFF CODE-------------------------
                                bool filterHeadgear = portrait && Prefs.HatsOnlyOnMap || !portrait && noRenderRoofed;

                                // Draw regular hair if appparel or environment allows it (FS feature)
                                if (bodyDrawType != RotDrawMode.Dessicated)
                                {
                                    // draw full or partial hair
                                    bool apCoversFullHead =
                                    headgearGraphics.Any(
                                                         x => x.sourceApparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf
                                                                                                                 .FullHead)
                                                           && !x.sourceApparel.def.apparel.hatRenderedFrontOfFace);

                                    bool apCoversUpperHead =
                                    headgearGraphics.Any(
                                                         x => x.sourceApparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf
                                                                                                                 .UpperHead)
                                                           && !x.sourceApparel.def.apparel.hatRenderedFrontOfFace);
                                    //-------------------------REPLICATED FACIAL STUFF CODE-------------------------

                                    if (this_HumanHeadDrawer.CompFace.Props.hasOrganicHair || noRenderBed || filterHeadgear
                                     || (!apCoversFullHead && !apCoversUpperHead && noRenderGoggles))
                                    {
                                        GenDraw.DrawMeshNowOrLater(hairMesh, hairLoc, headQuat, hairMat, portrait);
                                    }
                                    /*
                                    else if (FacialStuff.Controller.settings.MergeHair) // && !apCoversFullHead)
                                    {
                                        // If not, display the hair cut
                                        FacialStuff.HairCut.HairCutPawn hairPawn = FacialStuff.HairCut.CutHairDB.GetHairCache(this_HumanHeadDrawer.Pawn);
                                        Material hairCutMat = hairPawn.HairCutMatAt(curHeadFacing);
                                        if (hairCutMat != null)
                                        {
                                            GenDraw.DrawMeshNowOrLater(hairMesh, hairLoc, headQuat, hairCutMat, portrait);
                                        }
                                    }
                                    */
                                }
                            }
                            else
                            {
                                // Draw regular hair if no hat worn
                                if (bodyDrawType != RotDrawMode.Dessicated)
                                {
                                    GenDraw.DrawMeshNowOrLater(hairMesh, hairLoc, headQuat, hairMat, portrait);
                                }
                            }
                        }
                    }
                }
            }
            catch (TypeLoadException) { }
        }
    }
}
