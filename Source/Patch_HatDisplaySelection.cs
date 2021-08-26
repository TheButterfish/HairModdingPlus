using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;

namespace ButterfishHairModdingPlus
{
    class Patch_HatDisplaySelection
    {
        public static void HDCompat_DrawBackHairLayer(PawnRenderer __instance, Vector3 rootLoc, float angle, Rot4 bodyFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags)
        {
            //Hats Display Selection overwrites the calling of DrawHeadHair, so call from RenderPawnInternal instead

            if (__instance.graphics.headGraphic != null)
            {
                Vector3 headOffset = Quaternion.AngleAxis(angle, Vector3.up) * __instance.BaseHeadOffsetAt(bodyFacing);
                Patch_Core.DrawBackHairLayer(__instance, rootLoc, headOffset, angle, bodyFacing, bodyDrawType, flags);
            }
        }

        public static bool HDCompat_ShouldHideHair(Pawn pawn, string hatDefName)
        {
            Type t_Setting = GenTypes.GetTypeInAnyAssembly("HatDisplaySelection.Setting");
            try
            {
                if (t_Setting != null)
                {
                    int hatIndex = HatDisplaySelection.Setting.savedHats.IndexOf(hatDefName);

                    bool isDrafted = false;
                    if (pawn.Faction == Faction.OfPlayer && pawn.RaceProps.Humanlike)
                    {
                        isDrafted = pawn.Drafted;
                    }

                    return HatDisplaySelection.Setting.hatsParam[hatIndex] == 0 || (HatDisplaySelection.Setting.hatsParam[hatIndex] == 2 && HatDisplaySelection.Setting.hatsDrafted[hatIndex] && HatDisplaySelection.Setting.draftedSettings[hatIndex] && isDrafted);
                }
            }
            catch (TypeLoadException) { }

            return true;
        }
    }
}
