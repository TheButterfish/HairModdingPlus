﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace ButterfishHairModdingPlus
{
    class Patch_AlienRace
    {
        private static Mesh modifiedMesh_AlienRaces = null;

        [HarmonyAfter(new string[] { "babies.and.children.continued" })]
        public static void ARCompat_CopyModifiedPawnHairMesh(Mesh __result)
        {
            modifiedMesh_AlienRaces = __result;
        }

        public static Mesh ARCompat_GetCopiedMesh()
        {
            return modifiedMesh_AlienRaces;
        }
    }
}
