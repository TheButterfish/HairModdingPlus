using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Verse;
using BabiesAndChildren;

namespace ButterfishHairModdingPlus
{
    class Compat_BabiesAndChildren
    {
        public static bool BCCompat_TryCheckYoungerThanChild(Pawn pawn)
        {
            Type t_BCAgeStage = GenTypes.GetTypeInAnyAssembly("BabiesAndChildren.AgeStage");
            try
            {
                if (t_BCAgeStage != null)
                {
                    return AgeStage.IsYoungerThan(pawn, AgeStage.Child);
                }
            }
            catch (TypeLoadException) { }

            return true;
        }
    }
}
