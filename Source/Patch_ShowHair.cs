using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;
using System.Reflection;

namespace ButterfishHairModdingPlus
{
    class Patch_ShowHair
    {
        private static bool shouldHideHair = false;

        [HarmonyAfter(new string[] { "showhair.kv.rw" })]
        public static void SHCompat_CopyHideHair(bool hideHair)
        {
            shouldHideHair = hideHair;
        }

        public static bool SHCompat_OverrideTryGetCustomHairMat(object __instance, ref bool __result, Pawn pawn, Rot4 facing, ref Material mat)
        {
            mat = null;

            Type t_HairUtility = GenTypes.GetTypeInAnyAssembly("ShowHair.HairUtilityFactory").GetNestedType("HairUtility", BindingFlags.Static | BindingFlags.NonPublic);
            try
            {
                if (t_HairUtility != null)
                {
                    MethodInfo m_getMaxCoverageDef = __instance.GetType().GetMethod("getMaxCoverageDef", BindingFlags.Instance | BindingFlags.NonPublic);
                    Dictionary<bool, ShowHair.IHeadCoverage> headCoverages = (Dictionary<bool, ShowHair.IHeadCoverage>) __instance.GetType().GetField("headCoverages", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);

                    BodyPartGroupDef maxCoverageDef = (BodyPartGroupDef) m_getMaxCoverageDef.Invoke(__instance, new object[1] { pawn });
                    string texPath = headCoverages[maxCoverageDef.GetModExtension<ShowHair.BodyPartGroupDefExtension>().IsHeadDef].GetTexPath(pawn, maxCoverageDef.GetModExtension<ShowHair.BodyPartGroupDefExtension>().CoverageLevel);

                    if (texPath != null)
                    {
                        HairColor2_Comp comp = pawn.GetComp<HairColor2_Comp>();
                        Color hairColor2 = Color.white;
                        if (comp != null)
                        {
                            hairColor2 = comp.HairColorTwoExpo.hairColor2;
                        }
                        mat = GraphicDatabase.Get<Graphic_Multi_BHair>(texPath, ShaderDatabase.CutoutComplex, Vector2.one, pawn.story.hairColor, hairColor2).MatAt(facing);
                    }
                }
            }
            catch (TypeLoadException) { }

            __result = (mat != null);
            return false;    //skip original TryGetCustomHairMat method
        }

        public static bool SHCompat_ShouldHideHair()
        {
            return shouldHideHair;
        }
    }
}
