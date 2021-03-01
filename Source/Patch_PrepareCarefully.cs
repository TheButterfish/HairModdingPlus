using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    class Patch_PrepareCarefully
    {
        public static IEnumerable<object> PC_hairOptions;

        public static void PCCompat_AddHairColor2Layer(ref IEnumerable<object> __result)   //List<EdB.PrepareCarefully.PawnLayer> __result
        {
            Type t_PawnLayer = GenTypes.GetTypeInAnyAssembly("EdB.PrepareCarefully.PawnLayer");
            try
            {
                if (t_PawnLayer != null)
                {
                    List<EdB.PrepareCarefully.PawnLayer> result_PawnLayer = (List<EdB.PrepareCarefully.PawnLayer>)__result;
                    EdB.PrepareCarefully.PawnLayer hairColor2Layer = new EdB.PrepareCarefully.PawnLayerHair() { Name = "Hair Color 2", Label = ("HairModdingPlus.PCPatch.HairColorTwo").Translate() };
                    hairColor2Layer.Options = (List<EdB.PrepareCarefully.PawnLayerOption>)PC_hairOptions;
                    PC_hairOptions = null;
                    result_PawnLayer.Insert(1, hairColor2Layer);
                }
            }
            catch (TypeLoadException) { }
        }

        public static void PCCompat_GetHairOptions(ref IEnumerable<object> __result) //List<EdB.PrepareCarefully.PawnLayerOption> __result
        {
            PC_hairOptions = __result;
        }

        public static bool PCCompat_GetSelectedColor(object __instance, ref object pawn, ref Color __result)   //EdB.PrepareCarefully.PawnLayerHair __instance, ref EdB.PrepareCarefully.CustomPawn pawn
        {
            Type t_CustomPawn = GenTypes.GetTypeInAnyAssembly("EdB.PrepareCarefully.CustomPawn");
            try
            {
                if (t_CustomPawn != null)
                {
                    EdB.PrepareCarefully.PawnLayerHair this_PawnLayerHair = (EdB.PrepareCarefully.PawnLayerHair)__instance;
                    EdB.PrepareCarefully.CustomPawn o_CustomPawn = (EdB.PrepareCarefully.CustomPawn)pawn;
                    if (this_PawnLayerHair.Name == "Hair Color 2")
                    {
                        __result = HairColor2_API.GetHairColor2(o_CustomPawn.Pawn);

                        return false;
                    }
                }
            }
            catch (TypeLoadException) { }

            return true;
        }

        public static bool PCCompat_SelectColor(object __instance, ref object pawn, Color color)   //EdB.PrepareCarefully.PawnLayerHair __instance, ref EdB.PrepareCarefully.CustomPawn pawn
        {
            Type t_CustomPawn = GenTypes.GetTypeInAnyAssembly("EdB.PrepareCarefully.CustomPawn");
            try
            {
                if (t_CustomPawn != null)
                {
                    EdB.PrepareCarefully.PawnLayerHair this_PawnLayerHair = (EdB.PrepareCarefully.PawnLayerHair)__instance;
                    EdB.PrepareCarefully.CustomPawn o_CustomPawn = (EdB.PrepareCarefully.CustomPawn)pawn;
                    if (this_PawnLayerHair.Name == "Hair Color 2")
                    {
                        HairColor2_API.SetHairColor2(o_CustomPawn.Pawn, color);
                        o_CustomPawn.MarkPortraitAsDirty();

                        return false;
                    }
                }
            }
            catch (TypeLoadException) { }

            return true;
        }

        public static void PCCompat_IncludeCompToSave()
        {
            Type t_PawnCompRules = GenTypes.GetTypeInAnyAssembly("EdB.PrepareCarefully.DefaultPawnCompRules");
            try
            {
                if (t_PawnCompRules != null)
                {
                    EdB.PrepareCarefully.DefaultPawnCompRules.rulesForSaving.IncludeComp("ButterfishHairModdingPlus.HairColor2_Comp");
                }
            }
            catch (TypeLoadException) { }
        }
    }
}
