using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;
using System.Reflection;

namespace ButterfishHairModdingPlus
{
    class Patch_RimWorldChildren
    {
        private static Mesh modifiedMesh_RimWorldChildren = null;

        [HarmonyAfter(new string[] { "children.and.pregnancy" })]
        public static void RCCompat_CopyModifiedPawnHairMesh(Mesh __result)
        {
            modifiedMesh_RimWorldChildren = __result;
        }

        public static Mesh RCCompat_GetCopiedMesh()
        {
            return modifiedMesh_RimWorldChildren;
        }

        public static Material RCCompat_ModifyHairForChild(Material mat, Pawn pawn)
        {
            Type t_LifecycleComp = GenTypes.GetTypeInAnyAssembly("RimWorldChildren.LifecycleComp");
            try
            {
                if (t_LifecycleComp != null)
                {
                    //replicated code from RimWorldChildren.Children_Drawing.ModifyHairForChild()
                    RimWorldChildren.LifecycleComp lifecycleComp = pawn.TryGetComp<RimWorldChildren.LifecycleComp>();
                    if (RimWorldChildren.API.ChildrenUtility.RaceUsesChildren(pawn) && pawn.TryGetComp<RimWorldChildren.LifecycleComp>()?.CurrentLifestage?.graphics != null)
                    {
                        mat.mainTexture.wrapMode = TextureWrapMode.Clamp;
                        float num = 0f;
                        Vector2 hairOffset = lifecycleComp.CurrentLifestage.graphics.hairOffset;
                        hairOffset.x += num;
                        mat.mainTextureOffset = hairOffset;
                    }
                }
            }
            catch (TypeLoadException) { }

            return mat;
        }

        //alternate calling method for manual calling
        /*
        public static Mesh RCCompat_GetModifiedPawnHairMesh(PawnGraphicSet graphics, Pawn pawn, Rot4 headFacing)
        {
            Type t_PawnRenderer_RenderPawnInternal_Patch = GenTypes.GetTypeInAnyAssembly("RimWorldChildren.PawnRenderer_RenderPawnInternal_Patch");
            try
            {
                if (t_PawnRenderer_RenderPawnInternal_Patch != null)
                {
                    return RimWorldChildren.PawnRenderer_RenderPawnInternal_Patch.GetHairMesh(graphics, pawn, headFacing);
                }
            }
            catch (TypeLoadException) { }

            return null;
        }
        */
    }
}
