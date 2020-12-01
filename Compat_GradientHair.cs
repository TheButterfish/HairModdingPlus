using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Verse;
using GradientHair;

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
                    CompGradientHair comp = pawn.GetComp<CompGradientHair>();
                    if (comp != null)
                    {
                        GradientHairSettings settings = comp.Settings;
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
                    CompGradientHair comp = pawn.GetComp<CompGradientHair>();
                    if (comp != null)
                    {
                        GradientHairSettings settings = comp.Settings;
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
