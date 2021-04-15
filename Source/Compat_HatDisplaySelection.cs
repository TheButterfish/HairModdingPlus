using RimWorld;
using System;
using Verse;

namespace ButterfishHairModdingPlus
{
    class Compat_HatDisplaySelection
    {
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
