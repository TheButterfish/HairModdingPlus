using System;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    class Compat_GradientHair
    {
        public static string GHCompat_TryGetGradientPath(Pawn pawn)
        {
            Type t_CompGradientHair = GenTypes.GetTypeInAnyAssembly("GradientHair.CompGradientHair");
            try
            {
                if (t_CompGradientHair != null)
                {
                    GradientHair.CompGradientHair comp = pawn.GetComp<GradientHair.CompGradientHair>();
                    if (comp != null)
                    {
                        GradientHair.GradientHairSettings settings = comp.Settings;
                        if (settings.enabled)
                        {
                            return (':' + settings.mask);
                        }
                    }
                }
            }
            catch (TypeLoadException) { }

            return "";
        }

        public static Color GHCompat_TryGetGradientColor(Pawn pawn)
        {
            Type t_CompGradientHair = GenTypes.GetTypeInAnyAssembly("GradientHair.CompGradientHair");
            try
            {
                if (t_CompGradientHair != null)
                {
                    GradientHair.CompGradientHair comp = pawn.GetComp<GradientHair.CompGradientHair>();
                    if (comp != null)
                    {
                        GradientHair.GradientHairSettings settings = comp.Settings;
                        if (settings.enabled)
                        {
                            return (settings.colorB);
                        }
                    }
                }
            }
            catch (TypeLoadException) { }

            return Color.clear;
        }
    }
}
