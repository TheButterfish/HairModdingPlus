using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ButterfishHairModdingPlus
{
    class Compat_ShowHair
    {
        public static bool SHCompat_ShouldHideHair(Pawn pawn, ThingDef hat, bool portrait)
        {
            Type t_Settings = GenTypes.GetTypeInAnyAssembly("ShowHair.Settings");
            Type t_Patch_PawnRenderer_RenderPawnInternal = GenTypes.GetTypeInAnyAssembly("ShowHair.Patch_PawnRenderer_RenderPawnInternal");
            try
            {
                if (t_Settings != null && t_Patch_PawnRenderer_RenderPawnInternal != null)
                {
                    MethodInfo m_hideHats = t_Patch_PawnRenderer_RenderPawnInternal.GetMethod("HideHats", BindingFlags.Static | BindingFlags.NonPublic);
                    if ((bool) m_hideHats.Invoke(null, new object[1] { portrait }))
                    {
                        return false;
                    }
                    else
                    {
                        bool onlyApplyToColonists = (bool) t_Settings.GetField("OnlyApplyToColonists").GetValue(null);
                        Dictionary<ThingDef, bool> hatsThatHide = (Dictionary<ThingDef, bool>) t_Settings.GetField("HatsThatHide").GetValue(null);
                        Dictionary<HairDef, bool> hairToHide = (Dictionary<HairDef, bool>) t_Settings.GetField("HairToHide").GetValue(null);

                        if (onlyApplyToColonists && pawn.Faction != Faction.OfPlayer)
                        {
                            return true;
                        }
                        else
                        {
                            hatsThatHide.TryGetValue(hat, out bool hatHides);
                            hairToHide.TryGetValue(pawn.story.hairDef, out bool hairHidden);

                            return hatHides || hairHidden;
                        }
                    }
                }
            }
            catch (TypeLoadException) { }

            return true;
        }
    }
}
