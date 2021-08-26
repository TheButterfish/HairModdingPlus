using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace ButterfishHairModdingPlus
{
    class Patch_BabiesAndChildren
    {
        private static Mesh modifiedMesh_BabiesAndChildren = null;

        //credits to CentAtMoney for the basis of this snippet of code
        public static bool BCCompat_IsYoungerThanChild(Pawn pawn)
        {
			int curLifeStageIndex = pawn.ageTracker.CurLifeStageIndex;
			_ = pawn.RaceProps.lifeStageAges.Count;
			return (curLifeStageIndex < 2);
		}

        [HarmonyAfter(new string[] { "babies.and.children.continued", "babies.and.children.continued.13" })]
        public static void BCCompat_CopyModifiedPawnHairMesh(Mesh __result)
        {
            modifiedMesh_BabiesAndChildren = __result;
        }

        public static Mesh BCCompat_GetCopiedMesh()
        {
            return modifiedMesh_BabiesAndChildren;
        }
    }
}
